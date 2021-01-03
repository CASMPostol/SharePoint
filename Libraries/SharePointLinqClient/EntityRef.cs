//<summary>
//  Title   : class EntityRef<TEntity>
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-01-21 14:28:33 +0100 (śr., 21 sty 2015) $
//  $Rev: 11245 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/EntityRef.cs $
//  $Id: EntityRef.cs 11245 2015-01-21 13:28:33Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using Microsoft.SharePoint.Client;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Microsoft.SharePoint.Linq
{
  /// <summary>
  ///  Provides for deferred loading and relationship maintenance for the singleton side of a one-to-many relationship.
  /// </summary>
  /// <typeparam name="TEntity"> The type of the entity on the singleton side of the relationship.</typeparam>
  public class EntityRef<TEntity> : IEntityRef
    where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the Microsoft.SharePoint.Linq.EntityRef class.
    /// </summary>
    public EntityRef() { }
    #endregion

    #region public
    /// <summary>
    /// Raised after a change to this <see cref="EntityRef{TEntity}"/> object.
    /// </summary>
    public event EventHandler OnChanged;
    /// <summary>
    /// Raised before a change to this  <see cref="EntityRef{TEntity}"/> object.
    /// </summary>
    public event EventHandler OnChanging;
    /// <summary>
    /// Raised when the <see cref="EntityRef{TEntity}"/> object is synchronized with the entity it represents.
    /// </summary>
    public event EventHandler<AssociationChangedEventArgs<TEntity>> OnSync;
    /// <summary>
    /// Creates a shallow copy of the Microsoft.SharePoint.Linq.EntityRef.
    /// </summary>
    /// <returns>
    /// A <see cref="Object"/> (<see cref="EntityRef{TEntity}"/>) whose property values refer to the same objects as the property values of this <see cref="EntityRef{TEntity}"/>EntityRef.
    ///</returns>
    public object Clone()
    {
      return MemberwiseClone();
    }
    /// <summary>
    /// Returns the entity that is wrapped by this Microsoft.SharePoint.Linq.EntityRef object.
    /// </summary>
    /// <returns>A System.Object that represents the entity that is stored in a private field
    ///     of this Microsoft.SharePoint.Linq.EntityRef object.</returns>
    public TEntity GetEntity()
    {
      LoadEntity();
      if (m_Lookup != null && m_Lookup.EntityState == EntityState.Deleted)
        throw new ArgumentNullException("The target entity has been deleted.");
      return m_Lookup;
    }
    /// <summary>
    /// Sets the entity to which this <see cref="Microsoft.SharePoint.Linq.EntityRef{TEntity}"/> refers.
    /// </summary>
    /// <param name="entity">The entity to which the <see cref="Microsoft.SharePoint.Linq.EntityRef{TEntity}"/> is being pointed.</param>
    /// <exception cref="System.InvalidOperationException">The object is not registered in the context.</exception>
    public void SetEntity(Object entity)
    {
      if (entity == m_Lookup)
        return;
      if (OnChanging != null)
        OnChanging(this, new EventArgs());
      if (OnSync != null && m_Lookup != null)
        OnSync(this, new AssociationChangedEventArgs<TEntity>(m_Lookup, AssociationChangedState.Removed));
      m_Lookup = (TEntity)entity;
      m_EntityRefState = State.Normal;
      m_FieldLookupValue = null;
      if (OnSync != null && m_Lookup != null)
        OnSync(this, new AssociationChangedEventArgs<TEntity>(m_Lookup, AssociationChangedState.Added));
      if (OnChanged != null)
        OnChanged(this, new EventArgs());
    }
    #endregion

    #region IEntityRef Members
    FieldLookupValue IEntityRef.GetLookup(string listName)
    {
      LoadEntity();
      Debug.Assert(m_EntityRefState == State.Normal, "After loading the state must be normal");
      Debug.Assert(m_dataContext != null, "The field m_dataContext is null in the GetLookup");
      return m_dataContext.GetFieldLookupValue(listName, m_Lookup);
    }
    /// <summary>
    /// Sets the lookup.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="dataContext">The data context.</param>
    /// <param name="listName">Name of the list.</param>
    void IEntityRef.SetLookup(FieldLookupValue value, DataContext dataContext, string listName)
    {
      Debug.Assert(m_EntityRefState == State.NewOne, "After creation the state must be NewOne");
      Debug.Assert(dataContext != null, "The parameter dataContext is null in the SetLookup");
      Debug.Assert(!String.IsNullOrEmpty(listName), "The parameter listName is null in the SetLookup");
      m_dataContext = dataContext;
      m_listName = listName;
      m_EntityRefState = State.Normal;
      if (value == null)
        return;
      m_FieldLookupValue = value;
      m_EntityRefState = State.DeferredLoading;
      if (m_dataContext.DeferredLoadingEnabled)
        return;
      LoadEntity();
      Debug.Assert(m_EntityRefState == State.Normal, "After loading the state must be normal");
    }
    /// <summary>
    /// Loads the entity if the m_EntityRefState == State.DeferredLoading, otherwise do nothing.
    /// </summary>
    private void LoadEntity()
    {
      if (m_EntityRefState != State.DeferredLoading)
        return;
      Debug.Assert(m_dataContext != null, "DataContext null in Deferred state.");
      TEntity _entity = m_dataContext.GetFieldLookupValue<TEntity>(m_listName, m_FieldLookupValue);
      m_Lookup = _entity;
      m_EntityRefState = State.Normal;
      m_FieldLookupValue = null;
    }
    #endregion

    #region private
    private enum State { NewOne, DeferredLoading, Normal };
    private string m_listName = String.Empty;
    private DataContext m_dataContext = null;
    private FieldLookupValue m_FieldLookupValue = new FieldLookupValue() { LookupId = -1 };
    private TEntity m_Lookup = default(TEntity);
    private State m_EntityRefState = State.NewOne;
    #endregion

  }
}
