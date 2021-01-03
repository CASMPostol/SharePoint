//<summary>
//  Title   : ColumsRootTreeViewNode
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-06-30 00:01:22 +0200 (pon., 30 cze 2014) $
//  $Rev: 10529 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Tools/SPExplorer/ModelView/ColumsRootTreeViewNode.cs $
//  $Id: ColumsRootTreeViewNode.cs 10529 2014-06-29 22:01:22Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Collections.ObjectModel;
using System.Linq;

namespace CAS.SharePoint.Tools.SPExplorer.ModelView
{
  internal class ColumsRootTreeViewNode : FolderTreeViewNode
  {
    internal static ColumsRootTreeViewNode CreateTreeViewNode(SPMetalParameters.PRColumn[] pRColumn)
    {
      ColumsRootTreeViewNode _ret = new ColumsRootTreeViewNode();
      _ret.Items = new ObservableCollection<TreeViewNode>(ColumnTreeViewNode.CreateTreeViewNodes(pRColumn).Cast<TreeViewNode>());
      return _ret;
    }
    private ColumsRootTreeViewNode() : base(TreeViewIcon.TreeViewTreeField) { Header = "Columns"; }
  }
}
