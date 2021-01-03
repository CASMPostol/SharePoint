//<summary>
//  Title   : MainWindow
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-07-03 10:44:46 +0200 (czw., 03 lip 2014) $
//  $Rev: 10544 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Tools/SPExplorer/MainWindow.xaml.cs $
//  $Id: MainWindow.xaml.cs 10544 2014-07-03 08:44:46Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Tools.SPExplorer.ModelView;
using System;
using System.Windows;

namespace CAS.SharePoint.Tools.SPExplorer
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    //creators
    public MainWindow()
    {
      InitializeComponent();
      this.DataContext = new MainWindowModelView();
    }
    //helpers
    private MainWindowModelView MainWindowModelViewDataContext
    {
      get { return (MainWindowModelView)this.DataContext; }
    }
    //event handlers
    protected override void OnClosed(EventArgs e)
    {
      ((MainWindowModelView)this.DataContext).Dispose();
      base.OnClosed(e);
    }
    private void v_TreeView_Expanded(object sender, RoutedEventArgs e)
    {
      if (MainWindowModelViewDataContext.TreeViewExpanded == null)
        return;
      MainWindowModelViewDataContext.TreeViewExpanded(sender, e);
    }
    private void v_TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      if (MainWindowModelViewDataContext.TreeViewSelectedItemChanged == null)
        return;
      MainWindowModelViewDataContext.TreeViewSelectedItemChanged(sender, e);
    }
    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

  }
}
