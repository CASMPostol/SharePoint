using CAS.SharePoint.Tools.SPExplorer.SPMetalParameters.SQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CAS.SharePoint.Tools.SPExplorer.ModelView
{
  /// <summary>
  /// Save SQL DROP TABLES query script for the Share Point Data Model Command - synchronous implementation of the <see cref="ICommand"/>
  /// </summary>

  internal abstract class SaveDropTableSQLScriptCommandBase : ICommand
  {

    #region public API
    //creators
    public SaveDropTableSQLScriptCommandBase()
    {
    }
    //internal abstract 
    internal abstract string FileName { get; }
    internal abstract SPMetalParameters.PRWeb GetObjectModel { get; }
    internal abstract void Progress(ProgressChangedEventArgs progressInfo);

    #region ICommand
    /// <summary>
    /// Defines the method that determines whether the command can execute in its current state.
    /// </summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
    /// <returns>
    /// true if this command can be executed; otherwise, false.
    /// </returns>
    public bool CanExecute(object parameter)
    {
      return CanExecuteBackUp;
    }
    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute.
    /// </summary>
    public event EventHandler CanExecuteChanged;
    /// <summary>
    /// Defines the method to be called when the command is invoked.
    /// </summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
    public void Execute(object parameter)
    {
      try
      {
        if (GetObjectModel == null)
          return;
        Properties.Settings.Default.SelectedColumns = GetObjectModel.GetConfiguration();
        Properties.Settings.Default.Save();
        string _fileName = FileName;
        if (_fileName.IsNullOrEmpty())
          return;
        CanExecuteBackUp = false;
        StringBuilder _script = new StringBuilder();
        _script.AppendLine(String.Format("USE {0}", Properties.Settings.Default.DatabaseName));
        _script.AppendLine(String.Format("DROP TABLE History;\r\nDROP TABLE ArchivingLogs;\r\nDROP TABLE ActivitiesLogs;"));
        BuildScript(GetObjectModel.GetListsDescriptors(), _script);
        using (StreamWriter _file = new StreamWriter(_fileName, false))
          _file.Write(_script.ToString());
      }
      catch (Exception _ex)
      {
        Progress(new ProgressChangedEventArgs(100, _ex.ToString()));
      }
      finally
      {
        CanExecuteBackUp = true;
      }
    }
    #endregion

    #endregion

    #region private
    //vars
    private bool b_CanExecute = true;
    //procedures
    private static void BuildScript(List<ListDescriptor> objectModel, StringBuilder script)
    {
      for (int i = objectModel.Count - 1; i >= 0; i--)
      {
        SQLGenerator.DropTableScript _generator = new SQLGenerator.DropTableScript();
        _generator.TableName = objectModel[i].SQLTableName;
        script.Append(_generator.TransformText());
      }
    }
    private bool CanExecuteBackUp
    {
      get
      {
        return b_CanExecute; // && GetObjectModel != null;
      }
      set
      {
        if (b_CanExecute == value)
          return;
        b_CanExecute = value;
        EventHandler _canExecuteChanged = CanExecuteChanged;
        if (_canExecuteChanged == null)
          return;
        _canExecuteChanged(this, EventArgs.Empty);
      }
    }
    #endregion

  }
}