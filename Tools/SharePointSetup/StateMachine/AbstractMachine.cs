using System;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using CAS.SharePoint.Setup.Controls;
using CAS.SharePoint.Setup.Package;

namespace CAS.SharePoint.Setup.StateMachine
{

  internal abstract class StateMachineContext: IAbstractMachineEvents
  {
    #region creator
    internal StateMachineContext()
    {
      m_FinishedMachine = new AbstractMachine.FinishedMachine( this );
      m_InstallationMachine = new AbstractMachine.InstallationMachine( this );
      m_SetupDataDialogMachine = new AbstractMachine.SetupDataDialogMachine( this );
      m_UninstallMachine = new AbstractMachine.UninstallMachine( this );
      m_ValidationMachine = new AbstractMachine.ValidationMachine( this );
    }
    #endregion

    #region types
    [Flags]
    internal protected enum Events
    {
      Previous = 0x1,
      Next = 0x2,
      Cancel = 0x4,
      Uninstall = 0x8
    }
    internal protected enum ProcessState
    {
      SetupDataDialog,
      Validation,
      Installation,
      Finisched,
      Uninstall
    }
    /// <summary>
    /// Represents the method that will handle the event of the 
    /// <see cref="IInstallationMachineViewModel"/> interface.
    /// </summary>
    /// <param name="sender">The source of the event..</param>
    /// <param name="e">The <see cref="SPWebTeplateEventArgs" /> instance containing the event data.</param>
    internal delegate void SPWebTeplateEventHandler( object sender, SPWebTeplateEventArgs e );
    internal interface IAbstractMachineViewModel
    {
      event ProgressChangedEventHandler ProgressChanged;
    }
    internal interface IFinishedMachineViewModel: IAbstractMachineViewModel
    {
    }
    internal interface IInstallationMachineViewModel: IAbstractMachineViewModel
    {
      event SPWebTeplateEventHandler OnSPWebTeplateSelection;
    }
    internal interface ISetupDataDialogMachineViewModel: IAbstractMachineViewModel { }
    internal interface IUninstallMachineViewModel: IAbstractMachineViewModel { }
    internal interface IValidationMachineViewModel: IAbstractMachineViewModel { }
    #endregion

    #region IAbstractMachineEvents Members
    public void Cancel()
    {
      Machine.Cancel();
    }
    public void Exception( Exception exception )
    {
      Machine.Exception( exception );
    }
    public void NewPackageProperties( InstalationPackageProperties packageProperties )
    {
      Machine.NewPackageProperties( packageProperties );
    }
    public void Next()
    {
      Machine.Next();
    }
    public void Previous()
    {
      Machine.Previous();
    }
    public void Uninstall()
    {
      Machine.Uninstall();
    }
    #endregion

    #region public
    internal void OpenEntryState()
    {
      Machine = m_SetupDataDialogMachine;
    }
    internal InstallationStateData InstallationDescription { get; set; }
    internal InstalationPackageProperties PackageProperties { get; set; }
    internal bool IsPackagePropertiesEditable
    {
      get { return PackageProperties != null; }
    }
    internal IFinishedMachineViewModel FinishedMachineViewModel { get { return m_FinishedMachine; } }
    internal IInstallationMachineViewModel InstallationMachineViewModel { get { return m_InstallationMachine; } }
    internal ISetupDataDialogMachineViewModel SetupDataDialogMachineViewModel { get { return m_SetupDataDialogMachine; } }
    internal IUninstallMachineViewModel UninstallMachineViewModel { get { return m_UninstallMachine; } }
    internal IValidationMachineViewModel ValidationMachineViewModel { get { return m_ValidationMachine; } }
    #endregion

    #region private

    protected abstract class AbstractMachine: IAbstractMachineEvents
    {

      #region constructor
      public AbstractMachine( StateMachineContext context )
      {
        m_Context = context;
      }
      #endregion

      internal abstract void EnteringState();

      #region IAbstractMachineEvents Members
      public virtual void NewPackageProperties( InstalationPackageProperties packageProperties )
      {
        throw new NotImplementedException();
      }
      public virtual void Previous()
      {
        throw new NotImplementedException();
      }
      public virtual void Next()
      {
        throw new NotImplementedException();
      }
      public virtual void Cancel()
      {
        throw new NotImplementedException();
      }
      public virtual void Exception( Exception exception )
      {
        string _mssg = String.Format( Properties.Resources.InstalationAbortRollback, exception.Message );
        MessageBox.Show( _mssg, Properties.Resources.CaptionOperationFailure, MessageBoxButton.OK, MessageBoxImage.Error );
        m_Context.ExitlInstallation();
      }
      public virtual void Uninstall()
      {
        throw new NotImplementedException();
      }
      #endregion

      #region private

      #region state classes
      internal class SetupDataDialogMachine: AbstractMachine, ISetupDataDialogMachineViewModel
      {
        public SetupDataDialogMachine( StateMachineContext context )
          : base( context )
        { }
        protected internal override Events GetEventMask()
        {
          Events _ExpectedEvents = Events.Cancel;
          if ( m_Context.InstallationDescription != null )
            _ExpectedEvents |= Events.Next;
          return _ExpectedEvents;
        }
        protected internal override ProcessState State
        {
          get { return ProcessState.SetupDataDialog; }
        }
        internal override void EnteringState() { }
        public override void NewPackageProperties( InstalationPackageProperties packageProperties )
        {
          m_Context.PackageProperties = packageProperties;
          m_Context.InstallationDescription = CAS.SharePoint.Tools.Packaging.InstallationPackage.PackageDescriptionOpenRead<InstallationStateData>( packageProperties.Location, InstallationStateData.Read );
          m_Context.Update();
        }
        public override void Next()
        {
          if ( m_Context.InstallationDescription == null )
            throw new ArgumentException( "Instalation description is null" );
          //m_ApplicationState.Save();
          m_Context.Machine = m_Context.m_ValidationMachine;
        }
        public override void Cancel()
        {
          m_Context.CancelInstallation();
        }

        #region ISetupDataDialogMachineViewModel Members
        public event ProgressChangedEventHandler ProgressChanged;
        #endregion
      }
      internal class ValidationMachine: AbstractMachine, IValidationMachineViewModel
      {
        #region creator
        internal ValidationMachine( StateMachineContext context )
          : base( context )
        {
          m_BackgroundWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
          m_BackgroundWorker.DoWork += m_BackgroundWorker_DoWork;
          m_BackgroundWorker.ProgressChanged += m_BackgroundWorker_ProgressChanged;
          m_BackgroundWorker.RunWorkerCompleted += m_BackgroundWorker_RunWorkerCompleted;
        }
        #endregion

        #region IValidationMachineViewModel Members
        public event ProgressChangedEventHandler ProgressChanged;
        #endregion

        internal protected override Events GetEventMask()
        {
          Events m_ExpectedEvents = Events.Previous | Events.Cancel;
          if ( m_Context.InstallationDescription.SiteCollectionCreated )
            m_ExpectedEvents |= Events.Uninstall;
          if ( m_Validated )
            m_ExpectedEvents |= Events.Next;
          return m_ExpectedEvents;
        }
        protected internal override ProcessState State
        {
          get { return ProcessState.Validation; }
        }
        internal override void EnteringState()
        {
          m_Validated = false;
          m_BackgroundWorker.RunWorkerAsync();
        }
        public override void Previous()
        {
          m_Context.Machine = m_Context.m_SetupDataDialogMachine;
        }
        public override void Next()
        {
          if ( m_Context.InstallationDescription == null )
            throw new ArgumentException( "Instalation description is null" );
          //m_ApplicationState.Save();
          m_Context.Machine = m_Context.m_InstallationMachine;
        }
        public override void Uninstall()
        {
          m_Context.Machine = m_Context.m_UninstallMachine;
        }
        public override void Cancel()
        {
          m_Context.CancelInstallation();
        }

        #region private
        private void m_BackgroundWorker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
        {
          if ( e.Error != null )
            this.Exception( e.Error );
          else if ( e.Cancelled )
            return;
          else
          {
            //TODO m_NextButton.Text = Resources.NextButtonTextNext;
            m_Validated = (bool)e.Result;
            if ( !m_Validated )
              MessageBox.Show( Properties.Resources.CheckIinProcessFfailed, Properties.Resources.CheckIinProcessFfailedCaption, MessageBoxButton.OK, MessageBoxImage.Error );
            m_Context.Update();
          }
        }
        private void m_BackgroundWorker_ProgressChanged( object sender, ProgressChangedEventArgs e )
        {
          if ( ProgressChanged != null )
            ProgressChanged( this, e );
        }
        private void m_BackgroundWorker_DoWork( object sender, DoWorkEventArgs e )
        {
          BackgroundWorker _wrkr = (BackgroundWorker)sender;
          Commands.DoValidation( m_Context.InstallationDescription, _wrkr, e );
        }
        private bool m_Validated = false;
        private BackgroundWorker m_BackgroundWorker = null;
        #endregion

      }
      internal class InstallationMachine: AbstractMachine, IInstallationMachineViewModel
      {
        #region creator
        internal InstallationMachine( StateMachineContext context )
          : base( context )
        {
          m_BackgroundWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
          m_BackgroundWorker.DoWork += m_BackgroundWorker_DoWork;
          m_BackgroundWorker.ProgressChanged += m_BackgroundWorker_ProgressChanged;
          m_BackgroundWorker.RunWorkerCompleted += m_BackgroundWorker_RunWorkerCompleted;
          //  m_NextButton.Text = Resources.FinishButtonText;
        }
        #endregion

        #region IInstallationMachineViewModel Members
        public event ProgressChangedEventHandler ProgressChanged;
        public event SPWebTeplateEventHandler OnSPWebTeplateSelection;
        #endregion

        protected internal override Events GetEventMask()
        {
          return m_ExpectedEvents;
        }
        protected internal override ProcessState State
        {
          get { return ProcessState.Installation; }
        }
        internal override void EnteringState()
        {
          m_BackgroundWorker.RunWorkerAsync();
        }
        public override void Exception( Exception exception )
        {
          string _msg = String.Format( Properties.Resources.LastOperationFailed, exception.Message );
          switch ( MessageBox.Show( _msg, Properties.Resources.CaptionOperationFailure, MessageBoxButton.OKCancel, MessageBoxImage.Error ) )
          {
            case MessageBoxResult.Cancel:
              m_Context.CancelInstallation();
              break;
            case MessageBoxResult.OK:
            case MessageBoxResult.No:
            case MessageBoxResult.None:
            case MessageBoxResult.Yes:
            default:
              m_ExpectedEvents = Events.Cancel | Events.Previous;
              m_Context.Update();
              break;
          }
        }
        public override void Previous()
        {
          m_Context.Machine = m_Context.m_SetupDataDialogMachine;
        }
        public override void Next()
        {
          m_Context.ExitlInstallation();
        }
        public override void Cancel()
        {
          if ( m_BackgroundWorker.IsBusy )
            m_BackgroundWorker.CancelAsync();
          else
            m_Context.ExitlInstallation();
        }
        #region private

        #region events handlers
        private void m_BackgroundWorker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
        {
          m_ExpectedEvents = Events.Previous;
          if ( e.Error != null )
            this.Exception( e.Error );
          else if ( e.Cancelled )
            return;
          else
            m_ExpectedEvents = Events.Next;
          //TODO m_NextButton.Text = Resources.NextButtonTextNext;
          m_Context.Update();
        }
        private void m_BackgroundWorker_ProgressChanged( object sender, ProgressChangedEventArgs e )
        {
          SPWebTeplateEventArgs _swtea = e.UserState as SPWebTeplateEventArgs;
          if ( _swtea != null )
            if ( this.OnSPWebTeplateSelection != null )
              this.OnSPWebTeplateSelection( this, _swtea );
            else
            {
              _swtea.SPWebTeplateSelected();
              if ( ProgressChanged != null )
                ProgressChanged( this, _swtea );
            }
          else
            if ( ProgressChanged != null )
              ProgressChanged( this, e );
        }
        private void m_BackgroundWorker_DoWork( object sender, DoWorkEventArgs e )
        {
          BackgroundWorker _wrkr = (BackgroundWorker)sender;
          Commands.DoInstallation( m_Context.InstallationDescription, _wrkr, e, m_Context.PackageProperties.Location );
        }
        #endregion

        private Events m_ExpectedEvents = Events.Cancel;
        private BackgroundWorker m_BackgroundWorker = null;

        #endregion

      }
      internal class FinishedMachine: AbstractMachine, IFinishedMachineViewModel
      {
        protected internal override Events GetEventMask()
        {
          return m_ExpectedEvents;
        }
        protected internal override ProcessState State
        {
          get { return ProcessState.Finisched; }
        }
        internal override void EnteringState() { }
        public override void Next()
        {
          m_Context.ExitlInstallation();
        }
        public override void Uninstall()
        {
          m_Context.Machine = m_Context.m_UninstallMachine;
        }
        internal FinishedMachine( StateMachineContext context )
          : base( context )
        {
          //    m_NextButton.Text = Resources.FinishButtonText;
        }
        private Events m_ExpectedEvents = Events.Next | Events.Uninstall;

        #region IFinishedMachineViewModel Members
        public event ProgressChangedEventHandler ProgressChanged;
        #endregion
      }
      internal class UninstallMachine: AbstractMachine, IUninstallMachineViewModel
      {
        protected internal override Events GetEventMask()
        {
          throw new NotImplementedException();
        }
        //  m_PreviousButton.Visible = false;
        //  m_NextButton.Enabled = true;
        //  m_UninstallButton.Visible = false;
        //  m_CancelButton.Visible = false;
        protected internal override ProcessState State
        {
          get { return ProcessState.Uninstall; }
        }
        internal override void EnteringState()
        {
          throw new NotImplementedException();
        }
        public override void Next()
        {
          m_Context.ExitlInstallation();
        }
        internal UninstallMachine( StateMachineContext context )
          : base( context )
        {
          m_Context.Uninstallation();
          //  m_NextButton.Text = Resources.FinishButtonText;
        }

        #region IUninstallMachineViewModel Members
        public event ProgressChangedEventHandler ProgressChanged;
        #endregion
      }
      #endregion

      #region vars
      protected internal abstract ProcessState State { get; }
      private StateMachineContext m_Context = null;
      protected internal abstract Events GetEventMask();
      #endregion

      #endregion

    }

    #region StateMachineContext View Model
    protected abstract void CancelInstallation();
    protected abstract void Update();
    protected abstract void Setup();
    protected abstract void ExitlInstallation();
    protected abstract void Uninstallation();
    #endregion

    #region vars
    protected AbstractMachine Machine
    {
      get { return m_Machine; }
      set
      {
        Tracing.TraceEvent.TraceVerbose( 86, "State", String.Format( "Entered the state: {0}.", value ) );
        m_Machine = value;
        Setup();
        m_Machine.EnteringState();
      }
    }
    private AbstractMachine m_Machine = null;
    private AbstractMachine.FinishedMachine m_FinishedMachine;
    private AbstractMachine.InstallationMachine m_InstallationMachine;
    private AbstractMachine.SetupDataDialogMachine m_SetupDataDialogMachine;
    private AbstractMachine.UninstallMachine m_UninstallMachine;
    private AbstractMachine.ValidationMachine m_ValidationMachine;
    #endregion

    #endregion

  }

}