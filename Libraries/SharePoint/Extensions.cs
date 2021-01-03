//<summary>
//  Title   : static class Extensions
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-02-25 16:12:08 +0100 (śr., 25 lut 2015) $
//  $Rev: 11412 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePoint/Extensions.cs $
//  $Id: Extensions.cs 11412 2015-02-25 15:12:08Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Web;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace CAS.SharePoint
{

  /// <summary>
  /// Extensions definitions
  /// </summary>
  public static class Extensions
  {

    #region public

    #region string
    /// <summary>
    /// String2s the int.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static int? String2Int(this string _val)
    {
      int _ret;
      if (int.TryParse(_val, out _ret))
      {
        return _ret;
      }
      return new Nullable<int>();
    }
    /// <summary>
    /// If <paramref name="value"/> is null or empty returns default localized message indicatin the the value is not available.
    /// </summary>
    /// <param name="value">The _value.</param>
    /// <returns>Returns <paramref name="value"/> or default localized message indicatin the the value is not available</returns>
    public static string NotAvailable(this string value)
    {
      return String.IsNullOrEmpty(value) ? "NotApplicable".GetLocalizedString() : value;
    }
    /// <summary>
    ///  Indicates whether the specified System.String object is null or an System.String.Empty string.
    /// </summary>
    /// <param name="_val"> A System.String reference.</param>
    /// <returns>
    ///   true if the value parameter is null or an empty string (""); otherwise, false.
    /// </returns>
    public static bool IsNullOrEmpty(this string _val)
    {
      return String.IsNullOrEmpty(_val);
    }
    /// <summary>
    /// Gets the localized string.
    /// </summary>
    /// <param name="val">The val.</param>
    /// <returns></returns>
    public static string GetLocalizedString(this string val)
    {
      if (m_RootResourceFileName.IsNullOrEmpty())
        return val;
      string _frmt = "$Resources:{0}";
      return SPUtility.GetLocalizedString(String.Format(_frmt, val), m_RootResourceFileName, (uint)CultureInfo.CurrentUICulture.LCID);
    }
    /// <summary>
    /// Gets the localization expression used to provide localized text for WebParts.
    /// </summary>
    /// <param name="val">The key used to find localized string for current culture.</param>
    /// <returns></returns>
    public static string GetLocalizationExpresion(this string val)
    {
      string _frmt = "$Resources:{0},{1}";
      return String.Format(_frmt, m_RootResourceFileName, val);
    }
    /// <summary>
    /// Determines whether <paramref name="guid"/> is correct <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The unique identifier .</param>
    /// <returns><b>true</b> if <paramref name="guid"/> is correct <see cref="Guid"/></returns>
    public static bool IsGuidRegEx(this string guid)
    {
      if (guid != null)
      {
        Regex guidRegEx = new Regex(@"^[{]?[0-9a-fA-F]{8}[-]?([0-9a-fA-F]{4}[-]?){3}[0-9a-fA-F]{12}[}]?$");
        return guidRegEx.IsMatch(guid);
      }
      return false;
    }
    /// <summary>
    /// Return <see cref="Guid"/> for the <paramref name="value"/> if it can by parse, Empty otherwise.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static Guid GuidTryParse(this string value)
    {
      Guid _ret = Guid.Empty;
      if (value.IsGuidRegEx())
        return new Guid(value);
      return _ret;
    }
    /// <summary>
    /// Gets the first capture.
    /// </summary>
    /// <param name="input">The string to be tested for a match. <see cref="string" />.</param>
    /// <param name="pattern">The regular expression pattern to match..</param>
    /// <param name="defaultValue">The default value.</param>
    /// <param name="syntaxErrors">The syntax errors list.</param>
    /// <returns>
    /// The string that match the pattern
    /// </returns>
    public static string GetFirstCapture(this string input, string pattern, string defaultValue, List<string> syntaxErrors)
    {
      string _ret = Match(input, pattern);
      if (!_ret.IsNullOrEmpty())
        return _ret;
      string _template = "Cannot find the pattern {0} in the string {1}.";
      syntaxErrors.Add(string.Format(_template, pattern, input));
      return defaultValue;
    }
    /// <summary>
    /// Gets the first capture.
    /// </summary>
    /// <param name="input">The string to be tested for a match. <see cref="string"/>.</param>
    /// <param name="pattern">The regular expression pattern to match..</param>
    /// <returns>The string that match the pattern</returns>
    public static string GetFirstCapture(this string input, string pattern)
    {
      string _ret = Match(input, pattern);
      if (!_ret.IsNullOrEmpty())
        return _ret;
      string _template = "Cannot find the pattern {0} in the string {1}.";
      throw GenericStateMachineEngine.ActionResult.NotValidated(string.Format(_template, pattern, input));
    }
    /// <summary>
    /// Gets the first capture.
    /// </summary>
    /// <param name="input">The string to be tested for a match. <see cref="string" />.</param>
    /// <param name="pattern">The regular expression pattern to match..</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>
    /// The string that match the pattern
    /// </returns>
    public static string GetFirstCapture(this string input, string pattern, string defaultValue)
    {
      string _ret = Match(input, pattern);
      if (!_ret.IsNullOrEmpty())
        return _ret;
      return defaultValue;
    }
    /// <summary>
    /// Returns valid for SharePoint string removes control characters and limits its length.
    /// </summary>
    /// <param name="_value">The _value.</param>
    /// <returns></returns>
    public static string SPValidSubstring(this string _value)
    {
      string _goodsDescription = Microsoft.SharePoint.Utilities.SPStringUtility.RemoveControlChars(_value);
      int _gdl = _goodsDescription.Length;
      do
      {
        _goodsDescription = _goodsDescription.Replace("  ", " ");
        if (_gdl == _goodsDescription.Length)
          break;
        _gdl = _goodsDescription.Length;
      } while (true);
      int SPStringMAxLength = 250;
      if (_gdl >= SPStringMAxLength)
        _goodsDescription = _goodsDescription.Remove(SPStringMAxLength);
      return _goodsDescription;
    }
    #endregion

    #region DateTime
    /// <summary>
    /// Gets the value or minimal date 01/01/1990.
    /// </summary>
    /// <param name="value">The value to be returned if not null, otherwise DateTimeNull is returned. />.</param>
    /// <returns>The value /> </returns>
    public static DateTime GetValueOrNull(this DateTime? value) { return value.GetValueOrDefault(SPMinimum); }
    /// <summary>
    /// Determines whether the specified value is null or equal <see cref="DateTime" />.
    /// </summary>
    /// <param name="value">The value of <see cref="DateTime" />.</param>
    /// <returns>
    ///   <c>true</c> if the specified value is null or equal <see cref="DateTime" />; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNull(this DateTime? value) { return !value.HasValue || value.Value == SPMinimum; }
    /// <summary>
    /// Number of the week.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>Week number.</returns>
    public static int WeekNumber(this DateTime value) { return value.DayOfYear / 7; }
    /// <summary>
    /// The minimum date for - to avoid setting today 
    /// </summary>
    public static DateTime SPMinimum = new DateTime(1930, 1, 1); //Silverlight object model doesn't support 1900
    #endregion

    #region double
    /// <summary>
    /// Rounds the mass.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    public static double RountMass(this double value)
    {
      return Math.Round(value, 2);
    }
    /// <summary>
    /// Rounts the mass upper.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    public static double RountMassUpper(this double value)
    {
      return Math.Round(value + 0.005, 2);
    }
    /// <summary>
    /// Rounds the currency.
    /// </summary>
    /// <param name="value">The value to be rounded.</param>
    /// <returns>The <paramref name="value"/> rounded; the number of fractional digits in the return value is 2.</returns>
    public static double RoundCurrency(this double value)
    {
      return Math.Round(value, 3);
    }
    #endregion

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
      UTF8Encoding _encoding = new UTF8Encoding();
      return (_encoding.GetString(bytes.GetBuffer(), 0, Convert.ToInt32(bytes.Length)));
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

    #region Miscelanious
    /// <summary>
    /// Convert int to string.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static string IntToString(this int? _val)
    {
      return _val.HasValue ? _val.Value.ToString() : String.Empty;
    }
    /// <summary>
    /// Finds the control recursive.
    /// </summary>
    /// <param name="rootControl">The root control.</param>
    /// <param name="controlID">The control ID.</param>
    /// <returns></returns>
    public static Control FindControlRecursive(this Control rootControl, string controlID)
    {
      if (rootControl.ID == controlID)
        return rootControl;
      foreach (Control controlToSearch in rootControl.Controls)
      {
        Control controlToReturn = FindControlRecursive(controlToSearch, controlID);
        if (controlToReturn != null)
          return controlToReturn;
      }
      return null;
    }
    /// <summary>
    /// Values the or exception.
    /// </summary>
    /// <typeparam name="type">The type of the value.</typeparam>
    /// <param name="source">The source where the .</param>
    /// <param name="at">At.</param>
    /// <param name="value">The value.</param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <returns></returns>
    /// <exception cref="ApplicationError">null</exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    public static type ValueOrException<type>(this Nullable<type> value, string source, string at, string parameterName)
      where type : struct
    {
      if (value.HasValue)
        return value.Value;
      throw new ApplicationError(source, at, String.Format("The HasValue property of the parameter {0} is false.", parameterName), null);
    }
    /// <summary>
    /// Exceptions the diagnostic message.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="at">Bookmark of the current position.</param>
    public static string ExceptionDiagnosticMessage(this Exception exception, string at)
    {
      string _tmp = "Exception at {0} message: {1}; stack: {2}";
      return String.Format(_tmp, at, exception.Message, exception.StackTrace);
    }
    #endregion

    #endregion

    #region private
    private static string Match(string input, string pattern)
    {
      Match _match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
      string _ret = String.Empty;
      if (_match.Success && _match.Groups.Count > 1)
        _ret = _match.Groups[1].Value;
      return _ret;
    }
    private static string m_RootResourceFileName = "CASSmartFactoryIPRCode"; //"CASSmartFactoryShepherdCode";
    #endregion

  }

}
