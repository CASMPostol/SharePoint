using System;
using System.IO;

namespace CAS.SharePoint.Tools.Packaging
{
  class Program
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "args" )]
    static void Main( string[] args )
    {
      try
      {
        DirectoryInfo _contentLocation = new DirectoryInfo( Path.Combine( Directory.GetCurrentDirectory(), Properties.Settings.Default.ContentPath ) );
        IOPackage.Writer.CreatePackage( _contentLocation, Properties.Settings.Default.PackageName );
        Console.WriteLine( "Package created." );
        string _message = String.Empty;
        using ( PackageContent _pc = new PackageContent( new FileInfo( Properties.Settings.Default.PackageName ) ) )
        {
          _message = String.Format( "Package has been extracted to: {0}", _pc.ContentLocation );
          Console.WriteLine( _message );
          Console.WriteLine( "Press any key ......" );
          Console.Read();
        }
      }
      catch ( Exception ex )
      {
        Console.WriteLine( "Failed with the following exception:" );
        Console.WriteLine( ex.Message );
        Console.WriteLine( "Press any key ......" );
        Console.Read();
      }
    }
  }
}
