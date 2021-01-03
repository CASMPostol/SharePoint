//<summary>
//  Title   : static class JsonSerializer
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-02-20 15:40:38 +0100 (pt., 20 lut 2015) $
//  $Rev: 11394 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePoint/Serialization/JsonSerializer.cs $
//  $Id: JsonSerializer.cs 11394 2015-02-20 14:40:38Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Runtime.Serialization.Json;        // Reference System.ServiceModel.Web
using System.Text;

namespace CAS.SharePoint.Serialization
{
  /// <summary>
  /// static class JsonSerializer
  /// </summary>
  public static class JsonSerializer
  {
    /// <summary>
    /// Serializes the specified object.
    /// </summary>
    /// <typeparam name="Type">The type of the <paramref name="objectToSerialize"/>.</typeparam>
    /// <param name="objectToSerialize">The object to be serialized.</param>
    public static string Serialize<Type>(Type objectToSerialize)
    {
      DataContractJsonSerializer _Srlzr = new DataContractJsonSerializer(typeof(Type));
      using (System.IO.MemoryStream _writer = new System.IO.MemoryStream())
      {
        _Srlzr.WriteObject(_writer, objectToSerialize);
        return _writer.ReadString();
      }
    }
    /// <summary>
    /// Deserializes the specified object <paramref name="serializedObject"/> represented as the Json ASCII string.
    /// </summary>
    /// <param name="serializedObject">The serialized object.</param>
    /// <returns></returns>
    public static Type Deserialize<Type>(string serializedObject)
    {
      DataContractJsonSerializer _Srlzr = new DataContractJsonSerializer(typeof(Type));
      using (System.IO.MemoryStream _writer = new System.IO.MemoryStream())
      {
        _writer.WriteString(serializedObject);
        return (Type)_Srlzr.ReadObject(_writer);
      }
    }

  }
}
