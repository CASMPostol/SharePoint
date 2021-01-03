using System;
using System.IO;
using System.IO.Packaging;
using System.Reflection;

namespace CAS.SharePoint.Setup.Package
{
  internal class InstalationPackageProperties: PackageProperties
  {
    public static InstalationPackageProperties GetProperties( PackageProperties source, FileInfo Location )
    {
      InstalationPackageProperties _ret = new InstalationPackageProperties();
      _ret.Location = Location;
      foreach ( PropertyInfo _propertyX in typeof( PackageProperties ).GetProperties() )
      {
        object x = _propertyX.GetValue( source, null );
        _propertyX.SetValue( _ret, x, null );
      }
      return _ret;
    }
    public void UpdateProperties( PackageProperties properties )
    {
      foreach ( PropertyInfo _propertyX in typeof( PackageProperties ).GetProperties() )
      {
        object x = _propertyX.GetValue( this, null );
        _propertyX.SetValue( properties, x, null );
      }
    }
    public InstalationPackageProperties GetProperties()
    {
      return (InstalationPackageProperties)MemberwiseClone();
    }
    public override string Category { get; set; }
    public override string ContentStatus { get; set; }
    public override string ContentType { get; set; }
    public override DateTime? Created { get; set; }
    public override string Creator { get; set; }
    public override string Description { get; set; }
    public override string Identifier { get; set; }
    public override string Keywords { get; set; }
    public override string Language { get; set; }
    public override string LastModifiedBy { get; set; }
    public override DateTime? LastPrinted { get; set; }
    public override DateTime? Modified { get; set; }
    public override string Revision { get; set; }
    public override string Subject { get; set; }
    public override string Title { get; set; }
    public override string Version { get; set; }
    internal FileInfo Location { get; private set; }
  }
}
