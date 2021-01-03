//<summary>
//  Title   : ConflictMode enum
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-02-11 22:33:21 +0100 (wt., 11 lut 2014) $
//  $Rev: 10316 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePointLinqClient/ConflictMode.cs $
//  $Id: ConflictMode.cs 10316 2014-02-11 21:33:21Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace Microsoft.SharePoint.Linq
{
  /// <summary>
  /// Specifies when an attempt to submit changes to a list should be stopped and rolled back.
  /// </summary>
  public enum ConflictMode
  {
    /// <summary>
    /// Attempt all changes and, when done, if there have been any concurrency conflicts, throw a Microsoft.SharePoint.Linq.ChangeConflictException exception, 
    /// populate Microsoft.SharePoint.Linq.DataContext.ChangeConflicts, and rollback all changes.
    /// </summary>
    ContinueOnConflict = 0,
    /// <summary>
    /// Throw a Microsoft.SharePoint.Linq.ChangeConflictException exception when the first concurrency change conflict is found, stop making changes, populate 
    /// Microsoft.SharePoint.Linq.DataContext.ChangeConflicts, and rollback all changes that were made to that point.
    /// </summary>
    FailOnFirstConflict = 1,
  }
}
