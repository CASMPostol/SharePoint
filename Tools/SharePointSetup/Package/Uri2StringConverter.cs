using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace CAS.SharePoint.Setup.Package
{
  [ValueConversionAttribute( typeof( string ), typeof( Uri ) )]
  public class Uri2StringConverter: ValidationRule, IValueConverter
  {

    #region IValueConverter Members
    public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
    {
      if ( targetType != typeof( String ) )
        throw new ArgumentException( String.Format( "Unsupported terget type {0}", targetType ), "targetType" );
      return _basicConverter.ConvertTo( value, targetType );
    }
    public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
    {
      if ( targetType != typeof( Uri ) )
        throw new ArgumentException( String.Format( "Unsupported terget type {0}", targetType ), "targetType" );
      return _basicConverter.ConvertFrom( value );
    }
    #endregion

    #region ValidationRule
    /// <summary>
    /// When overridden in a derived class, performs validation checks on a value.
    /// </summary>
    /// <param name="value">The value from the binding target to check.</param>
    /// <param name="cultureInfo">The culture to use in this rule.</param>
    /// <returns>
    /// A <see cref="T:System.Windows.Controls.ValidationResult" /> object.
    /// </returns>
    public override ValidationResult Validate( object value, System.Globalization.CultureInfo cultureInfo )
    {
      if (value is Uri)
        return ValidationResult.ValidResult;
      string _uriString = value as String;
      if (_uriString == null )
      {
        string _unsuportedFormat = "Unsuported type: {0}";
        return new ValidationResult( false, String.Format( _unsuportedFormat, value.GetType() ) );
      }
      Uri _newUri = default(Uri);
      if ( Uri.TryCreate( _uriString, UriKind.Absolute, out _newUri ) )
        return ValidationResult.ValidResult;
      string _converErrorformat = "Cannot convert the string: {0} to valid uri. Provide a valid string that specifies an URL. For example 'http://computer.domain:Port'. It may either be server-relative or absolute for typical sites.";
      return new ValidationResult( false, String.Format( _converErrorformat, value ) );
    }
    #endregion

    #region private
    private UriTypeConverter _basicConverter = new UriTypeConverter();
    #endregion
  }
}
