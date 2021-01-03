//<summary>
//  Title   : partial class PRContentType
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/SPExplorer/SPMetalParameters/PRContentType.cs $
//  $Id: PRContentType.cs 10838 2014-10-10 14:13:54Z mpostol $
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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace CAS.SharePoint.Tools.SPExplorer.SPMetalParameters
{
  /// <summary>
  /// partial class PRContentType
  /// </summary>
  public partial class PRContentType : ICheckable
  {

    #region public
    internal static List<PRContentType>
      CreatePRContentTypes(SPContentTypeCollection spContentTypes, Dictionary<Guid, PRList> listsDictionary, Action<ProgressChangedEventArgs> progress, Func<string, bool, bool> selected)
    {
      List<PRContentType> _prContentTypeList = new List<PRContentType>();
      int _ix = 0;
      foreach (SPContentType _spctx in spContentTypes)
      {
        progress(new ProgressChangedEventArgs(_ix * 100 / spContentTypes.Count, _spctx.Name));
        _ix++;
        _prContentTypeList.Add(PRContentType.CreatePRContentType(_spctx, listsDictionary, selected));
      }
      return _prContentTypeList;
    }
    /// <summary>
    /// Gets the type of the base content.
    /// </summary>
    /// <value>
    /// The type of the base content.
    /// </value>
    [System.Xml.Serialization.XmlIgnore()]
    public PRContentType BaseContentType { get; private set; }
    /// <summary>
    /// Gets the content type identifier.
    /// </summary>
    /// <value>
    /// The content type identifier <see cref="SPContentTypeId"/>.
    /// </value>
    [System.Xml.Serialization.XmlIgnore()]
    private SPContentTypeId SPContentTypeId { get; set; }
    /// <summary>
    /// Gets a <see cref="SPContentTypeId"/> for this content type identifier as string.
    /// </summary>
    /// <value>
    /// The content type identifier.
    /// </value>
    public string ContentTypeId { get { return this.SPContentTypeId.ToString(); } }
    internal PRContentType GetSPMetalParameters()
    {
      Debug.Assert(Checked);
      PRContentType _ret = new PRContentType
      {
        Class = Class,
        Column = this.Column.Where<PRColumn>(x => x.Checked).Select<PRColumn, PRColumn>(x => x.GetSPMetalParameters()).ToArray<PRColumn>(),
        ExcludeOtherColumns = new PRFlagElement(),
        Name = this.Name
      };
      return _ret;
    }
    #endregion

    #region ICheckable
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="PRContentType"/> is checked.
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
    #endregion

    #region private
    private static PRContentType CreatePRContentType(SPContentType contentType, Dictionary<Guid, PRList> listsDictionary, Func<string, bool, bool> selected)
    {
      List<PRColumn> _columns = PRColumn.CreatePRColumns(contentType.Fields.Cast<SPField>(), (name, type) => type.IsSPFieldTypeSupported(), listsDictionary);
      PRContentType _baseContentType = null;
      if (contentType.Id != SPBuiltInContentTypeId.System)
        _baseContentType = PRContentType.CreatePRContentType(contentType.Parent, listsDictionary, (x, y) => false);
      PRContentType _ret = new PRContentType()
      {
        Class = contentType.Name,
        Checked = selected(contentType.Group, contentType.Hidden),
        Column = _columns.ToArray<PRColumn>(),
        SPContentTypeId = contentType.Id,
        ExcludeColumn = null,
        ExcludeOtherColumns = null,
        IncludeHiddenColumns = null,
        Name = contentType.Name,
        BaseContentType = _baseContentType
      };
      return _ret;
    }
    #endregion
  }
}
