//<summary>
//  Title   : ButtonsPanelState
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2014-12-22 14:26:23 +0100 (pon., 22 gru 2014) $
//  $Rev: 11137 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/ViewModel/Wizard/ButtonsPanelState.cs $
//  $Id: ButtonsPanelState.cs 11137 2014-12-22 13:26:23Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Windows;

namespace CAS.Common.ViewModel.Wizard
{
  //TODO - rename the file
  /// <summary>
  /// Class ButtonsSetState - represents state of the buttons set
  /// </summary>
  public class ButtonsSetState
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="ButtonsSetState"/> class.
    /// </summary>
    public ButtonsSetState(string leftButtonTitle, string leftMiddleButtonTitle, string rightMiddleButtonTitle, string rightButtonTitle)
    {
      b_LeftButtonTitle = leftButtonTitle;
      b_LeftMiddleButtonTitle = leftMiddleButtonTitle;
      b_RightMiddleButtonTitle = rightMiddleButtonTitle;
      b_RightButtonTitle = rightButtonTitle;
      InitializeEnabledEvents();
    }
    #endregion

    #region public API
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
    }
    /// <summary>
    /// Gets or sets the left-middle button title.
    /// </summary>
    /// <value>The left-middle button title.</value>
    public string LeftMiddleButtonTitle
    {
      get
      {
        return b_LeftMiddleButtonTitle;
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
    }
    /// <summary>
    /// Gets or sets the right-middle button title.
    /// </summary>
    /// <value>The right middle button title.</value>
    public string RightMiddleButtonTitle
    {
      get
      {
        return b_RightMiddleButtonTitle;
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
        return this.SetVisibility(LeftButtonTitle);
      }
    }
    /// <summary>
    /// Gets or sets the left-middle button visibility.
    /// </summary>
    /// <value>The left middle-button visibility.</value>
    public Visibility LeftMiddleButtonVisibility
    {
      get
      {
        return this.SetVisibility(LeftMiddleButtonTitle);
      }
    }
    /// <summary>
    /// Gets or sets the right-middle button visibility.
    /// </summary>
    /// <value>The right middle button visibility.</value>
    public Visibility RightMiddleButtonVisibility
    {
      get
      {
        return this.SetVisibility(RightMiddleButtonTitle);
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
        return this.SetVisibility(RightButtonTitle);
      }
    }
    /// <summary>
    /// Sets the events mask for the selected buttons.
    /// </summary>
    /// <param name="leftButton">if set to <c>true</c> left button is enabled.</param>
    /// <param name="leftMiddleButton">if set to <c>true</c> left-middle button is enabled.</param>
    /// <param name="rightMiddleButton">if set to <c>true</c> right-middle button i s enabled.</param>
    /// <param name="rightButton">if set to <c>true</c> right button is enabled.</param>
    /// <returns>Return the mask of type <see cref="StateMachineEvents"/> repressing enabled buttons.</returns>
    public virtual StateMachineEvents SetEventsMask(bool leftButton, bool leftMiddleButton, bool rightMiddleButton, bool rightButton)
    {
      EnabledEvents = leftButton ? StateMachineEvents.LeftButtonEvent : 0;
      EnabledEvents |= leftMiddleButton ? StateMachineEvents.LeftMiddleButtonEvent : 0;
      EnabledEvents |= rightMiddleButton ? StateMachineEvents.RightMiddleButtonEvent : 0;
      EnabledEvents |= rightButton ? StateMachineEvents.RightButtonEvent : 0;
      return EnabledEvents;
    }
    /// <summary>
    /// Gets the mask for the enabled events.
    /// </summary>
    /// <value>The get enabled events.</value>
    public StateMachineEvents GetEnabledEvents { get { return EnabledEvents; } }
    #endregion

    #region private
    /// <summary>
    /// The enabled events - current state of the buttons set
    /// </summary>
    protected StateMachineEvents EnabledEvents;
    private const Visibility m_DefaultVisibility = Visibility.Collapsed;
    private string b_LeftButtonTitle;
    private string b_LeftMiddleButtonTitle;
    private string b_RightButtonTitle;
    private string b_RightMiddleButtonTitle;
    private Visibility SetVisibility(string title)
    {
      return string.IsNullOrEmpty(title) ? m_DefaultVisibility : Visibility.Visible;
    }
    private void InitializeEnabledEvents()
    {
      SetEventsMask(LeftButtonVisibility == Visibility.Visible, LeftMiddleButtonVisibility == Visibility.Visible, RightMiddleButtonVisibility == Visibility.Visible, RightButtonVisibility == Visibility.Visible);
    }
    #endregion

  }

}
