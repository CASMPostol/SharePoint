//<summary>
//  Title   : abstract class GenericStateMachineEngine
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-04-03 12:57:48 +0200 (pt., 03 kwi 2015) $
//  $Rev: 11553 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePoint/Web/GenericStateMachineEngine.cs $
//  $Id: GenericStateMachineEngine.cs 11553 2015-04-03 10:57:48Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Web.UI.WebControls;

namespace CAS.SharePoint.Web
{
  /// <summary>
  /// Generic State MachineEngine
  /// </summary>
  public abstract class GenericStateMachineEngine
  {
    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericStateMachineEngine"/> class.
    /// </summary>
    public GenericStateMachineEngine() { }
    /// <summary>
    ///  enum ControlsSet used to control visibility of the controls.
    /// </summary>
    [Flags]
    public enum ControlsSet
    {
      /// <summary>
      /// The save on
      /// </summary>
      SaveOn = 0x01,
      /// <summary>
      /// The edit on
      /// </summary>
      EditOn = 0x02,
      /// <summary>
      /// The cancel on
      /// </summary>
      CancelOn = 0x04,
      /// <summary>
      /// The new on
      /// </summary>
      NewOn = 0x08,
      /// <summary>
      /// The delete on
      /// </summary>
      DeleteOn = 0x10,
      /// <summary>
      /// The edit mode on
      /// </summary>
      EditModeOn = 0x20,
      /// <summary>
      /// The new mode on
      /// </summary>
      NewModeOn = 0x40
    }
    /// <summary>
    /// enum InterfaceEvent
    /// </summary>
    public enum InterfaceEvent
    {
      /// <summary>
      /// The save click
      /// </summary>
      SaveClick,
      /// <summary>
      /// The edit click
      /// </summary>
      EditClick,
      /// <summary>
      /// The cancel click
      /// </summary>
      CancelClick,
      /// <summary>
      /// The new click
      /// </summary>
      NewClick,
      /// <summary>
      /// The new data
      /// </summary>
      NewData
    };
    /// <summary>
    /// enum InterfaceState
    /// </summary>
    public enum InterfaceState
    {
      /// <summary>
      /// The view state
      /// </summary>
      ViewState,
      /// <summary>
      /// The edit state
      /// </summary>
      EditState,
      /// <summary>
      /// The new state
      /// </summary>
      NewState
    }
    /// <summary>
    /// class ActionResult derived from <see cref="Exception"/> used to convey operation result
    /// </summary>
    public class ActionResult : Exception
    {
      #region public
      /// <summary>
      /// enumerates possible results of operation
      /// </summary>
      public enum Result
      {
        /// <summary>
        /// The operation succeeded
        /// </summary>
        Success,
        /// <summary>
        /// The provided data is not valid
        /// </summary>
        NotValidated,
        /// <summary>
        /// The exception has bee catched
        /// </summary>
        Exception
      }
      /// <summary>
      /// Gets the last action result.
      /// </summary>
      /// <value>
      /// The last action result.
      /// </value>
      public Result LastActionResult { get; private set; }
      /// <summary>
      /// Gets a value indicating that during execution an exception has been catched.
      /// </summary>
      /// <value>
      /// An object of <see cref="Exception"/> representing the exception that has been catched.
      /// </value>
      public Exception ActionException { get; private set; }
      /// <summary>
      /// Gets a value indicating whether the action has succeeded.
      /// </summary>
      /// <value>
      ///   <c>true</c> if [action succeeded]; otherwise, <c>false</c>.
      /// </value>
      public bool ActionSucceeded { get { return LastActionResult == Result.Success; } }
      /// <summary>
      /// Gets the object <see cref="ActionResult"/> representing success.
      /// </summary>
      /// <param name="excptn">The excptn.</param>
      /// <param name="_src">The _SRC.</param>
      /// <returns></returns>
      public static ActionResult Exception(Exception excptn, string _src)
      {
        excptn.Source += " at " + _src;
        return new ActionResult(Result.Exception, excptn.Message, excptn) { ActionException = excptn };
      }
      /// <summary>
      /// Gets the object <see cref="ActionResult"/> representing success.
      /// </summary>
      /// <value>
      /// The object <see cref="ActionResult"/> representing success.
      /// </value>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
      public static ActionResult Success { get { return new ActionResult(Result.Success, Result.Success.ToString(), null); } }
      /// <summary>
      /// Gets the object <see cref="ActionResult"/> representing result NotValidated.
      /// </summary>
      /// <param name="message">The message to be returned by the operation <see cref="ActionResult"/>.</param>
      /// <returns></returns>
      public static ActionResult NotValidated(string message)
      {
        return new ActionResult(Result.NotValidated, message, null) { ActionException = new ApplicationException(message) };
      }
      /// <summary>
      /// If LastActionResult is not equal Success new <see cref="Literal"/> is created and the <paramref name="controlsAdd"/> action is called to add it to the controls.
      /// </summary>
      /// <param name="controlsAdd">The action used to add new <see cref="Literal"/> containing information about current result to the controls.</param>
      public void CreateActionResultControl(Action<Literal> controlsAdd)
      {
        if (this.LastActionResult == GenericStateMachineEngine.ActionResult.Result.Success)
          return;
        if (this.LastActionResult == GenericStateMachineEngine.ActionResult.Result.Exception)
        {
          string _format = CommonDefinitions.Convert2ErrorMessageFormat("Exception at: {0} of : {2}.");
          controlsAdd(new Literal() { Text = String.Format(_format, this.ActionException.Source, this.ActionException.Message) });
        }
        else
        {
          string _format = CommonDefinitions.Convert2ErrorMessageFormat("Validation error at: {0}/{1} of : {2}.");
          controlsAdd(new Literal() { Text = String.Format(_format, this.ActionException.Message) });
        }
      }
      #endregion

      #region private
      private ActionResult(Result _rslt, string message, Exception innerException)
        : base(message, innerException)
      {
        LastActionResult = _rslt;
      }
      #endregion
    }
    #endregion

    #region Event Handlers
    /// <summary>
    /// Handles the Click event of the NewButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    public void NewButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          CurrentMachineState = InterfaceState.NewState;
          ClearUserInterface();
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
        default:
          SMError(InterfaceEvent.NewClick);
          break;
      }
    }
    /// <summary>
    /// Handles the Click event of the SaveButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    public void SaveButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.EditState:
          ActionResult _ur = Update();
          switch (_ur.LastActionResult)
          {
            case ActionResult.Result.Success:
              CurrentMachineState = InterfaceState.ViewState;
              break;
            case ActionResult.Result.NotValidated:
            case ActionResult.Result.Exception:
              ShowActionResult(_ur);
              break;
          }
          break;
        case InterfaceState.NewState:
          ActionResult _cr = this.Create();
          switch (_cr.LastActionResult)
          {
            case ActionResult.Result.Success:
              CurrentMachineState = InterfaceState.ViewState;
              break;
            case ActionResult.Result.NotValidated:
              ShowActionResult(_cr);
              break;
            case ActionResult.Result.Exception:
              ClearUserInterface();
              ShowActionResult(_cr);
              CurrentMachineState = InterfaceState.ViewState;
              break;
            default:
              break;
          }
          break;
        case InterfaceState.ViewState:
        default:
          SMError(InterfaceEvent.SaveClick);
          break;
      };
    }
    /// <summary>
    /// Handles the Click event of the CancelButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    public void CancelButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.NewState:
        case InterfaceState.EditState:
          CurrentMachineState = InterfaceState.ViewState;
          break;
        case InterfaceState.ViewState:
        default:
          SMError(InterfaceEvent.CancelClick);
          break;
      }
    }
    /// <summary>
    /// Handles the Click event of the EditButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    public void EditButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          CurrentMachineState = InterfaceState.EditState;
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
        default:
          SMError(InterfaceEvent.EditClick);
          break;
      }
    }
    /// <summary>
    /// Handles the Click event of the DeleteButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    public void DeleteButton_Click(object sender, EventArgs e)
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          ActionResult _dr = Delete();
          switch (_dr.LastActionResult)
          {
            case ActionResult.Result.Success:
              ClearUserInterface();
              break;
            case ActionResult.Result.NotValidated:
            case ActionResult.Result.Exception:
              ShowActionResult(_dr);
              break;
          }
          break;
        case InterfaceState.EditState:
        case InterfaceState.NewState:
        default:
          SMError(InterfaceEvent.EditClick);
          break;
      }
    }
    #endregion

    #region private

    #region Actions
    /// <summary>
    /// Updates this instance.
    /// </summary>
    /// <returns></returns>
    protected abstract ActionResult Update();
    /// <summary>
    /// Creates this instance.
    /// </summary>
    /// <returns></returns>
    protected abstract ActionResult Create();
    /// <summary>
    /// Deletes this instance.
    /// </summary>
    /// <returns></returns>
    protected abstract ActionResult Delete();
    /// <summary>
    /// Clears the user interface.
    /// </summary>
    protected abstract void ClearUserInterface();
    /// <summary>
    /// Sets the enabled.
    /// </summary>
    /// <param name="_buttons">The _buttons.</param>
    protected abstract void SetEnabled(ControlsSet _buttons);
    /// <summary>
    /// Sms the error.
    /// </summary>
    /// <param name="interfaceEvent">The interface event.</param>
    protected abstract void SMError(InterfaceEvent interfaceEvent);
    /// <summary>
    /// Shows the action result.
    /// </summary>
    /// <param name="_rslt">The _RSLT.</param>
    protected abstract void ShowActionResult(ActionResult _rslt);
    #endregion

    /// <summary>
    /// Gets or sets the state of the current machine.
    /// </summary>
    /// <value>
    /// The state of the current machine.
    /// </value>
    protected abstract InterfaceState CurrentMachineState { get; set; }
    /// <summary>
    /// Enters the state.
    /// </summary>
    protected virtual void EnterState()
    {
      switch (CurrentMachineState)
      {
        case InterfaceState.ViewState:
          SetEnabled(ControlsSet.EditOn | ControlsSet.NewOn | ControlsSet.DeleteOn);
          break;
        case InterfaceState.EditState:
          SetEnabled(ControlsSet.CancelOn | ControlsSet.SaveOn | ControlsSet.EditModeOn);
          break;
        case InterfaceState.NewState:
          SetEnabled(ControlsSet.CancelOn | ControlsSet.SaveOn | ControlsSet.EditModeOn | ControlsSet.NewModeOn);
          break;
      }
    }
    #endregion
  }
}
