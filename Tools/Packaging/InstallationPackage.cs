using System;
using System.IO;
using System.IO.Packaging;
using CAS.SharePoint.Tools.Packaging.IOPackage;

namespace CAS.SharePoint.Tools.Packaging
{
  /// <summary>
  /// Installation Package Data Access Helpers
  /// </summary>
  public static class InstallationPackage
  {
    /// <summary>
    /// Packages the description open read.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="packagePath">The package path.</param>
    /// <param name="reader">The reader.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">packagePath;packagePath cannot be null</exception>
    /// <exception cref="System.ArgumentException">Cannot open package description -  file does not exist</exception>
    public static TResult PackageDescriptionOpenRead<TResult>( FileInfo packagePath, Func<Stream, TResult> reader )
    {
      if ( packagePath == null )
        throw new ArgumentNullException( "packagePath", "packagePath cannot be null" );
      if ( !packagePath.Exists )
        throw new ArgumentException( "Cannot open package description -  file does not exist" );
      return Reader.OpenRead<TResult>( packagePath, Definitions.PackageDescriptionRelationshipType, reader );
    }
    /// <summary>
    /// Publishes the package.
    /// </summary>
    /// <param name="contentPath">The path.</param>
    /// <param name="filePath">The file path.</param>
    /// <param name="update">The update action.</param>
    /// <exception cref="System.ArgumentNullException">path;Path cannot be null or empty</exception>
    public static void PublishPackage( string contentPath, string filePath, Action<PackageProperties> update )
    {
      if ( String.IsNullOrEmpty( contentPath ) )
        throw new ArgumentNullException( "path", "Path cannot be null or empty" );
      DirectoryInfo _contentLocation = new DirectoryInfo( contentPath );
      IOPackage.Writer.CreatePackage( _contentLocation, filePath, update );
    }
    /// <summary>
    /// Reads the package properties.
    /// </summary>
    /// <param name="packagePath">The package path.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">packagePath;packagePath cannot be null</exception>
    /// <exception cref="System.ArgumentException">Cannot open package description -  file does not exist</exception>
    public static PackageProperties ReadPackageProperties( FileInfo packagePath )
    {
      if ( packagePath == null )
        throw new ArgumentNullException( "packagePath", "packagePath cannot be null" );
      if ( !packagePath.Exists )
        throw new ArgumentException( "Cannot open package description -  file does not exist" );
      return Reader.OpenRead( packagePath );
    }
  }
}
