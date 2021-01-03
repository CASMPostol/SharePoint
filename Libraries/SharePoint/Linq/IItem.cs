//_______________________________________________________________
//  Title   : IItem
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2015-04-03 12:57:48 +0200 (pt., 03 kwi 2015) $
//  $Rev: 11553 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePoint/Linq/IItem.cs $
//  $Id: IItem.cs 11553 2015-04-03 10:57:48Z mpostol $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

namespace CAS.SharePoint.Linq
{
  /// <summary>
  /// Interface IItem representing SPMetal Item class 
  /// </summary>
  public interface IItem
  {

    /// <summary>
    /// Gets or sets the identifier of the item.
    /// </summary>
    /// <value>The identifier <see cref="System.Nullable{t}"/>.</value>
    System.Nullable<int> Id { get; set; }
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    string Title { get; set; }

  }
}
