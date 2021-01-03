using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CAS.SharePoint.Tools.Packaging
{
  /// <summary>
  /// Gets package content to temporal folder
  /// </summary>
  public partial class PackageContent: Component
  {
    #region creators
    /// <summary>
    /// Initializes a new instance of the <see cref="PackageContent" /> class.
    /// </summary>
    public PackageContent( FileInfo packageFileInfo )
    {
      InitializeComponent();
      ContentLocation = new DirectoryInfo( Path.Combine( Path.GetTempPath(), Uri.EscapeDataString( Guid.NewGuid().ToString() ) ) );
      // If the Target directory exists, first delete it and then create a new empty one.
      if ( ContentLocation.Exists )
      {
        ContentLocation.Delete( true );
        ContentLocation.Refresh();
      }
      IOPackage.Reader.ExtractPackageParts( packageFileInfo, ContentLocation );
    }
    ///// <summary>
    ///// Initializes a new instance of the <see cref="PackageContent" /> class.
    ///// </summary>
    ///// <param name="container">The container.</param>
    //public PackageContent( IContainer container )
    //  : this()
    //{
    //  container.Add( this );
    //}
    #endregion

    #region public

    /// <summary>
    /// Gets the package content location.
    /// </summary>
    /// <value>
    /// <see cref="DirectoryInfo"/> containing information whre the package content is located.
    /// </value>
    public DirectoryInfo ContentLocation
    {
      get
      {
        m_ContentLocationDirectoryInfo.Refresh();
        return m_ContentLocationDirectoryInfo;
      }
      private set { m_ContentLocationDirectoryInfo = value; }
    }
    #endregion

    #region IDisposable
    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      if ( ContentLocation.Exists )
        ContentLocation.Delete( true );
      base.Dispose( disposing );
    }
    #endregion
    DirectoryInfo m_ContentLocationDirectoryInfo = default(DirectoryInfo);
  }
}
