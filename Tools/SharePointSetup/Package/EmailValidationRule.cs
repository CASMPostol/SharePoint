using System.Windows.Controls;

namespace CAS.SharePoint.Setup.Package
{
  public class EmailValidationRule : ValidationRule
  {
    public override ValidationResult Validate( object value, System.Globalization.CultureInfo cultureInfo )
    {
      string _emailAddress = value as string;
      if ( _emailAddress == null )
        return ValidationResult.ValidResult;
      // Confirm that the e-mail address string is not empty.
      if ( _emailAddress.Length == 0 )
      {
        
        string _errorMessage = "e-mail address is required.";
        return new ValidationResult(false, _errorMessage);
      }
      // Confirm that there is an "@" and a "." in the e-mail address, and in the correct order.
      if ( _emailAddress.IndexOf( "@" ) > -1 )
      {
        if ( _emailAddress.IndexOf( ".", _emailAddress.IndexOf( "@" ) ) > _emailAddress.IndexOf( "@" ) )
          return ValidationResult.ValidResult;
      }
      string _msg = "e-mail address must be valid e-mail address format.\n For example 'someone@example.com' ";
      return new ValidationResult(false, _msg);
    }
  }
}
