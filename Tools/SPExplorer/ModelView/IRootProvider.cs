//<summary>
//  Title   : public interface IRootProvider
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-06-28 22:52:29 +0200 (sob., 28 cze 2014) $
//  $Rev: 10528 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Tools/SPExplorer/ModelView/IRootProvider.cs $
//  $Id: IRootProvider.cs 10528 2014-06-28 20:52:29Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.SharePoint.Tools.SPExplorer.ModelView
{
  /// <summary>
  /// Interface providing object model root of <see cref="SPMetalParameters.PRWeb"/>
  /// </summary>
  public interface IRootProvider
  {
    SPMetalParameters.PRWeb ObjectModelRoot { get; }
  }
}
