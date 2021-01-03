//<summary>
//  Title   :  interface IArchivingLogs
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2014-09-05 20:47:03 +0200 (pt., 05 wrz 2014) $
//  $Rev: 10768 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/Link2SQL/IArchivingLogs.cs $
//  $Id: IArchivingLogs.cs 10768 2014-09-05 18:47:03Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.SharePoint.Client.Link2SQL
{
  /// <summary>
  /// Interface providing properties used to add entry to the log table.
  /// </summary>
  public interface IArchivingLogs
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
    /// Gets or sets the date.
    /// </summary>
    /// <value>
    /// The date.
    /// </value>
    System.DateTime Date { get; set; }
    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>
    /// The name of the user.
    /// </value>
    string UserName { get; set; }
  }
}
