using System;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CAS.SharePoint.Tools.Packaging;

namespace CAS.SharePoint.Setup.Package
{
  /// <summary>
  /// Interaction logic for PackageDialog.xaml
  /// </summary>
  public partial class PackageDialog: Window
  {

    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="PackageDialog" /> class.
    /// </summary>
    public PackageDialog()
    {
      InitializeComponent();
    }
    #endregion

    #region public
    /// <summary>
    /// Reads the installation description and ShowDialog.
    /// </summary>
    /// <returns></returns>
    internal bool? Open()
    {
      StackPanelButtons.Children.Clear();
      StackPanelButtons.Children.Add( ButtonOK );
      StackPanelButtons.Children.Add( ButtonCancel );
      // Configure open file dialog box
      Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
      dlg.FileName = Properties.Settings.Default.PackageName; // Default file name
      dlg.DefaultExt = Properties.Settings.Default.PackageExtension; // Default file extension
      string _filterFormat = "Installation package ({0})|*{0}";
      dlg.Filter = String.Format( _filterFormat, Properties.Settings.Default.PackageExtension ); // Filter files by extension
      // Process open file dialog box results
      if ( !dlg.ShowDialog().GetValueOrDefault( false ) )
        return false;
      FileInfo _location = new FileInfo( dlg.FileName );
      PackageProperties = InstalationPackageProperties.GetProperties( InstallationPackage.ReadPackageProperties( _location ), _location );
      x_MainGrid.IsEnabled = false;
      return this.ShowDialog();
    }
    internal bool? Edit( InstalationPackageProperties properties )
    {
      PackageProperties = properties.GetProperties();
      bool? _resulr = ShowDialog();
      m_PackageProperties.Modified = DateTime.Now;
      return _resulr;
    }
    internal void View( InstalationPackageProperties properties )
    {
      PackageProperties = properties;
      x_MainGrid.IsEnabled = false;
      ButtonCancel.IsEnabled = false;
      ShowDialog();
    }
    internal InstalationPackageProperties PackageProperties
    {
      get
      {
        return m_PackageProperties;
      }
      private set
      {
        m_PackageProperties = value;
        this.DataContext = value;
      }
    }
    /// <summary>
    /// Gets the package file information.
    /// </summary>
    /// <value>
    /// <see cref="FileInfo " /> containing information about the package file.
    /// </value>
    #endregion

    #region private
    private InstalationPackageProperties m_PackageProperties;

    #region private event hanlers
    private void ButtonOK_Click( object sender, RoutedEventArgs e )
    {
      this.DialogResult = true;
      this.Close();
    }
    private void ButtonCancel_Click( object sender, RoutedEventArgs e )
    {
      this.DialogResult = false;
      this.Close();
    }
    #endregion

    #endregion

  }
}
