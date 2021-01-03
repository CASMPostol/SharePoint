//<summary>
//  Title   : interface IInteractionRequest
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2014-12-10 16:16:26 +0100 (śr., 10 gru 2014) $
//  $Rev: 11083 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/CASCommon/Interactivity/InteractionRequest/IInteractionRequest.cs $
//  $Id: IInteractionRequest.cs 11083 2014-12-10 15:16:26Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;

namespace CAS.Common.Interactivity.InteractionRequest
{
  /// <summary>
  /// Represents a request from user interaction.
  /// </summary>
  /// <remarks>
  /// View models can expose interaction request objects through properties and raise them when user interaction
  /// is required so views associated with the view models can materialize the user interaction using an appropriate
  /// mechanism.
  /// </remarks>
  public interface IInteractionRequest<T>
    where T : INotification
  {
    /// <summary>
    /// Fired when the interaction is needed.
    /// </summary>
    event EventHandler<InteractionRequestedEventArgs<T>> Raised;
  }
}
