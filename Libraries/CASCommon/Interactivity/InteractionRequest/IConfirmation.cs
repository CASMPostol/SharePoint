//<summary>
//  Title   : interface IConfirmation
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-12-10 16:16:26 +0100 (śr., 10 gru 2014) $
//  $Rev: 11083 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/CASCommon/Interactivity/InteractionRequest/IConfirmation.cs $
//  $Id: IConfirmation.cs 11083 2014-12-10 15:16:26Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.Common.Interactivity.InteractionRequest
{
  /// <summary>
  /// Represents an interaction request used for confirmations.
  /// </summary>
  public interface IConfirmation : INotification
  {
    /// <summary>
    /// Gets or sets a value indicating that the confirmation is confirmed.
    /// </summary>
    bool Confirmed { get; set; }
  }
}
