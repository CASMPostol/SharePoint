//<summary>
//  Title   : Trace listener to be used for writhing logs
//  System  : Microsoft Visual C# .NET 2008
//  $LastChangedDate: 2015-09-10 12:58:18 +0200 (czw., 10 wrz 2015) $
//  $Rev: 11671 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/SharePointSetup/Diagnostics/AdvancedTraceListenerHelper.cs $
//  $Id: AdvancedTraceListenerHelper.cs 11671 2015-09-10 10:58:18Z mpostol $
//
//  Copyright (C)2009, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CAS.SharePoint.Setup.Diagnostics
{
  /// <summary>
  /// Advanced delimited trace listener which derives from DelimitedListTraceListener and prepares the log file name
  /// </summary>
  public class AdvancedDelimitedListTraceListener: DelimitedListTraceListener
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AdvancedDelimitedListTraceListener"/> class.
    /// </summary>
    /// <param name="logFileName">Name of the log file.</param>
    public AdvancedDelimitedListTraceListener( string logFileName )
      : base( CalculatePath( logFileName ) )
    {
    }
    private static string CalculatePath( string logFileName )
    {
      string _ret = @"CAS\CASSharePointSetup\Log"; //TODO it is not correct formating string.
      _ret = String.Format( _ret, Assembly.GetExecutingAssembly().FullName );
      DirectoryInfo _info = new DirectoryInfo( Path.Combine( Path.GetTempPath(), _ret ) );
      //now it yields C:\Users\<UserName>\AppData\Local\Temp\CAS\CASSharePointSetup\Log
      if ( !_info.Exists )
        _info.Create();
      return Path.Combine( _info.FullName, logFileName );
    }
  }
}
