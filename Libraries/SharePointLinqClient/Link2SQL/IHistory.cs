//<summary>
//  Title   : public interface IHistory
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2014-09-05 20:47:03 +0200 (pt., 05 wrz 2014) $
//  $Rev: 10768 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/Link2SQL/IHistory.cs $
//  $Id: IHistory.cs 10768 2014-09-05 18:47:03Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.SharePoint.Client.Link2SQL
{
  /// <summary>
  /// Interface representing properties in the History list.
  /// </summary>
  public interface IHistory
  {
    /// <summary>
    /// Gets or sets the name of the list.
    /// </summary>
    /// <value>
    /// The name of the list.
    /// </value>
    string ListName { get; set; }
    /// <summary>
    /// Gets or sets the item identifier.
    /// </summary>
    /// <value>
    /// The item identifier.
    /// </value>
    int ItemID { get; set; }
    /// <summary>
    /// Gets or sets the name of the field.
    /// </summary>
    /// <value>
    /// The name of the field.
    /// </value>
    string FieldName { get; set; }
    /// <summary>
    /// Gets or sets the field value.
    /// </summary>
    /// <value>
    /// The field value.
    /// </value>
    string FieldValue { get; set; }
    /// <summary>
    /// Gets or sets the <see cref="System.DateTime"/> of last modification.
    /// </summary>
    /// <value>
    /// The modified.
    /// </value>
    System.DateTime Modified { get; set; }
    /// <summary>
    /// Gets or sets the author of last modification.
    /// </summary>
    /// <value>
    /// The author of the last modification.
    /// </value>
    string ModifiedBy { get; set; }
  }
}
