//<summary>
//  Title   : ButtonsPanelViewModel
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2014-12-11 12:45:17 +0100 (Cz, 11 gru 2014) $
//  $Rev: 11086 $
//  $LastChangedBy: mpostol $
//  $URL: svn://svnserver.hq.cas.com.pl/VS/trunk/PR42-SmartFactory/Shepherd/Client/Management/Controls/ButtonsPanelViewModel.cs $
//  $Id: ButtonsPanelViewModel.cs 11086 2014-12-11 11:45:17Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.ComponentModel;
using System;
using System.ComponentModel;
using System.Windows;

namespace CAS.Common.ViewModel.Wizard
{
  /// <summary>
  /// Class ButtonsPanelBase base class to derive Buttons Panel ViewModel
  /// </summary>
  public class ButtonsPanelBase : PropertyChangedBase, IButtonsPanelBase
  {
    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonsPanelBase"/> class.
    /// </summary>
    /// <param name="parentMachine">The parent machine.</param>
    /// <exception cref="System.ArgumentNullException">parentViewMode</exception>
    public ButtonsPanelBase(StateMachineContext parentMachine)
    {
      if (parentMachine == null)
        throw new ArgumentNullException("parentViewMode");
      m_ParentViewMode = parentMachine;
      parentMachine.PropertyChanged += parentViewMode_PropertyChanged;
      LeftButtonCommand = new SynchronousCommandBase<object>
        (m_ParentViewMode.StateMachineActionsArray[(int)StateMachineEventIndex.LeftButtonEvent], y => (this.m_EnabledEvents & StateMachineEvents.LeftButtonEvent) != 0);
      LeftMiddleButtonCommand = new SynchronousCommandBase<object>
        (m_ParentViewMode.StateMachineActionsArray[(int)StateMachineEventIndex.LeftMiddleButtonEvent], y => (this.m_EnabledEvents & StateMachineEvents.LeftMiddleButtonEvent) != 0);
      RightMiddleButtonCommand = new SynchronousCommandBase<object>
        (m_ParentViewMode.StateMachineActionsArray[(int)StateMachineEventIndex.RightMiddleButtonEvent], y => (this.m_EnabledEvents & StateMachineEvents.RightMiddleButtonEvent) != 0);
      RightButtonCommand = new SynchronousCommandBase<object>
        (m_ParentViewMode.StateMachineActionsArray[(int)StateMachineEventIndex.RightButtonEvent], y => (this.m_EnabledEvents & StateMachineEvents.RightButtonEvent) != 0);
    }
    #endregion

    #region public UI API
    /// <summary>
    /// Gets or sets the left button command.
    /// </summary>
    /// <value>The left button command.</value>
    public ICommandWithUpdate LeftButtonCommand
    {
      get
      {
        return b_LeftButtonCommand;
      }
      set
      {
        RaiseHandlerICommandWithUpdate(value, ref b_LeftButtonCommand, "LeftButtonCommand", this);
      }
    }
    /// <summary>
    /// Gets or sets the left-middle button command.
    /// </summary>
    /// <value>The left middle button command.</value>
    public ICommandWithUpdate LeftMiddleButtonCommand
    {
      get
      {
        return b_LeftMiddleButtonCommand;
      }
      set
      {
        RaiseHandlerICommandWithUpdate(value, ref b_LeftMiddleButtonCommand, "LeftMiddleButtonCommand", this);
      }
    }
    /// <summary>
    /// Gets or sets the right-middle button command.
    /// </summary>
    /// <value>The right middle button command.</value>
    public ICommandWithUpdate RightMiddleButtonCommand
    {
      get
      {
        return b_RightMiddleButtonCommand;
      }
      set
      {
        RaiseHandlerICommandWithUpdate(value, ref b_RightMiddleButtonCommand, "RightMiddleButtonCommand", this);
      }
    }
    /// <summary>
    /// Gets or sets the right button command.
    /// </summary>
    /// <value>The right button command.</value>
    public ICommandWithUpdate RightButtonCommand
    {
      get
      {
        return b_RightButtonCommand;
      }
      set
      {
        RaiseHandlerICommandWithUpdate(value, ref b_RightButtonCommand, "RightButtonCommand", this);
      }
    }
    /// <summary>
    /// Gets or sets the left button title.
    /// </summary>
    /// <value>The left button title.</value>
    public string LeftButtonTitle
    {
      get
      {
        return b_LeftButtonTitle;
      }
      set
      {
        RaiseHandler<string>(value, ref b_LeftButtonTitle, "LeftButtonTitle", this);
      }
    }
    /// <summary>
    /// Gets or sets the left-middle button title.
    /// </summary>
    /// <value>The left middle button title.</value>
    public string LeftMiddleButtonTitle
    {
      get
      {
        return b_LeftMiddleButtonTitle;
      }
      set
      {
        RaiseHandler<string>(value, ref b_LeftMiddleButtonTitle, "LeftMiddleButtonTitle", this);
      }
    }
    /// <summary>
    /// Gets or sets the right middle button title.
    /// </summary>
    /// <value>The right middle button title.</value>
    public string RightMiddleButtonTitle
    {
      get
      {
        return b_RightMiddleButtonTitle;
      }
      set
      {
        RaiseHandler<string>(value, ref b_RightMiddleButtonTitle, "RightMiddleButtonTitle", this);
      }
    }
    /// <summary>
    /// Gets or sets the right button title.
    /// </summary>
    /// <value>The right button title.</value>
    public string RightButtonTitle
    {
      get
      {
        return b_RightButtonTitle;
      }
      set
      {
        RaiseHandler<string>(value, ref b_RightButtonTitle, "RightButtonTitle", this);
      }
    }
    /// <summary>
    /// Gets or sets the left button visibility.
    /// </summary>
    /// <value>The left button visibility.</value>
    public Visibility LeftButtonVisibility
    {
      get
      {
        return b_LeftButtonVisibility;
      }
      set
      {
        RaiseHandler<System.Windows.Visibility>(value, ref b_LeftButtonVisibility, "LeftButtonVisibility", this);
      }
    }
    /// <summary>
    /// Gets or sets the left middle button visibility.
    /// </summary>
    /// <value>The left-middle button visibility.</value>
    public Visibility LeftMiddleButtonVisibility
    {
      get
      {
        return b_LeftMiddleButtonVisibility;
      }
      set
      {
        RaiseHandler<Visibility>(value, ref b_LeftMiddleButtonVisibility, "LeftMiddleButtonVisibility", this);
      }
    }
    private Visibility b_LeftMiddleButtonVisibility;
    /// <summary>
    /// Gets or sets the right middle button visibility.
    /// </summary>
    /// <value>The right-middle button visibility.</value>
    public Visibility RightMiddleButtonVisibility
    {
      get
      {
        return b_RightMiddleButtonVisibility;
      }
      set
      {
        RaiseHandler<Visibility>(value, ref b_RightMiddleButtonVisibility, "RightMiddleButtonVisibility", this);
      }
    }
    /// <summary>
    /// Gets or sets the right button visibility.
    /// </summary>
    /// <value>The right button visibility.</value>
    public Visibility RightButtonVisibility
    {
      get
      {
        return b_RightButtonVisibility;
      }
      set
      {
        RaiseHandler<Visibility>(value, ref b_RightButtonVisibility, "RightButtonVisibility", this);
      }
    }
    #endregion

    #region private

    #region backing fields
    private ICommandWithUpdate b_LeftButtonCommand;
    private ICommandWithUpdate b_LeftMiddleButtonCommand;
    private ICommandWithUpdate b_RightMiddleButtonCommand;
    private ICommandWithUpdate b_RightButtonCommand;
    private Visibility b_LeftButtonVisibility;
    private Visibility b_RightMiddleButtonVisibility;
    private Visibility b_RightButtonVisibility;
    private string b_LeftButtonTitle = "Left";
    private string b_LeftMiddleButtonTitle = "Left Middle";
    private string b_RightMiddleButtonTitle = "Right Middle";
    private string b_RightButtonTitle = "Right";
    #endregion

    //vars
    private StateMachineContext m_ParentViewMode;
    private StateMachineEvents m_EnabledEvents = (StateMachineEvents)0;
    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute.
    /// </summary>
    private event EventHandler CanExecuteChanged;
    //procedures
    private void parentViewMode_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "ButtonPanelState")
        GetState(m_ParentViewMode.ButtonPanelState);
      if (e.PropertyName == "EnabledEvents")
        this.m_EnabledEvents = m_ParentViewMode.EnabledEvents;
      RaiseCanExecuteChanged();
    }
    private void GetState(ButtonsSetState state)
    {
      this.LeftButtonTitle = state.LeftButtonTitle;
      this.LeftButtonVisibility = state.LeftButtonVisibility;
      this.LeftMiddleButtonTitle = state.LeftMiddleButtonTitle;
      this.LeftMiddleButtonVisibility = state.LeftMiddleButtonVisibility;
      this.RightButtonTitle = state.RightButtonTitle;
      this.RightButtonVisibility = state.RightButtonVisibility;
      this.RightMiddleButtonTitle = state.RightMiddleButtonTitle;
      this.RightMiddleButtonVisibility = state.RightButtonVisibility;
    }
    private void RaiseCanExecuteChanged()
    {
      EventHandler _cec = CanExecuteChanged;
      if (_cec == null)
        return;
      CanExecuteChanged(this, EventArgs.Empty);
    }
    private bool RaiseHandlerICommandWithUpdate(ICommandWithUpdate value, ref ICommandWithUpdate oldValue, string propertyName, object sender)
    {
      bool _ret = base.RaiseHandler<ICommandWithUpdate>(value, ref oldValue, propertyName, sender);
      if (_ret)
        this.CanExecuteChanged += (sevder, e) => value.RaiseCanExecuteChanged();
      return _ret;
    }
    #endregion

  }
}
