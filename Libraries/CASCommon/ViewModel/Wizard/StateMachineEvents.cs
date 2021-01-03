//<summary>
//  Title   : StateMachineEvents
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2014-12-22 14:26:23 +0100 (pon., 22 gru 2014) $
//  $Rev: 11137 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/ViewModel/Wizard/StateMachineEvents.cs $
//  $Id: StateMachineEvents.cs 11137 2014-12-22 13:26:23Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;

namespace CAS.Common.ViewModel.Wizard
{
  /// <summary>
  /// Enum defining bit position for the allowed state machine events.
  /// </summary>
  [Flags]
  public enum StateMachineEvents : int
  {
    /// <summary>
    /// The left button event
    /// </summary>
    LeftButtonEvent = 0x1,
    /// <summary>
    /// The left middle button event
    /// </summary>
    LeftMiddleButtonEvent = 0x2,
    /// <summary>
    /// The right middle button event
    /// </summary>
    RightMiddleButtonEvent = 0x4,
    /// <summary>
    /// The right button event
    /// </summary>
    RightButtonEvent = 0x8
  }
  /// <summary>
  /// Enum defining bit position for the allowed state machine events.
  /// </summary>
  public enum StateMachineEventIndex : int
  {
    /// <summary>
    /// The left button event
    /// </summary>
    LeftButtonEvent = 0,
    /// <summary>
    /// The left middle button event
    /// </summary>
    LeftMiddleButtonEvent = 1,
    /// <summary>
    /// The right middle button event
    /// </summary>
    RightMiddleButtonEvent = 2,
    /// <summary>
    /// The right button event
    /// </summary>
    RightButtonEvent = 3
  }
}
