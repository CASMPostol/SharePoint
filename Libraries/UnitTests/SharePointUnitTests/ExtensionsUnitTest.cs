using CAS.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace CAS.SharePoin.UnitTests
{
  [TestClass]
  public class ExtensionsUnitTest
  {
    private static string[] guids = {
			"0C885DD3-7DD9-484B-9B20-3E6552BCA144", 
			"{0C885DD3-7DD9-484B-9B20-3E6552BCA144}",
			"0C885DD37DD9484B9B203E6552BCA144",
			"{0C885DD3-7DD9-484B-9B20-1E6552BCA144}",
			"0C885DD3-7DD9484B9B203E6552BCA144",
      "{A192AFCA-A70A-4617-989D-21EEA7DD23D1}"
			};

    [TestMethod]
    public void IsGuidRegExTestMethod()
    {
      foreach (string guid in guids)
        Assert.IsTrue(guid.IsGuidRegEx());
    }
    [TestMethod]
    public void GuidTryParseTestMethod()
    {
      string _wrongGuid = "0C885DD37DD9484B9B203E6552BCA14";
      Assert.AreEqual<Guid>(Guid.Empty, _wrongGuid.GuidTryParse());
      _wrongGuid = "any string";
      Assert.AreEqual<Guid>(Guid.Empty, _wrongGuid.GuidTryParse());
      _wrongGuid = String.Empty;
      Assert.AreEqual<Guid>(Guid.Empty, _wrongGuid.GuidTryParse());
      _wrongGuid = null;
      Assert.AreEqual<Guid>(Guid.Empty, _wrongGuid.GuidTryParse());
    }
    [TestMethod]
    public void MemoryStreamTestMethod()
    {
      string _src = @"!@#$%^&*()_AppDomain+QWERTYUIOP{}asdfghjkl;'\\zxcvbnm,./?><";
      using (MemoryStream _ms = new MemoryStream())
      using (StreamWriter _sw = new StreamWriter(_ms, new UTF8Encoding()))
      {
        _sw.Write(_src);
        _sw.Flush();
        string _recovered = _ms.ReadString();
        Assert.AreEqual<string>(_src, _recovered);
      }
    }

  }
}
