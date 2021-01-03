//<summary>
//  Title   : public class Version
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-09-01 15:16:13 +0200 (pon., 01 wrz 2014) $
//  $Rev: 10757 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/WebReferences/Version.cs $
//  $Id: Version.cs 10757 2014-09-01 13:16:13Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CAS.SharePoint.Client.WebReferences
{
  /// <summary>
  /// Provides information returned by the <see cref="WebServiceContext"/>
  /// </summary>
  public class Version
  {
    /// <summary>
    /// Gets the name of the list.
    /// </summary>
    /// <value>
    /// The name of the list.
    /// </value>
    public string ListName { get; private set; }
    /// <summary>
    /// Gets the item identifier.
    /// </summary>
    /// <value>
    /// The item identifier.
    /// </value>
    public int ItemID { get; private set; }
    /// <summary>
    /// Gets the name of the column.
    /// </summary>
    /// <value>
    /// The name of the column.
    /// </value>
    public string ColumnName { get; private set; }
    /// <summary>
    /// Gets the column value.
    /// </summary>
    /// <value>
    /// The value of the column.
    /// </value>
    public string Value { get; private set; }
    /// <summary>
    /// Gets the date of modification.
    /// </summary>
    /// <value>
    /// The modification date and time.
    /// </value>
    public DateTime Modified { get; private set; }
    /// <summary>
    /// Gets the editor.
    /// </summary>
    /// <value>
    /// The editor.
    /// </value>
    public string Editor { get; private set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="Version"/> class.
    /// </summary>
    /// <param name="listName">Name of the list.</param>
    /// <param name="itemID">The item identifier.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="value">The value.</param>
    public Version(string listName, int itemID, string columnName, XmlNode value)
    {
      ListName = listName;
      ColumnName = columnName;
      ItemID = itemID;
      foreach (XmlAttribute _attr in value.Attributes)
      {
        switch (_attr.LocalName)
        {
          case "Modified":
            Modified = XmlConvert.ToDateTime(_attr.Value, XmlDateTimeSerializationMode.Utc);
            break;
          case "Editor":
            Editor = _attr.Value;
            break;
          default:
            Value = _attr.Value;
            break;
        }
      }
    }
  }
}
