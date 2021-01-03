//_______________________________________________________________
//  Title   : IArchivingOperationLogs
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2015-01-21 11:05:59 +0100 (śr., 21 sty 2015) $
//  $Rev: 11242 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/Link2SQL/IArchivingOperationLogs.cs $
//  $Id: IArchivingOperationLogs.cs 11242 2015-01-21 10:05:59Z mpostol $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System;

namespace CAS.SharePoint.Client.Link2SQL
{
  /// <summary>
  /// Interface IArchivingOperationLogs - interface representing the entity <see cref="System.Data.Linq.Table{T}"/> of ArchivingOperationLogs.
  /// </summary>
  public interface IArchivingOperationLogs
  {

    /// <summary>
    /// Sets the modification date.
    /// </summary>
    /// <value>The <see cref="DateTime"/> of modification.</value>
    DateTime Date { set; get; }
    /// <summary>
    /// Sets the operation.
    /// </summary>
    /// <value>The operation.</value>
    string Operation { set; get; }
    /// <summary>
    /// Sets the name of the author of last modification.
    /// </summary>
    /// <value>The name of the user.</value>
    string UserName { set; }

  }
}
