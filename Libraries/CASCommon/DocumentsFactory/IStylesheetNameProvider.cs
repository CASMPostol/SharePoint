//<summary>
//  Title   : IStylesheetNameProvider
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2014-11-28 11:50:27 +0100 (pt., 28 lis 2014) $
//  $Rev: 11007 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/CASCommon/DocumentsFactory/IStylesheetNameProvider.cs $
//  $Id: IStylesheetNameProvider.cs 11007 2014-11-28 10:50:27Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
namespace CAS.Common.DocumentsFactory
{
  
  /// <summary>
  /// Represents XML file style-sheet name provider
  /// </summary>
  public interface IStylesheetNameProvider
  {
    /// <summary>
    /// The style-sheet mane
    /// </summary>
    string StylesheetNmane { get; }

  }
}
