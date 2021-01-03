//<summary>
//  Title   : class SQLStorageItem
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2015-01-23 11:55:00 +0100 (pt., 23 sty 2015) $
//  $Rev: 11254 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/Link2SQL/SQLStorageItem.cs $
//  $Id: SQLStorageItem.cs 11254 2015-01-23 10:55:00Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Client.Reflection;
using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;

namespace CAS.SharePoint.Client.Link2SQL
{
  /// <summary>
  /// SQL Storage Item class providing information and functionality to update SQL entity class.
  /// </summary>
  internal class SQLStorageItem
  {

    #region public
    /// <summary>
    /// Assigns the specified value provided by the <see cref="Linq2SP.StorageItem.Descriptor"/>.
    /// </summary>
    /// <param name="descriptor">The descriptor.</param>
    /// <param name="sqlItem">The SQL item.</param>
    internal void Assign(Linq2SP.StorageItem.Descriptor descriptor, object sqlItem)
    {
      object _value = descriptor.Value;
      if ((descriptor.Destination == Linq2SP.StorageItem.Destination.Lookup) && (int)descriptor.Value <= 0)
        _value = new Nullable<int>();//TODO remove RepositoryDataSet.Repository[descriptor.Name][(int)descriptor.Value];
      Info.SetValue(sqlItem, _value, null);
    }
    /// <summary>
    /// Fills up storage information dictionary.
    /// </summary>
    /// <param name="type">The type to be he analyzed.</param>
    /// <param name="mapping">The properties names mapping.</param>
    internal static Dictionary<string, SQLStorageItem> CreateStorageDescription(Type type, Dictionary<string, string> mapping)
    {
      Dictionary<string, SQLStorageItem> _ret = new Dictionary<string, SQLStorageItem>();
      FillUpStorageInfoDictionary(type, mapping, _ret);
      return _ret;
    }
    internal string Name { get; private set; }
    #endregion

    #region private
    private static void FillUpStorageInfoDictionary(Type type, Dictionary<string, string> mapping, Dictionary<string, SQLStorageItem> storage)
    {
      Dictionary<string, MemberInfo> _mmbrs = type.GetDictionaryOfMembers();
      foreach (MemberInfo _ax in from _pidx in _mmbrs where _pidx.Value.MemberType == MemberTypes.Property select _pidx.Value)
      {
        if (mapping.ContainsKey(_ax.Name) && String.IsNullOrEmpty(mapping[_ax.Name]))
          continue;
        foreach (Attribute _cax in _ax.GetCustomAttributes(false))
        {
          AssociationAttribute _ass = _cax as AssociationAttribute;
          if (_ass != null)
            continue;
          SQLStorageItem _newStorageItem = new SQLStorageItem(_ax.Name, _ax as PropertyInfo);
          string key = _ax.Name;
          if (mapping.ContainsKey(key))
            key = mapping[key];
          storage.Add(key, _newStorageItem);
          if (type.BaseType != typeof(Object))
            FillUpStorageInfoDictionary(type.BaseType, mapping, storage);
        }
      }
    }
    private SQLStorageItem(string name, PropertyInfo property)
    {
      Info = property;
      Name = name;
    }
    private PropertyInfo Info { get; set; }
    #endregion

  }
}
