//<summary>
//  Title   : class Double2StringConverter
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-27 10:47:33 +0100 (pon., 27 paź 2014) $
//  $Rev: 10915 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/CASCommon/ComponentModel/BoolNotConverter.cs $
//  $Id: BoolNotConverter.cs 10915 2014-10-27 09:47:33Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.ComponentModel;
using System.Windows.Data;

namespace CAS.Common.ComponentModel
{
  /// <summary>
  /// Double to string Converter
  /// </summary>
  [TypeConverter(typeof(bool))]
  public class BoolNotConverter : IValueConverter
  {

    #region IValueConverter Members
    /// <summary>
    /// Converts the specified value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (typeof(bool) != targetType)
        throw new ArgumentOutOfRangeException(String.Format("targetType", "Wrong target type {0} but expected bool", targetType.Name));
      return !(bool)value;
    }
    /// <summary>
    /// Converts the back.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="targetType">Type of the target.</param>
    /// <param name="parameter">The parameter.</param>
    /// <param name="culture">The culture.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException">ConvertBack doesn't make any sense.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException("ConvertBack doesn't make any sense.");
    }
    #endregion

  }

}