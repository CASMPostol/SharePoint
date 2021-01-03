//<summary>
//  Title   : NamedTraceLogger
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2015-04-15 17:55:29 +0200 (śr., 15 kwi 2015) $
//  $Rev: 11589 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePoint/Logging/NamedTraceLogger.cs $
//  $Id: NamedTraceLogger.cs 11589 2015-04-15 15:55:29Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Common.Configuration;
using CAS.SharePoint.Common.Logging;
using CAS.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace CAS.SharePoint.Logging
{
  /// <summary>
  /// Class NamedTraceLogger is implemented as <see cref="TraceSource"/> with the name defined in the setting by the entry Default.TraceSourceName.
  /// </summary>
  public class NamedTraceLogger : ILogger
  {

    #region public
    /// <summary>
    /// Delegate TraceAction
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="eventId">The event identifier.</param>
    /// <param name="severity">The severity.</param>
    public delegate void TraceAction(string message, int eventId, TraceSeverity severity);
    /// <summary>
    /// Gets the logger service - it is implemented as the singleton.
    /// </summary>
    /// <value>The logger.</value>
    public static NamedTraceLogger Logger
    {
      get
      {
        if (m_NamedTraceLogger == null)
        {
          ILogger m_logger = new DoNothingLogger();
          string m_TraceSourceName = typeof(NamedTraceLogger).Name;
          try
          {
            Microsoft.Practices.ServiceLocation.IServiceLocator _serviceLocator = SharePointServiceLocator.GetCurrent();
            m_logger = _serviceLocator.GetInstance<ILogger>();
            m_TraceSourceName = Assembly.GetCallingAssembly().GetName().Name;
          }
          catch (Exception) { }
          m_NamedTraceLogger = new NamedTraceLogger(m_TraceSourceName, m_logger);
        }
        return m_NamedTraceLogger;
      }
    }
    /// <summary>
    /// Traces the progress change.
    /// </summary>
    /// <param name="progress">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
    public void TraceProgressChange(ProgressChangedEventArgs progress)
    {
      if (progress.UserState == null)
        return;
      m_logger.TraceToDeveloper(progress.UserState.ToString(), TraceSeverity.Verbose);
    }
    /// <summary>
    /// Registers the logger source.
    /// </summary>
    /// <param name="LoggingArea">The logging area.</param>
    /// <param name="categories">The area categories.</param>
    public static void RegisterLoggerSource(string LoggingArea, string[] categories)
    {
      DiagnosticsArea _newArea = new DiagnosticsArea(LoggingArea);
      foreach (string _cn in categories)
        _newArea.DiagnosticsCategories.Add(new DiagnosticsCategory(_cn, EventSeverity.Verbose, TraceSeverity.Verbose));
      IConfigManager configMgr = SharePointServiceLocator.GetCurrent().GetInstance<IConfigManager>();
      DiagnosticsAreaCollection _configuredAreas = new DiagnosticsAreaCollection(configMgr);
      var existingArea = _configuredAreas[_newArea.Name];
      if (existingArea == null)
        _configuredAreas.Add(_newArea);
      else
        foreach (DiagnosticsCategory _dc in _newArea.DiagnosticsCategories)
        {
          DiagnosticsCategory existingCategory = existingArea.DiagnosticsCategories[_dc.Name];
          if (existingCategory == null)
            existingArea.DiagnosticsCategories.Add(_dc);
        }
      _configuredAreas.SaveConfiguration();
    }
    /// <summary>
    /// Unregisters the logger source.
    /// </summary>
    /// <param name="loggingArea">The logging area.</param>
    public static void UnregisterLoggerSource(string loggingArea)
    {
      IConfigManager _configMgr = SharePointServiceLocator.GetCurrent().GetInstance<IConfigManager>();
      DiagnosticsAreaCollection _configuredAreas = new DiagnosticsAreaCollection(_configMgr);
      DiagnosticsArea areaToRemove = _configuredAreas[loggingArea];
      if (areaToRemove != null)
        _configuredAreas.Remove(areaToRemove);
      _configuredAreas.SaveConfiguration();
    }
    #endregion

    #region ILogger
    /// <summary>
    /// Writes information about an exception into the log from the sandbox.
    /// </summary>
    /// <param name="exception">The exception to write into the log to be read by operations.</param>
    /// <param name="additionalMessage">Additional information about the exception message.</param>
    /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
    /// administrators and IT PRo's to monitor the EventLog of a system.</param>
    /// <param name="severity">The severity of the exception.</param>
    /// <param name="category">The category to write the message to.</param>
    public void LogToOperations(Exception exception, string additionalMessage, int eventId, Common.Logging.SandboxEventSeverity severity, string category)
    {
      m_logger.LogToOperations(exception, additionalMessage, eventId, severity, category);
    }
    /// <summary>
    /// Writes information about an exception into the log, don't use from sandbox.
    /// </summary>
    /// <param name="exception">The exception to write into the log to be read by operations.</param>
    /// <param name="additionalMessage">Additional information about the exception message.</param>
    /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
    /// administrators and IT PRo's to monitor the EventLog of a system.</param>
    /// <param name="severity">The severity of the exception.</param>
    /// <param name="category">The category to write the message to.</param>
    public void LogToOperations(Exception exception, string additionalMessage, int eventId, Microsoft.SharePoint.Administration.EventSeverity severity, string category)
    {
      m_logger.LogToOperations(exception, additionalMessage, eventId, severity, category);
    }
    /// <summary>
    /// Writes information about an exception into the log to be read by operations from the sandbox.
    /// </summary>
    /// <param name="exception">The exception to write into the log.</param>
    /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
    /// administrators and IT PRo's to monitor the EventLog of a system.</param>
    /// <param name="severity">The severity of the exception.</param>
    /// <param name="category">The category to write the message to.</param>
    public void LogToOperations(Exception exception, int eventId, Common.Logging.SandboxEventSeverity severity, string category)
    {
      m_logger.LogToOperations(exception, eventId, severity, category);
    }
    /// <summary>
    /// Writes information about an exception into the log to be read by operations. Don't use from sandbox.
    /// </summary>
    /// <param name="exception">The exception to write into the log.</param>
    /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
    /// administrators and IT PRo's to monitor the EventLog of a system.</param>
    /// <param name="severity">The severity of the exception.</param>
    /// <param name="category">The category to write the message to.</param>
    public void LogToOperations(Exception exception, int eventId, Microsoft.SharePoint.Administration.EventSeverity severity, string category)
    {
      m_logger.LogToOperations(exception, eventId, severity, category);
    }
    /// <summary>
    /// Log a message from the sandbox with specified <paramref name="message" />, <paramref name="eventId" />, <paramref name="severity" />
    /// and <paramref name="category" />.
    /// </summary>
    /// <param name="message">The message to write into the log.</param>
    /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
    /// administrators and IT PRo's to monitor the EventLog of a system.</param>
    /// <param name="severity">How serious the event is.</param>
    /// <param name="category">The category of the log message.</param>
    public void LogToOperations(string message, int eventId, Common.Logging.SandboxEventSeverity severity, string category)
    {
      m_logger.LogToOperations(message, eventId, severity, category);
    }
    /// <summary>
    /// Log a message with specified <paramref name="message" />, <paramref name="eventId" />, <paramref name="severity" />
    /// and <paramref name="category" />.  Don't use in sandbox.
    /// </summary>
    /// <param name="message">The message to write into the log.</param>
    /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
    /// administrators and IT PRo's to monitor the EventLog of a system.</param>
    /// <param name="severity">How serious the event is.</param>
    /// <param name="category">The category of the log message.</param>
    public void LogToOperations(string message, int eventId, Microsoft.SharePoint.Administration.EventSeverity severity, string category)
    {
      m_logger.LogToOperations(message, eventId, severity, category);
    }
    /// <summary>
    /// Writes information about an exception into the log, works from sandbox.
    /// </summary>
    /// <param name="exception">The exception to write into the log to be read by operations.</param>
    /// <param name="additionalMessage">Additional information about the exception message.</param>
    /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
    /// administrators and IT PRo's to monitor the EventLog of a system.</param>
    public void LogToOperations(Exception exception, string additionalMessage, int eventId)
    {
      m_logger.LogToOperations(exception, additionalMessage, eventId);
    }
    /// <summary>
    /// Writes an error message into the log with specified event Id, works from sandbox.
    /// </summary>
    /// <param name="message">The message to write into the log.</param>
    /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
    /// administrators and IT PRo's to monitor the EventLog of a system.</param>
    /// <param name="category">The category to write the message to.</param>
    public void LogToOperations(string message, int eventId, string category)
    {
      m_logger.LogToOperations(message, eventId, category);
    }
    /// <summary>
    /// Writes an error message into the log from the sandbox.
    /// </summary>
    /// <param name="message">The message to write</param>
    /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
    /// administrators and IT PRo's to monitor the EventLog of a system.</param>
    /// <param name="severity">How serious the event is.</param>
    public void LogToOperations(string message, int eventId, Common.Logging.SandboxEventSeverity severity)
    {
      m_logger.LogToOperations(message, eventId, severity);
    }
    /// <summary>
    /// Writes an error message into the log, don't use in sandbox.
    /// </summary>
    /// <param name="message">The message to write</param>
    /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
    /// administrators and IT PRo's to monitor the EventLog of a system.</param>
    /// <param name="severity">How serious the event is.</param>
    public void LogToOperations(string message, int eventId, Microsoft.SharePoint.Administration.EventSeverity severity)
    {
      m_logger.LogToOperations(message, eventId, severity);
    }
    /// <summary>
    /// Writes an error message into the log with specified category, works from sandbox.
    /// </summary>
    /// <param name="message">The message to write into the log.</param>
    /// <param name="category">The category to write the message to.</param>
    public void LogToOperations(string message, string category)
    {
      m_logger.LogToOperations(message, category);
    }
    /// <summary>
    /// Writes information about an exception into the log to be read by operations, , works from sandbox.
    /// </summary>
    /// <param name="exception">The exception to write into the log.</param>
    /// <param name="additionalMessage">Additional information about the exception message.</param>
    public void LogToOperations(Exception exception, string additionalMessage)
    {
      m_logger.LogToOperations(exception, additionalMessage);
    }
    /// <summary>
    /// Writes an error message into the log with specified event Id, works from sandbox.
    /// </summary>
    /// <param name="message">The message to write into the log.</param>
    /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
    /// administrators and IT PRo's to monitor the EventLog of a system.</param>
    public void LogToOperations(string message, int eventId)
    {
      m_logger.LogToOperations(message, eventId);
    }
    /// <summary>
    /// Writes an error message into the log from the sandbox.
    /// </summary>
    /// <param name="message">The message to write</param>
    /// <param name="severity">How serious the event is.</param>
    public void LogToOperations(string message, Common.Logging.SandboxEventSeverity severity)
    {
      m_logger.LogToOperations(message, severity);
    }
    /// <summary>
    /// Writes an error message into the log, do not use in sandbox.
    /// </summary>
    /// <param name="message">The message to write</param>
    /// <param name="severity">How serious the event is.</param>
    public void LogToOperations(string message, Microsoft.SharePoint.Administration.EventSeverity severity)
    {
      m_logger.LogToOperations(message, severity);
    }
    /// <summary>
    /// Writes information about an exception into the log to be read by operations,
    /// works from sandbox.
    /// </summary>
    /// <param name="exception">The exception to write into the log.</param>
    public void LogToOperations(Exception exception)
    {
      m_logger.LogToOperations(exception);
    }
    /// <summary>
    /// Writes an error message into the log, works from sandbox.
    /// </summary>
    /// <param name="message">The message to write</param>
    public void LogToOperations(string message)
    {
      m_logger.LogToOperations(message);
    }
    /// <summary>
    /// Writes information about an exception into the log from the sandbox.
    /// Will work from outside the sandbox as well.
    /// </summary>
    /// <param name="exception">The exception to write into the log.</param>
    /// <param name="additionalMessage">Additional information about the exception message.</param>
    /// <param name="eventId">The eventId that corresponds to the event.</param>
    /// <param name="severity">The severity of the exception.</param>
    /// <param name="category">The category to write the message to.</param>
    public void TraceToDeveloper(Exception exception, string additionalMessage, int eventId, Common.Logging.SandboxTraceSeverity severity, string category)
    {
      m_logger.TraceToDeveloper(exception, additionalMessage, eventId, severity, category);
    }
    /// <summary>
    /// Writes information about an exception into the log. Don't use from sandbox.
    /// </summary>
    /// <param name="exception">The exception to write into the log.</param>
    /// <param name="additionalMessage">Additional information about the exception message.</param>
    /// <param name="eventId">The eventId that corresponds to the event.</param>
    /// <param name="severity">The severity of the exception.</param>
    /// <param name="category">The category to write the message to.</param>
    public void TraceToDeveloper(Exception exception, string additionalMessage, int eventId, Microsoft.SharePoint.Administration.TraceSeverity severity, string category)
    {
      m_logger.TraceToDeveloper(exception, additionalMessage, eventId, severity, category);
    }
    /// <summary>
    /// Writes information about an exception into the log from the sandbox.  This will work
    /// from outside the sandbox as well.
    /// </summary>
    /// <param name="exception">The exception to write into the log.</param>
    /// <param name="eventId">The eventId that corresponds to the event.</param>
    /// <param name="severity">The severity of the exception.</param>
    /// <param name="category">The category to write the message to.</param>
    public void TraceToDeveloper(Exception exception, int eventId, Common.Logging.SandboxTraceSeverity severity, string category)
    {
      m_logger.TraceToDeveloper(exception, eventId, severity, category);
    }
    /// <summary>
    /// Writes information about an exception into the log, don't use from sandbox.
    /// </summary>
    /// <param name="exception">The exception to write into the log.</param>
    /// <param name="eventId">The eventId that corresponds to the event.</param>
    /// <param name="severity">The severity of the exception.</param>
    /// <param name="category">The category to write the message to.</param>
    public void TraceToDeveloper(Exception exception, int eventId, Microsoft.SharePoint.Administration.TraceSeverity severity, string category)
    {
      m_logger.TraceToDeveloper(exception, eventId, severity, category);
    }
    /// <summary>
    /// Writes a diagnostic message into the trace log, with specified <see cref="T:CAS.SharePoint.Common.Logging.SandboxTraceSeverity" />.
    /// </summary>
    /// <param name="message">The message to write into the log.</param>
    /// <param name="eventId">The eventId that corresponds to the event.</param>
    /// <param name="severity">The severity of the trace.</param>
    /// <param name="category">The category to write the message to.</param>
    public void TraceToDeveloper(string message, int eventId, Common.Logging.SandboxTraceSeverity severity, string category)
    {
      m_logger.TraceToDeveloper(message, eventId, severity, category);
    }
    /// <summary>
    /// Writes a diagnostic message into the trace log, with specified <see cref="T:Microsoft.SharePoint.Administration.TraceSeverity" />. Don't
    /// use in sandbox.
    /// </summary>
    /// <param name="message">The message to write into the log.</param>
    /// <param name="eventId">The eventId that corresponds to the event.</param>
    /// <param name="severity">The severity of the trace.</param>
    /// <param name="category">The category to write the message to.</param>
    public void TraceToDeveloper(string message, int eventId, Microsoft.SharePoint.Administration.TraceSeverity severity, string category)
    {
      m_logger.TraceToDeveloper(message, eventId, severity, category);
    }
    /// <summary>
    /// Writes information about an exception into the log, works from sandbox.
    /// </summary>
    /// <param name="exception">The exception to write into the log to be read by operations.</param>
    /// <param name="additionalMessage">Additional information about the exception message.</param>
    /// <param name="eventId">The eventId that corresponds to the event.</param>
    public void TraceToDeveloper(Exception exception, string additionalMessage, int eventId)
    {
      m_logger.TraceToDeveloper(exception, additionalMessage, eventId);
    }
    /// <summary>
    /// Writes a diagnostic message into the log, works from sandbox.
    /// </summary>
    /// <param name="message">The message to write into the log.</param>
    /// <param name="eventId">The eventId that corresponds to the event.</param>
    /// <param name="category">The category to write the message to.</param>
    public void TraceToDeveloper(string message, int eventId, string category)
    {
      m_logger.TraceToDeveloper(message, eventId, category);
    }
    /// <summary>
    /// Writes a diagnostic message into the trace log, with severity <see cref="F:Microsoft.SharePoint.Administration.TraceSeverity.Medium" />,
    /// works from sandbox.
    /// </summary>
    /// <param name="message">The message to write into the log.</param>
    /// <param name="category">The category to write the message to.</param>
    public void TraceToDeveloper(string message, string category)
    {
      m_logger.TraceToDeveloper(message, category);
    }
    /// <summary>
    /// Writes a diagnostic message into the trace log, with severity <see cref="F:Microsoft.SharePoint.Administration.TraceSeverity.Medium" />,
    /// works from sandbox.
    /// </summary>
    /// <param name="message">The message to write into the log.</param>
    /// <param name="eventId">The eventId that corresponds to the event. This value, coupled with the EventSource is often used by
    /// administrators and IT PRo's to monitor the EventLog of a system.</param>
    public void TraceToDeveloper(string message, int eventId)
    {
      m_logger.TraceToDeveloper(message, eventId);
    }
    /// <summary>
    /// Write a diagnostic message into the log for the default category with default severity
    /// from the sandbox, works from sandbox.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="severity">The severity.</param>
    public void TraceToDeveloper(string message, Common.Logging.SandboxTraceSeverity severity)
    {
      m_logger.TraceToDeveloper(message, severity);
    }
    /// <summary>
    /// Write a diagnostic message into the log for the default category with severity, don't use
    /// from sandbox.
    /// </summary>
    /// <param name="message">The message to write into the log.</param>
    /// <param name="severity">The severity of the exception.</param>
    public void TraceToDeveloper(string message, Microsoft.SharePoint.Administration.TraceSeverity severity)
    {
      m_logger.TraceToDeveloper(message, severity);
    }
    /// <summary>
    /// Writes information about an exception into the log, works from sandbox.
    /// </summary>
    /// <param name="exception">The exception to write into the log.</param>
    /// <param name="additionalMessage">Additional information about the exception message.</param>
    public void TraceToDeveloper(Exception exception, string additionalMessage)
    {
      m_logger.TraceToDeveloper(exception, additionalMessage);
    }
    /// <summary>
    /// Writes information about an exception into the log. with the default category, default id,
    /// and default severity, works from sandbox.
    /// </summary>
    /// <param name="exception">The exception to write into the log.</param>
    public void TraceToDeveloper(Exception exception)
    {
      m_logger.TraceToDeveloper(exception);
    }
    /// <summary>
    /// Write a diagnostic message into the log, with the default category, default id,
    /// and default severity, works from sandbox.
    /// </summary>
    /// <param name="message">The message to write into the log.</param>
    public void TraceToDeveloper(string message)
    {
      m_logger.TraceToDeveloper(message);
    }
    #endregion

    #region private
    private class DoNothingLogger : ILogger
    {
      public void LogToOperations(Exception exception, string additionalMessage, int eventId, SandboxEventSeverity severity, string category)
      {
      }
      public void LogToOperations(Exception exception, string additionalMessage, int eventId, Microsoft.SharePoint.Administration.EventSeverity severity, string category)
      {
      }

      public void LogToOperations(Exception exception, int eventId, SandboxEventSeverity severity, string category)
      {
      }
      public void LogToOperations(Exception exception, int eventId, Microsoft.SharePoint.Administration.EventSeverity severity, string category)
      {
      }
      public void LogToOperations(string message, int eventId, SandboxEventSeverity severity, string category)
      {
      }
      public void LogToOperations(string message, int eventId, Microsoft.SharePoint.Administration.EventSeverity severity, string category)
      {
      }
      public void LogToOperations(Exception exception, string additionalMessage, int eventId)
      {
      }
      public void LogToOperations(string message, int eventId, string category)
      {
      }
      public void LogToOperations(string message, int eventId, SandboxEventSeverity severity)
      {
      }
      public void LogToOperations(string message, int eventId, Microsoft.SharePoint.Administration.EventSeverity severity)
      {
      }
      public void LogToOperations(string message, string category)
      {
      }
      public void LogToOperations(Exception exception, string additionalMessage)
      {
      }
      public void LogToOperations(string message, int eventId)
      {
      }
      public void LogToOperations(string message, SandboxEventSeverity severity)
      {
      }
      public void LogToOperations(string message, Microsoft.SharePoint.Administration.EventSeverity severity)
      {
      }
      public void LogToOperations(Exception exception)
      {
      }
      public void LogToOperations(string message)
      {
      }
      public void TraceToDeveloper(Exception exception, string additionalMessage, int eventId, SandboxTraceSeverity severity, string category)
      {
      }
      public void TraceToDeveloper(Exception exception, string additionalMessage, int eventId, Microsoft.SharePoint.Administration.TraceSeverity severity, string category)
      {
      }
      public void TraceToDeveloper(Exception exception, int eventId, SandboxTraceSeverity severity, string category)
      {
      }
      public void TraceToDeveloper(Exception exception, int eventId, Microsoft.SharePoint.Administration.TraceSeverity severity, string category)
      {
      }
      public void TraceToDeveloper(string message, int eventId, SandboxTraceSeverity severity, string category)
      {
      }
      public void TraceToDeveloper(string message, int eventId, Microsoft.SharePoint.Administration.TraceSeverity severity, string category)
      {
      }
      public void TraceToDeveloper(Exception exception, string additionalMessage, int eventId)
      {
      }
      public void TraceToDeveloper(string message, int eventId, string category)
      {
      }
      public void TraceToDeveloper(string message, string category)
      {
      }
      public void TraceToDeveloper(string message, int eventId)
      {
      }
      public void TraceToDeveloper(string message, SandboxTraceSeverity severity)
      {
      }
      public void TraceToDeveloper(string message, Microsoft.SharePoint.Administration.TraceSeverity severity)
      {
      }
      public void TraceToDeveloper(Exception exception, string additionalMessage)
      {
      }
      public void TraceToDeveloper(Exception exception)
      {
      }
      public void TraceToDeveloper(string message)
      {
      }
    }
    private static NamedTraceLogger m_NamedTraceLogger = null;
    private ILogger m_logger = null;
    private string m_TraceSourceName;
    private NamedTraceLogger(string traceSourceName, ILogger logger)
    {
      m_logger = logger;
      m_TraceSourceName = traceSourceName;
    }
    #endregion

  }
}
