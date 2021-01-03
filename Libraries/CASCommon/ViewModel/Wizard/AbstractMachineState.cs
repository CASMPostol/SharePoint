//<summary>
//  Title   : Name of Application
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-12-31 12:42:50 +0100 (śr., 31 gru 2014) $
//  $Rev: 11161 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/ViewModel/Wizard/AbstractMachineState.cs $
//  $Id: AbstractMachineState.cs 11161 2014-12-31 11:42:50Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.ComponentModel;

namespace CAS.Common.ViewModel.Wizard
{

  /// <summary>
  /// It implements basic functionality of the state for the state machine of the <see cref="StateMachineContext" />
  /// </summary>
  /// <typeparam name="StateMachineContextType">The type of the state machine context type.</typeparam>
  /// <typeparam name="ViewModelContextType">The type of the view model context type.</typeparam>
  public abstract class AbstractMachineState<StateMachineContextType, ViewModelContextType> : IAbstractMachineState
    where StateMachineContextType : StateMachineContext
    where ViewModelContextType : IViewModelContext
  {

    #region constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractMachineState{StateMachineContextType, ViewModelContextType}"/> class.
    /// </summary>
    public AbstractMachineState() { }
    #endregion

    #region public
    /// <summary>
    /// Called on entering new state.
    /// </summary>
    public virtual void OnEnteringState()
    {
      Context.ProgressChang(this, new ProgressChangedEventArgs(0, String.Format("Entered state {0}", ToString())));
    }
    /// <summary>
    /// Called on state exiting.
    /// </summary>
    public virtual void OnExitingState()
    {
      Context.ProgressChang(this, new ProgressChangedEventArgs(0, String.Format("Exiting the state {0}", ToString())));
    }
    /// <summary>
    /// Called when exception has occurred. Make context aware about exception.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public virtual void OnException(Exception exception)
    {
      Context.Exception(exception);
    }
    /// <summary>
    /// Called when cancellation is requested.
    /// </summary>
    public virtual void OnCancellation()
    {
      Context.Close();
    }
    #endregion

    #region IAbstractMachineEvents Members
    /// <summary>
    /// Cancels this instance.
    /// </summary>
    public virtual void Cancel()
    {
      OnCancellation();
    }
    /// <summary>
    /// Gets the state of the buttons panel.
    /// </summary>
    /// <value>The state of the buttons panel.</value>
    protected abstract ButtonsSetState ButtonsPanelState { get; }
    /// <summary>
    /// Sets the context <see cref="StateMachineContext" />.
    /// </summary>
    /// <value>
    /// The context.
    /// </value>
    StateMachineContext IAbstractMachineState.Context
    {
      set
      {
        this.Context = (StateMachineContextType)value;
        this.Context.ButtonPanelState = ButtonsPanelState;
      }
    }
    /// <summary>
    /// Sets the view model context.
    /// </summary>
    /// <value>The view model context.</value>
    IViewModelContext IAbstractMachineState.ViewModelContext
    {
      set { this.ViewModelContext = (ViewModelContextType)value; }
    }
    /// <summary>
    /// Gets the current context derived form <see cref="StateMachineContext"/>.
    /// </summary>
    /// <value>
    /// The context.
    /// </value>
    public StateMachineContextType Context { get; private set; }
    /// <summary>
    /// Gets the view model context derived from <see cref="IViewModelContext"/>.
    /// </summary>
    /// <value>The view model context.</value>
    public ViewModelContextType ViewModelContext { get; private set; }
    /// <summary>
    /// The state machine array of actions <see cref="Action" />
    /// </summary>
    /// <value>The state machine actions array <see cref="Action"/>.</value>
    public abstract Action<object>[] StateMachineActionsArray { get; }
    #endregion

    #region IDisposable
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public abstract void Dispose();
    #endregion

  }
}
