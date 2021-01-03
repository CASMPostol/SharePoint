using CAS.Common.Interactivity.InteractionRequest;
using CAS.Common.ViewModel.Wizard;
using CAS.Common.ViewModel.Wizard.ButtonsPanelStateTemplates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CAS.Common.UnitTest
{
  [TestClass]
  public class ButtonsPanelBaseUnitTest
  {
    #region TestMethod
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ButtonsPanelBaseCreationErrorTest()
    {
      ButtonsPanelBase _bpb = new ViewModel.Wizard.ButtonsPanelBase(null);
    }
    [TestMethod]
    public void StateMachineContextCreation()
    {
      StateMachineContextMoc _sm = new StateMachineContextMoc();
      _sm.CheckConsistencyAfterCreation();
    }
    [TestMethod]
    public void ButtonsPanelBaseCreationTest()
    {
      StateMachineContextMoc _sm = new StateMachineContextMoc();
      ButtonsPanelBase _bpb = new ViewModel.Wizard.ButtonsPanelBase(_sm);
      Assert.IsNotNull(_bpb);
      SimpleState _ss = _sm.EnterState<SimpleState>(new SimpleViewModel());
    }
    [TestMethod]
    public void SimpleViewModelCreationTestMethod()
    {
      using (StateMachineContextMoc _sm = new StateMachineContextMoc())
      {
        SimpleViewModel _svm = new SimpleViewModel();
      }
    }

    #endregion

    #region private
    private class StateMachineContextMoc : StateMachineContext
    {
      public StateMachineContextMoc()
      {
        this.CancelConfirmation.Raised += CancelConfirmation_Raised;
      }
      internal void CheckConsistencyAfterCreation()
      {
        Assert.IsNull(this.ButtonPanelState);
        Assert.IsTrue(String.IsNullOrEmpty(m_LastCancelConfirmationTitle));
        Assert.IsInstanceOfType(m_LastCancelConfirmationContent, typeof(string));
        Assert.IsTrue(String.IsNullOrEmpty((string)m_LastCancelConfirmationContent));
        Assert.IsTrue(this.CancellationConfirmation());
      }
      internal void TestUserInteractionAfterCancel()
      {
        Assert.IsInstanceOfType(m_LastCancelConfirmationContent, typeof(string));
        Assert.IsFalse(String.IsNullOrEmpty((string)m_LastCancelConfirmationContent));
        Assert.IsFalse(String.IsNullOrEmpty(m_LastCancelConfirmationTitle));
      }
      private void CancelConfirmation_Raised(object sender, InteractionRequestedEventArgs<IConfirmation> e)
      {
        //if (MessageBox.Show((string)e.Context.Content, e.Context.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        //  e.Context.Confirmed = true;
        //else
        //  e.Context.Confirmed = false;
        m_LastCancelConfirmationContent = e.Context.Content;
        m_LastCancelConfirmationTitle = e.Context.Title;
        e.Context.Confirmed = true;
        e.Callback();
      }
      private object m_LastCancelConfirmationContent = String.Empty;
      private string m_LastCancelConfirmationTitle = String.Empty;
      protected override void StateNameProgressChang(string machineStateName)
      {
        Assert.AreEqual<string>(SimpleState.StateName, machineStateName);
      }
    }
    private class SimpleState : BackgroundWorkerMachine<StateMachineContextMoc, SimpleViewModel>
    {

      public SimpleState()
      {
        this.m_ButtonsPanelState = new CancelTemplate(string.Empty, string.Empty, string.Empty);
      }
      internal void TestIt()
      {
        Assert.IsInstanceOfType(Context, typeof(StateMachineContextMoc));
        Assert.IsTrue(Context.CancellationConfirmation());
        Context.TestUserInteractionAfterCancel();

        Assert.IsFalse(m_ActionExecuted[(int)StateMachineEventIndex.LeftButtonEvent]);
        Assert.IsFalse(m_ActionExecuted[(int)StateMachineEventIndex.LeftMiddleButtonEvent]);
        Assert.IsFalse(m_ActionExecuted[(int)StateMachineEventIndex.RightMiddleButtonEvent]);
        Assert.IsFalse(m_ActionExecuted[(int)StateMachineEventIndex.RightButtonEvent]);

        Context.StateMachineActionsArray[(int)StateMachineEventIndex.LeftButtonEvent](null);
        Assert.IsTrue(m_ActionExecuted[(int)StateMachineEventIndex.LeftButtonEvent]);
        Assert.IsFalse(m_ActionExecuted[(int)StateMachineEventIndex.LeftMiddleButtonEvent]);
        Assert.IsFalse(m_ActionExecuted[(int)StateMachineEventIndex.RightMiddleButtonEvent]);
        Assert.IsFalse(m_ActionExecuted[(int)StateMachineEventIndex.RightButtonEvent]);

        Context.StateMachineActionsArray[(int)StateMachineEventIndex.LeftMiddleButtonEvent](null);
        Assert.IsTrue(m_ActionExecuted[(int)StateMachineEventIndex.LeftMiddleButtonEvent]);
        Assert.IsFalse(m_ActionExecuted[(int)StateMachineEventIndex.RightMiddleButtonEvent]);
        Assert.IsFalse(m_ActionExecuted[(int)StateMachineEventIndex.RightButtonEvent]);

        Context.StateMachineActionsArray[(int)StateMachineEventIndex.RightMiddleButtonEvent](null);
        Assert.IsTrue(m_ActionExecuted[(int)StateMachineEventIndex.RightMiddleButtonEvent]);
        Assert.IsFalse(m_ActionExecuted[(int)StateMachineEventIndex.RightButtonEvent]);

        Context.StateMachineActionsArray[(int)StateMachineEventIndex.RightButtonEvent](null);
        Assert.IsTrue(m_ActionExecuted[(int)StateMachineEventIndex.RightButtonEvent]);

      }
      public override Action<object>[] StateMachineActionsArray
      {
        get
        {
          return new Action<object>[] 
          { x => m_ActionExecuted[(int)StateMachineEventIndex.LeftButtonEvent] = true , 
            x => m_ActionExecuted[(int)StateMachineEventIndex.LeftMiddleButtonEvent] = true ,
            x => m_ActionExecuted[(int)StateMachineEventIndex.RightMiddleButtonEvent] = true ,
            x => m_ActionExecuted[(int)StateMachineEventIndex.RightButtonEvent] = true 
          };
        }
      }
      protected override void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
      {
        throw new NotImplementedException();
      }
      protected override void RunWorkerCompleted(object result)
      {
        throw new NotImplementedException();
      }
      protected override void OnlyCancelActive()
      {
        throw new NotImplementedException();
      }
      protected override ButtonsSetState ButtonsPanelState { get { return m_ButtonsPanelState; } }
      public override void OnException(Exception exception)
      {
        Context.Exception(exception);
        m_ButtonsPanelState.OnlyCancel();
      }
      public override string ToString()
      {
        return StateName;
      }
      private bool[] m_ActionExecuted = new bool[] { false, false, false, false };
      private CancelTemplate m_ButtonsPanelState;
      public const string StateName = "SimpleStateName";
    }
    private class SimpleViewModel : ViewModelBase<SimpleState>
    {

      private string b_MyProperty;
      public string MyProperty
      {
        get
        {
          return b_MyProperty;
        }
        set
        {
          RaiseHandler<string>(value, ref b_MyProperty, "MyProperty", this);
        }
      }

    }
    #endregion

  }
}

