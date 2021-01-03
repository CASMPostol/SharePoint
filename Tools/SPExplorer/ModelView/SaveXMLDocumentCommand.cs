//<summary>
//  Title   : SaveSPDataModelCommandBase
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-08-11 14:37:23 +0200 (pon., 11 sie 2014) $
//  $Rev: 10691 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/SPExplorer/ModelView/SaveXMLDocumentCommand.cs $
//  $Id: SaveXMLDocumentCommand.cs 10691 2014-08-11 12:37:23Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Windows.Input;

namespace CAS.SharePoint.Tools.SPExplorer.ModelView
{
  /// <summary>
  /// Save Share Point Data Model Command - synchronous implementation of the <see cref="ICommand"/>
  /// /// </summary>
  internal abstract class SaveSPDataModelCommandBase : ICommand
  {

    internal abstract string FileName { get; }
    internal abstract SPMetalParameters.PRWeb GetObjectModel { get; }
    //ICommand
    public bool CanExecute(object parameter)
    {
      return true;
    }
    public event EventHandler CanExecuteChanged;
    public void Execute(object parameter)
    {
      if (GetObjectModel == null)
        return;
      Properties.Settings.Default.SelectedColumns = GetObjectModel.GetConfiguration();
      Properties.Settings.Default.Save();
      string _fileName = FileName;
      if (_fileName.IsNullOrEmpty())
        return;
      DocumentsFactory.XmlFile.WriteXmlFile<SPMetalParameters.PRWeb>(GetObjectModel.GetSPMetalParameters(), _fileName, System.IO.FileMode.Create, "SPParametersStylesheet");
      EventHandler _canExecuteChanged = CanExecuteChanged;
      if (_canExecuteChanged == null)
        return;
      _canExecuteChanged(this, EventArgs.Empty);
    }

  }
}
