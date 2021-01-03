using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace CAS.Common.ViewModel.Wizard.ButtonsPanelStateTemplates.Tests
{
  [TestClass]
  public class ButtonsPanelStateTemplatesUnitTest
  {
    [TestMethod]
    public void CreateCancelTemplate()
    {
      CancelTemplate _template = new CancelTemplate(String.Empty, String.Empty, String.Empty);
      AssertOnlyCancel(_template);
      _template.OnlyCancel();
      AssertOnlyCancel(_template);
      _template.RevertChanges();
      AssertOnlyCancel(_template);
    }
    [TestMethod]
    public void CreateConnectTemplate()
    {
      ConnectCancelTemplate _template = new ConnectCancelTemplate(String.Empty, String.Empty);
      AssertOnlyConnectAndCancel(_template);
      _template.OnlyCancel();
      AssertOnlyCancel(_template);
      _template.RevertChanges();
      AssertOnlyConnectAndCancel(_template);
      _template.OnlyMe();
      AssertOnlyConnectAndCancel(_template);
      _template.RevertChanges();
      AssertOnlyConnectAndCancel(_template);
    }
    [TestMethod]
    public void CreateAllButtonsTemplate()
    {
      ConnectCancelTemplate _template = new ConnectCancelTemplate("BL", "BML");
      AssertAll(_template);
      _template.OnlyCancel();
      AssertOnlyCancel(_template);
      _template.RevertChanges();
      AssertAll(_template);
      _template.OnlyMe();
      AssertOnlyConnectAndCancel(_template);
      _template.RevertChanges();
      AssertAll(_template);
    }

    //private helper methods
    private static void AssertAll(ConnectCancelTemplate _template)
    {
      Assert.AreEqual<StateMachineEvents>
        (StateMachineEvents.RightButtonEvent | StateMachineEvents.RightMiddleButtonEvent | StateMachineEvents.LeftButtonEvent | StateMachineEvents.LeftMiddleButtonEvent, _template.GetEnabledEvents);
      Assert.AreEqual<Visibility>(Visibility.Visible, _template.RightButtonVisibility);
      Assert.AreEqual<Visibility>(Visibility.Visible, _template.RightMiddleButtonVisibility);
      Assert.AreEqual<Visibility>(Visibility.Visible, _template.LeftMiddleButtonVisibility);
      Assert.AreEqual<Visibility>(Visibility.Visible, _template.LeftButtonVisibility);
    }
    private static void AssertOnlyConnectAndCancel(ConnectCancelTemplate _template)
    {
      Assert.AreEqual<StateMachineEvents>(StateMachineEvents.RightButtonEvent | StateMachineEvents.RightMiddleButtonEvent, _template.GetEnabledEvents);
      Assert.AreEqual<Visibility>(Visibility.Visible, _template.RightButtonVisibility);
      Assert.AreEqual<Visibility>(Visibility.Visible, _template.RightMiddleButtonVisibility);
    }
    private static void AssertOnlyCancel(CancelTemplate _template)
    {
      Assert.AreEqual<StateMachineEvents>(StateMachineEvents.RightButtonEvent, _template.GetEnabledEvents);
      Assert.AreEqual<Visibility>(Visibility.Visible, _template.RightButtonVisibility);
    }
  }
}
