//<summary>
//  Title   : BaseTemplate.cs
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2014-12-22 14:26:23 +0100 (pon., 22 gru 2014) $
//  $Rev: 11137 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/CASCommon/ViewModel/Wizard/ButtonsPanelStateTemplates/BaseTemplate.cs $
//  $Id: BaseTemplate.cs 11137 2014-12-22 13:26:23Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;

namespace CAS.Common.ViewModel.Wizard.ButtonsPanelStateTemplates
{
  /// <summary>
  /// Class BaseTemplate.
  /// </summary>
  public class BaseTemplate : ButtonsSetState
  {

    #region constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseTemplate"/> class.
    /// </summary>
    public BaseTemplate(string leftButtonTitle, string leftMiddleButtonTitle, string rightMiddleButtonTitle, string rightButtonTitle) :
      base(leftButtonTitle, leftMiddleButtonTitle, rightMiddleButtonTitle, rightButtonTitle)
    {
      m_State = GetEnabledEvents;
      m_PreviousEnabledEvents = GetEnabledEvents;
    }
    #endregion

    #region private
    /// <summary>
    /// Reverts the changes to last known state.
    /// </summary>
    public void RevertChanges()
    {
      base.EnabledEvents = m_PreviousEnabledEvents;
    }
    /// <summary>
    /// Saves the current state.
    /// </summary>
    public void SaveState()
    {
      m_PreviousEnabledEvents = EnabledEvents;
    }
    /// <summary>
    /// Called when only buttons defined by the template must be active.
    /// </summary>
    public void Rever2Template()
    {
      base.EnabledEvents = m_State;
    }
    /// <summary>
    /// Saves the state and sets the events mask for the selected buttons.
    /// </summary>
    /// <param name="leftButton">if set to <c>true</c> left button is enabled.</param>
    /// <param name="leftMiddleButton">if set to <c>true</c> left-middle button is enabled.</param>
    /// <param name="rightMiddleButton">if set to <c>true</c> right-middle button i s enabled.</param>
    /// <param name="rightButton">if set to <c>true</c> right button is enabled.</param>
    /// <returns>Return the mask of type <see cref="StateMachineEvents" /> repressing enabled buttons.</returns>
    public override StateMachineEvents SetEventsMask(bool leftButton, bool leftMiddleButton, bool rightMiddleButton, bool rightButton)
    {
      SaveState();
      return base.SetEventsMask(leftButton, leftMiddleButton, rightMiddleButton, rightButton);
    }
    #endregion

    #region private
    private StateMachineEvents m_State;
    private StateMachineEvents m_PreviousEnabledEvents;
    #endregion

  }
}
