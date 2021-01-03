using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;

namespace CAS.SharePoint.Tools.Packaging.IOPackage
{
  internal static class Reader
  {
    internal static PackageProperties OpenRead( FileInfo packagePath )
    {
      using ( Package package = Package.Open( packagePath.FullName, FileMode.Open, FileAccess.Read ) )
      {
        return package.PackageProperties;
      }
    }
    /// <summary>
    /// Opens the read.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="packagePath">The package path.</param>
    /// <param name="relationshipType">  The System.IO.Packaging.PackageRelationship.RelationshipType to match and return in the collection..</param>
    /// <param name="reader">The reader.</param>
    /// <returns>The return value of the method</returns>
    internal static TResult OpenRead<TResult>( FileInfo packagePath, string relationshipType, Func<Stream, TResult> reader )
    {
      using ( Package package = Package.Open( packagePath.FullName, FileMode.Open, FileAccess.Read ) )
      {
        // Get the Package Relationships and look for the Document part based on the RelationshipType
        Uri uriDocumentTarget = null;
        PackageRelationship relationship = package.GetRelationshipsByType( relationshipType ).FirstOrDefault();
        if ( relationship == null )
          throw new ArgumentException( "the package does not contain the requested part" );
        // Resolve the Relationship Target Uri so the Document Part can be retrieved.
        uriDocumentTarget = PackUriHelper.ResolvePartUri( new Uri( "/", UriKind.Relative ), relationship.TargetUri );
        PackagePart _documentPart = package.GetPart( uriDocumentTarget ); // Open the Part
        TResult _rslt = default( TResult );
        using ( Stream _strm = _documentPart.GetStream( FileMode.Open, FileAccess.Read ) )
          _rslt = reader( _strm ); //let read the content by the caller
        return _rslt;
      }
    }
    internal static void ExtractPackageParts( FileInfo packagePath, DirectoryInfo targetDirectory )
    {
      //Create a new Target directory.
      if ( targetDirectory.Exists )
        throw new ArgumentException( "Target directory exist", "targetDirectory" );
      targetDirectory.Create();
      //Start reading
      using ( Package package = Package.Open( packagePath.FullName, FileMode.Open, FileAccess.Read ) )
      {
        //PackagePart documentPart = null;

        //// Get the Package Relationships and look for
        ////   the Document part based on the RelationshipType
        //Uri uriDocumentTarget = null;
        //foreach ( PackageRelationship relationship in package.GetRelationshipsByType( Definitions.PackageRelationshipType ) )
        //{
        //  // Resolve the Relationship Target Uri so the Document Part can be retrieved.
        //  uriDocumentTarget = PackUriHelper.ResolvePartUri( new Uri( "/", UriKind.Relative ), relationship.TargetUri );

        //  // Open the Document Part, write the contents to a file.
        //  documentPart = package.GetPart( uriDocumentTarget );
        //  ExtractPart( documentPart, targetDirectory );
        //}
        foreach ( PackagePart _ppx in package.GetParts() )
        {
          ExtractPart( _ppx, targetDirectory );
        }
        //// Get the Document part's Relationships,
        ////   and look for required resources.
        //Uri uriResourceTarget = null;
        //foreach ( PackageRelationship relationship in documentPart.GetRelationshipsByType( ResourceRelationshipType ) )
        //{
        //  // Resolve the Relationship Target Uri
        //  //   so the Resource Part can be retrieved.
        //  uriResourceTarget = PackUriHelper.ResolvePartUri( documentPart.Uri, relationship.TargetUri );

        //  // Open the Resource Part and write the contents to a file.
        //  resourcePart = package.GetPart( uriResourceTarget );
        //  ExtractPart( resourcePart, targetDirectory );
        //}

      }// end:using(Package package) - Close & dispose package.
    }// end:ExtractPackageParts()
    /// <summary>
    /// Extracts a specified package part to a target folder.
    /// </summary>
    /// <param name="packagePart">The package part to extract.</param>
    /// <param name="targetDirectory">The relative path from the 'current' directory to the targer folder.</param>
    private static void ExtractPart( PackagePart packagePart, DirectoryInfo targetDirectory )
    {
      //// Create a string with the full path to the target directory.
      //string currentDirectory = Directory.GetCurrentDirectory();
      //string pathToTarget = currentDirectory + @"\" + targetDirectory;

      // Remove leading slash from the Part Uri, and make a new Uri from the result
      string _stringPart = packagePart.Uri.ToString().TrimStart( '/' );
      Uri _partUri = new Uri( _stringPart, UriKind.Relative );

      // Create a full Uri to the Part based on the Package Uri
      Uri _uriFullPartPath = new Uri( new Uri( targetDirectory.FullName + Path.DirectorySeparatorChar, UriKind.Absolute ), _partUri );

      FileInfo _FullPartPathDirectoryInfo = new FileInfo( _uriFullPartPath.LocalPath );
      if ( !_FullPartPathDirectoryInfo.Directory.Exists )
        _FullPartPathDirectoryInfo.Directory.Create(); // Create the necessary Directories based on the Full Part Path
      // Create the file with the Part content
      using ( FileStream fileStream = _FullPartPathDirectoryInfo.Open( FileMode.Create ) )
        CopyStream( packagePart.GetStream(), fileStream );
    }// end:ExtractPart()

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
