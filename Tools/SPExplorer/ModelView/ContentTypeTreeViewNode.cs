//<summary>
//  Title   : class ContentTypeTreeViewNode
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/SPExplorer/ModelView/ContentTypeTreeViewNode.cs $
//  $Id: ContentTypeTreeViewNode.cs 10838 2014-10-10 14:13:54Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Tools.SPExplorer.SPMetalParameters;
using System;
using System.Collections.ObjectModel;

namespace CAS.SharePoint.Tools.SPExplorer.ModelView
{
  internal class ContentTypeTreeViewNode : ModelTreeViewNode
  {
    internal static ContentTypeTreeViewNode CreateTreeViewNode(PRContentType prContentType)
    {
      ContentTypeTreeViewNode _ret = new ContentTypeTreeViewNode(prContentType) { Header = String.Format("ContentType: {0}, Id={1}", prContentType.Name, prContentType.ContentTypeId) };
      _ret.Items = new ObservableCollection<TreeViewNode>();
      if (prContentType.BaseContentType != null)
      {
        ContentTypeTreeViewNode _baseContentTyoe = ContentTypeTreeViewNode.CreateTreeViewNode(prContentType.BaseContentType);
        _ret.Items.Add(_baseContentTyoe);
      }
      foreach (ColumnTreeViewNode _columnX in ColumnTreeViewNode.CreateTreeViewNodes(prContentType.Column))
        _ret.Items.Add(_columnX);
      return _ret;
    }
    private ContentTypeTreeViewNode(PRContentType parent) : base(parent, TreeViewIcon.TreeViewIconClassFriend) { }
  }
}
