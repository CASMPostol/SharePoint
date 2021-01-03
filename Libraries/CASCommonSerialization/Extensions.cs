//_______________________________________________________________
//  Title   : Extensions
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2015-03-11 12:41:19 +0100 (śr., 11 mar 2015) $
//  $Rev: 11474 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/CASCommonSerialization/Extensions.cs $
//  $Id: Extensions.cs 11474 2015-03-11 11:41:19Z mpostol $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System;
using System.IO;
using System.Text;

namespace CAS.Common.Serialization
{
  /// <summary>
  /// Class Extensions - contains helper function extending basic functionality of the .NET selected classes.
  /// </summary>
  public static class Extensions
  {

    #region MemoryStream
    /// <summary>
    /// Gets the string from <see cref="System.IO.MemoryStream"/>. Decodes using <see cref="UTF8Encoding"/> a range of bytes from <see cref="System.IO.MemoryStream"/> a that 
    /// is an array of unsigned bytes from which this <paramref name="bytes"/> stream was created into a string.
    /// </summary>
    /// <param name="bytes">A stream whose backing store is memory containing encoded as the <see cref="UTF8Encoding"/> string .</param>
    /// <returns> A <see cref="System.String"/> containing the results of decoding the specified sequence of bytes.</returns>
    public static string ReadString(this MemoryStream bytes)
    {
      bytes.Flush();
      byte[] _buffer = new byte[bytes.Length];
      bytes.Position = 0;
      int _numberOfBytes = bytes.Read(_buffer, 0, Convert.ToInt32(bytes.Length));
      UTF8Encoding _encoding = new UTF8Encoding();
      return _encoding.GetString(_buffer, 0, Convert.ToInt32(bytes.Length));
    }
    /// <summary>
    /// Writes the string <paramref name="serializedObject"/> to <see cref="System.IO.MemoryStream"/> amd encodes it using <see cref="UTF8Encoding"/>. 
    /// The result as a range of bytes is written to <see cref="System.IO.MemoryStream"/> that is an array of unsigned bytes from which 
    /// this <paramref name="bytes"/> stream was created.
    /// </summary>
    /// <param name="bytes">A stream whose backing store is memory containing encoded as the <see cref="UTF8Encoding"/> string.</param>
    /// <param name="serializedObject">The serialized object.</param>
    public static void WriteString(this MemoryStream bytes, string serializedObject)
    {
      UTF8Encoding _encoding = new UTF8Encoding();
      byte[] _string = _encoding.GetBytes(serializedObject);
      bytes.Write(_string, 0, _string.Length);
      bytes.Flush();
      bytes.Position = 0;
    }
    #endregion

  }
}
