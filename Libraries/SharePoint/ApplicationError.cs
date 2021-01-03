//<summary>
//  Title   : ApplicationError exception.
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-02-07 11:54:17 +0100 (pt., 07 lut 2014) $
//  $Rev: 10293 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePoint/ApplicationError.cs $
//  $Id: ApplicationError.cs 10293 2014-02-07 10:54:17Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Web.UI.WebControls;
using CAS.SharePoint.Web;

namespace CAS.SharePoint
{
  /// <summary>
  /// ApplicationError exception.
  /// </summary>
  public class ApplicationError: ApplicationException
  {
    /// <summary>
    /// Gets the detailed location in code.
    /// </summary>
    public string At { get; private set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationError" /> class.
    /// </summary>
    /// <param name="_source">The source class of the problem.</param>
    /// <param name="at">Location of the error.</param>
    /// <param name="_message">The message.</param>
    /// <param name="_innerException">The inner exception.</param>
    public ApplicationError( string _source, string at, string _message, Exception _innerException )
      : base( _message, _innerException )
    {
      Source = _source;
      At = at;
    }
    /// <summary>
    /// Gets a message that describes the current exception.
    /// </summary>
    /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
    public override string Message
    {
      get
      {
        string _format = "Application error at: {0}/{1} Message: {2}.";
        return String.Format( _format, Source, At, base.Message );
      }
    }
    /// <summary>
    /// Creates the message.
    /// </summary>
    /// <param name="at">At.</param>
    /// <param name="details">if set to <c>true</c> a string representation of the frames on the call stack at the time the current exception was thrown is also provided.</param>
    /// <returns><see cref="Literal"/> providing the formated exception message.</returns>
    public Literal CreateMessage( string at, bool details )
    {
      Literal _return = new Literal()
      {
        Text = CommonDefinitions.Convert2ErrorMessageFormat( Message )
      };
      if (details) 
        _return.Text += "Stack trace: + " + StackTrace;
      return _return;
    }
    private ApplicationError()
      : base()
    { }
  }
}
