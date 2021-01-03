//<summary>
//  Title   : class DefaultNotificationWindow
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-12-10 16:16:26 +0100 (śr., 10 gru 2014) $
//  $Rev: 11083 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/Interactivity/DefaultPopupWindows/DefaultNotificationWindow.sl.xaml.cs $
//  $Id: DefaultNotificationWindow.sl.xaml.cs 11083 2014-12-10 15:16:26Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.Interactivity.InteractionRequest;
using System.Windows;
using System.Windows.Controls;

namespace CAS.Common.Interactivity.DefaultPopupWindows
{
  /// <summary>
  /// Interaction logic for NotificationChildWindow.xaml
  /// </summary>
  public partial class DefaultNotificationWindow : ChildWindow
  {
    /// <summary>
    /// Creates a new instance of <see cref="DefaultNotificationWindow"/>
    /// </summary>
    public DefaultNotificationWindow()
    {
      InitializeComponent();
    }
    /// <summary>
    /// Sets or gets the <see cref="INotification"/> shown by this window./>
    /// </summary>
    public INotification Notification
    {
      get
      {
        return this.DataContext as INotification;
      }
      set
      {
        this.DataContext = value;
      }
    }
    private void OKButton_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}

