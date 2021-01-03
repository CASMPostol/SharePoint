//<summary>
//  Title   : public partial class PRList
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/SPExplorer/SPMetalParameters/PRList.cs $
//  $Id: PRList.cs 10838 2014-10-10 14:13:54Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.ComponentModel;
using CAS.SharePoint.Tools.SPExplorer.SPMetalParameters.SQL;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CAS.SharePoint.Tools.SPExplorer.SPMetalParameters
{

  /// <summary>
  /// <see cref="PRList"/> is a SP Metal Object Model representation of the <see cref="SPList"/>
  /// </summary>
  public partial class PRList : ICheckable
  {

    #region creators
    /// <summary>
    /// Initializes a new instance of the <see cref="PRList"/> class.
    /// </summary>
    public PRList()
    {
      Member = "IsProxy";
      ExcludeContentType = null;
      Type = null;
      IsProxy = true;
    }
    internal static PRList[] CreateLists
      (SPListCollection listsCollection, Dictionary<Guid, PRList> listsDictionary, Action<ProgressChangedEventArgs> progress, Func<string, bool, bool> selected)
    {
      List<PRList> _lists = new List<PRList>();
      int _count = 0;
      foreach (SPList _lix in listsCollection)
      {
        progress(new ProgressChangedEventArgs(_count * 100 / listsCollection.Count, _lix.Title));
        _count++;
        _lists.Add(PRList.Create(_lix, listsDictionary, x => { }, selected));
      }
      return _lists.ToArray<PRList>();
    }
    internal static PRList Create(Guid listGuid, Dictionary<Guid, PRList> listsDictionary)
    {
      if (listGuid.Equals(Guid.Empty))
        return null;
      PRList _ret = new PRList();
      listsDictionary.Add(listGuid, _ret);
      return _ret;
    }
    #endregion

    #region public API
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="PRList"/> is checked.
    /// </summary>
    /// <value>
    ///   <c>true</c> if checked; otherwise, <c>false</c>.
    /// </value>
    [System.Xml.Serialization.XmlIgnore()]
    public bool Checked
    {
      get;
      set;
    }
    /// <summary>
    /// Gets the list unique identifier.
    /// </summary>
    /// <value>
    /// The list unique identifier.
    /// </value>
    [System.Xml.Serialization.XmlIgnore()]
    internal Guid ListGuid { get; private set; }
    internal void AddList2ListListDescriptor
      (List<ListDescriptor> ListDescriptorsList, Dictionary<Guid, ListDescriptor> listsDictionary, PRColumn[] commonColumns)
    {
      try
      {
        if (listsDictionary.ContainsKey(this.ListGuid) || this.IsProxy || !this.Checked)
          return;
        if (m_recursive)
          throw new ApplicationException(); //TODO 
        m_recursive = true;
        UniqueColumns _columns = new UniqueColumns(CreateColumnDescriptors(commonColumns, commonColumns, ListDescriptorsList, listsDictionary));
        ListDescriptor _ListDescriptor = new ListDescriptor(this.Name);
        listsDictionary.Add(this.ListGuid, _ListDescriptor);
        foreach (PRContentType _clx in ContentType)
          _columns.AddRange(CreateColumnDescriptors(_clx.Column, commonColumns, ListDescriptorsList, listsDictionary));
        _ListDescriptor.Columns = _columns.ToArray();
        ListDescriptorsList.Add(_ListDescriptor);
      }
      finally
      {
        m_recursive = false;
      }
    }
    internal bool IsProxy { get; private set; }
    internal PRList GetSPMetalParameters()
    {
      return new PRList()
      {
        ContentType = this.ContentType.Where<PRContentType>(x => x.Checked).Select<PRContentType, PRContentType>(x => x.GetSPMetalParameters()).ToArray<PRContentType>(),
        Member = this.Member,
        Name = this.Name,
        Type = this.Type
      };
    }
    #endregion

    #region private
    private class UniqueColumns
    {
      private Dictionary<string, RegularColumn> m_Dictionary = new Dictionary<string, RegularColumn>();
      public UniqueColumns(IEnumerable<RegularColumn> enumerable)
      {
        AddRange(enumerable);
      }
      internal void AddRange(IEnumerable<RegularColumn> enumerable)
      {
        foreach (RegularColumn _clmx in enumerable)
        {
          if (m_Dictionary.ContainsKey(_clmx.ColumnName))
            continue;
          m_Dictionary.Add(_clmx.ColumnName, _clmx);
        };
      }
      internal RegularColumn[] ToArray()
      {
        return m_Dictionary.Values.OrderBy<RegularColumn, string>(key => key.ColumnName).ToArray<RegularColumn>();
      }
    }
    private static PRList Create(SPList list, Dictionary<Guid, PRList> listsDictionary, Action<ProgressChangedEventArgs> progress, Func<string, bool, bool> selected)
    {
      PRList _ret;
      if (listsDictionary.ContainsKey(list.ID))
        _ret = listsDictionary[list.ID];
      else
      {
        if (list.ID.Equals(Guid.Empty))
          throw new ArgumentOutOfRangeException("The list Guid should not be empty");
        _ret = new PRList();
        listsDictionary.Add(list.ID, _ret);
      }
      //Update
      _ret.ListGuid = list.ID;
      _ret.Name = list.Title;
      _ret.Member = list.Title;
      _ret.ContentType = PRContentType.CreatePRContentTypes(list.ContentTypes, listsDictionary, x => { }, (x, y) => false).ToArray<PRContentType>();
      _ret.Checked = !list.ContentTypes.Cast<SPContentType>().Select<SPContentType, bool>(x => selected(x.Group, x.Hidden)).Any<bool>(x => !x);
      _ret.ContentTypesID = list.ContentTypes.Cast<SPContentType>().Select<SPContentType, SPContentTypeId>(_ctx => _ctx.Id).ToArray<SPContentTypeId>();
      _ret.IsProxy = false;
      return _ret;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="PRList" /> class.
    /// </summary>
    /// <param name="columns">The columns.</param>
    /// <param name="commonColumns">The common columns.</param>
    /// <param name="listDescriptorsList">The list descriptors list.</param>
    /// <param name="listsDictionary">The lists dictionary.</param>
    /// <returns></returns>
    private IEnumerable<RegularColumn> CreateColumnDescriptors
      (IEnumerable<PRColumn> columns, PRColumn[] commonColumns, List<ListDescriptor> listDescriptorsList, Dictionary<Guid, ListDescriptor> listsDictionary)
    {
      List<RegularColumn> _regularColums = new List<RegularColumn>();
      foreach (PRColumn _clmx in columns)
      {
        if (_clmx.PivateKeyColumn)
          _regularColums.Add(new RegularColumn(ColumnType.PrivateKey, _clmx.Member, false, _clmx.SQLDataType, _clmx.SQLPrecision));
        else if (!_clmx.Checked)
          continue;
        else if (_clmx.LookupColumn && _clmx.ListLookup.Checked && !_clmx.ListLookup.IsProxy)
        {
          _clmx.ListLookup.AddList2ListListDescriptor(listDescriptorsList, listsDictionary, commonColumns);
          ListDescriptor _dest = listsDictionary[_clmx.ListLookup.ListGuid];
          _regularColums.Add(new ForeignKeyColumn(_clmx.Member, _clmx.SQLNullable, _clmx.SQLDataType)
          {
            ReferencedTableName = _dest.SQLTableName,
            ReferencedColumn = _dest.PFColumnName,
          });
        }
        else
          _regularColums.Add(new RegularColumn(ColumnType.Regular, _clmx.Member, _clmx.SQLNullable, _clmx.SQLDataType, _clmx.SQLPrecision));
      }
      return _regularColums;
    }
    private SPContentTypeId[] ContentTypesID { get; set; }
    private bool m_recursive = false;
    #endregion

  }

  internal static class PRListExtension
  {
    internal static string GetName(this RegularColumn valaue)
    {
      return valaue == null ? "-- Unknown -- value " : valaue.ColumnName;
    }
  }

}
