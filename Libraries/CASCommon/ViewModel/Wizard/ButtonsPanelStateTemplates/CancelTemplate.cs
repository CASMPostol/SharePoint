//<summary>
//  Title   : CancelBaseTemplate
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2014-12-22 14:26:23 +0100 (pon., 22 gru 2014) $
//  $Rev: 11137 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/CASCommon/ViewModel/Wizard/ButtonsPanelStateTemplates/CancelTemplate.cs $
//  $Id: CancelTemplate.cs 11137 2014-12-22 13:26:23Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.Common.ViewModel.Wizard.ButtonsPanelStateTemplates
{
  /// <summary>
  /// Class CancelBaseTemplate - template providing template wit the Cancel button.
  /// </summary>
  public class CancelTemplate : BaseTemplate
  {

    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelTemplate"/> class.
    /// </summary>
    public CancelTemplate(string leftButtonTitle, string leftMiddleButtonTitle, string rightMiddleButtonTitle)
      : base(leftButtonTitle, leftMiddleButtonTitle, rightMiddleButtonTitle, Properties.Resources.CanceButtonTitle)
    { }
    /// <summary>
    /// Gets the cancel button position in the array of actions.
    /// </summary>
    /// <value>The cancel position.</value>
    public StateMachineEventIndex CancelPosition { get { return m_ButtonIndex; } }
    /// <summary>
    /// Called when only Cancel button must be active.
    /// </summary>
    public StateMachineEvents OnlyCancel()
    {
      return SetEventsMask(false, false, false, true);
    }
    /// <summary>
    /// Saves the stats and sets the events mask for the selected buttons. The Cancel button is enabled
    /// </summary>
    /// <param name="leftButton">if set to <c>true</c> left button is enabled.</param>
    /// <param name="leftMiddleButton">if set to <c>true</c> left-middle button is enabled.</param>
    /// <param name="rightMiddleButton">if set to <c>true</c> right-middle button i s enabled.</param>
    /// <returns>Return the mask of type <see cref="StateMachineEvents" /> repressing enabled buttons.</returns>
    public StateMachineEvents SetEventsMask(bool leftButton, bool leftMiddleButton, bool rightMiddleButton)
    {
      return base.SetEventsMask(leftButton, leftMiddleButton, rightMiddleButton, true);
    }
    #endregion    

    #region MyRegion
    private const StateMachineEventIndex m_ButtonIndex = StateMachineEventIndex.RightButtonEvent;
    #endregion

  }
}
