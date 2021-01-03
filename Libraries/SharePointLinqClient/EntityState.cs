//<summary>
//  Title   : enum EntityState
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-08-14 09:43:47 +0200 (czw., 14 sie 2014) $
//  $Rev: 10697 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePointLinqClient/EntityState.cs $
//  $Id: EntityState.cs 10697 2014-08-14 07:43:47Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace Microsoft.SharePoint.Linq
{  
  /// <summary>
  ///  Records the changed state of an entity (usually a list item; but possibly a detached entity).
  /// </summary>
  public enum EntityState
  {   
    /// <summary>
    ///  The entity is not changed.
    /// </summary>
    Unchanged = 0,   
    /// <summary>
    ///  The entity will be inserted into a list.
    /// </summary>
    ToBeInserted = 1,    
    /// <summary>
    /// The entity will be updated.
    /// </summary>
    ToBeUpdated = 2,     
    /// <summary>
    /// The entity will be recycled.
    /// </summary>
    ToBeRecycled = 3,     
    /// <summary>
    /// The entity will be deleted.
    /// </summary>
    ToBeDeleted = 4,     
    /// <summary>
    /// The entity has been deleted or recycled.
    /// </summary>
    Deleted = 5,
  }
}
