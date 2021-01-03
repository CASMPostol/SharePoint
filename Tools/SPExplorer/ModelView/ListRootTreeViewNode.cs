//<summary>
//  Title   : ListRootTreeViewNode
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/SPExplorer/ModelView/ListRootTreeViewNode.cs $
//  $Id: ListRootTreeViewNode.cs 10838 2014-10-10 14:13:54Z mpostol $
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
  internal class ListRootTreeViewNode : FolderTreeViewNode
  {
    private ListRootTreeViewNode()
      : base(TreeViewIcon.TreeViewIconList)
    {
      this.Header = "Lists";
    }
    internal static ListRootTreeViewNode CreateTreeViewNode(PRList[] value)
    {
      ListRootTreeViewNode _ret = new ListRootTreeViewNode();
      _ret.Items = new ObservableCollection<TreeViewNode>(ListTreeViewNode.CreateTreeViewNodes(value).Cast<TreeViewNode>());
      return _ret;
    }
  }
}
