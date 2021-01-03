//<summary>
//  Title   : IAbstractMachineState
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-12-09 15:20:47 +0100 (Wt, 09 gru 2014) $
//  $Rev: 11078 $
//  $LastChangedBy: mpostol $
//  $URL: svn://svnserver.hq.cas.com.pl/VS/trunk/PR44-SharePoint/Libraries/CASCommon/ViewModel/Wizard/IAbstractMachineEvents.cs $
//  $Id: IAbstractMachineEvents.cs 11078 2014-12-09 14:20:47Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;

namespace CAS.Common.ViewModel.Wizard
{
  /// <summary>
  /// Abstract Machine Events Interface
  /// </summary>
  public interface IAbstractMachineState : IDisposable
  {

    /// <summary>
    /// The state machine array of actions <see cref="Action"/>
    /// </summary>
    Action<object>[] StateMachineActionsArray { get; }
    /// <summary>
    /// Called on state exiting.
    /// </summary>
    void OnExitingState();
    /// <summary>
    /// Called on entering new state.
    /// </summary>
    void OnEnteringState();
    /// <summary>
    /// Sets the context <see cref="StateMachineContext"/>.
    /// </summary>
    /// <value>
    /// The context <see cref="StateMachineContext"/>.
    /// </value>
    StateMachineContext Context { set; }
    /// <summary>
    /// Sets the view model context.
    /// </summary>
    /// <value>The view model context.</value>
    IViewModelContext ViewModelContext { set; }

  }
}
