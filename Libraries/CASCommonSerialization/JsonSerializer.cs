//<summary>
//  Title   : static class JsonSerializer
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-03-11 12:41:19 +0100 (śr., 11 mar 2015) $
//  $Rev: 11474 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommonSerialization/JsonSerializer.cs $
//  $Id: JsonSerializer.cs 11474 2015-03-11 11:41:19Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Runtime.Serialization.Json;

namespace CAS.Common.Serialization
{
  /// <summary>
  /// static class JsonSerializer
  /// </summary>
  public static class JsonSerialization
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
        _writer.Flush();
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
