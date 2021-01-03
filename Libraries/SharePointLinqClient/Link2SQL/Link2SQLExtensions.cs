//_______________________________________________________________
//  Title   : public static class Link2SQLExtensions
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2015-01-28 15:08:14 +0100 (śr., 28 sty 2015) $
//  $Rev: 11268 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePointLinqClient/Link2SQL/Link2SQLExtensions.cs $
//  $Id: Link2SQLExtensions.cs 11268 2015-01-28 14:08:14Z mpostol $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.SharePoint.Client.Linq2SP;
using Microsoft.SharePoint.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;

namespace CAS.SharePoint.Client.Link2SQL
{
  /// <summary>
  /// Provides extensions to Linq to SQL
  /// </summary>
  public static class Link2SQLExtensions
  {
    /// <summary>
    /// Gets entity at <paramref name="index"/> if exist, otherwise returns null.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity </typeparam>
    /// <param name="table">The SQL table <see cref="Table{TEntity}"/>.</param>
    /// <param name="index">The index.</param>
    /// <returns>An instance of <typeparamref name="TEntity"/> at <paramref name="index"/> if exist, otherwise null.</returns>
    public static TEntity GetAt<TEntity>(this Table<TEntity> table, int index)
      where TEntity : class, IId
    {
      //cast is not supported by linq to SQL. see revision 10753
      IId _ret = table.Cast<IId>().Where<IId>(x => x.ID == index).FirstOrDefault<IId>();
      return _ret == null ? null : (TEntity)_ret;
    }
    /// <summary>
    /// Adds the log entry.
    /// </summary>
    /// <typeparam name="TTable">The type of the table items.</typeparam>
    /// <param name="log">A table of the log.</param>
    /// <param name="itemID">The item identifier.</param>
    /// <param name="listName">Name of the list.</param>
    /// <param name="userName">Name of the user.</param>
    public static void AddLog<TTable>(this Table<TTable> log, int itemID, string listName, string userName)
      where TTable : class, IArchivingLogs, new()
    {
      TTable _newItem = new TTable()
      {
        Date = DateTime.UtcNow,
        ItemID = itemID,
        ListName = listName,
        UserName = userName
      };
      log.InsertOnSubmit(_newItem);
    }
    /// <summary>
    /// Deletes the specified list.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TVersion">The type of the version.</typeparam>
    /// <param name="list">The list.</param>
    /// <param name="entities">The entities.</param>
    /// <param name="toBeMarkedAsArchival">To be marked as archival.</param>
    /// <param name="getSQLEntity">The delegate to get SQL entity.</param>
    /// <param name="addLog">The delegate to add new entry to the log.</param>
    /// <param name="addHistoryEntry">The delegate to be used while new history entry is to be added.</param>
    /// <exception cref="System.ArgumentNullException">Delete operation does not support null value for entities parameter.</exception>
    public static void Delete<TEntity, TVersion>
      (
        this EntityList<TEntity> list,
        IEnumerable<TEntity> entities,
        IEnumerable<IArchival> toBeMarkedAsArchival,
        Func<int, IItem> getSQLEntity,
        Action<int, string> addLog,
        Action<TVersion> addHistoryEntry
      )
      where TEntity : class, Linq2SP.ISPItem, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
      where TVersion : class, IHistory, new()
    {
      if (entities == null)
        throw new ArgumentNullException("Delete operation does not support null value for entities parameter.");
      foreach (TEntity _item in entities)
      {
        IItem _sqlItem = getSQLEntity(_item.Id.Value);
        if (_sqlItem != null)
          _sqlItem.OnlySQL = true;
        addLog(_item.Id.Value, list.Name);
        list.DeleteOnSubmit(_item);
      }
      if (toBeMarkedAsArchival == null)
        return;
      foreach (IArchival _ax in toBeMarkedAsArchival)
      {
        if (_ax == null)
          continue;
        _ax.Archival = true;
      }
    }
  }
}
