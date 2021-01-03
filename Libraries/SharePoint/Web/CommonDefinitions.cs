//<summary>
//  Title   : public class CommonDefinitions
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-02-07 11:54:17 +0100 (pt., 07 lut 2014) $
//  $Rev: 10293 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePoint/Web/CommonDefinitions.cs $
//  $Id: CommonDefinitions.cs 10293 2014-02-07 10:54:17Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;

namespace CAS.SharePoint.Web
{
  /// <summary>
  /// class CommonDefinitions for 
  /// </summary>
  public class CommonDefinitions
  {
    /// <summary>
    /// The error message format string.
    /// </summary>
    public const string ErrorMessageFormat = "<font color=red>{0}</font><br/>";
    /// <summary>
    /// Converts the <paramref name="message"/> into the error message format.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    public static string Convert2ErrorMessageFormat( string message ) { return String.Format( ErrorMessageFormat, message ); }
  }
}
