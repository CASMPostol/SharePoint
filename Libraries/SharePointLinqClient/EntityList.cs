//<summary>
//  Title   : public sealed class EntityList
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2015-01-09 16:00:57 +0100 (pt., 09 sty 2015) $
//  $Rev: 11188 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/EntityList.cs $
//  $Id: EntityList.cs 11188 2015-01-09 15:00:57Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Client.CAML;
using Microsoft.SharePoint.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Microsoft.SharePoint.Linq
{
  /// <summary>
  /// Represents a Windows SharePoint Services "14" list that can be queried with Language Integrated Query (LINQ).
  /// </summary>
  /// <typeparam name="TEntity">The content type of the list items.</typeparam>
  public sealed class EntityList<TEntity> : IOrderedQueryable<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>, IOrderedQueryable, IQueryable, IEnumerable
    where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
  {

    #region creator
    internal EntityList(DataContext dataContext, EntityListItemsCollection<TEntity> itemsCollection, string listName, int rowLimit)
    {
      this.m_ListName = listName;
      this.Query = CamlQueryDefinition.CAMLAllItemsQuery(rowLimit);
      this.m_AllItemsCollection = itemsCollection;
      this.DataContext = dataContext;
    }
    #endregion

    #region public
    /// <summary>
    /// Registers a disconnected or "detached" entity with the object tracking system of the <see cref="DataContext"/> object associated with the list.
    /// </summary>
    /// <param name="entity">The entity that is registered.</param>
    /// <exception cref="System.ArgumentNullException">entity is null.</exception>
    /// <exception cref="System.InvalidOperationException">
    ///   Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext object.
    ///- or 
    ///   -entity is not of the same type as the list items.
    /// - or 
    ///   -entity has been deleted.
    /// - or 
    ///   -There is a problem with the internal ID of entity that is used by the object tracking system.
    /// </exception>
    public void Attach(TEntity entity) { throw new NotImplementedException(); }
    /// <summary>
    /// Marks the specified entities for deletion on the next call of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    /// </summary>
    /// <param name="entities">The entities to be marked for deletion.</param>
    /// <exception cref="System.ArgumentNullException">At least one member of entities is null.</exception>
    /// <exception cref="System.InvalidOperationException">
    /// Object tracking is not enabled for the <see cref="DataContext"/> object.
    /// - or -
    /// At least one member of entities is not of the same type as the list items.
    /// </exception>
    public void DeleteAllOnSubmit(IEnumerable<TEntity> entities)
    {
      foreach (TEntity item in entities)
        DeleteOnSubmit(item);
    }
    /// <summary>
    /// Marks the specified entity for deletion on the next call of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    /// </summary>
    /// <param name="entity">The entity to be marked for deletion.</param>
    /// <exception cref="System.ArgumentNullException">entity is null.</exception>
    /// <exception cref="System.InvalidOperationException">Object tracking is not enabled for the <see cref="DataContext"/> object.
    ///  - or -
    /// entity is not of the same type as the list items.</exception>
    public void DeleteOnSubmit(TEntity entity)
    {
      CheckObjectTrackingEnabled();
      m_AllItemsCollection.DeleteOnSubmit(entity);
    }
    /// <summary>
    /// Marks the specified entities for insertion into the list on the next call of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges
    /// </summary>
    /// <param name="entities">The entities to be inserted.</param>
    /// <exception cref="System.ArgumentNullException">entity is null.</exception>
    /// <exception cref="System.InvalidOperationException">Object tracking is not enabled for the <see cref="DataContext"/> object.
    /// - or -
    /// entity is not of the same type as the list items.
    /// - or -
    /// entity has been deleted.
    /// - or -
    /// entity has been updated. 
    /// - or -
    /// There is a problem with the internal ID of entity that is used by the object tracking system.
    /// </exception>
    public void InsertAllOnSubmit(IEnumerable<TEntity> entities)
    {
      foreach (TEntity _item in entities)
        InsertOnSubmit(_item);
    }
    /// <summary>
    /// Marks the specified entity for insertion into the list on the next call of DataContext.SubmitChanges.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <exception cref="System.ArgumentNullException">entity is null.</exception>
    /// <exception cref="System.InvalidOperationException">Object tracking is not enabled for the <see cref="DataContext"/> object.
    /// - or -
    /// entity is not of the same type as the list items.
    /// - or -
    /// entity has been deleted.
    /// - or -
    /// entity has been updated. 
    /// - or -
    /// There is a problem with the internal ID of entity that is used by the object tracking system.
    /// </exception>
    public void InsertOnSubmit(TEntity entity)
    {
      CheckObjectTrackingEnabled();
      if (entity == null)
        throw new ArgumentNullException("entity", "entity is null.");
      m_AllItemsCollection.Add(entity, this.m_ListName);
      m_LocalItemsCollection.Add(entity);
    }
    /// <summary>
    /// Marks the specified entities to be put in the Recycle Bin on the next call Overload: DataContext.SubmitChanges.
    /// </summary>
    /// <param name="entities">The entities to be recycled.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    /// <exception cref="System.ArgumentNullException">At least one member of entities is null.</exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    public void RecycleAllOnSubmit(IEnumerable<TEntity> entities) { throw new NotImplementedException(); }
    /// <summary>
    /// Marks the specified entity to be put in the Recycle Bin on the next call
    /// of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    /// </summary>
    /// <param name="entity">The entity to be recycled.</param>
    /// <exception cref="System.ArgumentNullException">entity is null.</exception>
    /// <exception cref="System.InvalidOperationException">Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    ///     object.- or -entity is not of the same type as the list items.</exception>
    public void RecycleOnSubmit(TEntity entity) { throw new NotImplementedException(); }
    /// <summary>
    /// Gets the subset of the <see cref="Microsoft.SharePoint.Linq.EntityList{TEntity}"/> that 
    /// consists of all and only the list items that belong to a particular folder, with or without the items in sub-folders.
    /// </summary>
    /// <param name="folderUrl">The list-relative path to the folder.</param>
    /// <param name="recursive">true to include items in sub-folders; false to exclude them.</param>
    /// <returns>An <see cref="System.Linq.IQueryable{T}"/> object that can be cast to <see cref="Microsoft.SharePoint.Linq.EntityList{TEntity}"/> .</returns>
    public IQueryable<TEntity> ScopeToFolder(string folderUrl, bool recursive) { throw new NotImplementedException(); }
    /// <summary>
    /// Gets the name of the list.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get { return m_ListName; } }
    /// <summary>
    /// Gets the data context.
    /// </summary>
    /// <value>
    /// The data context.
    /// </value>
    public DataContext DataContext { get; private set; }

    #region internal
    internal CamlQuery Query { private get; set; }
    #endregion

    #endregion

    #region IQueryable Members
    /// <summary>
    /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable" /> is executed.
    /// </summary>
    /// <returns>A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public Type ElementType
    {
      get { return typeof(TEntity); }
    }
    /// <summary>
    /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable" />.
    /// </summary>
    /// <returns>The <see cref="T:System.Linq.Expressions.Expression" /> that is associated with this instance of <see cref="T:System.Linq.IQueryable" />.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
    public Expression Expression
    {
      get { throw new NotImplementedException(); }
    }
    /// <summary>
    /// Gets the query provider that is associated with this data source.
    /// </summary>
    /// <returns>The <see cref="T:System.Linq.IQueryProvider" /> that is associated with this data source.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
    public IQueryProvider Provider
    {
      get { throw new NotImplementedException(); }
    }
    #endregion

    #region IEnumerable Members
    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="Microsoft.SharePoint.Linq.EntityList{TEntity}"/>.
    /// </summary>
    /// <returns>An <see cref="System.Collections.Generic.IEnumerator{TEntity}"/> that can be used to iterate the list.</returns>
    public IEnumerator<TEntity> GetEnumerator()
    {
      if (m_2BeExecuted)
        GetListItems();
      return m_LocalItemsCollection.GetEnumerator();
    }
    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      if (m_2BeExecuted)
        GetListItems();
      return m_LocalItemsCollection.GetEnumerator();
    }
    #endregion

    #region private
    private string m_ListName = String.Empty;
    private EntityListItemsCollection<TEntity> m_AllItemsCollection = null;
    private List<TEntity> m_LocalItemsCollection = new List<TEntity>();
    private bool m_2BeExecuted = true;
    private void GetListItems()
    {
      DataContext.GetListItemCollection(m_AllItemsCollection.MyList, Query, _items => { foreach (ListItem _listItemx in _items) Add(_listItemx); });
      m_2BeExecuted = false;
    }
    private void Add(ListItem listItem)
    {
      TEntity _newEntity = null;
      if (m_AllItemsCollection.ContainsKey(listItem.Id))
      {
        _newEntity = m_AllItemsCollection[listItem.Id];
        m_LocalItemsCollection.Add(_newEntity);
      }
      else
      {
        _newEntity = m_AllItemsCollection.Add(listItem);
        m_LocalItemsCollection.Add(_newEntity);
      }
    }
    private void CheckObjectTrackingEnabled()
    {
      if (!DataContext.ObjectTrackingEnabled)
        throw new InvalidOperationException("Object tracking is not enabled for the DataContext object");
    }
    #endregion

  }
}
