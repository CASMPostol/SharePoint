//<summary>
//  Title   : public class DataContext
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-01-09 10:06:57 +0100 (pt., 09 sty 2015) $
//  $Rev: 11186 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/DataContext.cs $
//  $Id: DataContext.cs 11186 2015-01-09 09:06:57Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
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
using System.Diagnostics;
using System.IO;

namespace Microsoft.SharePoint.Linq
{
  /// <summary>
  ///  Provides client site LINQ (Language Integrated Query) access to, and change tracking for, the lists and document libraries of a Windows SharePoint Services "14" Web site.
  /// </summary>
  public class DataContext : IDisposable
  {

    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="DataContext" /> class.
    /// </summary>
    /// <param name="requestUrl">The URL of a Windows SharePoint Services "14" Web site that provides client site access and change tracking for the specified Web site..</param>
    public DataContext(string requestUrl)
    {
#if DEBUG
      Log = new StringWriter();
#endif
      this.ObjectTrackingEnabled = true;
      this.DeferredLoadingEnabled = true;
      RowLimit = 30;
      CreateContext(requestUrl);
    }
    /// <summary>
    /// Gets a collection of objects that represent discrepancies between the current client value and the current database value of a field in a list item.
    /// </summary>
    /// <value>
    ///  A <see cref="ChangeConflictCollection"/> each of whose members represents a discrepancy.
    /// </value>
    public ChangeConflictCollection ChangeConflicts { get; internal set; }
    /// <summary>
    /// Gets or sets a value indicating whether the LINQ to SharePoint provider should allow delay loading of <see cref="EntityRef{TEntity}"/> and <see cref="EntitySet{TEntity}"/> objects.
    /// </summary>
    /// <value>
    /// true if deferred loading enabled, otherwise, false.
    /// </value>
    public bool DeferredLoadingEnabled { get; set; }
    /// <summary>
    /// Gets or sets an object that will write the Collaborative Application Markup Language (CAML) query that results from the translation of the LINQ query.
    /// </summary>
    /// <value>
    /// A System.IO.TextWriter that can be called to write the CAML query.
    /// </value>
    public TextWriter Log { get; set; }
    /// <summary>
    /// Gets or sets a value that indicates whether changes in objects are tracked.
    /// </summary>
    /// <value>
    /// true, if changes are tracked; false otherwise.
    /// </value>
    public bool ObjectTrackingEnabled { get; set; }
    /// <summary>
    /// Gets the full URL of the Web site whose data is represented by the Microsoft.SharePoint.Linq.DataContext object.
    /// </summary>
    /// <value>
    /// A System.String that represents the full URL of the Web site.
    /// </value>
    public string Web { get { return m_ClientContext.Url; } }
    /// <summary>
    /// Gets the list.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="listName">Name of the list.</param>
    /// <returns></returns>
    public virtual EntityList<TEntity> GetList<TEntity>(string listName)
       where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      IEntityListItemsCollection _nwLst = GetOrCreateListEntry<TEntity>(listName);
      return new EntityList<TEntity>(this, (EntityListItemsCollection<TEntity>)_nwLst, listName, RowLimit);
    }
    /// <summary>
    /// Refreshes a collection of entities with the latest data from the content database according to the specified mode.
    /// </summary>
    /// <param name="mode">A value that specifies how to resolve differences between the current client values and the database values.</param>
    /// <param name="entities"> The entities that are refreshed.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Refresh(RefreshMode mode, IEnumerable entities) { throw new NotImplementedException(); }
    /// <summary>
    /// Refreshes the specified entity with the latest data from the content database according to the specified mode.
    /// </summary>
    /// <param name="mode">A value that specifies how to resolve differences between the current client values and the database values.</param>
    /// <param name="entity">The object that is refreshed.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Refresh(RefreshMode mode, object entity) { throw new NotImplementedException(); }
    /// <summary>
    /// Refreshes an array of entities with the latest data from the content database according to the specified mode.
    /// </summary>
    /// <param name="mode">A value that specifies how to resolve differences between the current client values and the database values.</param>
    /// <param name="entities">The entities that are refreshed.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Refresh(RefreshMode mode, params object[] entities) { throw new NotImplementedException(); }
    /// <summary>
    /// Enables continued reading and writing to an <see cref="EntityList{TEntity}"/>EntityList even after it has been renamed.
    /// </summary>
    /// <typeparam name="T">The type of the list items.</typeparam>
    /// <param name="newListName">The new name of the list.</param>
    /// <param name="oldListName">The old name of the list.</param>
    public void RegisterList<T>(string newListName, string oldListName) { throw new NotImplementedException(); }
    /// <summary>
    /// Enables continued reading and writing to an Microsoft.SharePoint.Linq.EntityList even after it has been moved to another Web site.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newListName">The new name of the list.</param>
    /// <param name="newWebUrl">The URL of the Web site to which the list was moved.</param>
    /// <param name="oldListName">The old name of the list.</param>
    public void RegisterList<T>(string newListName, string newWebUrl, string oldListName) { throw new NotImplementedException(); }
    /// <summary>
    /// Persists to the content database changes made by the current user to one
    /// or more lists using the specified failure mode; or, if a concurrency conflict
    /// is found, populates the Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    /// property.
    /// </summary>
    public void SubmitChanges()
    {
      foreach (IEntityListItemsCollection _elx in m_AllLists.Values)
        _elx.SubmitChanges(this.RetryExecuteQuery);
    }
    /// <summary>
    /// Persists to the content database changes made by the current user to one
    /// or more lists using the specified failure mode; or, if a concurrency conflict
    /// is found, populates the Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    /// property.
    /// </summary>
    /// <param name="failureMode">A value that specifies when a concurrency conflict should stop the attempt to persist changes and roll back any changes already made.</param>
    /// <exception cref="System.NotImplementedException">
    /// Microsoft.SharePoint.Linq.DataContext.ObjectTrackingEnabled is false- or
    ///-At least one conflict in Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    ///     from the last time Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges
    ///     was called is not yet resolved.
    ///     </exception>
    public void SubmitChanges(ConflictMode failureMode) { throw new NotImplementedException(); }
    /// <summary>
    /// Persists, to the content database, changes made by the current user to one
    /// or more lists using the specified failure mode and the specified indication
    /// of whether the versions of changed list items should be incremented; or,
    /// if a concurrency conflict is found, populates the Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    /// property.
    /// </summary>
    /// <param name="failureMode">A value that specifies when a concurrency conflict should stop the attempt
    /// to persist changes and roll back any changes already made.</param>
    /// <param name="systemUpdate">if set to true [system update].</param>
    /// <exception cref="System.NotImplementedException">Microsoft.SharePoint.Linq.DataContext.ObjectTrackingEnabled is false- or
    ///     -At least one conflict in Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    ///     from the last time Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges
    ///     was called is not yet resolved.</exception>
    public void SubmitChanges(ConflictMode failureMode, bool systemUpdate) { throw new NotImplementedException(); }
    /// <summary>
    /// Gets or sets the maximal number of items queried form the ShsrePoint.
    /// </summary>
    /// <value>
    /// The limit of the number of items returned by the query.
    /// </value>
    public int RowLimit { get; set; }
    #endregion

    #region internal
    internal void SubmitChanges(string listName)
    {
      if (m_AllLists.ContainsKey(listName))
        m_AllLists[listName].SubmitChanges(RetryExecuteQuery);
    }
    internal FieldLookupValue GetFieldLookupValue(string listName, Object entity)
    {
      return m_AllLists[listName].GetFieldLookupValue(entity);
    }
    internal TEntity GetFieldLookupValue<TEntity>(string listName, FieldLookupValue fieldLookupValue)
      where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      IEntityListItemsCollection _newList = GetOrCreateListEntry<TEntity>(listName);
      return (TEntity)_newList.GetFieldLookupValue(fieldLookupValue);
    }
    internal Dictionary<int, Object> GetListItemCollection<TEntity>(AssociationAttribute associationInfo, int m_Id)
      where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      IEntityListItemsCollection _newList = GetOrCreateListEntry<TEntity>(associationInfo.List);
      return _newList.GetEntitiesForReverseLookup(associationInfo.Name, m_Id);
    }
    internal void LoadListItem(ListItem listItem)
    {
      m_ClientContext.Load<ListItem>(listItem);
    }
    internal void GetListItemCollection(List list, CamlQuery query, Action<ListItemCollection> GetItems)
    {
      int _items = 0;
      ListItemCollectionPosition position = GetListItemCollectionPage(list, query, x => { ListItemCollection _ret = x; _items += _ret.Count; GetItems(x); });
      while (position != null)
      {
        query.ListItemCollectionPosition = position;
        position = GetListItemCollectionPage(list, query, x => { ListItemCollection _ret = x; _items += _ret.Count; GetItems(x); });
      };
      Trace(new ProgressChangedEventArgs(1, String.Format("Loaded Items = {0}.", _items)));
    }
    internal void LoadListItem<TEntity>(int fieldLookupValue, EntityListItemsCollection<TEntity> entityListItemsCollection)
       where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      GetListItemCollection(entityListItemsCollection.MyList,
                            CamlQueryDefinition.GetCAMLSelectedID(fieldLookupValue),
                            x => { foreach (ListItem _listItemx in x) entityListItemsCollection.Add(_listItemx); });
    }
    internal int GetIndex<TEntity>(AssociationAttribute associations, TEntity item)
      where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      IEntityListItemsCollection _newList = GetOrCreateListEntry<TEntity>(associations.List);
      return _newList.GetIndex(item);
    }
    #endregion

    #region IDisposing
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      // This object will be cleaned up by the Dispose method. 
      // Therefore, you should call GC.SupressFinalize to 
      // take this object off the finalization queue 
      // and prevent finalization code for this object 
      // from executing a second time.
      GC.SuppressFinalize(this);
    }

    //
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.  Dispose(bool disposing) executes in two distinct scenarios. 
    /// If disposing equals true, the method has been called directly or indirectly by a user's code. Managed and unmanaged resources
    /// can be disposed. If disposing equals false, the method has been called by the runtime from inside the finalizer 
    /// and you should not reference other objects. Only unmanaged resources can be disposed. 
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      // Check to see if Dispose has already been called. 
      if (!this.disposed)
      {
        // If disposing equals true, dispose all managed 
        // and unmanaged resources. 
        if (disposing)
        {
          // Dispose managed resources.
          m_ClientContext.Dispose();
        }
        // Note disposing has been done.
        disposed = true;
      }
    }
    // Use C# destructor syntax for finalization code. 
    // This destructor will run only if the Dispose method 
    // does not get called. 
    // It gives your base class the opportunity to finalize. 
    // Do not provide destructors in types derived from this class.
    /// <summary>
    /// Finalizes an instance of the <see cref="DataContext"/> class.
    /// </summary>
    ~DataContext()
    {
      // Do not re-create Dispose clean-up code here. 
      // Calling Dispose(false) is optimal in terms of 
      // readability and maintainability.
      Dispose(false);
    }
    // Track whether Dispose has been called. 
    private bool disposed = false;
    #endregion

    #region private
    //vars
    private ClientContext m_ClientContext = default(ClientContext);
    private Site m_site { get; set; }
    private Web m_RootWeb { get; set; }
    private Dictionary<string, IEntityListItemsCollection> m_AllLists = new Dictionary<string, IEntityListItemsCollection>();
    //methods
    private ListItemCollectionPosition GetListItemCollectionPage(List list, CamlQuery query, Action<ListItemCollection> GetItems)
    {
      int _retry = 0;
      bool _done = false;
      ListItemCollection _ListItemCollection = null;
      do
      {
        try
        {
          _ListItemCollection = _ListItemCollection = list.GetItems(query);
          m_ClientContext.Load<ListItemCollection>(_ListItemCollection, x => x.IncludeWithDefaultProperties(ct => ct.ContentType), y => y.ListItemCollectionPosition);
          // Execute the prepared command against the target ClientContext
          ExecuteQuery(this, new ProgressChangedEventArgs(1, String.Format("Loading ListItemCollection for list {0}.", list.Title)));
          _done = true;
        }
        catch (Exception _ex)
        {
          ReportException(ref _retry, _ex);
        }
      } while (!_done);
      GetItems(_ListItemCollection);
      return _ListItemCollection.ListItemCollectionPosition;
    }
    private void RetryExecuteQuery(ProgressChangedEventArgs args, Action prepareContext)
    {
      bool _done = false;
      int _retry = 0;
      do
      {
        try
        {
          prepareContext();
          ExecuteQuery(this, args);
          _done = true;
        }
        catch (Exception _ex)
        {
          ReportException(ref _retry, _ex);
        }
      } while (!_done);
    }
    private void ExecuteQuery(object sender, ProgressChangedEventArgs args)
    {
      m_ClientContext.ExecuteQuery();
      Trace(args);
    }
    private void ReportException(ref int _retry, Exception _ex)
    {
      _retry++;
      string _msg = string.Empty;
      if (_retry >= 7)
      {
        _msg = String.Format("Retry {3} - cannot execute SharePoint list query because of error {0}:{1} at {2}, too many tries, so I must break the operation.", _ex.GetType().Name, _ex.Message, _ex.Source, _retry);
        System.Diagnostics.EventLog.WriteEntry(CAS.SharePoint.Client.Properties.Settings.Default.EvenlLogSource, _msg, System.Diagnostics.EventLogEntryType.Error, 224, 0);
        throw _ex;
      }
      else
      {
        int _delay = CalculateDelay(_retry);
        _msg = String.Format("Retry {3} - cannot execute SharePoint list query because of error {0}:{1} at {2}, I mill do next try after {4} ms.", _ex.GetType().Name, _ex.Message, _ex.Source, _retry, _delay);
        System.Diagnostics.EventLog.WriteEntry(CAS.SharePoint.Client.Properties.Settings.Default.EvenlLogSource, _msg, System.Diagnostics.EventLogEntryType.Error, 224, 0);
        System.Threading.Thread.Sleep(_delay);
      };
      Trace(new ProgressChangedEventArgs(1, _msg));
    }
    private int CalculateDelay(int retry)
    {

      int _acum = 1;
      for (int _ii = 1; _ii <= retry; _ii++)
        _acum *= _ii;
      return _acum * 10;
    }
    private void Trace(ProgressChangedEventArgs e)
    {
      if (Log != null)
        Log.WriteLine(String.Format("{0}; {1}", "DataContext", e.UserState));
    }
    private IEntityListItemsCollection GetOrCreateListEntry<TEntity>(string listName)
      where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      Debug.Assert(!String.IsNullOrEmpty(listName), "listName null in GetOrCreateListEntry.");
      IEntityListItemsCollection _nwLst = null;
      if (!m_AllLists.TryGetValue(listName, out _nwLst))
      {
        List _list = null;
        // Execute the prepared commands against the target ClientContext
        RetryExecuteQuery(new ProgressChangedEventArgs(1, String.Format("Loading list = {0}", listName)),
                          () =>
                          {
                            _list = m_RootWeb.Lists.GetByTitle(listName);
                            m_ClientContext.Load<List>(_list);
                          }
                          );
        _nwLst = new EntityListItemsCollection<TEntity>(this, _list);
        m_AllLists.Add(listName, _nwLst);
      }
      else
        _nwLst = m_AllLists[listName];
      return _nwLst;
    }
    private void CreateContext(string requestUrl)
    {
      m_ClientContext = new ClientContext(requestUrl);
      m_site = m_ClientContext.Site;
      m_ClientContext.Load<Site>(m_site, s => s.Url);
      m_RootWeb = m_site.RootWeb;
      m_ClientContext.Load<Web>(m_RootWeb, w => w.Title);
      ExecuteQuery(this, new ProgressChangedEventArgs(1, String.Format("Loading Site={0}", requestUrl)));
      Trace(new ProgressChangedEventArgs(1, String.Format("Loaded site={0} Web={1}", m_site.Url, m_RootWeb.Title)));
    }
    #endregion

  }
}
