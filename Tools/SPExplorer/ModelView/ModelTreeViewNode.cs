//<summary>
//  Title   : class ModelTreeViewNode
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 14:20:07 +0200 (pt., 10 paź 2014) $
//  $Rev: 10836 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Tools/SPExplorer/ModelView/ModelTreeViewNode.cs $
//  $Id: ModelTreeViewNode.cs 10836 2014-10-10 12:20:07Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.ComponentModel;

namespace CAS.SharePoint.Tools.SPExplorer.ModelView
{
  internal class ModelTreeViewNode : TreeViewNode
  {
    internal ModelTreeViewNode(ICheckable modelParent, TreeViewIcon icon)
      : base(icon)
    {
      b_ModelParent = modelParent;
      b_Checked = b_ModelParent.Checked;
    }
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="ModelTreeViewNode"/> is checked.
    /// </summary>
    /// <value>
    ///   <c>true</c> if checked; otherwise, <c>false</c>.
    /// </value>
    public override bool Checked
    {
      get
      {
        return b_Checked;
      }
      set
      {
        b_Checked = ModelParent.Checked;
        if (!RaiseHandler<bool>(value, ref b_Checked, "Checked", this))
          return;
        ModelParent.Checked = b_Checked;
        if (Items == null)
          return;
        foreach (TreeViewNode _node in Items)
          _node.Checked = value;
      }
    }
    public ICheckable ModelParent
    {
      get
      {
        return b_ModelParent;
      }
    }

    private ICheckable b_ModelParent;
    private bool b_Checked;

  }
}
