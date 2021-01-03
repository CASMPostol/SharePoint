//<summary>
//  Title   : EntitySet class
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-11-28 11:47:47 +0100 (pt., 28 lis 2014) $
//  $Rev: 11006 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/EntitySet.cs $
//  $Id: EntitySet.cs 11006 2014-11-28 10:47:47Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.SharePoint.Linq
{
  /// <summary>
  /// Provides for deferred loading and relationship maintenance for the many side of one-to-many and many-to-many relationships
  /// </summary>
  /// <typeparam name="TEntity">The type of the member of the collection.</typeparam>
  public class EntitySet<TEntity> : ICollection<TEntity>, IOrderedQueryable<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>, IOrderedQueryable, IQueryable, IList, ICollection, IEnumerable, IEntitySet//, ICloneable
    where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
  {
    /// <summary>
    /// Initializes a new instance of the Microsoft.SharePoint.Linq.EntitySet class
    /// </summary>
    public EntitySet() { }
    /// <summary>
    /// Raised after a change to this Microsoft.SharePoint.Linq.EntitySet object.
    /// </summary>
    public event EventHandler OnChanged;
    /// <summary>
    /// Raised before a change to this Microsoft.SharePoint.Linq.EntitySet object.
    /// </summary>
    public event EventHandler OnChanging;
    /// <summary>
    /// Raised when the Microsoft.SharePoint.Linq.EntitySet object is synchronized with the entities that it represents.
    /// </summary>
    public event EventHandler<AssociationChangedEventArgs<TEntity>> OnSync;
    /// <summary>
    /// Replaces the entities currently associated with this Microsoft.SharePoint.Linq.EntitySet with the specified collection.
    /// </summary>
    /// <param name="entities">The collection of entities with which the current set is replaced.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Assign(IEnumerable<TEntity> entities) { throw new NotImplementedException(); }
    /// <summary>
    /// Creates a shallow copy of the Microsoft.SharePoint.Linq.EntitySet.
    /// </summary>
    /// <returns>
    /// A System.Object (cast-able Microsoft.SharePoint.Linq.EntitySet) whose property values refer to the same objects as the property values of this Microsoft.SharePoint.Linq.EntitySet.
    /// </returns>
    public object Clone() { return this.Clone(); }
    /// <summary>
    /// Indicates whether a specified entity is in the <see cref="Microsoft.SharePoint.Linq.EntitySet{TEntity}"/>
    /// </summary>
    /// <param name="value">The <see cref="T:System.Object" />The object whose presence is questioned.</param>
    /// <returns>
    /// Indicates whether a specified object is in the Microsoft.SharePoint.Linq.EntitySet.
    /// </returns>
    public bool Contains(TEntity value)
    {
      if (value == null)
        throw new ArgumentNullException("value of IndexOf cannot be null");
      int _ret = GetIndex(value);
      return ItemsCollection.ContainsKey(_ret);
    }
    /// <summary>
    /// Copies the members of the Microsoft.SharePoint.Linq.EntitySet to the specified array beginning at the specified array index.
    /// </summary>
    /// <param name="array">The target array.</param>
    /// <param name="index">The zero-based index in the array at which copying begins.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void CopyTo(Array array, int index) { ItemsCollection.Select(x => x.Value).ToArray().CopyTo(array, index); }
    /// <summary>
    /// Returns the zero-based index of the first occurrence of the specified object in the collection.
    /// </summary>
    /// <param name="value">The object whose index is returned.</param>
    /// <returns>
    /// A System.Int32 that represents the zero-based index of the specified entity in the Microsoft.SharePoint.Linq.EntitySet
    /// </returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public int IndexOf(object value) { return GetIndex((TEntity)value); }
    /// <summary>
    /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList" />.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Collections.IList" />.</param>
    public void Remove(object value) { throw new NotImplementedException(); }

    #region IQueryable Members
    /// <summary>
    /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable" /> is executed.
    /// </summary>
    /// <returns>A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.</returns>
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
    public System.Linq.Expressions.Expression Expression
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

    #region IEnumerable
    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
    /// </returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public IEnumerator<TEntity> GetEnumerator()
    {
      return ItemsCollection.Select<KeyValuePair<int, TEntity>, TEntity>(x => x.Value).GetEnumerator();
    }
    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return ItemsCollection.Select<KeyValuePair<int, TEntity>, TEntity>(x => x.Value).GetEnumerator();
    }
    #endregion

    #region ICollection
    /// <summary>
    /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public int Count
    {
      get { return ItemsCollection.Count; }
    }
    /// <summary>
    /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public bool IsSynchronized
    {
      get { return ((ICollection)ItemsCollection).IsSynchronized; }
    }
    /// <summary>
    /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public object SyncRoot
    {
      get { return ((ICollection)ItemsCollection).SyncRoot; }
    }
    #endregion

    #region ICollection{TEntity},
    /// <summary>
    /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
    /// </summary>
    /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
    public void Add(TEntity item)
    {
      ItemsCollection.Add(GetIndex(item), item);
    }
    /// <summary>
    /// Copies to.
    /// </summary>
    /// <param name="array">The array.</param>
    /// <param name="arrayIndex">Index of the array.</param>
    public void CopyTo(TEntity[] array, int arrayIndex)
    {
      ItemsCollection.Values.CopyTo(array, arrayIndex);
    }
    /// <summary>
    /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
    /// </summary>
    /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
    /// <returns>
    /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
    /// </returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public bool Remove(TEntity item)
    {
      return ItemsCollection.Remove(GetIndex(item));
    }
    /// <summary>
    /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
    /// </summary>
    public void Clear()
    {
      ItemsCollection.Clear();
    }
    /// <summary>
    /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
    /// </summary>
    public bool IsReadOnly
    {
      get { return ((ICollection<TEntity>)ItemsCollection).IsReadOnly; }
    }
    #endregion

    #region ICollection
    /// <summary>
    /// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
    /// </summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
    /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    void ICollection.CopyTo(Array array, int index)
    {
      ItemsCollection.Select(x => x.Value).ToArray().CopyTo(array, index); ;
    }
    /// <summary>
    /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
    /// </summary>
    int ICollection.Count
    {
      get { return ((ICollection)ItemsCollection).Count; }
    }
    /// <summary>
    /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).
    /// </summary>
    bool ICollection.IsSynchronized
    {
      get { return ((ICollection)ItemsCollection).IsSynchronized; }
    }
    /// <summary>
    /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
    /// </summary>
    object ICollection.SyncRoot
    {
      get { return ((ICollection)ItemsCollection).SyncRoot; }
    }
    #endregion

    #region IList
    /// <summary>
    /// Adds an item to the <see cref="T:System.Collections.IList" />.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Collections.IList" />.</param>
    /// <returns>
    /// The position into which the new element was inserted.
    /// </returns>
    int IList.Add(object value)
    {
      TEntity _item = (TEntity)value;
      int _ret = GetIndex(_item);
      ItemsCollection.Add(_ret, _item);
      return _ret;
    }
    /// <summary>
    /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
    /// </summary>
    void IList.Clear()
    {
      ItemsCollection.Clear();
    }
    bool IList.Contains(object value)
    {
      TEntity _item = (TEntity)value;
      int _ret = GetIndex(_item);
      return ItemsCollection.ContainsKey(_ret);
    }
    /// <summary>
    /// Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />.</param>
    /// <returns>
    /// The index of <paramref name="value" /> if found in the list; otherwise, -1.
    /// </returns>
    int IList.IndexOf(object value)
    {
      if (value == null)
        throw new ArgumentNullException("value of IndexOf cannot be null");
      TEntity _item = (TEntity)value;
      int _ret = GetIndex(_item);
      return _ret;
    }
    /// <summary>
    /// Inserts an item to the <see cref="T:System.Collections.IList" /> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
    /// <param name="value">The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Collections.IList" />.</param>
    void IList.Insert(int index, object value)
    {
      throw new NotImplementedException();
    }
    /// <summary>
    /// Gets a value indicating whether the <see cref="T:System.Collections.IList" /> has a fixed size.
    /// </summary>
    bool IList.IsFixedSize
    {
      get { return ((IList)ItemsCollection).IsFixedSize; }
    }
    /// <summary>
    /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
    /// </summary>
    bool IList.IsReadOnly
    {
      get { return ((IList)ItemsCollection).IsReadOnly; }
    }
    /// <summary>
    /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList" />.
    /// </summary>
    /// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Collections.IList" />.</param>
    void IList.Remove(object value) { throw new NotImplementedException(); }
    /// <summary>
    /// Removes the <see cref="T:System.Collections.IList" /> item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the item to remove.</param>
    void IList.RemoveAt(int index) { throw new NotImplementedException(); }
    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    object IList.this[int index]
    {
      get
      {
        return ItemsCollection[index];
      }
      set
      {
        throw new NotImplementedException();
      }
    }
    #endregion

    #region IEntitySet
    void IEntitySet.LoadValues(DataContext dataContext, AssociationAttribute association, int id)
    {
      m_AssociationInfo = association;
      m_Id = id;
      m_DataContext = dataContext;
      if (!m_DataContext.DeferredLoadingEnabled)
        _LoadValues();
    }
    #endregion

    #region private
    private Dictionary<int, TEntity> ItemsCollection
    {
      get
      {
        _LoadValues();
        return p_ItemsCollection;
      }
    }
    private Dictionary<int, TEntity> p_ItemsCollection = new Dictionary<int, TEntity>();
    private bool m_2BeLoaded = true;
    private DataContext m_DataContext = null;
    private AssociationAttribute m_AssociationInfo;
    private int m_Id;
    private void _LoadValues()
    {
      if (!m_2BeLoaded)
        return;
      Dictionary<int, Object> _items = m_DataContext.GetListItemCollection<TEntity>(m_AssociationInfo, m_Id);
      foreach (KeyValuePair<int, Object> _ex in _items)
      {
        if (p_ItemsCollection.ContainsKey(_ex.Key))
          continue;
        p_ItemsCollection.Add(_ex.Key, (TEntity)_ex.Value);
      }
      m_2BeLoaded = false;
    }
    private int GetIndex(TEntity item)
    {
      return m_DataContext.GetIndex<TEntity>(m_AssociationInfo, item);
    }
    #endregion

  }
}
