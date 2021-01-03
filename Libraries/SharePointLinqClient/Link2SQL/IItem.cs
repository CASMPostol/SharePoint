//<summary>
//  Title   : public interface IItem:IId
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2015-01-14 12:28:15 +0100 (śr., 14 sty 2015) $
//  $Rev: 11208 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/Link2SQL/IItem.cs $
//  $Id: IItem.cs 11208 2015-01-14 11:28:15Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.SharePoint.Client.Link2SQL
{
  /// <summary>
  /// SQL Item representing SP item base interface.
  /// </summary>
  public interface IItem:IId
  {
    /// <summary>
    /// Gets or sets a value indicating whether this entity saved only in SQL database.
    /// </summary>
    /// <value>
    ///   <c>true</c> if it is only in SQLDatabase; otherwise, <c>false</c>.
    /// </value>
    bool OnlySQL { get; set; } 
  }
  /// <summary>
  /// SQL Item base interface.
  /// </summary>
  public interface IId
  {
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    int ID { get; set; }
  }
}
