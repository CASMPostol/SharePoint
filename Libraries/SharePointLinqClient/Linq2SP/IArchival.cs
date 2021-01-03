//<summary>
//  Title   : public interface IArchival
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2014-09-05 20:47:03 +0200 (pt., 05 wrz 2014) $
//  $Rev: 10768 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePointLinqClient/Linq2SP/IArchival.cs $
//  $Id: IArchival.cs 10768 2014-09-05 18:47:03Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.SharePoint.Client.Linq2SP
{
  /// <summary>
  /// The interface provides access to the Archival bit used for some list.
  /// </summary>
  public interface IArchival
  {
    /// <summary>
    /// Gets or sets the archival bit.
    /// </summary>
    /// <value>
    /// The archival bit.
    /// </value>
    System.Nullable<bool> Archival { get; set; }
  }

}
