//<summary>
//  Title   : ImportDataModelCommand
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/SPExplorer/ModelView/ImportDataModelCommand.cs $
//  $Id: ImportDataModelCommand.cs 10838 2014-10-10 14:13:54Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.ViewModel;
using CAS.SharePoint.Tools.SPExplorer.SPMetalParameters;
using System;
using System.ComponentModel;

namespace CAS.SharePoint.Tools.SPExplorer.ModelView
{
  internal abstract class ImportDataModelCommand : AsynchronousCommandBase
  {

    #region public API
    internal abstract PRWeb Result { set; }
    internal abstract Argument ImportArgument { get; }
    internal abstract void Progress(ProgressChangedEventArgs progressInfo);
    internal class Argument
    {
      public Argument(string url, string linqEntitiesClassName, bool inludeHidden, string groupName, string configuration)
      {
        URL = url;
        LinqEntitiesClassName = linqEntitiesClassName;
        InludeHidden = inludeHidden;
        GroupName = groupName;
        Configuration = configuration;
      }
      internal string URL { get; private set; }
      internal string LinqEntitiesClassName { get; private set; }
      internal bool InludeHidden { get; private set; }
      internal string GroupName { get; private set; }
      internal string Configuration { get; private set; }
    }
    #endregion

    #region private
    protected override CAS.Common.ViewModel.ViewModelBackgroundWorker.DoWorkEventHandler GetDoWorkEventHandler
    {
      get { return new DoWorkEventHandler(DoWorkEventHandlerHandler); }
    }
    protected override RunWorkerCompletedEventHandler CompletedEventHandler
    {
      get { return CallResulyCallBack; }
    }
    private void CallResulyCallBack(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Cancelled)
      {
        Result = null;
        ProgressChangedEventHandler(this, new ProgressChangedEventArgs(100, "Operation Canceled"));
        return;
      }
      if (e.Error != null)
      {
        Result = null;
        ProgressChangedEventHandler(this, new ProgressChangedEventArgs(100, e.Error));
        return;
      }
      Result = e.Result as PRWeb;
      ProgressChangedEventHandler(this, new ProgressChangedEventArgs(100, "Import Data Mode Command finished"));
    }
    protected override System.ComponentModel.ProgressChangedEventHandler ProgressChangedEventHandler
    {
      get { return (source, progressInfo) => Progress(progressInfo); }
    }
    private Object DoWorkEventHandlerHandler(object argument, Action<ProgressChangedEventArgs> progress, Func<bool> cancellationPending)
    {
      progress(new ProgressChangedEventArgs(0, "DoWorkEventHandlerHandler starting"));
      Argument _Arg = ImportArgument;
      PRWeb _web = PRWeb.ImportDataModel(_Arg.URL, _Arg.LinqEntitiesClassName, _Arg.InludeHidden, _Arg.GroupName, _Arg.Configuration, progress);
      progress(new ProgressChangedEventArgs(100, "DoWorkEventHandlerHandler finishing"));
      return _web;
    }
    #endregion

  }
}
