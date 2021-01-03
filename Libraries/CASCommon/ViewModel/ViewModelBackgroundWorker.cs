//<summary>
//  Title   : class BaseModelView
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-17 16:26:55 +0200 (pt., 17 paź 2014) $
//  $Rev: 10870 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/ViewModel/ViewModelBackgroundWorker.cs $
//  $Id: ViewModelBackgroundWorker.cs 10870 2014-10-17 14:26:55Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.ComponentModel;
using System;
using System.ComponentModel;

namespace CAS.Common.ViewModel
{
  /// <summary>
  /// Base class to create ModelView in the MVVM pattern containing <see cref="BackgroundWorker"/> to handle time consuming operation asynchronously. 
  /// </summary>
  public abstract class ViewModelBackgroundWorker : PropertyChangedBase, IDisposable
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModelBackgroundWorker"/> class. Creates ans initializes 
    /// <see cref="BackgroundWorker"/> to handle time consuming operation asynchronously.
    /// </summary>
    public ViewModelBackgroundWorker()
    {
      m_BackgroundWorker.DoWork += BackgroundWorker_DoWork;
      m_BackgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
      m_BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
    }
    #endregion

    #region UI properties
    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="BackgroundWorker"/> is busy.
    /// </summary>
    /// <value>
    ///   <c>true</c> if <see cref="BackgroundWorker"/> is not busy; otherwise, <c>false</c>.
    /// </value>
    public bool NotBusy
    {
      get
      {
        return b_NotBusy;
      }
      set
      {
        RaiseHandler<bool>(value, ref b_NotBusy, "NotBusy", this);
        OnNotBusyChanged();
      }
    }
    #endregion

    #region protected
    /// <summary>
    /// Called when the NotBusy has been changed].
    /// </summary>
    protected abstract void OnNotBusyChanged();
    /// <summary>
    /// Represents the method that will handle the work of the <see cref="System.ComponentModel.BackgroundWorker" />
    /// event of a System.ComponentModel.BackgroundWorker class.
    /// </summary>
    /// <param name="argument">The argument.</param>
    /// <param name="progress">The progress.</param>
    /// <param name="cancellationPending">Gets a value indicating whether the application has requested cancellation of a background operation.</param>
    /// <returns>Returns a value that represents the result of an asynchronous operation.</returns>
    protected delegate Object DoWorkEventHandler(object argument, Action<ProgressChangedEventArgs> progress, Func<bool> cancellationPending);
    /// <summary>
    /// Gets the do work event handler.
    /// </summary>
    /// <value>
    /// The do work event handler.
    /// </value>
    protected abstract DoWorkEventHandler GetDoWorkEventHandler { get; }
    /// <summary>
    /// Gets the completed event handler.
    /// </summary>
    /// <value>
    /// The completed event handler.
    /// </value>
    protected abstract RunWorkerCompletedEventHandler CompletedEventHandler { get; }
    /// <summary>
    /// Gets the progress changed event handler.
    /// </summary>
    /// <value>
    /// The progress changed event handler.
    /// </value>
    protected abstract ProgressChangedEventHandler ProgressChangedEventHandler { get; }
    /// <summary>
    /// Starts the background worker.
    /// </summary>
    protected virtual void StartBackgroundWorker()
    {
      NotBusy = false;
      m_BackgroundWorker.RunWorkerAsync();
    }
    /// <summary>
    /// Starts the background worker.
    /// </summary>
    /// <param name="argument">The argument to send to the background worker.</param>
    protected virtual void StartBackgroundWorker(object argument)
    {
      NotBusy = false;
      m_BackgroundWorker.RunWorkerAsync(argument);
    }
    /// <summary>
    /// Requests cancellation of a pending background operation.
    /// </summary>
    protected void CancelAsync()
    {
      m_BackgroundWorker.CancelAsync();
    }
    /// <summary>
    /// Checks if an operation is performed on a disposed object.
    /// </summary>
    /// <typeparam name="type">The derived type.</typeparam>
    /// <exception cref="System.ObjectDisposedException"></exception>
    protected void CheckDisposed<type>()
      where type : ViewModelBackgroundWorker
    {
      if (m_Disposed)
        throw new ObjectDisposedException(typeof(type).Name);
    }
    #endregion

    #region IDisposable
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      // This object will be cleaned up by the Dispose method. 
      // Therefore, you should call GC.SupressFinalize to 
      // take this object off the finalization queue 
      // and prevent finalization code for this object 
      // from executing a second time.
      GC.SuppressFinalize(this);
    }
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources. It executes in two distinct scenarios. 
    /// If disposing equals true, the method has been called directly or indirectly by a user's code. Managed and unmanaged resources
    /// can be disposed. 
    /// If disposing equals false, the method has been called by the runtime from inside the finalizer and you should not reference other objects. 
    /// Only unmanaged resources can be disposed. 
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      // Check to see if Dispose has already been called. 
      if (!this.m_Disposed)
      {
        // If disposing equals true, dispose all managed and unmanaged resources. 
        if (disposing)
        {
#if !SILVERLIGHT
          // Dispose managed resources.
          if (m_BackgroundWorker == null)
            m_BackgroundWorker.Dispose();
          m_BackgroundWorker = null;
          m_Disposed = true;
#endif
        }
        // Call the appropriate methods to clean up unmanaged resources here. 
        // If disposing is false, only the following code is executed.
        // Note disposing has been done.
        m_Disposed = true;
      }
    }
    /// <summary>
    /// Finalizes an instance of the <see cref="ViewModelBackgroundWorker"/> class.
    /// </summary>
    ~ViewModelBackgroundWorker()
    {
      // Do not re-create Dispose clean-up code here. 
      // Calling Dispose(false) is optimal in terms of 
      // readability and maintainability.
      Dispose(false);
    }
    #endregion

    #region private
    private bool b_NotBusy = true;
    private bool m_Disposed = false;
    private System.ComponentModel.BackgroundWorker m_BackgroundWorker = new BackgroundWorker()
      {
        WorkerReportsProgress = true,
        WorkerSupportsCancellation = true
      };
    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (CompletedEventHandler != null)
        CompletedEventHandler(sender, e);
      NotBusy = true;
    }
    private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (ProgressChangedEventHandler != null)
        ProgressChangedEventHandler(sender, e);
    }
    private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      e.Cancel = false;
      if (GetDoWorkEventHandler == null)
        return;
      BackgroundWorker _mbw = (BackgroundWorker)sender;
      e.Result = GetDoWorkEventHandler(e.Argument, x =>
        {
          _mbw.ReportProgress(x.ProgressPercentage, x.UserState);
          if (_mbw.CancellationPending) throw new OperationCanceledException("The background worker job has been canceled by the user.");
        },
        () => { return _mbw.CancellationPending; });
    }
    #endregion

  }
}
