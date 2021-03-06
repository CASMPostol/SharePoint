﻿//<summary>
//  Title   : Documents Factory class File
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-11-28 11:36:13 +0100 (pt., 28 lis 2014) $
//  $Rev: 11005 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/CASCommon/DocumentsFactory/XmlFile.cs $
//  $Id: XmlFile.cs 11005 2014-11-28 10:36:13Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CAS.Common.DocumentsFactory
{
  /// <summary>
  /// Provides static methods for serialization objects into XML documents and writing the XML document to a file.
  /// </summary>
  public static class XmlFile
  {

    #region public
    /// <summary>
    /// Serializes the specified <paramref name="dataObject"/> and writes the XML document to a file.
    /// </summary>
    /// <typeparam name="type">The type of the object to be serialized and saved in the file.</typeparam>
    /// <param name="dataObject">The object containing working data to be serialized and saved in the file.</param>
    /// <param name="path">A relative or absolute path for the file that the object to be serialized and saved.</param>
    /// <param name="mode">Specifies how the operating system should open a file <see cref="FileMode"/>.</param>
    /// <param name="stylesheetName">Name of the style sheet.</param>
    /// <exception cref="System.ArgumentNullException">
    ///   path
    /// or
    ///   dataObject
    /// or
    ///   stylesheetName
    /// </exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
    public static void WriteXmlFile<type>(type dataObject, string path, FileMode mode, string stylesheetName)
    {
      if (string.IsNullOrEmpty(path))
        throw new ArgumentNullException("path");
      if (string.IsNullOrEmpty(stylesheetName))
        throw new ArgumentNullException("stylesheetName");
      if (dataObject == null)
        throw new ArgumentNullException("dataObject");
      XmlSerializer _srlzr = new XmlSerializer(typeof(type));
      XmlWriterSettings _setting = new XmlWriterSettings()
      {
        Indent = true,
        IndentChars = "  ",
        NewLineChars = "\r\n"
      };
      using (FileStream _docStrm = new FileStream(path, mode, FileAccess.Write))
      using (XmlWriter _writer = XmlWriter.Create(_docStrm, _setting))
      {
        _writer.WriteProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" " + String.Format("href=\"{0}.xslt\"", stylesheetName));
        _srlzr.Serialize(_writer, dataObject);
      }
    }
    /// <summary>
    /// Writes the XML file.
    /// </summary>
    /// <typeparam name="type">The type of the <paramref name="dataObject"/>.</typeparam>
    /// <param name="dataObject">The data object.</param>
    /// <param name="path">The path of the file.</param>
    /// <param name="mode">The mode of the file - specifies how the operating system should open a file.</param>
    public static void WriteXmlFile<type>(type dataObject, string path, FileMode mode)
      where type : IStylesheetNameProvider
    {
      XmlFile.WriteXmlFile<type>(dataObject, path, mode, dataObject.StylesheetNmane);
    }
    /// <summary>
    /// Reads an XML document from the file <paramref name="path"/> and deserializes its content to returned object.
    /// </summary>
    /// <typeparam name="type">The type of the object to be deserialized and saved in the file.</typeparam>
    /// <param name="path">A relative or absolute path for the file that contains the serialized object.</param>
    /// <returns>An object containing working data retrieved from an XML file.</returns>
    /// <exception cref="System.ArgumentNullException">path</exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
    public static type ReadXmlFile<type>(string path)
    {
      if (string.IsNullOrEmpty(path))
        throw new ArgumentNullException("path");
      type _content = default(type);
      XmlSerializer _srlzr = new XmlSerializer(typeof(type));
      using (FileStream _docStrm = new FileStream(path, FileMode.Open, FileAccess.Read))
      using (XmlReader _writer = XmlReader.Create(_docStrm))
        _content = (type)_srlzr.Deserialize(_writer);
      return _content;
    }
    #endregion

  }
}
