//<summary>
//  Title   : class BackgroundWorkerVizardMachine
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-12-23 14:57:39 +0100 (wt., 23 gru 2014) $
//  $Rev: 11139 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/ViewModel/Wizard/BackgroundWorkerVizardMachine.cs $
//  $Id: BackgroundWorkerVizardMachine.cs 11139 2014-12-23 13:57:39Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.ComponentModel;

namespace CAS.Common.ViewModel.Wizard
{
  /// <summary>
  /// The machine state that uses <see cref="BackgroundWorker" /> to process the operation.
  /// </summary>
  /// <typeparam name="ContextType">The type of the context - a class derived from <see cref="StateMachineContext" /> a parent for objects of this class.</typeparam>
  /// <typeparam name="ViewModelContextType">The type of the view model context.</typeparam>
  public abstract class BackgroundWorkerMachine<ContextType, ViewModelContextType> : AbstractMachineState<ContextType, ViewModelContextType>
    where ContextType : StateMachineContext
    where ViewModelContextType : IViewModelContext
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundWorkerMachine{ContextType, ViewModelContextType}"/> class.
    /// </summary>
    public BackgroundWorkerMachine()
    {
      m_BackgroundWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
      m_BackgroundWorker.DoWork += DoWork;
      m_BackgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
      m_BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
      Success = false;
    }
    #endregion

    #region IDisposable
    // Track whether Dispose has been called.
    private bool disposed = false;
    /// <summary>
    /// Implement IDisposable. Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// Do not make this method virtual. A derived class should not be able to override this method.
    /// </summary>
    public override void Dispose()
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
    /// Releases unmanaged and - optionally - managed resources. Dispose(bool disposing) executes in two distinct scenarios. 
    /// If disposing equals true, the method has been called directly or indirectly by a user's code. Managed and unmanaged resources can be disposed.
    /// If disposing equals false, the method has been called by the runtime from inside the finalizer and you should not reference other objects. 
    /// Only unmanaged resources can be disposed.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      // Check to see if Dispose has already been called.
      if (this.disposed)
        return;
      // If disposing equals true, dispose all managed and unmanaged resources.
      if (disposing)
      {
#if !SILVERLIGHT
        m_BackgroundWorker.Dispose();
#endif
      }
      // Call the appropriate methods to clean up  unmanaged resources here.
      // If disposing is false, only the following code is executed.

      // The code to be executed 
      
      // Note disposing has been done.
      disposed = true;
    }
    /// <summary>
    /// Finalizes an instance of the <see cref="BackgroundWorkerMachine{ContextType, ViewModelContextType}"/> class. 
    /// Use C# destructor syntax for finalization code. This destructor will run only if the Dispose method does not get called. 
    /// It gives your base class the opportunity to finalize. Do not provide destructors in types derived from this class.
    /// </summary>
    ~BackgroundWorkerMachine()
    {
      // Do not re-create Dispose clean-up code here. Calling Dispose(false) is optimal in terms of readability and maintainability.
      Dispose(false);
    }
    #endregion

    #region AbstractMachine implementation
    /// <summary>
    /// Runs the operation of the current state asynchronously.
    /// </summary>
    /// <param name="argument"> A parameter for use by the background operation to be executed in the <see cref="System.ComponentModel.BackgroundWorker.DoWork"/> event handler.</param>
    /// <exception cref="System.ApplicationException">Background worker is busy.</exception>
    /// <exception cref="ApplicationException">Background worker is busy.</exception>
    public virtual void RunAsync(object argument)
    {
      if (m_BackgroundWorker.IsBusy)
        throw new ApplicationException("Background worker is busy");
      OnlyCancelActive();
      m_BackgroundWorker.RunWorkerAsync(argument);
    }
    /// <summary>
    /// Cancels this operation.
    /// </summary>
    public override void Cancel()
    {
      if (!m_BackgroundWorker.IsBusy)
        base.Cancel();
      else if (m_BackgroundWorker.WorkerSupportsCancellation && !m_BackgroundWorker.CancellationPending)
        m_BackgroundWorker.CancelAsync();
    }
    #endregion

    #region private
    /// <summary>
    /// Handles the DoWork event of the BackgroundWorker control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
    protected abstract void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e);
    /// <summary>
    /// Handles the RunWorkerCompleted event of the BackgroundWorker control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
    private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Cancelled)
        OnCancellation();
      else if (e.Error != null)
        OnException(e.Error);
      else
      {
        Success = true;
        this.RunWorkerCompleted(e.Result);
      }
    }
    /// <summary>
    /// Handles the ProgressChanged event of the BackgroundWorker control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="ProgressChangedEventArgs"/> instance containing the event data.</param>
    protected virtual void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      Context.ProgressChang(this, e);
    }
    /// <summary>
    /// Called when worker task has been completed.
    /// </summary>
    /// <param name="result">An object that represents the result of an asynchronous operation.</param>
    protected abstract void RunWorkerCompleted(object result);
    /// <summary>
    /// Reports the progress of the current task.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="progress">The <see cref="ProgressChangedEventArgs" /> instance containing the event data.</param>
    /// <exception cref="System.OperationCanceledException"></exception>
    protected void ReportProgress(object source, ProgressChangedEventArgs progress)
    {
      m_BackgroundWorker.ReportProgress(progress.ProgressPercentage, progress.UserState);
      if (!m_BackgroundWorker.CancellationPending)
        return;
      m_BackgroundWorker.ReportProgress(0, String.Format("Operation {0} canceled by the user.", this.ToString()));
      throw new OperationCanceledException();
    }
    /// <summary>
    /// Called when only cancel button must be active - after starting background worker.
    /// </summary>
    protected abstract void OnlyCancelActive();
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="BackgroundWorkerMachine{ContextType, ViewModelContextType}"/> is finished with success.
    /// </summary>
    /// <value>
    ///   <c>true</c> if success; otherwise, <c>false</c>.
    /// </value>
    protected bool Success { get; private set; }
    private BackgroundWorker m_BackgroundWorker;
    private void DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        BackgroundWorker_DoWork(sender, e);
      }
      catch (OperationCanceledException)
      {
        e.Cancel = true;
        e.Result = null;
      }
    }
    #endregion

  }
}
