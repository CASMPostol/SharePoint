//<summary>
//  Title   : class TEntityWrapper<TEntity>
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-01-23 11:55:00 +0100 (pt., 23 sty 2015) $
//  $Rev: 11254 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/TEntityWrapper.cs $
//  $Id: TEntityWrapper.cs 11254 2015-01-23 10:55:00Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Client.Linq2SP;
using CAS.SharePoint.Client.Reflection;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Microsoft.SharePoint.Linq
{
  /// <summary>
  /// TEntityWrapper class
  /// </summary>
  /// <typeparam name="TEntity">The type of the entity.</typeparam>
  internal class TEntityWrapper<TEntity> : ITrackEntityState
    where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
  {

    #region creators
    internal TEntityWrapper(DataContext dataContext, ListItem listItem, Type type, StorageItemsList storageList, PropertyChangedEventHandler handler)
      : this(dataContext)
    {
      if (listItem == null)
        throw new ArgumentNullException("listItem");
      MyListItem = listItem;
      ContentTypeID = listItem.GetContentTypeID();
      this.TEntityGetter = NewTEntity(storageList, type);
      TEntityGetter.EntityState = EntityState.Unchanged;
      TEntityGetter.OriginalValues.Clear();
      TEntityGetter.PropertyChanged += handler;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="TEntityWrapper{TEntity}" /> class. Used to create a new list item.
    /// </summary>
    /// <param name="dataContext">The data context.</param>
    /// <param name="entity">The new entity to be inserted.</param>
    /// <param name="list">The list.</param>
    /// <param name="handler">The <see cref="PropertyChangedEventHandler" /> handler.</param>
    internal TEntityWrapper(DataContext dataContext, TEntity entity, List list, PropertyChangedEventHandler handler)
      : this(dataContext)
    {
      entity.EntityState = EntityState.ToBeInserted;
      entity.PropertyChanged += handler;
      this.TEntityGetter = entity;
      this.MyListItem = list.AddItem(new ListItemCreationInformation());
      ContentTypeID = entity.GetType().GetContentType().Id;
      Index = b_indexCounter--;
    }
    #endregion

    #region internal
    /// <summary>
    /// Assigns the values to entity.
    /// </summary>
    /// <param name="storageList">The storage dictionary containing field name and <see cref="StorageItem" /> pairs.</param>
    /// <exception cref="System.NotImplementedException">Only ColumnAttribute is supported.
    /// or
    /// IsLookupId must be true for lookup field.
    /// </exception>
    internal void AssignValues2Entity(StorageItemsList storageList)
    {
      AssignValues2Entity(storageList, this.TEntityGetter);
    }
    /// <summary>
    /// Gets the values from entity.
    /// </summary>
    /// <param name="entityDictionary">The entity dictionary containing property name <see cref="StorageItem"/>pairs..</param>
    internal void GetValuesFromEntity(Dictionary<string, StorageItem> entityDictionary)
    {
      ITrackOriginalValues _entity = (ITrackOriginalValues)this.TEntityGetter;
      foreach (var _ovx in _entity.OriginalValues)
      {
        StorageItem _storage = entityDictionary[_ovx.Key];
        _storage.GetValueFromEntity(_entity, (name, value) => MyListItem[name] = value);
      }
      MyListItem.Update();
      _entity.OriginalValues.Clear();
      EntityState = EntityState.Unchanged;
    }
    internal void Attach(Dictionary<string, StorageItem> dictionary, string listName)
    {
      Debug.Assert(!String.IsNullOrEmpty(listName), "TEntityWrapper<TEntity>.Attach the parameter listName is null");
      foreach (KeyValuePair<string, StorageItem> _item in dictionary)
        _item.Value.Attach(this.TEntityGetter, m_DataContext, listName);

    }
    internal int Index { get { return b_Index; } private set { b_Index = value; } }
    internal TEntity TEntityGetter { get; private set; }
    internal void DeleteObject()
    {
      EntityState = EntityState.Deleted;
      this.MyListItem.DeleteObject();
    }
    internal void RefreshLoad()
    {
      m_DataContext.LoadListItem(this.MyListItem);
    }
    internal string ContentTypeID { get; private set; }
    #endregion

    #region ITrackEntityState Members
    public EntityState EntityState
    {
      get
      {
        return TEntityGetter.EntityState;
      }
      set
      {
        TEntityGetter.EntityState = value;
      }
    }
    #endregion

    #region private
    private ListItem MyListItem = null;
    private DataContext m_DataContext = default(DataContext);
    private int b_Index = -1;
    private static int b_indexCounter = -1;
    private TEntityWrapper(DataContext dataContext)
    {
      m_DataContext = dataContext;
    }
    private void AssignValues2Entity(StorageItemsList storageList, TEntity _entity)
    {
      Index = MyListItem.Id;
      foreach (StorageItem _storage in storageList.Values)
      {
        if (_storage.IsReverseLookup())
        {
          _storage.AssignReverseLookupValues2Entity(m_DataContext, _entity, Index);
          continue;
        }
        if (!MyListItem.FieldValues.ContainsKey(_storage.SPFieldName))
          throw new ArgumentOutOfRangeException(String.Format("Cannot get value from SharePoint for the field {0}", _storage.SPFieldName));
        object Value = MyListItem.FieldValues[_storage.SPFieldName];
        _storage.AssignValue2Entity(m_DataContext, _entity, Value);
      }
    }
    private TEntity NewTEntity(StorageItemsList storageList, Type type)
    {
      System.Reflection.ConstructorInfo _constructor = type.GetConstructor(new Type[0]);
      Debug.Assert(_constructor != null, String.Format("Cannot get constructor for the type {0}", type.Name));
      TEntity _ret = (TEntity)_constructor.Invoke(new object[0]);
      Debug.Assert(_ret != null, String.Format("Cannot invoke constructor for the type {0}", type.Name));
      AssignValues2Entity(storageList, _ret);
      return _ret;
    }
    #endregion

  }
}
