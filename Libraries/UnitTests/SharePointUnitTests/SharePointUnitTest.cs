using CAS.SharePoint;
using CAS.SharePoint.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CAS.SharePoin.UnitTests
{
  [TestClass]
  public class SharePointUnitTest
  {
    const string _value = "TYTON ODZYLOWANY BRAZIL FLUE CURED VIRGINIA STRIPS                                                                                 GRADE:  XBRFCB SKU:  12352149 BATCH:  0003814564                                       czesciowa likwidacja OGL/362010/00/002494/12";
    const string _batch = "  0003930000 ";
    const string _batchError = "  003930000 ";
    const string BatchNumberPattern = @"\b(000\d{7})";
    /// <summary>
    ///A test for SPValidSubstring
    ///</summary>
    [TestMethod()]
    public void SPValidSubstringTest()
    {
      string _cchar = "\0x10";
      _value.Insert( 10, _cchar );
      string actual;
      actual = Extensions.SPValidSubstring( _value );
      Assert.IsFalse( actual.Contains( "  " ), "Text should not contain double spaces" );
      Assert.IsTrue( actual.Length <= 254, "Text is too long" );
      Assert.IsTrue( actual.Min<char>() >= 0x20, "Text is too long" );
    }

    /// <summary>
    ///A test for GetFirstCapture
    ///</summary>
    [TestMethod()]
    public void GetFirstCaptureTest()
    {
      string _pattern = @"\b(.*)(?=\sGRADE:)";
      string expected = "TYTON ODZYLOWANY BRAZIL FLUE CURED VIRGINIA STRIPS";
      string actual;
      actual = Extensions.GetFirstCapture( Extensions.SPValidSubstring( _value ), _pattern );
      Assert.AreEqual( expected, actual );
      _pattern = @"(?<=\WGRADE:)\W*\b(\w*)";
      expected = "XBRFCB";
      actual = Extensions.GetFirstCapture( Extensions.SPValidSubstring( _value ), _pattern );
      Assert.AreEqual( expected, actual );
      _pattern = @"(?<=\WSKU:)\W*\b(\d*)";
      expected = "12352149";
      actual = Extensions.GetFirstCapture( Extensions.SPValidSubstring( _value ), _pattern );
      Assert.AreEqual( expected, actual );
      _pattern = @"(?<=\WBatch:)\W*\b(\d*)";
      expected = "0003814564";
      actual = Extensions.GetFirstCapture( Extensions.SPValidSubstring( _value ), _pattern );
      Assert.AreEqual( expected, actual );
    }
    [TestMethod]
    public void BatchNumberTest()
    {
      string expected = "0003930000";
      string actual = Extensions.GetFirstCapture( Extensions.SPValidSubstring( _batch ), BatchNumberPattern );
      Assert.AreEqual( expected, actual );
      expected = "0003930000";
      actual = Extensions.GetFirstCapture( Extensions.SPValidSubstring( _batch ), BatchNumberPattern, "Error" );
      Assert.AreEqual( expected, actual );
      expected = "Error";
      actual = Extensions.GetFirstCapture( Extensions.SPValidSubstring( _batchError ), BatchNumberPattern, "Error" );
      Assert.AreEqual( expected, actual );
    }
    [TestMethod]
    [Microsoft.VisualStudio.TestTools.UnitTesting.ExpectedException( typeof( GenericStateMachineEngine.ActionResult ) )]
    public void BatchNumberTestNotValidated()
    {
      string actual = Extensions.GetFirstCapture( Extensions.SPValidSubstring( _batchError ), BatchNumberPattern );
      Assert.Fail();
    }
    private const string RequiredDocumentFinishedGoodExportConsignmentPattern = @"\bP\w+\s+t\w+\s+(\d{7})";
    [TestMethod]
    public void ConsentNameRecogitionTest()
    {
      string actual = Extensions.GetFirstCapture( Extensions.SPValidSubstring( "  Pxxxxxx    Txxxxxxx     0000689" ), RequiredDocumentFinishedGoodExportConsignmentPattern );
      Assert.AreEqual( "0000689", actual );
      actual = Extensions.GetFirstCapture( Extensions.SPValidSubstring( "  Pxxxxxx    Txxxxxxx     0000689" ), RequiredDocumentFinishedGoodExportConsignmentPattern, "xxx" );
      Assert.AreEqual( "0000689", actual );
      actual = Extensions.GetFirstCapture( Extensions.SPValidSubstring( "  xxxxxxx    Txxxxxxx     0000689" ), RequiredDocumentFinishedGoodExportConsignmentPattern, "Error" );
      Assert.AreEqual( "Error", actual );
    }
    [TestMethod]
    [Microsoft.VisualStudio.TestTools.UnitTesting.ExpectedException( typeof( GenericStateMachineEngine.ActionResult ) )]
    public void ConsentNameRecogitionTestExc()
    {
      string actual = Extensions.GetFirstCapture( Extensions.SPValidSubstring( "xxxxxx    Txxxxxxx     0000689" ), RequiredDocumentFinishedGoodExportConsignmentPattern );
      Assert.AreEqual( "0000689", actual );
    }
  }
}