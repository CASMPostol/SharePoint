//<summary>
//  Title   : ColumnTreeViewNode
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/SPExplorer/ModelView/ColumnTreeViewNode.cs $
//  $Id: ColumnTreeViewNode.cs 10838 2014-10-10 14:13:54Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Tools.SPExplorer.SPMetalParameters;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SharePoint.Tools.SPExplorer.ModelView
{
  internal class ColumnTreeViewNode : ModelTreeViewNode
  {

    private string b_Member;
    public string Member
    {
      get
      {
        return b_Member;
      }
      set
      {
        RaiseHandler<string>(value, ref b_Member, "Member", this);
        ((PRColumn)ModelParent).Member = b_Member;
      }
    } 
                
    internal static IEnumerable<ColumnTreeViewNode> CreateTreeViewNodes(PRColumn[] pRColumn)
    {
      return pRColumn.AsEnumerable<PRColumn>().Select<PRColumn, ColumnTreeViewNode>(x => new ColumnTreeViewNode(x) { Header = x.ToString() });
    }
    private ColumnTreeViewNode(PRColumn modelParent) : base(modelParent, TreeViewIcon.TreeViewTreeField) 
    {
      b_Member = modelParent.Member;
    }
  }
}
