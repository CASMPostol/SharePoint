
//<summary>
//  Title   : ICheckable interface
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 14:20:07 +0200 (pt., 10 paź 2014) $
//  $Rev: 10836 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/ComponentModel/ICheckable.cs $
//  $Id: ICheckable.cs 10836 2014-10-10 12:20:07Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.Common.ComponentModel
{
  /// <summary>
  /// interface ICheckable used to select elements to be serialized.
  /// </summary>
  public interface ICheckable
  {
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="ICheckable"/> is checked and should be added to the generated final script file.
    /// </summary>
    /// <value>
    ///   <c>true</c> if checked; otherwise, <c>false</c>.
    /// </value>
    bool Checked { get; set; }
  }
}
