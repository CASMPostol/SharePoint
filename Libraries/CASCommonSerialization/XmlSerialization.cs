//_______________________________________________________________
//  Title   : Name of Application
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2015-03-11 16:00:22 +0100 (śr., 11 mar 2015) $
//  $Rev: 11476 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/CASCommonSerialization/XmlSerialization.cs $
//  $Id: XmlSerialization.cs 11476 2015-03-11 15:00:22Z mpostol $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace CAS.Common.Serialization
{
  /// <summary>
  /// Class XmlSerialization - provides helper functions for serialization and deserialization to XML
  /// </summary>
  public static class XmlSerialization
  {
    /// <summary>
    /// Serializes the specified object.
    /// </summary>
    /// <typeparam name="Type">The type of the <paramref name="objectToSerialize"/>.</typeparam>
    /// <param name="objectToSerialize">The object to be serialized.</param>
    /// <param name="stylesheetName">Name of the stylesheet.</param>
    /// <returns>System.String.</returns>
    public static string Serialize<Type>(Type objectToSerialize, string stylesheetName)
    {
      using (System.IO.MemoryStream _writer = new System.IO.MemoryStream())
      {
        WriteXmlFile<Type>(objectToSerialize, _writer, stylesheetName);
        return _writer.ReadString();
      }
    }
    /// <summary>
    /// Deserializes the specified object <paramref name="serializedObject"/> represented as the XML ASCII string.
    /// </summary>
    /// <param name="serializedObject">The serialized object.</param>
    /// <returns></returns>
    public static Type Deserialize<Type>(string serializedObject)
    {
      using (MemoryStream _writer = new MemoryStream())
      {
        _writer.WriteString(serializedObject);
        return ReadXmlFile<Type>(_writer);
      }
    }
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
      DataContractSerializer _serializer = new DataContractSerializer(typeof(type));
      XmlWriterSettings _setting = new XmlWriterSettings()
      {
        Indent = true,
        IndentChars = "  ",
        NewLineChars = "\r\n"
      };
      using (XmlWriter _writer = XmlWriter.Create(output, _setting))
      {
        _writer.WriteProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" " + String.Format("href=\"{0}.xslt\"", stylesheetName));
        _serializer.WriteObject(_writer, dataObject);
      }
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
      DataContractSerializer _serializer = new DataContractSerializer(typeof(type));
      using (XmlReader _writer = XmlReader.Create(input))
        _content = (type)_serializer.ReadObject(_writer);
      return _content;
    }

  }
}
