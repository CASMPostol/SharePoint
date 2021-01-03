//<summary>
//  Title   : Documents Factory class File
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-02-19 16:43:02 +0100 (czw., 19 lut 2015) $
//  $Rev: 11388 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePoint/DocumentsFactory/XmlFile.cs $
//  $Id: XmlFile.cs 11388 2015-02-19 15:43:02Z mpostol $
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

namespace CAS.SharePoint.DocumentsFactory
{
  /// <summary>
  /// Provides static methods for serialization objects into XML documents and writing the XML document to a file.
  /// </summary>
  public static class XmlFile
  {

    #region public
    /// <summary>
    /// Serializes the specified <paramref name="dataObject"/> and writes the XML document to a <see cref="Stream"/>. 
    /// The document contains name of the stylesheet that should be used for transformation purpose.
    /// </summary>
    /// <typeparam name="type">The type of the object to be serialized and saved in the file.</typeparam>
    /// <param name="dataObject">The object containing working data to be serialized and saved in the file.</param>
    /// <param name="output">The <see cref="Stream"/> to which you want to write. The <see cref="System.Xml.XmlWriter"/> writes XML 1.0 text 
    /// syntax and appends it to the specified stream.</param>
    /// <param name="stylesheetName">Name of the stylesheet.</param>
    public static void WriteXmlFile<type>(type dataObject, Stream output, string stylesheetName)
    {
      XmlSerializer _serializer = new XmlSerializer(typeof(type));
      XmlWriterSettings _setting = new XmlWriterSettings()
      {
        Indent = true,
        IndentChars = "  ",
        NewLineChars = "\r\n"
      };
      using (XmlWriter _writer = XmlWriter.Create(output, _setting))
      {
        _writer.WriteProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" " + String.Format("href=\"{0}.xslt\"", stylesheetName));
        _serializer.Serialize(_writer, dataObject);
      }
    }
    /// <summary>
    /// Serializes the specified <paramref name="dataObject"/> and writes the XML document to a file.
    /// </summary>
    /// <typeparam name="type">The type of the object to be serialized and saved in the file.</typeparam>
    /// <param name="dataObject">The object containing working data to be serialized and saved in the file.</param>
    /// <param name="path">A relative or absolute path for the file that the object to be serialized and saved.</param>
    /// <param name="mode">Specifies how the operating system should open a file <see cref="FileMode"/>.</param>
    /// <param name="stylesheetName">Name of the style sheet.</param>
    /// <exception cref="System.ArgumentNullException">
    /// path
    /// or
    /// dataObject
    /// or
    /// stylesheetName
    /// </exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
    public static void WriteXmlFile<type>(type dataObject, string path, FileMode mode, string stylesheetName)
    {
      if (string.IsNullOrEmpty(path))
        throw new ArgumentNullException("path");
      if (string.IsNullOrEmpty(stylesheetName))
        throw new ArgumentNullException("stylesheetName");
      if (dataObject == null)
        throw new ArgumentNullException("content");
      using (FileStream _docStrm = new FileStream(path, mode, FileAccess.Write))
        WriteXmlFile<type>(dataObject, _docStrm, stylesheetName);
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
    /// <typeparam name="type">The type of the object to be deserialized and returned.</typeparam>
    /// <param name="path">A relative or absolute path for the file that contains the serialized object.</param>
    /// <returns>An object containing working data retrieved from an XML file.</returns>
    /// <exception cref="System.ArgumentNullException">path</exception>
    public static type ReadXmlFile<type>(string path)
    {
      if (string.IsNullOrEmpty(path))
        throw new ArgumentNullException("path");
      XmlSerializer _serializer = new XmlSerializer(typeof(type));
      using (FileStream _docStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        return ReadXmlFile<type>(_docStream);
    }
    /// <summary>
    /// Reads an XML document from the <see cref="Stream"/> and deserializes its content to returned object.
    /// </summary>
    /// <typeparam name="type">The type of the object to be deserialized and returned as an object.</typeparam>
    /// <param name="input">The stream containing the XML data. The <see cref="System.Xml.XmlReader"/> scans the first bytes of the stream looking for a byte order mark or other sign of encoding. 
    /// When encoding is determined, the encoding is used to continue reading the stream, and processing continues parsing the input as a stream of (Unicode) characters.</param>
    /// <returns>An object containing working data retrieved from an XML file.</returns>
    public static type ReadXmlFile<type>(Stream input)
    {
      type _content = default(type);
      XmlSerializer _serializer = new XmlSerializer(typeof(type));
      using (XmlReader _writer = XmlReader.Create(input))
        _content = (type)_serializer.Deserialize(_writer);
      return _content;
    }
    #endregion
  }
}
