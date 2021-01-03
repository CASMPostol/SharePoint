//<summary>
//  Title   : StateMachineContext class
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-01-07 14:45:01 +0100 (śr., 07 sty 2015) $
//  $Rev: 11179 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/ViewModel/Wizard/StateMachineContext.cs $
//  $Id: StateMachineContext.cs 11179 2015-01-07 13:45:01Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.ComponentModel;
using CAS.Common.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAS.Common.ViewModel.Wizard
{

  /// <summary>
  /// Abstract StateMachineContext class - contains main engine of the state machine concept.
  /// </summary>
  public abstract class StateMachineContext : PropertyChangedBase, IDisposable
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="StateMachineContext"/> class.
    /// </summary>
    public StateMachineContext()
    {
      CancelConfirmation = new InteractionRequest<IConfirmation>();
      CloseWindow = new InteractionRequest<INotification>();
      ExceptionNotification = new InteractionRequest<INotification>();
      StateMachineActionsArray = new Action<object>[4];
      StateMachineActionsArray[(int)StateMachineEventIndex.LeftButtonEvent] = x => Machine.StateMachineActionsArray[(int)StateMachineEventIndex.LeftButtonEvent](x);
      StateMachineActionsArray[(int)StateMachineEventIndex.LeftMiddleButtonEvent] = x => Machine.StateMachineActionsArray[(int)StateMachineEventIndex.LeftMiddleButtonEvent](x);
      StateMachineActionsArray[(int)StateMachineEventIndex.RightButtonEvent] = x => Machine.StateMachineActionsArray[(int)StateMachineEventIndex.RightButtonEvent](x);
      StateMachineActionsArray[(int)StateMachineEventIndex.RightMiddleButtonEvent] = x => Machine.StateMachineActionsArray[(int)StateMachineEventIndex.RightMiddleButtonEvent](x);
    }
    #endregion

    #region View Model
    /// <summary>
    /// Gets the cancel confirmation <see cref="InteractionRequest{T}"/>.
    /// </summary>
    /// <value>
    /// The cancel confirmation.
    /// </value>
    public InteractionRequest<IConfirmation> CancelConfirmation { get; private set; }
    /// <summary>
    /// Interacts with the shell to close the window <see cref="InteractionRequest{T}"/>
    /// </summary>
    public InteractionRequest<INotification> CloseWindow { get; private set; }
    /// <summary>
    /// Interacts with the shell to notify about exception <see cref="InteractionRequest{T}"/>.
    /// </summary>
    /// <value>
    /// The exception notification.
    /// </value>
    public InteractionRequest<INotification> ExceptionNotification { get; private set; }
    /// <summary>
    /// Gets or sets the state of the button panel.
    /// </summary>
    /// <value>The state of the button panel.</value>
    public ButtonsSetState ButtonPanelState
    {
      get
      {
        return b_ButtonPanelState;
      }
      set
      {
        RaiseHandler<ButtonsSetState>(value, ref b_ButtonPanelState, "ButtonPanelState", this);
      }
    }
    /// <summary>
    /// Gets or sets the enabled events.
    /// </summary>
    /// <value>The enabled events.</value>
    public StateMachineEvents EnabledEvents
    {
      get
      {
        return b_EnabledEvents;
      }
      set
      {
        RaiseHandler<StateMachineEvents>(value, ref b_EnabledEvents, "EnabledEvents", this);
      }
    }
    #endregion

    #region public
    /// <summary>
    /// Enters the state.
    /// </summary>
    /// <typeparam name="StateType">The type of the state type.</typeparam>
    /// <param name="viewModel">The view model.</param>
    /// <returns>StateType.</returns>
    public StateType EnterState<StateType>(IViewModelContext viewModel)
      where StateType : IAbstractMachineState, new()
    {
      StateType _newOne = new StateType();
      _newOne.Context = this;
      _newOne.ViewModelContext = viewModel;
      this.Machine = _newOne;
      return _newOne;
    }
    /// <summary>
    /// Gets or sets the state machine.
    /// </summary>
    /// <value>
    /// The current machine.
    /// </value>
    public IAbstractMachineState Machine
    {
      protected get { return this.m_Machine; }
      set
      {
        if (m_Machine != null)
        {
          m_Machine.OnExitingState();
          m_Machine.Dispose();
        }
        m_Machine = value;
        StateNameProgressChang(value.ToString());
        m_Machine.OnEnteringState();
      }
    }
    /// <summary>
    /// Closes the application.
    /// </summary>
    public virtual void Close()
    {
      CloseWindow.Raise(new Notification() { Title = "Closing confirmation", Content = "The window will be closed? Are you sure?" });
    }
    /// <summary>
    /// Report the specified exception.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public virtual void Exception(Exception exception)
    {
      string _msg = String.Format("{0} exception occurred at state {1}. Details: {2}.", exception.GetType().Name, Machine.ToString(), exception.Message);
      ProgressChang(Machine, new ProgressChangedEventArgs(0, _msg));
      ExceptionNotification.Raise(new Notification() { Title = "Exception notification.", Content = _msg });
    }
    /// <summary>
    /// Pop-up a message box asking the user to confirm cancellation.
    /// </summary>
    /// <returns><c>true</c> if cancellation has been accepted by the user, <c>false</c> otherwise.</returns>
    internal bool CancellationConfirmation()
    {
      bool _confirmed = false;
      CancelConfirmation.Raise(
        new Confirmation() { Title = "Cancellation confirmation.", Content = "The operation will be canceled. Are you sure?", Confirmed = true },
        c => _confirmed = c.Confirmed);
      return _confirmed;
    }
    /// <summary>
    /// Is called by the event handler of the <see cref="BackgroundWorker.ProgressChanged"/>.
    /// </summary>
    /// <param name="activationMachine">The activation machine.</param>
    /// <param name="entitiesState">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
    public virtual void ProgressChang(IAbstractMachineState activationMachine, ProgressChangedEventArgs entitiesState) { }
    /// <summary>
    /// The state machine actions array contains actions that are to be executed as the result of commands raising.
    /// </summary>
    internal Action<object>[] StateMachineActionsArray = null;
    #endregion

    #region private
    //vars
    private ButtonsSetState b_ButtonPanelState;
    private StateMachineEvents b_EnabledEvents;
    private IAbstractMachineState m_Machine = null;
    /// <summary>
    /// Enter the selected state.
    /// </summary>
    /// <param name="machine">The machine.</param>
    protected void OpenEntryState(IAbstractMachineState machine)
    {
      Machine = machine;
    }
    /// <summary>
    /// The components to be disposed
    /// </summary>
    protected readonly List<IDisposable> Components = new List<IDisposable>();
    /// <summary>
    /// Called when the name of state changes.
    /// </summary>
    /// <param name="machineStateName">Name of the machine state.</param>
    protected abstract void StateNameProgressChang(string machineStateName);
    #endregion

    #region IDisposable
    /// <summary>
    /// Implement IDisposable. Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// Do not make this method virtual. A derived class should not be able to override this method. 
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      // This object will be cleaned up by the Dispose method. 
      // Therefore, you should call GC.SupressFinalize to 
      // take this object off the finalization queue 
      // and prevent finalization code for this object 
      // from executing a second time.
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources. Dispose(bool disposing) executes in two distinct scenarios. 
    /// If disposing equals true, the method has been called directly or indirectly by a user's code. Managed and unmanaged resources 
    /// can be disposed. If disposing equals false, the method has been called by the runtime from inside the finalizer and you should not reference 
    /// other objects. Only unmanaged resources can be disposed. 
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      // Check to see if Dispose has already been called. 
      if (!this.disposed)
      {
        // If disposing equals true, dispose all managed 
        // and unmanaged resources. 
        if (disposing)
          foreach (IDisposable _item in Components)
            // Dispose managed resources.
            _item.Dispose();

        // Note disposing has been done.
        disposed = true;
      }
    }
    // Track whether Dispose has been called. 
    private bool disposed = false;
    // Use C# destructor syntax for finalization code. 
    // This destructor will run only if the Dispose method 
    // does not get called. 
    // It gives your base class the opportunity to finalize. 
    // Do not provide destructors in types derived from this class.
    /// <summary>
    /// Finalizes an instance of the <see cref="StateMachineContext"/> class. Use C# destructor syntax for finalization code. This destructor will run only if the Dispose method 
    /// does not get called. It gives your base class the opportunity to finalize. Do not provide destructors in types derived from this class.
    /// </summary>
    ~StateMachineContext()
    {
      // Do not re-create Dispose clean-up code here. Calling Dispose(false) is optimal in terms of readability and maintainability.
      Dispose(false);
    }
    #endregion

  }

}
