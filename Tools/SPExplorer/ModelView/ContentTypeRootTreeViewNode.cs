//<summary>
//  Title   : ContentTypeRootTreeViewNode 
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Tools/SPExplorer/ModelView/ContentTypeRootTreeViewNode.cs $
//  $Id: ContentTypeRootTreeViewNode.cs 10838 2014-10-10 14:13:54Z mpostol $
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
  internal class ContentTypeRootTreeViewNode : FolderTreeViewNode
  {
    private ContentTypeRootTreeViewNode()
      : base(TreeViewIcon.TreeViewIconClassFriend)
    {
      Header = "Content types";
    }
    internal static ContentTypeRootTreeViewNode CreateTreeViewNode(SPMetalParameters.PRContentType[] value)
    {
      ContentTypeRootTreeViewNode _ret = new ContentTypeRootTreeViewNode();
      if (value != null)
        _ret.Items = new ObservableCollection<TreeViewNode>(value.AsEnumerable<PRContentType>().Select<PRContentType, TreeViewNode>(x => ContentTypeTreeViewNode.CreateTreeViewNode(x)));
      return _ret;
    }
  }
}
