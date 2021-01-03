//<summary>
//  Title   : TreeViewNode
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 14:20:07 +0200 (pt., 10 paź 2014) $
//  $Rev: 10836 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Tools/SPExplorer/ModelView/TreeViewNode.cs $
//  $Id: TreeViewNode.cs 10836 2014-10-10 12:20:07Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.ComponentModel;
using System.Collections.ObjectModel;

namespace CAS.SharePoint.Tools.SPExplorer.ModelView
{
  public enum TreeViewIcon
  {
    TreeViewIconClassFriend, TreeViewIconList, TreeViewRoot, TreeViewTreeField
  }
  public abstract class TreeViewNode : PropertyChangedBase, ICheckable
  {

    #region creator
    public TreeViewNode(TreeViewIcon icon)
    {
      switch (icon)
      {
        case TreeViewIcon.TreeViewIconClassFriend:
          b_ImagePath = Properties.Settings.Default.TreeViewIconClassFriend;
          break;
        case TreeViewIcon.TreeViewIconList:
          b_ImagePath = Properties.Settings.Default.TreeViewIconList;
          break;
        case TreeViewIcon.TreeViewRoot:
          b_ImagePath = Properties.Settings.Default.TreeViewRoot;
          break;
        case TreeViewIcon.TreeViewTreeField:
          b_ImagePath = Properties.Settings.Default.TreeViewTreeField;
          break;
        default:
          break;
      }
    }
    #endregion

    #region View API
    public string Header
    {
      get
      {
        return b_Header;
      }
      set
      {
        RaiseHandler<string>(value, ref b_Header, "Header", this);
      }
    }
    public ObservableCollection<TreeViewNode> Items
    {
      get
      {
        return b_Items;
      }
      set
      {
        RaiseHandler<System.Collections.ObjectModel.ObservableCollection<TreeViewNode>>(value, ref b_Items, "Items", this);
      }
    }
    public string ImagePath
    {
      get
      {
        //return b_ImagePath;
        return @"imgs\Class-Friend_491.png";
      }
      set
      {
        RaiseHandler<string>(value, ref b_ImagePath, "ImagePath", this);
      }
    }
    #endregion

    #region ICheckable
    public abstract bool Checked { get; set; }
    #endregion

    #region private
    private string b_ImagePath = @"imgs\Class-Friend_491.png";
    private ObservableCollection<TreeViewNode> b_Items;
    private string b_Header;
    #endregion

  }
}
