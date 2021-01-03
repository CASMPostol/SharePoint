//<summary>
//  Title   : SaveSQLScriptCommandBase
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Tools/SPExplorer/ModelView/SaveSQLScriptCommandBase.cs $
//  $Id: SaveSQLScriptCommandBase.cs 10838 2014-10-10 14:13:54Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

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
  /// Save SQL create tables query script for the Share Point Data Model Command - synchronous implementation of the <see cref="ICommand"/>
  /// </summary>
  internal abstract class SaveSQLScriptCommandBase : ICommand
  {

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
        string _path = Path.GetDirectoryName(_fileName);
        string _name = Path.GetFileNameWithoutExtension(_fileName);
        string _dstPath = Path.Combine(_path, _name);
        if (!Directory.Exists(_dstPath))
          Directory.CreateDirectory(_dstPath);
        StringBuilder _script = new StringBuilder();
        _script.AppendLine(String.Format("USE {0}", Properties.Settings.Default.DatabaseName));
        List<ListDescriptor> _listOfDescriptors = GetObjectModel.GetListsDescriptors();
        List<RegularColumn> _colums = new List<RegularColumn>();
        _colums.Add(new RegularColumn(ColumnType.PrivateKey, "ID", false, "INT IDENTITY(1,1)"));
        _colums.Add(new RegularColumn(ColumnType.Regular, "ListName", false, "NVARCHAR(255)"));
        _colums.Add(new RegularColumn(ColumnType.Regular, "ItemID", false, "INT"));
        _colums.Add(new RegularColumn(ColumnType.Regular, "FieldName", false, "NVARCHAR(255)"));
        _colums.Add(new RegularColumn(ColumnType.Regular, "FieldValue", false, "NVARCHAR(255)"));
        _colums.Add(new RegularColumn(ColumnType.Regular, "Modified", false, "DATETIME"));
        _colums.Add(new RegularColumn(ColumnType.Regular, "ModifiedBy", false, "NVARCHAR(255)"));
        _listOfDescriptors.Add( new ListDescriptor("History"){ Columns = _colums.ToArray()});
        _colums = new List<RegularColumn>();
        _colums.Add(new RegularColumn(ColumnType.PrivateKey, "ID", false, "INT IDENTITY(1,1)"));
        _colums.Add(new RegularColumn(ColumnType.Regular, "ListName", false, "NVARCHAR(255)"));
        _colums.Add(new RegularColumn(ColumnType.Regular, "ItemID", false, "INT"));
        _colums.Add(new RegularColumn(ColumnType.Regular, "Date", false, "DATETIME"));
        _colums.Add(new RegularColumn(ColumnType.Regular, "UserName", false, "NVARCHAR(255)"));
        _listOfDescriptors.Add(new ListDescriptor("ArchivingLogs") { Columns = _colums.ToArray() });
        _colums = new List<RegularColumn>();
        _colums.Add(new RegularColumn(ColumnType.PrivateKey, "ID", false, "INT IDENTITY(1,1)"));
        _colums.Add(new RegularColumn(ColumnType.Regular, "Operation", false, "NVARCHAR(255)"));
        _colums.Add(new RegularColumn(ColumnType.Regular, "Date", false, "DATETIME"));
        _colums.Add(new RegularColumn(ColumnType.Regular, "UserName", false, "NVARCHAR(255)"));
        _listOfDescriptors.Add(new ListDescriptor("ArchivingOperationLogs") { Columns = _colums.ToArray() });
        //TODO
        foreach (ListDescriptor _ldx in _listOfDescriptors)
        {
          string _tabScript = BuildScript(_ldx, _script);
          using (StreamWriter _file = new StreamWriter(Path.Combine(_dstPath, _ldx.SQLTableName + ".sql"), false))
            _file.Write(_tabScript);
          _script.Append(_tabScript);
        }
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

    private static string BuildScript(ListDescriptor _ldx, StringBuilder script)
    {
      SQLGenerator.CreateTableScript _generator = new SQLGenerator.CreateTableScript();
      _generator.ColumnDefinitions = new List<SQLGenerator.CreateTableScript.ColumDescriptor>(GetColumnDefinitions(_ldx.Columns));
      _generator.ConstrainDefinitions = new List<SQLGenerator.CreateTableScript.ConstrainDescriptor>(GetConstrainDescriptors(_ldx.Columns, _generator));
      _generator.PrivateKeyColumnName = _ldx.PFColumnName;
      _generator.TableName = _ldx.SQLTableName;
      return _generator.TransformText();
      //IEnumerable<PRColumn> _columns = ObjectModelRoot.ObjectModelRoot.Columns.Where<PRColumn>( _clx => _clx.) ;
    }
    private static IEnumerable<SQLGenerator.CreateTableScript.ColumDescriptor> GetColumnDefinitions(IEnumerable<RegularColumn> columns)
    {
      return columns.Select(clx => new SQLGenerator.CreateTableScript.ColumDescriptor() { ColumnName = clx.ColumnName, DataType = clx.SQLDataType, Nullable = clx.SQLNullable, Precision = clx.SQLPrecision });
    }
    private static IEnumerable<SQLGenerator.CreateTableScript.ConstrainDescriptor> GetConstrainDescriptors(IEnumerable<RegularColumn> columns, SQLGenerator.CreateTableScript parent)
    {
      return columns.Where<RegularColumn>(_clmx => _clmx is ForeignKeyColumn).Cast<ForeignKeyColumn>().Select<ForeignKeyColumn, SQLGenerator.CreateTableScript.ConstrainDescriptor>(clx => new SQLGenerator.CreateTableScript.ConstrainDescriptor(parent)
      {
        ReferencedColumn = clx.ColumnName,
        FGColumnName = clx.ReferencedColumn,
        ReferencedTableName = clx.ReferencedTableName
      });
    }
    private bool b_CanExecute = true;
    private bool CanExecuteBackUp
    {
      get
      {
        return b_CanExecute;//TODO && GetObjectModel != null;
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

  }
}
