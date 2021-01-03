//<summary>
//  Title   : public class SPIItem
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2014-08-29 14:23:35 +0200 (pt., 29 sie 2014) $
//  $Rev: 10752 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePointLinqClient/Linq2SP/IItem.cs $
//  $Id: IItem.cs 10752 2014-08-29 12:23:35Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;

namespace CAS.SharePoint.Client.Linq2SP
{
  /// <summary>
  /// The interface providing basic properties for SharePoint entities classes.
  /// </summary>
  public interface ISPItem
  {
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    Nullable<int> Id { get; set; }
  }
}
