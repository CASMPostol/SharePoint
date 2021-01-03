//<summary>
//  Title   : class InteractionRequest<T> 
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2014-12-10 16:16:26 +0100 (śr., 10 gru 2014) $
//  $Rev: 11083 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/Interactivity/InteractionRequest/InteractionRequest.cs $
//  $Id: InteractionRequest.cs 11083 2014-12-10 15:16:26Z mpostol $
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
  /// Implementation of the <see cref="IInteractionRequest{T}"/> interface.
  /// </summary>
  public class InteractionRequest<T> : IInteractionRequest<T>
      where T : INotification
  {
    /// <summary>
    /// Fired when interaction is needed.
    /// </summary>
    public event EventHandler<InteractionRequestedEventArgs<T>> Raised;

    /// <summary>
    /// Fires the Raised event.
    /// </summary>
    /// <param name="context">The context for the interaction request.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
    public void Raise(T context)
    {
      this.Raise(context, c => { });
    }

    /// <summary>
    /// Fires the Raised event.
    /// </summary>
    /// <param name="context">The context for the interaction request.</param>
    /// <param name="callback">The callback to execute when the interaction is completed.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
    public void Raise(T context, Action<T> callback)
    {
      var handler = this.Raised;
      if (handler != null)
      {
        handler(this, new InteractionRequestedEventArgs<T>(context, () => { if (callback != null) callback(context); }));
      }
    }
  }
}
