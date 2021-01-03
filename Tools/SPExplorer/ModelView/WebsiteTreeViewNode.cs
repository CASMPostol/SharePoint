//<summary>
//  Title   : WebsiteTreeViewNode
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/SPExplorer/ModelView/WebsiteTreeViewNode.cs $
//  $Id: WebsiteTreeViewNode.cs 10838 2014-10-10 14:13:54Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Tools.SPExplorer.SPMetalParameters;
using System.Collections.ObjectModel;
using System.Linq;

namespace CAS.SharePoint.Tools.SPExplorer.ModelView
{
  /// <summary>
  /// class WebsiteTreeViewNode
  /// </summary>
  internal class WebsiteTreeViewNode : ModelTreeViewNode
  {
    internal static WebsiteTreeViewNode CreateChildren(PRWeb parent, string url)
    {
      WebsiteTreeViewNode _ret = new WebsiteTreeViewNode(parent, url);
      _ret.Items = new ObservableCollection<TreeViewNode>();
      _ret.Items.Add(ListRootTreeViewNode.CreateTreeViewNode(parent.List));
      _ret.Items.Add(ContentTypeRootTreeViewNode.CreateTreeViewNode(parent.ContentType.OrderBy<PRContentType, string>(_pctx => _pctx.Name).ToArray<PRContentType>()));
      _ret.Items.Add(ColumsRootTreeViewNode.CreateTreeViewNode(parent.SharePointColumns));
      return _ret;
    }
    private WebsiteTreeViewNode(PRWeb parent, string url)
      : base(parent, TreeViewIcon.TreeViewRoot)
    {
      this.Header = url;
    }
  }
}
