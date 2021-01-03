//<summary>
//  Title   : CAS.SharePoint.Client.CAML.CamlQueryDefinition
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-09-09 11:17:04 +0200 (wt., 09 wrz 2014) $
//  $Rev: 10772 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/CAML/CamlQueryDefinition.cs $
//  $Id: CamlQueryDefinition.cs 10772 2014-09-09 09:17:04Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using Microsoft.SharePoint.Client;

namespace CAS.SharePoint.Client.CAML
{
  /// <summary>
  /// CAML Common Definition 
  /// </summary>
  internal static class CamlQueryDefinition
  {
    internal static CamlQuery CAMLAllItemsQuery(int rowLimit)
    {
      return new CamlQuery() { ViewXml = string.Format(CAMLAllItemsQueryString, rowLimit) };
    }
    internal static CamlQuery GetCAMLSelectedID(int value)
    {
      return new CamlQuery() { ViewXml = string.Format(CAMLQueryString, value, ColumnNameId, CAMLTypeNumber, string.Empty, 5) };
    }
    internal static CamlQuery GetCAMLSelectedLookup(int value, string fieldName, int rowLimit)
    {
      return new CamlQuery() { ViewXml = string.Format(CAMLQueryString, value, fieldName, CAMLTypeLookup, m_CAMLLookupId, rowLimit) };
    }
    internal static string CAMLTypeNumber = "Number";
    internal static string CAMLTypeText = "Text";
    internal static string CAMLTypeLookup = "Lookup";
    private const string ColumnNameId = "ID";
    private static string m_CAMLLookupId = " LookupId='TRUE' ";
    private static string CAMLQueryString = @"
      <View>
        <Query>
          <Where>
            <Eq>
              <FieldRef Name='{1}'{3}></FieldRef>
              <Value Type='{2}'>{0}</Value>
            </Eq>
          </Where>
        </Query>
        <RowLimit>{4}</RowLimit>
      </View>";
    private static string CAMLAllItemsQueryString = @"
      <View>
        <RowLimit>{0}</RowLimit>
      </View>";
  }
}
