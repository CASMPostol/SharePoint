//<summary>
//  Title   : ConnectCancelTemplate
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2014-12-22 14:26:23 +0100 (pon., 22 gru 2014) $
//  $Rev: 11137 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/ViewModel/Wizard/ButtonsPanelStateTemplates/ConnectCancelTemplate.cs $
//  $Id: ConnectCancelTemplate.cs 11137 2014-12-22 13:26:23Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.Properties;

namespace CAS.Common.ViewModel.Wizard.ButtonsPanelStateTemplates
{
  /// <summary>
  /// Class ConnectCancelTemplate.
  /// </summary>
  public class ConnectCancelTemplate : CancelTemplate
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectCancelTemplate"/> class.
    /// </summary>
    public ConnectCancelTemplate(string leftButtonTitle, string leftMiddleButtonTitle) :
      base(leftButtonTitle, leftMiddleButtonTitle, Resources.ConnectButtonTitle)
    { }
    /// <summary>
    /// Gets the connect button position in the array of actions.
    /// </summary>
    /// <value>The connect position.</value>
    public StateMachineEventIndex ConnectPosition { get { return StateMachineEventIndex.RightMiddleButtonEvent; } }
    /// <summary>
    /// Gets the left button position.
    /// </summary>
    /// <value>The left button position.</value>
    public StateMachineEventIndex LeftButtonPosition { get { return StateMachineEventIndex.LeftButtonEvent; } }
    /// <summary>
    /// Gets the left-middle button position.
    /// </summary>
    /// <value>The left middle button position.</value>
    public StateMachineEventIndex LeftMiddleButtonPosition { get { return StateMachineEventIndex.LeftMiddleButtonEvent; } }
    /// <summary>
    /// Called when only my buttons must be enabled.
    /// </summary>
    /// <returns>StateMachineEvents.</returns>
    public StateMachineEvents OnlyMe()
    {
      return SetEventsMask(false, false);
    }
    /// <summary>
    /// Saves the state and sets the events mask for the selected buttons. The Connect and Cancel buttons are active.
    /// </summary>
    /// <param name="leftButton">if set to <c>true</c> left button is enabled.</param>
    /// <param name="leftMiddleButton">if set to <c>true</c> left-middle button is enabled.</param>
    /// <returns>Return the mask of type <see cref="StateMachineEvents" /> repressing enabled buttons.</returns>
    public StateMachineEvents SetEventsMask(bool leftButton, bool leftMiddleButton)
    {
      return base.SetEventsMask(leftButton, leftMiddleButton, true, true);
    }
  }
}
