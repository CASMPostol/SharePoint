//<summary>
//  Title   : IButtonsPanelBase
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
      
      
using CAS.Common.ViewModel;
using System;
using System.Windows;

namespace CAS.Common.ViewModel.Wizard
{
  /// <summary>
  /// Interface IButtonsPanelBase - base to export button panels ViewModels
  /// </summary>
  public interface IButtonsPanelBase
  {
    /// <summary>
    /// Gets the left button command.
    /// </summary>
    /// <value>The left button command.</value>
    ICommandWithUpdate LeftButtonCommand { get; set; }
    /// <summary>
    /// Gets the left button title.
    /// </summary>
    /// <value>The left button title.</value>
    string LeftButtonTitle { get; }
    /// <summary>
    /// Gets or sets the left button visibility.
    /// </summary>
    /// <value>The left button visibility.</value>
    Visibility LeftButtonVisibility { get; set; }
    /// <summary>
    /// Gets or sets the left middle button command.
    /// </summary>
    /// <value>The left middle button command.</value>
    ICommandWithUpdate LeftMiddleButtonCommand { get; set; }
    /// <summary>
    /// Gets or sets the left middle button title.
    /// </summary>
    /// <value>The left middle button title.</value>
    string LeftMiddleButtonTitle { get; set; }
    /// <summary>
    /// Gets or sets the left middle button visibility.
    /// </summary>
    /// <value>The left middle button visibility.</value>
    Visibility LeftMiddleButtonVisibility { get; set; }
    /// <summary>
    /// Gets or sets the right button command.
    /// </summary>
    /// <value>The right button command.</value>
    ICommandWithUpdate RightButtonCommand { get; set; }
    /// <summary>
    /// Gets or sets the right button title.
    /// </summary>
    /// <value>The right button title.</value>
    string RightButtonTitle { get; set; }
    /// <summary>
    /// Gets or sets the right button visibility.
    /// </summary>
    /// <value>The right button visibility.</value>
    Visibility RightButtonVisibility { get; set; }
    /// <summary>
    /// Gets or sets the right middle button command.
    /// </summary>
    /// <value>The right middle button command.</value>
    ICommandWithUpdate RightMiddleButtonCommand { get; set; }
    /// <summary>
    /// Gets or sets the right middle button title.
    /// </summary>
    /// <value>The right middle button title.</value>
    string RightMiddleButtonTitle { get; set; }
    /// <summary>
    /// Gets or sets the right middle button visibility.
    /// </summary>
    /// <value>The right middle button visibility.</value>
    Visibility RightMiddleButtonVisibility { get; set; }
  }
}
