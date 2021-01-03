//<summary>
//  Title   : CommandBase
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 12:12:35 +0200 (pt., 10 paź 2014) $
//  $Rev: 10832 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/ViewModel/AsynchronousCommandBase.cs $
//  $Id: AsynchronousCommandBase.cs 10832 2014-10-10 10:12:35Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;
using System.Windows.Input;

namespace CAS.Common.ViewModel
{
  /// <summary>
  /// Provides implementation of the <see cref="ICommand"/> using <see cref="System.ComponentModel.BackgroundWorker"/>
  /// </summary>
  public abstract class AsynchronousCommandBase : ViewModelBackgroundWorker, ICommand
  {

    #region ICommand
    bool ICommand.CanExecute(object parameter)
    {
      return this.NotBusy;
    }
    void ICommand.Execute(object parameter)
    {
      this.StartBackgroundWorker(parameter);
    }
    event EventHandler ICommand.CanExecuteChanged
    {
      add
      {
        lock (m_ObjectLock)
        {
          m_CanExecuteChanged += value;
        }
      }
      remove
      {
        lock (m_ObjectLock)
        {
          m_CanExecuteChanged -= value;
        }
      }
    }
    #endregion

    #region private
    /// <summary>
    /// Called when not busy state changed.
    /// </summary>
    protected override void OnNotBusyChanged()
    {
      EventHandler _CanExecuteChanged = m_CanExecuteChanged;
      if (_CanExecuteChanged == null)
        return;
      _CanExecuteChanged(this, EventArgs.Empty);
    }
    private object m_ObjectLock = new Object();
    private event EventHandler m_CanExecuteChanged;
    #endregion
  }
}
