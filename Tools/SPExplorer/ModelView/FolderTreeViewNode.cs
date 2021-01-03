//<summary>
//  Title   : class FolderTreeViewNode
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-06-30 14:06:30 +0200 (pon., 30 cze 2014) $
//  $Rev: 10531 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/SPExplorer/ModelView/FolderTreeViewNode.cs $
//  $Id: FolderTreeViewNode.cs 10531 2014-06-30 12:06:30Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SharePoint.Tools.SPExplorer.ModelView
{
  public class FolderTreeViewNode : TreeViewNode
  {

    public FolderTreeViewNode(TreeViewIcon icon)
      : base(icon)
    { }
    public override bool Checked
    {
      get
      {
        return b_Checked;
      }
      set
      {
        if (!RaiseHandler<bool>(value, ref b_Checked, "Checked", this))
          return;
        if (Items == null)
          return;
        foreach (TreeViewNode _node in Items)
          _node.Checked = b_Checked;
      }
    }
    private bool b_Checked = false;

  }
}
