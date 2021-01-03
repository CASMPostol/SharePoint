//<summary>
//  Title   : SelectionFilter
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Tools/SPExplorer/SPMetalParameters/SQL/SelectionFilter.cs $
//  $Id: SelectionFilter.cs 10838 2014-10-10 14:13:54Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>


namespace CAS.SharePoint.Tools.SPExplorer.SPMetalParameters.SQL
{
  /// <summary>
  /// Filter used to calculate if the item should be selected in user interface.
  /// </summary>
  internal class SelectionFilter
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectionFilter"/> class.
    /// </summary>
    /// <param name="inludeHidden">if set to <c>true</c> [inlude hidden].</param>
    /// <param name="groupNamePhrase">The group name phrase.</param>
    internal SelectionFilter(string groupNamePhrase, bool inludeHidden)
    {
      InludeHidden = inludeHidden;
      GroupNamePhrase = groupNamePhrase;
    }
    /// <summary>
    /// Calculate if the item passes the filter.
    /// </summary>
    /// <param name="groupName">Name of the group the item belongs to.</param>
    /// <param name="hidden">if set to <c>true</c> the item is hidden.</param>
    /// <returns></returns>
    internal bool PassFilter(string groupName, bool hidden)
    {
      if (hidden && !InludeHidden)
        return false;
      if (!groupName.Contains(GroupNamePhrase))
        return false;
      return true;
    }
    private bool InludeHidden { get; set; }
    private string GroupNamePhrase { get; set; }

  }
}
