//<summary>
//  Title   : public partial class PRColumn
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Tools/SPExplorer/SPMetalParameters/PRColumn.cs $
//  $Id: PRColumn.cs 10838 2014-10-10 14:13:54Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.ComponentModel;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CAS.SharePoint.Tools.SPExplorer.SPMetalParameters
{
  /// <summary>
  /// PRColumn is a SP Metal Object Model representation of the <see cref="SPField"/>
  /// </summary>
  public partial class PRColumn : ICheckable
  {

    #region public API
    /// <summary>
    /// Creates the lists of columns descriptors <see cref="PRColumn" />.
    /// </summary>
    /// <param name="sPFieldCollection">The s p field collection.</param>
    /// <param name="selected">The selected.</param>
    /// <param name="listsDictionary">The lists dictionary.</param>
    internal static List<PRColumn> CreatePRColumns(IEnumerable<SPField> sPFieldCollection, Func<string, SPFieldType, bool> selected, Dictionary<Guid, PRList> listsDictionary)
    {
      List<PRColumn> _ret = new List<PRColumn>();
      foreach (SPField _SPFieldX in sPFieldCollection)
      {
        PRColumn _PRColumn = CreatePRColumn(_SPFieldX, listsDictionary, selected);
        if (_PRColumn == null)
          continue;
        _ret.Add(_PRColumn);
      }
      _ret.Sort((x, y) => { return x.Name.CompareTo(y.Name); });
      return _ret;
    }
    /// <summary>
    /// Gets or sets the type of the field.
    /// </summary>
    /// <value>
    /// The type of the field.
    /// </value>
    [System.Xml.Serialization.XmlIgnore()]
    public SPFieldType FieldType { get; set; }
    /// <summary>
    /// Gets the list lookup.
    /// </summary>
    /// <value>
    /// The list lookup.
    /// </value>
    [System.Xml.Serialization.XmlIgnore()]
    internal PRList ListLookup { get; private set; }
    /// <summary>
    /// Gets the type of the SQL data.
    /// </summary>
    /// <value>
    /// The type of the SQL data.
    /// </value>
    [System.Xml.Serialization.XmlIgnore()]
    public string SQLDataType { get; private set; }
    /// <summary>
    /// Gets a value indicating whether the value is nullable in the database.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the value is nullable in the database; otherwise, <c>false</c>.
    /// </value>
    [System.Xml.Serialization.XmlIgnore()]
    public bool SQLNullable { get; private set; }
    /// <summary>
    /// Gets the SQL precision.
    /// </summary>
    /// <value>
    /// The SQL precision.
    /// </value>
    [System.Xml.Serialization.XmlIgnore()]
    public int SQLPrecision { get; private set; }
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
      if (ListLookup == null)
        return String.Format("{0} of type: {1}", this.Name, FieldType);
      else
        return String.Format("{0} of type: {1} to {2}", this.Name, FieldType, ListLookup.Name);
    }
    /// <summary>
    /// Gets a value indicating whether it is the lookup column.
    /// </summary>
    /// <value>
    ///   <c>true</c> if it the lookup column; otherwise, <c>false</c>.
    /// </value>
    [System.Xml.Serialization.XmlIgnore()]
    public bool LookupColumn { get { return FieldType == SPFieldType.Lookup; } }
    /// <summary>
    /// Gets a value indicating whether it is private key column.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this column is the private key column; otherwise, <c>false</c>.
    /// </value>
    [System.Xml.Serialization.XmlIgnore()]
    public bool PivateKeyColumn { get { return FieldType == SPFieldType.Counter; } }
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="PRColumn"/> is checked and should be processed.
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
    internal PRColumn GetSPMetalParameters()
    {
      return new PRColumn()
      {
        FieldType = this.FieldType,
        Member = this.Member,
        Name = this.Name,
        Type = this.Type,
        TypeSpecified = this.TypeSpecified
      };
    }
    #endregion

    #region private
    //http://msdn.microsoft.com/en-us/library/office/ee535721(v=office.15).aspx
    private static PRColumn CreatePRColumn(SPField sPField, Dictionary<Guid, PRList> listsDictionary, Func<string, SPFieldType, bool> selected)
    {
      SPFieldLookup _spFieldLookup = sPField as SPFieldLookup;
      PRList _lookupList = null;
      if (_spFieldLookup != null)
      {
        if (_spFieldLookup.IsDependentLookup)
          return null;
        Guid _lookupListid = _spFieldLookup.LookupList.GuidTryParse();
        if (_lookupListid.Equals(Guid.Empty))
          return null;
        if (listsDictionary.ContainsKey(_lookupListid))
          _lookupList = listsDictionary[_lookupListid];
        else
          _lookupList = PRList.Create(_lookupListid, listsDictionary);
        Debug.Assert(_lookupList != null);
      }
      else
        Debug.Assert(sPField.Type != SPFieldType.Lookup);
      PRColumn _PRColumn = new PRColumn()
      {
        Checked = selected(sPField.InternalName, sPField.Type),
        Member = sPField.InternalName.SQLName(),
        Name = sPField.InternalName,
        Type = PRType.Enum, //http://msdn.microsoft.com/en-us/library/office/ee536245(v=office.15).aspx
        TypeSpecified = false,
        FieldType = sPField.Type,
        ListLookup = _lookupList,
        SQLDataType = sPField.Type.SQLDataType(),
        SQLNullable = !sPField.Required,
        SQLPrecision = -1,
      };
      return _PRColumn;
    }
    #endregion


  }
}
