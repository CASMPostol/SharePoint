//_______________________________________________________________
//  Title   : class Synchronizer
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2015-01-23 11:55:00 +0100 (pt., 23 sty 2015) $
//  $Rev: 11254 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePointLinqClient/SP2SQLInteroperability/Synchronizer.cs $
//  $Id: Synchronizer.cs 11254 2015-01-23 10:55:00Z mpostol $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.SharePoint.Client.Link2SQL;
using CAS.SharePoint.Client.Linq2SP;
using CAS.SharePoint.Client.Reflection;
using Microsoft.SharePoint.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;

namespace CAS.SharePoint.Client.SP2SQLInteroperability
{
  /// <summary>
  /// Class Synchronizer - helper methods to synchronize SQL database with the SharePoint website content.
  /// </summary>
  public static class Synchronizer
  {
    /// <summary>
    /// Synchronizes the specified SQL table with the SharePoint list.
    /// </summary>
    /// <typeparam name="TSQL">The type of the SQL <see cref="Table{TSQL}"/>.</typeparam>
    /// <typeparam name="TSP">The type of the TSP.</typeparam>
    /// <param name="sqlTable">The SQL table.</param>
    /// <param name="spList">The sp list.</param>
    /// <param name="progressChanged">The progress changed.</param>
    /// <param name="mapping">The mapping.</param>
    /// <param name="port2210">if set to <c>true</c> [port2210].</param>
    public static void Synchronize<TSQL, TSP>(Table<TSQL> sqlTable, EntityList<TSP> spList, ProgressChangedEventHandler progressChanged, Dictionary<string, string> mapping, bool port2210)
      where TSQL : class, Link2SQL.IItem, INotifyPropertyChanged, new()
      where TSP : class, Linq2SP.ISPItem, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Reading the table {0} from SharePoint website.", typeof(TSP).Name)));
      List<TSP> _scrList = spList.ToList<TSP>();
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Reading the table {0} from SQL database.", typeof(TSQL).Name)));
      Dictionary<int, IItem> _dictinary = sqlTable.Where<TSQL>(x => !x.OnlySQL).ToDictionary<TSQL, int, IItem>(x => x.ID, y => (SharePoint.Client.Link2SQL.IItem)y);
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Synchronization {0} elements in the SharePoint source tables with the {1} element in the SQL table.", _scrList.Count, _dictinary.Count)));
      //create descriptors using reflection
      Dictionary<string, StorageItemsList> _spDscrpt = StorageItem.CreateStorageDescription(typeof(TSP));
      Dictionary<string, SQLStorageItem> _sqlDscrpt = SQLStorageItem.CreateStorageDescription(typeof(TSQL), mapping);
      int _archivalCount = 0;
      int _archivalCountOld = 0;
      int _counter = 0;
      int _counterOld = 0;
      int _itemChanges = 0;
      int _itemChangesOld = 0;
      bool _itemChanged;
      Action<TSP> _port = x => { };
      List<string> _changes = new List<string>();
      if (port2210 && typeof(IArchival).IsAssignableFrom(typeof(TSP)))
      {
        _port = x => { ((IArchival)x).Archival = false; _archivalCount++; };
        progressChanged(1, new ProgressChangedEventArgs(1, "The table will be updated new software version"));
      }
      int _loopCounter = 0;
      foreach (TSP _spItem in _scrList)
      {
        _port(_spItem);
        SharePoint.Client.Link2SQL.IItem _sqlItem = default(SharePoint.Client.Link2SQL.IItem);
        if (!_dictinary.TryGetValue(_spItem.Id.Value, out _sqlItem))
        {
          _sqlItem = new TSQL();
          _sqlItem.OnlySQL = false;
          _dictinary.Add(_spItem.Id.Value, _sqlItem);
          sqlTable.InsertOnSubmit((TSQL)_sqlItem);
        }
        Microsoft.SharePoint.Linq.ContentTypeAttribute _attr = _spItem.GetType().GetContentType();
        _itemChanged = false;
        Synchronize<TSQL, TSP>(
          (TSQL)_sqlItem,
          _spItem, _spDscrpt[_attr.Id].Values,
          _sqlDscrpt, progressChanged,
          spList.DataContext,
          (x, y) =>
          {
            _counter++;
            _changes.Add(y.PropertyName);
            _itemChanged = true;
          });
        if (_itemChanged)
          _itemChanges++;
        _loopCounter++;
        if (_loopCounter > 10000)
        {
          progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Intermediate: submitting {0} changes on {1} list rows to SQL database", _counter - _counterOld, _itemChanges - _itemChangesOld)));
          sqlTable.Context.SubmitChanges();
          if (_archivalCount > 0)
          {
            progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Intermediate: update to Rel 2.10 Submitting {0} Archival bit changes", _archivalCount - _archivalCountOld)));
            spList.DataContext.SubmitChanges();
          }
          _loopCounter = 0;
          _counterOld = _counter;
          _itemChangesOld = _itemChanges;
        }
      }
      progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Submitting total {0} changes on {1} list rows to SQL database", _counter, _itemChanges)));
      sqlTable.Context.SubmitChanges();
      if (_archivalCount > 0)
      {
        progressChanged(1, new ProgressChangedEventArgs(1, String.Format("Update to Rel 2.10 Submitting total {0} Archival bit changes", _archivalCount)));
        spList.DataContext.SubmitChanges();
      }
    }
    private static void Synchronize<TSQL, TSP>
      (
          TSQL sqlItem,
          TSP splItem,
          IEnumerable<StorageItem> _spDscrpt,
          Dictionary<string, SQLStorageItem> _sqlDscrpt,
          ProgressChangedEventHandler progressChanged,
          Microsoft.SharePoint.Linq.DataContext dataContext,
          PropertyChangedEventHandler propertyChanged
      )
        where TSQL : class, SharePoint.Client.Link2SQL.IItem, INotifyPropertyChanged, new()
    {
      sqlItem.PropertyChanged += propertyChanged;
      foreach (StorageItem _si in _spDscrpt.Where<StorageItem>(x => !x.IsReverseLookup()))
        if (_sqlDscrpt.ContainsKey(_si.PropertyName))
          _si.GetValueFromEntity(splItem, x => _sqlDscrpt[_si.PropertyName].Assign(x, sqlItem));
      sqlItem.PropertyChanged -= propertyChanged;
    }

#if DEBUG
    /// <summary>
    /// Tests to dictionary.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <param name="expectedCount">The expected count.</param>
    /// <returns>System.Int32.</returns>
    public static int TestToDictionary<TEntity>(int expectedCount)
    {
      Linq2SP.StorageItemsList _storageList = Linq2SP.StorageItem.CreateStorageDescription(typeof(TEntity), false);
      return  _storageList.Count();
    }
    /// <summary>
    /// Compares the content of the selected storages.
    /// </summary>
    /// <typeparam name="TSP">The type of the TSP.</typeparam>
    /// <typeparam name="TSQL">The type of the TSQL.</typeparam>
    /// <param name="mapping">The mapping.</param>
    /// <exception cref="System.ApplicationException">
    /// </exception>
    public static void CompareSelectedStoragesContent<TSP, TSQL>(Dictionary<string, string> mapping)
    {
      //Get SP stage info
      StorageItemsList _storageListSP = Linq2SP.StorageItem.CreateStorageDescription(typeof(TSP), false);
      //Get SQL stage info
      Dictionary<string, Link2SQL.SQLStorageItem> _storageListSQLDictionary = Link2SQL.SQLStorageItem.CreateStorageDescription(typeof(TSQL), mapping);
      //Assert.AreEqual<int>(_storageSPDictionary.Count, _storageListSQL.Count, String.Format("Storage length of {0} must be equal, if not loss of data may occur", typeof(TSP).Name));
      foreach (string _item in _storageListSQLDictionary.Keys)
        if (!_storageListSP.ContainsKey(_item))
          throw new ApplicationException(String.Format("Storage SP of {1} does not contain property {0}", _item, typeof(TSP).Name));
      foreach (string _item in _storageListSP.Keys)
      {
        if (!_storageListSQLDictionary.ContainsKey(_item))
          throw new ApplicationException(String.Format("Storage SQL of {1} does not contain property {0}", _item, typeof(TSQL).Name));
      }
    }
#endif

  }
}
