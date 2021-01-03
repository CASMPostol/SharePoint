using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using Microsoft.SharePoint;

namespace CAS.SharePoint.Setup.Controls
{
  /// <summary>
  /// Interaction logic for WebTemplateDialog.xaml
  /// </summary>
  public partial class WebTemplateDialog: Window
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="WebTemplateDialog" /> class.
    /// </summary>
    public WebTemplateDialog()
    {
      InitializeComponent();
    }
    /// <summary>
    /// Gets or sets the templates collection.
    /// </summary>
    /// <value>
    /// The templates collection.
    /// </value>
    public SPWebTemplateCollection TemplatesCollection
    {
      get { return bp_TemplatesCollection; }
      set
      {
        bp_TemplatesCollection = value;
        List<string> _list = new List<string>();
        foreach ( SPWebTemplate item in value )
        {
          _list.Add( SPWebTemplateToString (item) );
        }
        this.x_TemplatesListBox.ItemsSource = _list;
      }
    }
    internal SPWebTemplate WebTemplate { get; private set; }
    internal static string SPWebTemplateToString( SPWebTemplate template )
    {
      string _patter = "Title:{0}, Name:{1}, Multilingual:{2}, Lcid:{3}, Root:{4}, Category:{5} - {6}";
      return String.Format( _patter, template.Title, template.Name, template.SupportsMultilingualUI, template.Lcid, template.IsRootWebOnly, template.DisplayCategory, template.Description );
    }
    #region private
    private SPWebTemplateCollection bp_TemplatesCollection = default( SPWebTemplateCollection );
    private void x_OKButton_Click( object sender, RoutedEventArgs e )
    {
      if ( x_TemplatesListBox.SelectedIndex != -1 )
      {
        WebTemplate = TemplatesCollection[ x_TemplatesListBox.SelectedIndex ];
        this.DialogResult = true;
      }
      else
      {
        WebTemplate = null;
        this.DialogResult = false;
      }
      this.Close();
    }
    private void x_CancelButton_Click( object sender, RoutedEventArgs e )
    {
      this.DialogResult = false;
      this.Close();
    }
    private void x_WindowLoaded( object sender, RoutedEventArgs e )
    {
      //this.DialogResult = false;
    }
    #endregion

  }
}
