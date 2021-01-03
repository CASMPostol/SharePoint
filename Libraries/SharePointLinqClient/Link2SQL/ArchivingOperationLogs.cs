//_______________________________________________________________
//  Title   : ArchivingOperationLogs
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2015-01-28 15:08:14 +0100 (śr., 28 sty 2015) $
//  $Rev: 11268 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePointLinqClient/Link2SQL/ArchivingOperationLogs.cs $
//  $Id: ArchivingOperationLogs.cs 11268 2015-01-28 14:08:14Z mpostol $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;

namespace CAS.SharePoint.Client.Link2SQL
{
  /// <summary>
  /// ActivitiesLogs table contains logs for each operation.
  /// </summary>
  public static class ArchivingOperationLogs
  {
    /// <summary>
    /// Operation Name 
    /// </summary>
    public enum OperationName
    {

      /// <summary>
      /// The cleanup operation name
      /// </summary>
      Cleanup,
      /// <summary>
      /// The synchronization operation name
      /// </summary>
      Synchronization,
      /// <summary>
      /// The archiving operation name
      /// </summary>
      Archiving
    }
    /// <summary>
    /// Updates the activities logs.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="sqlEntities">The SQL entities.</param>
    /// <param name="operation">The operation.</param>
    /// <param name="progressChanged">The progress changed.</param>
    public static void UpdateActivitiesLogs<TEntity>(DataContext sqlEntities, OperationName operation, Action<ProgressChangedEventArgs> progressChanged)
      where TEntity : class, IArchivingOperationLogs, new()
    {
      TEntity _logs = new TEntity()
      {
        Date = DateTime.Now,
        Operation = operation.ToString(),
        UserName = UserName()
      };
      sqlEntities.GetTable<TEntity>().InsertOnSubmit(_logs);
      sqlEntities.SubmitChanges();
      progressChanged(new ProgressChangedEventArgs(1, String.Format("Updated ActivitiesLogs for operation {0}", operation)));
    }
    /// <summary>
    /// Gets the recent actions.
    /// </summary>
    /// <typeparam name="TEntity">The type of the <see cref="Table{TEntity}" />.</typeparam>
    /// <param name="sqlEntities">The SQL entities.</param>
    /// <param name="operation">The operation.</param>
    /// <returns>Last entry of from the <see cref="Table{TEntity}" /> for the selected <paramref name="operation" />.</returns>
    public static TEntity GetRecentActions<TEntity>(DataContext sqlEntities, OperationName operation)
      where TEntity : class, IArchivingOperationLogs, new()
    {
      TEntity _recentActions = sqlEntities.GetTable<TEntity>().Where<TEntity>(x => x.Operation.Contains(operation.ToString())).OrderByDescending<TEntity, DateTime>(x => x.Date).FirstOrDefault();
      return _recentActions;
    }
    private static string UserName()
    {
      return String.Format(Properties.Settings.Default.ActivitiesLogsUserNamePattern, Environment.UserName, Environment.MachineName);
    }
  }
}
