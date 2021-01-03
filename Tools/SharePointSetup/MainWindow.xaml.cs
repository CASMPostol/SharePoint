//<summary>
//  Title   : MainWindow - Interaction logic.
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-02-11 22:33:21 +0100 (wt., 11 lut 2014) $
//  $Rev: 10316 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/SharePointSetup/MainWindow.xaml.cs $
//  $Id: MainWindow.xaml.cs 10316 2014-02-11 21:33:21Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using CAS.SharePoint.Setup.Controls;
using CAS.SharePoint.Setup.StateMachine;
using Microsoft.SharePoint;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

namespace CAS.SharePoint.Setup
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public sealed partial class MainWindow: Window, IDisposable
  {

    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow" /> class.
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();
      this.Loaded += MainWindow_Loaded;
    }
    #endregion

    #region State machine
    private MainWindowStateMachine m_MainWindowStateMachine = null;
    private class MainWindowStateMachine: StateMachineContext
    {
      #region creator
      internal MainWindowStateMachine( MainWindow parent )
      {
        m_Parent = parent;
      }
      #endregion

      #region private
      #region StateMachineContext View Model
      protected override void CancelInstallation()
      {
        m_Parent.ExitlInstallation();
      }
      protected override void ExitlInstallation()
      {
        m_Parent.ExitlInstallation();
      }
      protected override void Uninstallation()
      {
        //  m_UninstallUserControl.Uninstallation( m_ApplicationState );
      }
      protected override void Update()
      {
        m_Parent.Update( Machine.GetEventMask(), Machine.State );
      }
      protected override void Setup()
      {
        m_Parent.Setup( Machine.GetEventMask(), Machine.State );
      }
      #endregion

      private MainWindow m_Parent { get; set; }
      #endregion
    }

    #region StateMachineContext View Model
    private void ExitlInstallation()
    {
      Tracing.TraceEvent.TraceVerbose( 532, "ExitlInstallation", "Closing the application" );
      this.Close();
    }
    private void Update( StateMachineContext.Events expectedEvents, StateMachineContext.ProcessState state )
    {
      Setup( expectedEvents );
      switch ( state )
      {
        case StateMachineContext.ProcessState.SetupDataDialog:
          SetupSetupDialog();
          SetupDataDialogPanel.Focus();
          break;
        case StateMachineContext.ProcessState.Validation:
          SetupDataDialogPanel.DataContext = m_MainWindowStateMachine.InstallationDescription.Wrapper;
          break;
        case StateMachineContext.ProcessState.Installation:
          break;
        case StateMachineContext.ProcessState.Finisched:
          break;
        case StateMachineContext.ProcessState.Uninstall:
          break;
      }
      this.UpdateLayout();
    }
    private void Setup( StateMachineContext.Events expectedEvents, StateMachineContext.ProcessState state )
    {
      Setup( expectedEvents );
      m_ContentTabControl.Items.Clear();
      switch ( state )
      {
        case StateMachineContext.ProcessState.SetupDataDialog:
          m_ContentTabControl.Items.Add( SetupDataDialogPanel );
          SetupSetupDialog();
          UpdateProgressBar( 1 );
          SetupDataDialogPanel.Focus();
          break;
        case StateMachineContext.ProcessState.Validation:
          m_ContentTabControl.Items.Add( ValidationPanel );
          UpdateProgressBar( 2 );
          ValidationPanel.Focus();
          SetupValidationDialog();

          //m_ValidationListBox.Items.Clear();
          //m_ValidationPropertyGrid.SelectedObject = m_ApplicationState.Wrapper;
          //m_ValidationPropertyGrid.Text = Properties.Resources.InstallationProperties;
          break;
        case StateMachineContext.ProcessState.Installation:
          m_ContentTabControl.Items.Add( InstallationPanel );
          UpdateProgressBar( 3 );
          InstallationPanel.Focus();
          InstallationProgressBar.Minimum = 0;
          InstallationProgressBar.Maximum = 10;
          InstallationListBox.AddMessage( "Starting installation - it could take several minutes." );
          break;
        case StateMachineContext.ProcessState.Finisched:
          m_ContentTabControl.Items.Add( FinischedPanel );
          UpdateProgressBar( 4 );
          FinischedPanel.Focus();
          break;
        case StateMachineContext.ProcessState.Uninstall:
          m_ContentTabControl.Items.Add( UninstallPanel );
          UpdateProgressBar( 4 );
          UninstallPanel.Focus();
          break;
      }
      this.UpdateLayout();
    }
    private void Setup( StateMachineContext.Events expectedEvents )
    {
      StackPanelButtons.Children.Clear();
      if ( ( expectedEvents & StateMachineContext.Events.Previous ) != 0 )
        StackPanelButtons.Children.Add( ButtonGoBackward );
      if ( ( expectedEvents & StateMachineContext.Events.Next ) != 0 )
        StackPanelButtons.Children.Add( ButtonGoForward );
      if ( ( expectedEvents & StateMachineContext.Events.Cancel ) != 0 )
        StackPanelButtons.Children.Add( ButtonCancel );
      if ( ( expectedEvents & StateMachineContext.Events.Uninstall ) != 0 )
        StackPanelButtons.Children.Add( ButtonUninstall );
    }
    private void UpdateProgressBar( int step )
    {
      ProgressBarApplicationInstallationProgress.Value = step * ProgressBarApplicationInstallationProgress.Maximum / 4;
      ProgressBarApplicationInstallationProgress.ToolTip = String.Format( "Step {0} from 4.", step );
    }
    #endregion

    #region Setup
    private void SetupSetupDialog()
    {
      SetupDockPanel.Children.Clear();
      if ( ( m_MainWindowStateMachine == null ) || ( m_MainWindowStateMachine.InstallationDescription == null ) )
      {
        SetupLabelReview.Content = @"Open the installation package (Files\Open Package)";
        SetupDockPanel.Children.Add( SetupLabelReview );
        SetupDockPanel.Children.Add( SetupFrame );
      }
      else
      {
        SetupLabelReview.Content = @"Review the installation data.";
        SetupDockPanel.Children.Add( SetupLabelReview );
        InstallationDataEditor _editor = new InstallationDataEditor()
        {
          InstallationData = m_MainWindowStateMachine.InstallationDescription.Wrapper
        };
        //x_SetupPropertyGrid.SelectedObject = m_MainWindowStateMachine.InstallationDescription.Wrapper;
        SetupDockPanel.Children.Add( _editor );
      }
    }
    #endregion

    #region Validation
    private void SetupValidationDialog()
    {
      ValidationListBox.AddMessage( Properties.Resources.ValidationProcessStarting );
    }
    private void ValidationMachineViewModel_ProgressChanged( object sender, ProgressChangedEventArgs e )
    {
      ValidationListBox.AddMessage( (string)e.UserState );
    }
    #endregion

    #region Installation
    private void InstallationMachineViewModel_ProgressChanged( object sender, System.ComponentModel.ProgressChangedEventArgs e )
    {
      InstallationListBox.AddMessage( (string)e.UserState );
    }
    private void InstallationMachineViewModel_OnSPWebTeplateSelection( object sender, SPWebTeplateEventArgs e )
    {
      try
      {
        ValidationListBox.AddMessage( (string)e.UserState );
        SPWebTemplate _selectedTemplate = null;
        foreach ( SPWebTemplate _tmplt in e.WebTemplates )
        {
          if ( !m_MainWindowStateMachine.InstallationDescription.SiteTemplate.Contains( _tmplt.Name ) )
            continue;
          _selectedTemplate = _tmplt;
          break;
        }
        if ( _selectedTemplate == null )
        {
          Controls.WebTemplateDialog _dialog = new WebTemplateDialog();
          _dialog.TemplatesCollection = e.WebTemplates;
          _dialog.ShowDialog();
          if ( !_dialog.DialogResult.GetValueOrDefault( false ) )
          {
            Tracing.TraceEvent.TraceWarning( 97, "OnSPWebTeplateSelection", "Canceled template selection" );
            return;
          }
          _selectedTemplate = _dialog.WebTemplate;
        }
        m_MainWindowStateMachine.InstallationDescription.SiteTemplate = _selectedTemplate.Name;
        m_MainWindowStateMachine.InstallationDescription.SPWebTemplate = _selectedTemplate;
        string _msg = String.Format( "New template selected: {0}", Controls.WebTemplateDialog.SPWebTemplateToString( _selectedTemplate ) );
        Tracing.TraceEvent.TraceInformation( 97, "OnSPWebTeplateSelection", _msg );
      }
      finally
      {
        e.SPWebTeplateSelected();
      }
    }
    #endregion

    #endregion

    #region IDisposable Members
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      if ( TracingComponent != null )
        TracingComponent.Dispose();
    }
    private Tracing TracingComponent { get; set; }
    #endregion

    #region Events
    private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
    {
      Dispose();
      e.Cancel = false;
    }
    private void MainWindow_Loaded( object sender, RoutedEventArgs e )
    {
      AssemblyName _name = Assembly.GetExecutingAssembly().GetName();
      this.Title = this.Title + " Rel " + _name.Version.ToString( 4 );
      TracingComponent = new Tracing();
      Tracing.TraceEvent.TraceVerbose( 31, "MainWindow", "Starting the application" );
      m_MainWindowStateMachine = new MainWindowStateMachine( this );
      // TODO Context.ProcessState.SetupDataDialog:
      // Context.ProcessState.Validation:
      m_MainWindowStateMachine.ValidationMachineViewModel.ProgressChanged += ValidationMachineViewModel_ProgressChanged;
      ValidationListBox.SelectionChanged += ValidationListBox_SelectionChanged; // new EventHandler( this.m_ListBox_TextChanged );
      // TODO Context.ProcessState.Installation:
      m_MainWindowStateMachine.InstallationMachineViewModel.ProgressChanged += InstallationMachineViewModel_ProgressChanged;
      m_MainWindowStateMachine.InstallationMachineViewModel.OnSPWebTeplateSelection += InstallationMachineViewModel_OnSPWebTeplateSelection;
      InstallationListBox.SelectionChanged += InstallationListBox_SelectionChanged;

      // TODO Context.ProcessState.Finisched:
      // TODO Context.ProcessState.Uninstall:
      m_MainWindowStateMachine.OpenEntryState();
    }
    private void ValidationListBox_SelectionChanged( object sender, SelectionChangedEventArgs e )
    {
      TraceListBox( (ListBox)sender, StateMachineContext.ProcessState.Validation );
    }
    private void InstallationListBox_SelectionChanged( object sender, SelectionChangedEventArgs e )
    {
      TraceListBox( (ListBox)sender, StateMachineContext.ProcessState.Installation );
      if ( InstallationProgressBar.Value == InstallationProgressBar.Maximum )
        InstallationProgressBar.Maximum *= 2;
      InstallationProgressBar.Value++;
      //TODO remove InstallationProgressBar.Refresh();
    }
    private static void TraceListBox( ListBox sender, StateMachine.StateMachineContext.ProcessState state )
    {
      Tracing.TraceEvent.TraceInformation( 270, state.ToString(), sender.Items[ sender.Items.Count - 1 ].ToString() );
    }

    #region Menu
    private void MenuItemFiles_SubmenuOpened( object sender, RoutedEventArgs e )
    {
      bool _IsEnabled = m_MainWindowStateMachine.IsPackagePropertiesEditable;
      MenuItemView.IsEnabled = _IsEnabled;
      x_MenuItemEdot.IsEnabled = _IsEnabled;
    }
    private void MenuItemView_Click( object sender, RoutedEventArgs e )
    {
      Package.PackageDialog _pd = new Package.PackageDialog();
      _pd.View( m_MainWindowStateMachine.PackageProperties );
    }
    private void MenuItemEdit_Click( object sender, RoutedEventArgs e )
    {
      Package.PackageDialog _pd = new Package.PackageDialog();
      if ( !_pd.Edit( m_MainWindowStateMachine.PackageProperties ).GetValueOrDefault( false ) )
        return;
      m_MainWindowStateMachine.PackageProperties = _pd.PackageProperties;
    }
    private void MenuItemOpenSolution_Click( object sender, RoutedEventArgs e )
    {
      try
      {
        Package.PackageDialog _pd = new Package.PackageDialog();
        if ( !_pd.Open().GetValueOrDefault( false ) )
          return;
        m_MainWindowStateMachine.NewPackageProperties( _pd.PackageProperties );
        ToolBarSolutionLabel.Content = m_MainWindowStateMachine.InstallationDescription.Title;
      }
      catch ( Exception ex )
      {
        ToolBarSolutionLabel.Content = "Error";
        string _message = String.Format( Properties.Resources.LastOperationFailed, ex.Message );
        Tracing.TraceEvent.TraceError( 49, "MenuItemOpenSolution_Click", _message );
        MessageBox.Show( _message, "Open solution", MessageBoxButton.OK, MessageBoxImage.Error );
      }
    }
    private void MenuItemPublishPackage_Click( object sender, RoutedEventArgs e )
    {
      using ( FolderBrowserDialog _directoryPicker = new FolderBrowserDialog() )
      {
        _directoryPicker.ShowNewFolderButton = false;
        _directoryPicker.SelectedPath = m_MainWindowStateMachine.PackageProperties.Location.DirectoryName;
        _directoryPicker.Description = "Folder containing the installation package content";
        switch ( _directoryPicker.ShowDialog() )
        {
          case System.Windows.Forms.DialogResult.OK:
            string _pext = ".package";
            string _filterFormat = String.Format( "Installation package ({0})|*{0}", _pext );
            DirectoryInfo _drctry = Directory.GetParent( _directoryPicker.SelectedPath );
            Microsoft.Win32.SaveFileDialog _filePicker = new Microsoft.Win32.SaveFileDialog()
               {
                 CheckFileExists = false,
                 AddExtension = true,
                 DefaultExt = _pext,
                 FileName = "CASSmartFactory",
                 InitialDirectory = _drctry.FullName,
                 Filter = _filterFormat // Filter files by extension
               };
            if ( _filePicker.ShowDialog().GetValueOrDefault( false ) )
              CAS.SharePoint.Tools.Packaging.InstallationPackage.PublishPackage( _directoryPicker.SelectedPath, _filePicker.FileName, m_MainWindowStateMachine.PackageProperties.UpdateProperties );
            break;
          default:
            break;
        }
      }
    }
    private void MenuItemExit_Click( object sender, RoutedEventArgs e )
    {
      this.Close();
    }
    #endregion

    #region Buttons
    private void ButtonGoBackward_Click( object sender, RoutedEventArgs e )
    {
      m_MainWindowStateMachine.Previous();
    }
    private void ButtonGoForward_Click( object sender, RoutedEventArgs e )
    {
      m_MainWindowStateMachine.Next();
    }
    private void ButtonUninstall_Click( object sender, RoutedEventArgs e )
    {
      m_MainWindowStateMachine.Uninstall();
    }
    private void ButtonCancel_Click( object sender, RoutedEventArgs e )
    {
      if ( MessageBox.Show( Properties.Resources.AreYouSure2Cancel, Properties.Resources.CancelInstallationCaption, MessageBoxButton.OKCancel, MessageBoxImage.Question ) != MessageBoxResult.OK )
        return;
      m_MainWindowStateMachine.Cancel();
    }
    #endregion

    #endregion

  }
}

