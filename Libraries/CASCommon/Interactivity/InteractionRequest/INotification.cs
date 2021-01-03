//<summary>
//  Title   : interface INotification
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2014-12-10 16:16:26 +0100 (śr., 10 gru 2014) $
//  $Rev: 11083 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/Interactivity/InteractionRequest/INotification.cs $
//  $Id: INotification.cs 11083 2014-12-10 15:16:26Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
            
namespace CAS.Common.Interactivity.InteractionRequest
{
  /// <summary>
  /// Represents an interaction request used for notifications.
  /// </summary>
  public interface INotification
  {
    /// <summary>
    /// Gets or sets the title to use for the notification.
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// Gets or sets the content of the notification.
    /// </summary>
    object Content { get; set; }
  }
}
