using System;
using System.IO;
using System.IO.Packaging;
using System.Net.Mime;

namespace CAS.SharePoint.Tools.Packaging.IOPackage
{
  /// <summary>
  /// Package writer helper
  /// </summary>
  internal static class Writer
  {
    /// <summary>
    /// Creates a package zip file containing specified content and resource files.
    /// </summary>
    internal static void CreatePackage( DirectoryInfo content, string packageName, Action<PackageProperties> update )
    {
      // Create the Package (If the package file already exists, FileMode.Create will automatically delete it first before creating a new one.
      using ( Package package = Package.Open( packageName, FileMode.Create ) )
      {
        PackageProperties _Properties = package.PackageProperties;
        update( _Properties );
        foreach ( FileInfo _item in content.GetFileSystemInfos() )
        {
          Uri _partUri = PackUriHelper.CreatePartUri( new Uri( _item.Name, UriKind.Relative ) );
          // Add the Document part to the Package
          string _mediaType = String.Empty;
          string _contentType = String.Empty;
          switch ( _item.Extension.ToLower() )
          {
            case ".xml":
              _mediaType = MediaTypeNames.Text.Xml;
              _contentType = Definitions.PackageDescriptionRelationshipType;
              break;
            case ".wsp":
              _mediaType = MediaTypeNames.Application.Zip;
              _contentType = String.Format( Definitions.PackageSolutionRelationshipTypeFormat, Uri.EscapeDataString( _item.Name ) );
              break;
            case ".jpg":
              _mediaType = MediaTypeNames.Image.Jpeg;
              _contentType = Definitions.PackageOthetRelationshipType;
              break;
            default:
              _mediaType = MediaTypeNames.Application.Octet;
              _contentType = Definitions.PackageOthetRelationshipType;
              break;
          }
          PackagePart _packagePart = package.CreatePart( _partUri, _mediaType );
          // Copy the data to the Document Part
          using ( FileStream _fileStream = _item.OpenRead() )
          {
            CopyStream( _fileStream, _packagePart.GetStream() );
          }// end:using(fileStream) - Close and dispose fileStream.
          // Add a Package Relationship to the Document Part
          package.CreateRelationship( _packagePart.Uri, TargetMode.Internal, _contentType );
        }
        package.Flush();
        package.Close();
      }// end:using (Package package) - Close and dispose package.
    }// end:CreatePackage()

    /// <summary>
    ///   Copies data from a source stream to a target stream.</summary>
    /// <param name="source">
    ///   The source stream to copy from.</param>
    /// <param name="target">
    ///   The destination stream to copy to.</param>
    private static void CopyStream( Stream source, Stream target )
    {
      const int bufSize = 0x1000;
      byte[] buf = new byte[ bufSize ];
      int bytesRead = 0;
      while ( ( bytesRead = source.Read( buf, 0, bufSize ) ) > 0 )
        target.Write( buf, 0, bytesRead );
    }// end:CopyStream()

  }
}
