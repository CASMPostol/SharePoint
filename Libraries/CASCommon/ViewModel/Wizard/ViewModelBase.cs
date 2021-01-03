//<summary>
//  Title   : ViewModelBase
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using CAS.Common.ComponentModel;

namespace CAS.Common.ViewModel.Wizard
{

  /// <summary>
  /// Class ViewModelBase - base to derived by the ViewModel for any Wizard state machine
  /// </summary>
  /// <typeparam name="StateType">The type of the state type.</typeparam>
  public class ViewModelBase<StateType> : PropertyChangedBase, IViewModelContext
    where StateType: class, IAbstractMachineState, new()
  {

    /// <summary>
    /// Enters the state <typeparamref name="StateType"/> after the view is navigated to. 
    /// </summary>
    /// <param name="value">The value.</param>
    protected virtual void EnterState(StateMachineContext value)
    {
      MyState = value.EnterState<StateType>(this);
    }
    /// <summary>
    /// Because the view is reusable and the state is instantiated every time it is navigated to this method must be called after 
    /// the view is moving away to release the state and dispose all resources.
    /// </summary>
    protected virtual void ExitState()
    {
      MyState = null;
    }
    /// <summary>
    /// Gets my state derived form <see cref="IAbstractMachineState"/>.
    /// </summary>
    /// <value>My state of the state machine.</value>
    protected StateType MyState { get; private set; }

  }
}
