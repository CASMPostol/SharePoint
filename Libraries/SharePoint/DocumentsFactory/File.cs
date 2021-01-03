//<summary>
//  Title   : Documents Factory class File
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-02-19 16:43:02 +0100 (czw., 19 lut 2015) $
//  $Rev: 11388 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePoint/DocumentsFactory/File.cs $
//  $Id: File.cs 11388 2015-02-19 15:43:02Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using Microsoft.SharePoint;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CAS.SharePoint.DocumentsFactory
{
  /// <summary>
  /// Provides static methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of <see cref="SPFile"/> objects.
  /// </summary>
  public static class File
  {

    #region public
    /// <summary>
    /// Creates the XML file and adds it to the library <paramref name="listName" />.
    /// </summary>
    /// <typeparam name="type">The type of the object to be serialized and saved in the file.</typeparam>
    /// <param name="site">The <see cref="SPWeb" /> object representing a Windows SharePoint Services Web site.</param>
    /// <param name="content">The content of the object to be serialized and saved in the file.</param>
    /// <param name="fileName">A <see cref="String" /> that specifies the URL for the file.</param>
    /// <param name="listName">A <see cref="String" /> that contains the name.</param>
    /// <param name="stylesheetName">Name of the style sheet.</param>
    /// <returns>
    /// An <see cref="Microsoft.SharePoint.SPFile" /> object that represents the file.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">fileName
    /// or
    /// listName
    /// or
    /// stylesheetName</exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
    public static SPFile CreateXmlFile<type>(SPWeb site, type content, string fileName, string listName, string stylesheetName)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");
      if (string.IsNullOrEmpty(listName))
        throw new ArgumentNullException("listName");
      if (string.IsNullOrEmpty(stylesheetName))
        throw new ArgumentNullException("stylesheetName");
      if (site == null)
        throw new ArgumentNullException("site");
      if (content == null)
        throw new ArgumentNullException("content");
      SPDocumentLibrary _lib = (SPDocumentLibrary)site.Lists[listName];
      SPFile _newFile = default(SPFile);
      XmlWriterSettings _setting = new XmlWriterSettings()
      {
        Indent = true,
        IndentChars = "  ",
        NewLineChars = "\r\n"
      };
      using (MemoryStream _docStream = new MemoryStream())
      {
        XmlFile.WriteXmlFile<type>(content, _docStream, stylesheetName);
        _newFile = _lib.RootFolder.Files.Add(fileName + ".xml", _docStream, true);
      }
      _newFile.DocumentLibrary.Update();
      return _newFile;
    }
    /// <summary>
    /// Creates the XML file and adds it to the library <paramref name="listName" />.
    /// </summary>
    /// <typeparam name="type">The type of the object to be serialized and saved in the file.</typeparam>
    /// <param name="site">The <see cref="SPWeb" /> object representing a Windows SharePoint Services Web site.</param>
    /// <param name="content">The content.</param>
    /// <param name="fileName">A <see cref="String" /> that specifies the URL for the file.</param>
    /// <param name="listName">A <see cref="String" /> that contains the name.</param>
    /// <returns></returns>
    public static SPFile CreateXmlFile<type>(SPWeb site, type content, string fileName, string listName)
      where type : IStylesheetNameProvider
    {
      return File.CreateXmlFile<type>(site, content, fileName, listName, content.StylesheetNmane);
    }
    /// <summary>
    /// Writes the XML file to the an existing file that is represented by the item from a document library<paramref name="listName"/>.
    /// </summary>
    /// <typeparam name="type">The type of the object to serialize.</typeparam>
    /// <param name="web">The web.</param>
    /// <param name="fileId">The file identifier.</param>
    /// <param name="listName">Name of the list.</param>
    /// <param name="content">The object to serialize.</param>
    /// <param name="stylesheetName">Name of the XML style sheet.</param>
    public static void WriteXmlFile<type>(SPWeb web, int fileId, string listName, type content, string stylesheetName)
    {
      SPDocumentLibrary _lib = (SPDocumentLibrary)web.Lists[listName];
      SPFile _file = _lib.GetItemByIdSelectedFields(fileId).File;
      File.WriteXmlFile<type>(_file, content, stylesheetName);
    }
    /// <summary>
    /// Writes the XML file.
    /// </summary>
    /// <typeparam name="type">The type of the object to serialize.</typeparam>
    /// <param name="docFile">The <see cref="SPFile"/> file.</param>
    /// <param name="content">The object to serialize.</param>
    /// <param name="stylesheetName">Name of the XML style sheet.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
    public static void WriteXmlFile<type>(SPFile docFile, type content, string stylesheetName)
    {
      XmlSerializer _srlzr = new XmlSerializer(typeof(type));
      XmlWriterSettings _setting = new XmlWriterSettings()
      {
        Indent = true,
        IndentChars = "  ",
        NewLineChars = "\r\n"
      };
      using (Stream _docStrm = new MemoryStream(30000))
      {
        using (XmlWriter _file = XmlWriter.Create(_docStrm, _setting))
        {
          _file.WriteProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" " + String.Format("href=\"{0}.xslt\"", stylesheetName));
          _srlzr.Serialize(_file, content);
        }
        docFile.SaveBinary(_docStrm);
      }
      docFile.Update();
    }
    /// <summary>
    /// Writes the XML file.
    /// </summary>
    /// <typeparam name="type">The type of the object to serialize.</typeparam>
    /// <param name="library">The <see cref="SPDocumentLibrary"/> that is a placeholder of the file.</param>
    /// <param name="itemId">The item unique identifier.</param>
    /// <param name="content">The object to serialize.</param>
    public static void WriteXmlFile<type>(SPDocumentLibrary library, int itemId, type content)
      where type : IStylesheetNameProvider
    {
      SPFile _file = library.GetItemByIdSelectedFields(itemId).File;
      File.WriteXmlFile<type>(_file, content, content.StylesheetNmane);
    }
    /// <summary>
    /// Reads the XML file from an item in a document library <see cref="SPDocumentLibrary" /> that is a file placeholder and deserializes its content to returned object.
    /// </summary>
    /// <typeparam name="type">The type of the an object containing working data retrieved from an XML file.</typeparam>
    /// <param name="library">The <see cref="SPDocumentLibrary" /> that is a placeholder of the file.</param>
    /// <param name="itemId">The item unique identifier. A 32-bit integer that identifies the item. The value of this parameter does 
    /// not correspond to the index of the item within the collection of items for the list, but to the value of the <see cref="Microsoft.SharePoint.SPListItem.ID"/> property
    /// of the <see cref="Microsoft.SharePoint.SPListItem"/> class.
    /// </param>
    /// <returns>An object of <typeparamref name="type" /> containing working data retrieved from an XML file.</returns>
    public static type ReadXmlFile<type>(SPDocumentLibrary library, int itemId)
    {
      SPFile _file = library.GetItemById(itemId).File;
      return File.ReadXmlFile<type>(_file);
    }
    /// <summary>
    /// Reads the XML file from the <see cref="SPFile" /> that represents a file in a SharePoint Web site, an item in a document library, or a file in a folder
    /// and deserializes its content to returned object.
    /// </summary>
    /// <typeparam name="type">The type of an object containing working data retrieved from an XML file.</typeparam>
    /// <param name="file">The <see cref="SPFile" /> that represents a file in a SharePoint Web site.</param>
    /// <returns>An object of <typeparamref name="type" /> containing working data retrieved from an XML file.</returns>
    public static type ReadXmlFile<type>(SPFile file)
    {
      using (Stream _str = file.OpenBinaryStream())
        return XmlFile.ReadXmlFile<type>(_str);
    }
    #endregion

  }
}
