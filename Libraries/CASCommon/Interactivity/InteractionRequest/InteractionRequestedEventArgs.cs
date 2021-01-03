//<summary>
//  Title   : class InteractionRequestedEventArgs
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-12-10 16:16:26 +0100 (śr., 10 gru 2014) $
//  $Rev: 11083 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/Interactivity/InteractionRequest/InteractionRequestedEventArgs.cs $
//  $Id: InteractionRequestedEventArgs.cs 11083 2014-12-10 15:16:26Z mpostol $
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
  /// <see cref="EventArgs"/>for the <see cref="IInteractionRequest{T}.Raised"/> event.
  /// </summary>
  public class InteractionRequestedEventArgs<T> : EventArgs
    where T : INotification
  {
    /// <summary>
    /// Constructs a new instance of <see cref="InteractionRequestedEventArgs{T}"/>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="callback"></param>
    public InteractionRequestedEventArgs(T context, Action callback)
    {
      this.Context = context;
      this.Callback = callback;
    }
    /// <summary>
    /// Gets the context for a requested interaction.
    /// </summary>
    public T Context { get; private set; }
    /// <summary>
    /// Gets the callback to execute when an interaction is completed.
    /// </summary>
    public Action Callback { get; private set; }
  }
}
