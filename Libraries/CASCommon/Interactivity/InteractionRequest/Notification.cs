//<summary>
//  Title   : class Notification
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-12-10 16:16:26 +0100 (śr., 10 gru 2014) $
//  $Rev: 11083 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/CASCommon/Interactivity/InteractionRequest/Notification.cs $
//  $Id: Notification.cs 11083 2014-12-10 15:16:26Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
namespace CAS.Common.Interactivity.InteractionRequest
{
  /// <summary>
  /// Basic implementation of <see cref="INotification"/>.
  /// </summary>
  public class Notification : INotification
  {
    /// <summary>
    /// Gets or sets the title to use for the notification.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the content of the notification.
    /// </summary>
    public object Content { get; set; }
  }
}
