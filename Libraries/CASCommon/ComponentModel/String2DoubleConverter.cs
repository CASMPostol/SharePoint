//<summary>
//  Title   : class Double2StringConverter
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-27 10:47:33 +0100 (pon., 27 paź 2014) $
//  $Rev: 10915 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/ComponentModel/String2DoubleConverter.cs $
//  $Id: String2DoubleConverter.cs 10915 2014-10-27 09:47:33Z mpostol $
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
  [TypeConverter(typeof(string))]
  public class Double2StringConverter : IValueConverter
  {

    #region IValueConverter Members
    /// <summary>
    /// Modifies the source data before passing it to the target for display in the UI.
    /// </summary>
    /// <param name="value">The source data being passed to the target.</param>
    /// <param name="targetType">The <see cref="T:System.Type" /> of data expected by the target dependency property.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
    /// <param name="culture">The culture of the conversion.</param>
    /// <returns>
    /// The value to be passed to the target dependency property.
    /// </returns>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      double _doubleValue = (double)value;
      if (typeof(String) != targetType)
        throw new ArgumentOutOfRangeException(String.Format("targetType", "Wrong target type {0} but expected String", targetType.Name));
      return _doubleValue.ToString("F2", culture);
    }
    /// <summary>
    /// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay" /> bindings.
    /// </summary>
    /// <param name="value">The target data being passed to the source.</param>
    /// <param name="targetType">The <see cref="T:System.Type" /> of data expected by the source object.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
    /// <param name="culture">The culture of the conversion.</param>
    /// <returns>
    /// The value to be passed to the source object.
    /// </returns>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      string _stringValue = (String)value;
      if (typeof(Double) != targetType)
        throw new ArgumentOutOfRangeException(String.Format("targetType", "Wrong target type {0} but expected Double", targetType.Name));
      return Double.Parse(_stringValue, culture);
    }
    #endregion

  }

}