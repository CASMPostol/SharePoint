//<summary>
//  Title   : ApplicationExceptionException
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2014-12-10 16:16:26 +0100 (śr., 10 gru 2014) $
//  $Rev: 11083 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/ApplicationException.sl.cs $
//  $Id: ApplicationException.sl.cs 11083 2014-12-10 15:16:26Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;

namespace CAS.Common
{
#if SILVERLIGHT
  /// <summary>
  /// ApplicationExceptionException implementation for SilverLight
  /// </summary>
  public class ApplicationException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationException"/> class.
    /// </summary>
    public ApplicationException() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ApplicationException(string message) : base(message) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="inner">The inner.</param>
    public ApplicationException(string message, Exception inner) : base(message, inner) { }
  }
#endif
}
