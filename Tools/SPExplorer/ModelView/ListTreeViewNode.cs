﻿//<summary>
//  Title   : ListTreeViewNode
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/SPExplorer/ModelView/ListTreeViewNode.cs $
//  $Id: ListTreeViewNode.cs 10838 2014-10-10 14:13:54Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Tools.SPExplorer.SPMetalParameters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CAS.SharePoint.Tools.SPExplorer.ModelView
{
  /// <summary>
  /// I Root Provider
  /// </summary>
  internal class ListTreeViewNode : ModelTreeViewNode
  {
    internal static IEnumerable<ListTreeViewNode> CreateTreeViewNodes(PRList[] value)
    {
      return value.Select<PRList, ListTreeViewNode>(x => ListTreeViewNode.CreateTreeViewNode(x));
    }
    private ListTreeViewNode(PRList pbList)
      : base(pbList, TreeViewIcon.TreeViewIconList)
    { }
    private static ListTreeViewNode CreateTreeViewNode(PRList parent)
    {
      ListTreeViewNode _ret = new ListTreeViewNode(parent)
      {
        Header = parent.Name,
      };
      _ret.Items = new ObservableCollection<TreeViewNode>();
      _ret.Items.Add(ContentTypeRootTreeViewNode.CreateTreeViewNode(parent.ContentType));
      return _ret;
    }

  }
}
