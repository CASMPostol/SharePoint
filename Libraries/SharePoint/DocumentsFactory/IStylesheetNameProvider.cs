//<summary>
//  Title   : interface IStylesheetNameProvider
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-02-11 14:18:03 +0100 (wt., 11 lut 2014) $
//  $Rev: 10314 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePoint/DocumentsFactory/IStylesheetNameProvider.cs $
//  $Id: IStylesheetNameProvider.cs 10314 2014-02-11 13:18:03Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SharePoint.DocumentsFactory
{
  /// <summary>
  /// Represents XML file stylesheet name provider
  /// </summary>
  public interface IStylesheetNameProvider
  {
    /// <summary>
    /// The stylesheet nmane
    /// </summary>
    string StylesheetNmane { get; }

  }
}
