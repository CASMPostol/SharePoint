//_______________________________________________________________
//  Title   : Name of Application
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2015-04-03 12:57:48 +0200 (pt., 03 kwi 2015) $
//  $Rev: 11553 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePoint/Serialization/XmlSerializer.cs $
//  $Id: XmlSerializer.cs 11553 2015-04-03 10:57:48Z mpostol $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System.IO;

namespace CAS.SharePoint.Serialization
{
  /// <summary>
  /// Class XmlSerializer.
  /// </summary>
  public static class XmlSerializer
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
        DocumentsFactory.XmlFile.WriteXmlFile<Type>(objectToSerialize, _writer, stylesheetName);
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
        return DocumentsFactory.XmlFile.ReadXmlFile<Type>(_writer);
      }
    }

  }
}
