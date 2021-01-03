using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CAS.SharePoint.Setup.Controls
{
  /// <summary>
  /// Interaction logic for InstallationDataEditor.xaml
  /// </summary>
  public partial class InstallationDataEditor: UserControl
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InstallationDataEditor" /> class.
    /// </summary>
    public InstallationDataEditor()
    {
      InitializeComponent();
      this.Loaded += InstallationDataEditor_Loaded;
    }
    internal Package.InstallationStateDataWrapper InstallationData
    {
      get { return this.DataContext as Package.InstallationStateDataWrapper; }
      set
      {
        this.DataContext = value;
        //x_GlobalData.DataContext = value;
      }
    }
    void InstallationDataEditor_Loaded( object sender, RoutedEventArgs e )
    {
    }
    private void UserControl_Initialized( object sender, EventArgs e )
    {

    }
    private void x_MenuItemPriorityAscending_Click( object sender, RoutedEventArgs e )
    {
      InstallationData.SortSolutions( ListSortDirection.Ascending );
    }
    private void x_MenuItemPriorityDescending_Click( object sender, RoutedEventArgs e )
    {
      InstallationData.SortSolutions( ListSortDirection.Descending );
    }
  }
}
