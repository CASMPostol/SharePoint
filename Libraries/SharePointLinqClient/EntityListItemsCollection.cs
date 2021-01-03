//<summary>
//  Title   : class EntityListItemsCollection
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-01-23 11:55:00 +0100 (pt., 23 sty 2015) $
//  $Rev: 11254 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePointLinqClient/EntityListItemsCollection.cs $
//  $Id: EntityListItemsCollection.cs 11254 2015-01-23 10:55:00Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Client.CAML;
using CAS.SharePoint.Client.Linq2SP;
using CAS.SharePoint.Client.Reflection;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using SPCList = Microsoft.SharePoint.Client.List;

namespace Microsoft.SharePoint.Linq
{
  /// <summary>
  /// Class EntityListItemsCollection - manages the collection of items of a list. 
  /// </summary>
  /// <typeparam name="TEntity">The type of the t entity.</typeparam>
  internal class EntityListItemsCollection<TEntity> : IEntityListItemsCollection
    where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
  {

    #region creator
    internal EntityListItemsCollection(DataContext dataContext, SPCList list)
    {
      this.m_list = list;
      this.m_DataContext = dataContext;
      m_DerivedTypes = typeof(TEntity).GetDerivedTypes();
      m_EntityPropertiesDictionary = new Dictionary<string, Dictionary<string, StorageItem>>();
      m_ListItemPropertiesDictionary = new Dictionary<string, StorageItemsList>();
      foreach (var _typeidx in m_DerivedTypes)
      {
        StorageItemsList _si = StorageItem.CreateStorageDescription(_typeidx.Value, true);
        m_EntityPropertiesDictionary.Add(_typeidx.Key, _si.Values.ToDictionary<StorageItem, string>(storageItem => storageItem.PropertyName));
        m_ListItemPropertiesDictionary.Add(_typeidx.Key, _si);
      }
    }
    #endregion

    #region IEntityListItemsCollection Members
    /// <summary>
    /// Submit the changes.
    /// </summary>
    /// <param name="executeQuery">The action used to execute query.</param>
    void IEntityListItemsCollection.SubmitChanges(Action<ProgressChangedEventArgs, Action> executeQuery)
    {
      if (m_Unchanged)
      {
        //TODO Wrong Assertion in SubmitingChanges caused by an exception
        //TODO http://casas:11227/sites/awt/Lists/TaskList/_cts/Tasks/displayifs.aspx?List=72c511b5%2D8b63%2D4dfa%2Dad34%2D133a97eba469&ID=4105
        IEnumerable<TEntityWrapper<TEntity>> _errors = (from _x in m_Collection
                                                        where _x.Value.TEntityGetter.EntityState != EntityState.Unchanged && _x.Value.TEntityGetter.EntityState != EntityState.Deleted
                                                        select _x.Value); //.ToList<TEntityWrapper<TEntity>>();
        Debug.Assert(!_errors.Any(), "Wrong value of Unchanged in the SubmitingChanges - expected false");
        return;
      }
      Debug.Assert((from _x in m_Collection where _x.Value.TEntityGetter.EntityState != EntityState.Unchanged select _x).Any(), "Wrong value of Unchanged in the SubmitingChanges - expected true");
      SubmittingChanges4Parents();
      int _cntrNew = 0;
      int _cntrUpdt = 0;
      int _cntrDlt = 0;
      //ToBeDeleted
      List<TEntityWrapper<TEntity>> _toBeDeletedCollection = (from _tewx in m_Collection.Values where _tewx.EntityState == EntityState.ToBeDeleted select _tewx).ToList();
      foreach (TEntityWrapper<TEntity> _itemX in _toBeDeletedCollection)
      {
        _itemX.DeleteObject();
        _cntrDlt++;
        if (_cntrDlt % 10 == 0)
          executeQuery(new ProgressChangedEventArgs(1, String.Format("Execute query ListItem Delete # {0}", _cntrDlt)), () => { });
      }
      executeQuery(new ProgressChangedEventArgs(1, String.Format("Execute query ListItem Update # {0}", _cntrDlt)), () => { });
      //ToBeInserted
      List<TEntityWrapper<TEntity>> _toBeInsertedCollection = (from _tewx in m_Collection.Values where _tewx.EntityState == EntityState.ToBeInserted select _tewx).ToList();
      foreach (TEntityWrapper<TEntity> _toBeInsertedItem in _toBeInsertedCollection)
      {
        Debug.Assert(_toBeInsertedItem.Index < 0, "index of a new item must be initiated as  value < 0");
        m_Collection.Remove(_toBeInsertedItem.Index);
        _toBeInsertedItem.GetValuesFromEntity(m_EntityPropertiesDictionary[_toBeInsertedItem.ContentTypeID]);
        executeQuery(new ProgressChangedEventArgs(1, String.Format("Execute query ListItem to be inserted # {0}", _cntrNew++)), () => { _toBeInsertedItem.RefreshLoad(); });
        _toBeInsertedItem.AssignValues2Entity(m_ListItemPropertiesDictionary[_toBeInsertedItem.ContentTypeID]);
        Debug.Assert(_toBeInsertedItem.Index > 0, "index of a created item must be a value > 0");
        m_Collection.Add(_toBeInsertedItem.Index, _toBeInsertedItem);
      }
      //ToBeUpdated
      List<TEntityWrapper<TEntity>> _toBeUpdatedCollection = (from _tewx in m_Collection.Values where _tewx.EntityState == EntityState.ToBeUpdated select _tewx).ToList();
      foreach (TEntityWrapper<TEntity> _itemX in _toBeUpdatedCollection)
      {
        _itemX.GetValuesFromEntity(m_EntityPropertiesDictionary[_itemX.ContentTypeID]);
        _cntrUpdt++;
        if (_cntrUpdt % 10 == 0)
          executeQuery(new ProgressChangedEventArgs(1, String.Format("Execute query ListItem Update # {0}", _cntrUpdt)), () => { });
      }
      executeQuery(new ProgressChangedEventArgs(1, String.Format("Execute query ListItem Update # {0}", _cntrUpdt)), () => { });
      m_Unchanged = true;
    }
    /// <summary>
    /// Gets the field lookup value.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>An <see cref="FieldLookupValue" /> object containing reference to the entity</returns>
    /// <exception cref="System.ArgumentOutOfRangeException">Reference to unattached Entity</exception>
    FieldLookupValue IEntityListItemsCollection.GetFieldLookupValue(Object entity)
    {
      int _lookupId = entity == null ? -1 : GetIndex(entity);
      if ((entity != null) && (_lookupId < 0))
        throw new ArgumentOutOfRangeException("Reference to unattached Entity");
      if (_lookupId > 0 && !m_Collection.ContainsKey(_lookupId))
        throw new ArgumentOutOfRangeException("Reference to unattached Entity");
      return new FieldLookupValue()
      {
        LookupId = _lookupId
      };
    }
    /// <summary>
    /// Gets the field lookup value.
    /// </summary>
    /// <param name="fieldLookupValue">The field lookup value.</param>
    /// <returns>Return the value.</returns>
    Object IEntityListItemsCollection.GetFieldLookupValue(FieldLookupValue fieldLookupValue)
    {
      Debug.Assert(fieldLookupValue != null, "Parameter fieldLookupValue is null at GetFieldLookupValue");
      if (fieldLookupValue.LookupId < 0)
        return null;
      if (!this.ContainsKey(fieldLookupValue.LookupId))
        m_DataContext.LoadListItem<TEntity>(fieldLookupValue.LookupId, this);
      if (!m_Collection.ContainsKey(fieldLookupValue.LookupId))
        return null;
      return m_Collection[fieldLookupValue.LookupId].TEntityGetter;
    }
    /// <summary>
    /// Gets the entities for reverse lookup.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="parentItemId">The parent item identifier.</param>
    /// <returns>Dictionary&lt;System.Int32, Object&gt;.</returns>
    Dictionary<int, Object> IEntityListItemsCollection.GetEntitiesForReverseLookup(string fieldName, int parentItemId)
    {
      Dictionary<int, Object> _ret = new Dictionary<int, Object>();
      m_DataContext.GetListItemCollection(this.MyList, CamlQueryDefinition.GetCAMLSelectedLookup(parentItemId, fieldName, m_DataContext.RowLimit), _items =>
      {
        foreach (ListItem _lix in _items)
        {
          if (m_Collection.ContainsKey(_lix.Id))
            _ret.Add(_lix.Id, m_Collection[_lix.Id].TEntityGetter);
          else
            _ret.Add(_lix.Id, this.Add(_lix));
        };
      });
      return _ret;
    }
    /// <summary>
    /// Gets the index of the <paramref name="item" /> using reflection and <see cref="CAS.SharePoint.Client.Linq2SP.StorageItemsList" />.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns>System.Int32.</returns>
    /// <exception cref="System.ArgumentNullException">You are trying to get index for null reference</exception>
    /// <exception cref="System.ArgumentOutOfRangeException">Before getting the Id of an abject it must be inserted or attached first.</exception>
    public int GetIndex(object item)
    {
      if (item == null)
        throw new ArgumentNullException("You are trying to get index for null reference");
      string _typeName = item.GetType().Name;
      Dictionary<string, KeyValuePair<string, string>> _TNameContentIdPairs =
        m_DerivedTypes.Select<KeyValuePair<string, Type>, KeyValuePair<string, string>>(x => new KeyValuePair<string, string>(x.Value.Name, x.Key)).ToDictionary<KeyValuePair<string, string>, string>(x => x.Key);
      Debug.Assert(_TNameContentIdPairs.ContainsKey(_typeName), "Mismatch between type name and recognized types");
      int _ret = this.m_ListItemPropertiesDictionary[_TNameContentIdPairs[_typeName].Value].GetId<TEntity>((TEntity)item);
      if (_ret < 0)
        throw new ArgumentOutOfRangeException("Before getting the Id of an abject it must be inserted or attached first.");
      return _ret;
    }
    #endregion

    #region internal
    internal List MyList { get { return m_list; } }
    /// <summary>
    /// Adds new entity to be inserted
    /// </summary>
    /// <param name="entity">The entity to be added to teh collection.</param>
    /// <param name="listName">Name of the list.</param>
    internal void Add(TEntity entity, string listName)
    {
      TEntityWrapper<TEntity> _wrpr = new TEntityWrapper<TEntity>(m_DataContext, entity, MyList, PropertyChanged);
      _wrpr.Attach(m_EntityPropertiesDictionary[_wrpr.ContentTypeID], listName);
      m_Collection.Add(_wrpr.Index, _wrpr);
      m_Unchanged = false;
    }
    /// <summary>
    /// Adds an entity for the specified list item.
    /// </summary>
    /// <param name="listItem">The list item.</param>
    /// <returns>Created wrapper object</returns>
    internal TEntity Add(ListItem listItem)
    {
      string _contentTypeID = listItem.GetContentTypeID();
      TEntityWrapper<TEntity> _ewrp = new TEntityWrapper<TEntity>(m_DataContext, listItem, m_DerivedTypes[_contentTypeID], m_ListItemPropertiesDictionary[_contentTypeID], PropertyChanged);
      TEntity _newEntity = _ewrp.TEntityGetter;
      m_Collection.Add(_ewrp.Index, _ewrp);
      return _newEntity;
    }
    internal TEntity this[int index]
    {
      get { return m_Collection[index].TEntityGetter; }
    }
    internal bool ContainsKey(int index)
    {
      return m_Collection.ContainsKey(index);
    }
    internal void DeleteOnSubmit(TEntity entity)
    {
      entity.EntityState = EntityState.ToBeDeleted;
      m_Unchanged = false;
    }
    #endregion

    #region private

    private DataContext m_DataContext = default(DataContext);
    private SPCList m_list = default(SPCList);
    private bool m_Unchanged = true;
    private Dictionary<string, Type> m_DerivedTypes = null;
    private Dictionary<string, Dictionary<string, StorageItem>> m_EntityPropertiesDictionary;
    private Dictionary<string, StorageItemsList> m_ListItemPropertiesDictionary;
    private Dictionary<int, TEntityWrapper<TEntity>> m_Collection = new Dictionary<int, TEntityWrapper<TEntity>>();
    private void SubmittingChanges4Parents()
    {
      foreach (KeyValuePair<string, Dictionary<string, StorageItem>> _contentTypeDescription in m_EntityPropertiesDictionary)
        foreach (StorageItem _si in _contentTypeDescription.Value.Values)
        {
          if (!_si.IsLookup)
            continue;
          m_DataContext.SubmitChanges(_si.TargetListName );
        };
    }
    private void PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      m_Unchanged = false;
      ITrackEntityState _entity = sender as ITrackEntityState;
      if (_entity == null)
        throw new ArgumentNullException("sender", "PropertyChanged must be called from ITrackEntityState");
      switch (_entity.EntityState)
      {
        case EntityState.Unchanged:
          _entity.EntityState = EntityState.ToBeUpdated;
          break;
        case EntityState.ToBeInserted:
        case EntityState.ToBeUpdated:
        case EntityState.ToBeRecycled:
        case EntityState.ToBeDeleted:
        case EntityState.Deleted:
          break;
      }
    }

    #endregion

  }
}
