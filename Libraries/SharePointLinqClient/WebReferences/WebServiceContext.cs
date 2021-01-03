//<summary>
//  Title   : class VersioningInformation 
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-11-28 11:47:47 +0100 (pt., 28 lis 2014) $
//  $Rev: 11006 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePointLinqClient/WebReferences/WebServiceContext.cs $
//  $Id: WebServiceContext.cs 11006 2014-11-28 10:47:47Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Client.WebReferences.ServiceModel;
using System;
using System.Collections.Generic;
using System.Xml;

namespace CAS.SharePoint.Client.WebReferences
{
  /// <summary>
  /// Class used to get access to web services defined by the SharePoint.
  /// </summary>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
  public class WebServiceContext
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="WebServiceContext"/> class.
    /// </summary>
    /// <param name="url">The URL.</param>
    internal WebServiceContext(string url)
    {
      m_Lists = new Lists();
      m_Lists.Url = url;
      m_Lists.Credentials = System.Net.CredentialCache.DefaultCredentials;
    }
    internal List<Version> GetVersionDescriptor(string strlistID, int strlistItemID, List<string> strFieldNames)
    {
      List<Version> history = new List<Version>();
      foreach (string _fnx in strFieldNames)
        GetVersionDescriptor(history, strlistID, strlistItemID, _fnx);
      history.Sort((x, y) => x.Modified.CompareTo(y.Modified));
      return history;
    }
    /// <summary>
    /// Gets the version descriptor.
    /// </summary>
    /// <param name="history">The history.</param>
    /// <param name="strlistID">The list identifier.</param>
    /// <param name="listItemID">The list item identifier.</param>
    /// <param name="strFieldName">Name of the string field.</param>
    /// <returns>
    /// Returns lists of <see cref="Version" /> objects describing all the selected item versions.
    /// </returns>
    private void GetVersionDescriptor(List<Version> history, string strlistID, int listItemID, string strFieldName)
    {
      if (strFieldName.Contains("Modified"))
        return;
      XmlNode _response = m_Lists.GetVersionCollection(strlistID, Convert.ToString(listItemID), strFieldName);
      foreach (XmlNode _version in _response.ChildNodes)
        history.Add(new Version(strlistID, listItemID, strFieldName, _version));
    }
    private Lists m_Lists = null;

  }
}
