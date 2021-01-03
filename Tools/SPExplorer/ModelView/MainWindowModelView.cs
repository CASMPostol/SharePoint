//<summary>
//  Title   : MainWindowModelView
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/SPExplorer/ModelView/MainWindowModelView.cs $
//  $Id: MainWindowModelView.cs 10838 2014-10-10 14:13:54Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.ComponentModel;
using CAS.SharePoint.Tools.SPExplorer.SPMetalParameters;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CAS.SharePoint.Tools.SPExplorer.ModelView
{
  internal class MainWindowModelView : PropertyChangedBase, IDisposable
  {

    #region creator
    public MainWindowModelView()
    {
      //initialization
      this.Messages = new ObservableCollection<string>();
      SPDataModel = null;
      //commands
      this.ImportDataModel = new ImportDataModelCommandImplementation(this);
      this.SaveDataModel = new SaveSPDataModelCommand(this);
      this.SaveSQLScript = new SaveSQLScriptCommand(this);
      this.SaveDropTableSQLScript = new SaveDropTableSQLScriptCommand(this);
      //event handlers
      TreeViewExpanded = new RoutedEventHandler(TreeViewExpandedHandler);
      TreeViewSelectedItemChanged = new RoutedPropertyChangedEventHandler<Object>(TreeViewSelectedItemChangedHandler);
      //Get default values from the Properties.Settings
      b_URL = Properties.Settings.Default.URL;
      b_InludeHidden = Properties.Settings.Default.InludeHidden;
      b_GroupName = Properties.Settings.Default.GroupName;
      b_DatabaseName = Properties.Settings.Default.DatabaseName;
      b_FileName = Properties.Settings.Default.FileName;
    }
    #endregion

    #region view interface
    private string b_DatabaseName;
    public string DatabaseName
    {
      get
      {
        return b_DatabaseName;
      }
      set
      {
        RaiseHandler<string>(value, ref b_DatabaseName, "DatabaseName", this);
        Properties.Settings.Default.DatabaseName = b_DatabaseName;
      }
    }
    private bool b_InludeHidden;
    public bool InludeHidden
    {
      get
      {
        return b_InludeHidden;
      }
      set
      {
        RaiseHandler<bool>(value, ref b_InludeHidden, "InludeHidden", this);
        Properties.Settings.Default.InludeHidden = b_InludeHidden;
      }
    }
    private string b_GroupName;
    public string GroupName
    {
      get
      {
        return b_GroupName;
      }
      set
      {
        RaiseHandler<string>(value, ref b_GroupName, "GroupName", this);
        Properties.Settings.Default.GroupName = b_GroupName;
      }
    }
    private PRWeb b_SPDataModel;
    public PRWeb SPDataModel
    {
      get
      {
        return b_SPDataModel;
      }
      set
      {
        RaiseHandler<PRWeb>(value, ref b_SPDataModel, "SPDataModel", this);
      }
    }
    private ObservableCollection<TreeViewNode> b_Nodes;
    public ObservableCollection<TreeViewNode> Nodes
    {
      get
      {
        return b_Nodes;
      }
      set
      {
        RaiseHandler<ObservableCollection<TreeViewNode>>(value, ref b_Nodes, "Nodes", this);
      }
    }
    private ObservableCollection<String> b_Messages;
    public ObservableCollection<String> Messages
    {
      get
      {
        return b_Messages;
      }
      set
      {
        RaiseHandler<System.Collections.ObjectModel.ObservableCollection<String>>(value, ref b_Messages, "Messages", this);
      }
    }
    private string b_LinqEntitiesClassName;
    public string LinqEntitiesClassName
    {
      get
      {
        return b_LinqEntitiesClassName;
      }
      set
      {
        RaiseHandler<string>(value, ref b_LinqEntitiesClassName, "LinqEntitiesClassName", this);
      }
    }
    private string b_URL;
    public string URL
    {
      get
      {
        return b_URL;
      }
      set
      {
        RaiseHandler<string>(value, ref b_URL, "URL", this);
      }
    }
    private string b_FileName;
    public string FileName
    {
      get
      {
        return b_FileName;
      }
      set
      {
        RaiseHandler<string>(value, ref b_FileName, "FileName", this);
      }
    }
    private int b_Progress;
    public int Progress
    {
      get
      {
        return b_Progress;
      }
      set
      {
        RaiseHandler<int>(value, ref b_Progress, "Progress", this);
      }
    }
    #endregion

    #region Commands
    public ImportDataModelCommand ImportDataModel
    {
      get
      {
        return b_ImportDataModel;
      }
      set
      {
        RaiseHandler<ImportDataModelCommand>(value, ref b_ImportDataModel, "ImportDataModel", this);
      }
    }
    public ImportDataModelCommand.Argument ImportDataModelCommandArgument
    {
      get
      {
        return new ImportDataModelCommand.Argument(this.URL, this.LinqEntitiesClassName, InludeHidden, GroupName, Properties.Settings.Default.SelectedColumns);
      }
    }
    public SaveSPDataModelCommandBase SaveDataModel
    {
      get
      {
        return b_SaveDataModel;
      }
      set
      {
        RaiseHandler<SaveSPDataModelCommandBase>(value, ref b_SaveDataModel, "SaveDataModel", this);
      }
    }
    public ICommand SaveSQLScript { get; private set; }
    public ICommand SaveDropTableSQLScript { get; private set; }
    /// <summary>
    /// Gets the tree expanded.
    /// </summary>
    /// <value>
    /// The tree tree expanded.
    /// </value>
    public RoutedEventHandler TreeViewExpanded { get; private set; }
    public RoutedPropertyChangedEventHandler<Object> TreeViewSelectedItemChanged { get; private set; }
    public string TreeNodeDetails
    {
      get
      {
        return b_TreeNodeDetails;
      }
      set
      {
        RaiseHandler<string>(value, ref b_TreeNodeDetails, "TreeNodeDetails", this);
      }
    }
    #endregion

    #region event handlers
    internal void TreeViewExpandedHandler(object sender, System.Windows.RoutedEventArgs e)
    {
      ;
    }
    internal void TreeViewSelectedItemChangedHandler(object sender, RoutedPropertyChangedEventArgs<Object> e)
    {
      TreeViewNode _content = e.NewValue as TreeViewNode;
      this.TreeNodeDetails = _content == null ? "-- select item --" : _content.ToString();
    }
    #endregion

    #region IDisposable
    public void Dispose()
    {
      Dispose(true);
    }
    #endregion

    #region private
    //vars 
    private ImportDataModelCommand b_ImportDataModel;
    private SaveSPDataModelCommandBase b_SaveDataModel;
    private string b_TreeNodeDetails;
    //commands implementation
    private class SaveSPDataModelCommand : SaveSPDataModelCommandBase
    {
      public SaveSPDataModelCommand(MainWindowModelView parent)
      {
        m_Parent = parent;
      }
      internal override string FileName
      {
        get
        {
          // Configure save file dialog box
          Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
          dlg.FileName = m_Parent.FileName; // Default file name
          dlg.DefaultExt = ".xml"; // Default file extension
          dlg.Filter = "Text documents (.xml)|*.xml"; // Filter files by extension 
          // Show save file dialog box
          Nullable<bool> result = dlg.ShowDialog();
          // Process save file dialog box results 
          if (result == true)
          {
            m_Parent.FileName = dlg.FileName;
            return dlg.FileName;
          }
          return String.Empty;
        }
      }
      internal override PRWeb GetObjectModel
      {
        get { return m_Parent.SPDataModel; }
      }
      MainWindowModelView m_Parent;
    }
    private class ImportDataModelCommandImplementation : ImportDataModelCommand
    {
      #region public
      internal ImportDataModelCommandImplementation(MainWindowModelView parent)
      {
        m_Parent = parent;
      }
      internal override PRWeb Result
      {
        set
        {
          m_Parent.SPDataModel = value;
          if (value == null)
            m_Parent.Nodes = null;
          else
          {
            m_Parent.Nodes = new ObservableCollection<TreeViewNode>();
            m_Parent.Nodes.Add(WebsiteTreeViewNode.CreateChildren(value, m_Parent.URL));
          }
        }
      }
      internal override void Progress(ProgressChangedEventArgs progressInfo)
      {
        m_Parent.AddProgress(progressInfo);
      }
      internal override ImportDataModelCommand.Argument ImportArgument
      {
        get { return m_Parent.ImportDataModelCommandArgument; }
      }
      #endregion

      private MainWindowModelView m_Parent { get; set; }
    }
    private class SaveSQLScriptCommand : SaveSQLScriptCommandBase
    {

      #region creator
      public SaveSQLScriptCommand(MainWindowModelView parent)
      {
        m_Parent = parent;
      }
      #endregion

      #region SaveSQLScriptCommandBase
      internal override string FileName
      {
        get
        {
          // Configure save file dialog box
          Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
          dlg.FileName = Properties.Settings.Default.SQLScriptFileName; // Default file name
          dlg.DefaultExt = ".sql"; // Default file extension
          dlg.Filter = "Script documents (.sql)|*.sql|All Files|*.*"; // Filter files by extension 
          // Show save file dialog box
          Nullable<bool> result = dlg.ShowDialog();
          // Process save file dialog box results 
          if (result == true)
            return dlg.FileName;
          return String.Empty;
        }
      }
      internal override PRWeb GetObjectModel
      {
        get { return m_Parent.SPDataModel; }
      }
      internal override void Progress(ProgressChangedEventArgs progressInfo)
      {
        m_Parent.AddProgress(progressInfo);
      }
      #endregion
      private MainWindowModelView m_Parent = null;

    }
    private class SaveDropTableSQLScriptCommand : SaveDropTableSQLScriptCommandBase
    {
      public SaveDropTableSQLScriptCommand(MainWindowModelView mainWindowModelView)
      {
        m_Parent = mainWindowModelView;
      }
      //SaveDropTableSQLScriptCommandBase implementation
      internal override string FileName
      {
        get
        {
          // Configure save file dialog box
          Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
          dlg.FileName = Properties.Settings.Default.SQlLDropScriptFileName; // Default file name
          dlg.DefaultExt = ".sql"; // Default file extension
          dlg.Filter = "Script documents (.sql)|*.sql|All Files|*.*"; // Filter files by extension 
          // Show save file dialog box
          Nullable<bool> result = dlg.ShowDialog();
          // Process save file dialog box results 
          if (result == true)
          {
            Properties.Settings.Default.SQlLDropScriptFileName = dlg.FileName;
            return dlg.FileName;
          }
          return String.Empty;
        }
      }
      internal override PRWeb GetObjectModel
      {
        get { return m_Parent.SPDataModel; }
      }
      internal override void Progress(ProgressChangedEventArgs progressInfo)
      {
        m_Parent.AddProgress(progressInfo);
      }
      //vars
      private MainWindowModelView m_Parent;
    }
    //procedures
    protected virtual void Dispose(bool disposing)
    {
      Properties.Settings.Default.URL = URL;
      Properties.Settings.Default.LinqEntitiesClassName = LinqEntitiesClassName;
      Properties.Settings.Default.FileName = FileName;
      if (SPDataModel != null)
        Properties.Settings.Default.SelectedColumns = SPDataModel.GetConfiguration();
      Properties.Settings.Default.Save();
      if (!disposing)
        return;
      if (b_ImportDataModel != null)
        b_ImportDataModel.Dispose();
    }
    private void AddProgress(ProgressChangedEventArgs progressInfo)
    {
      Progress = progressInfo.ProgressPercentage;
      if (progressInfo.UserState is string)
        Messages.Add(String.Format("Progress {0,5:D} at: {1}", progressInfo.ProgressPercentage, (string)progressInfo.UserState));
      else
        Messages.Add(progressInfo.UserState.ToString());
    }
    #endregion

  }
}
