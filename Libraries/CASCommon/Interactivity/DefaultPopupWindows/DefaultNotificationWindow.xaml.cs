//<summary>
//  Title   : class DefaultNotificationWindow
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-17 17:07:46 +0200 (pt., 17 paź 2014) $
//  $Rev: 10871 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/CASCommon/Interactivity/DefaultPopupWindows/DefaultNotificationWindow.xaml.cs $
//  $Id: DefaultNotificationWindow.xaml.cs 10871 2014-10-17 15:07:46Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.Interactivity.InteractionRequest;
using System.Windows;

namespace CAS.Common.Interactivity.DefaultPopupWindows
{
  /// <summary>
  /// Interaction logic for NotificationChildWindow.xaml
  /// </summary>
  public partial class DefaultNotificationWindow : Window
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
