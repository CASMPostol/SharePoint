using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAS.Common.Serialization.UnitTests.CustomData;

namespace CAS.Common.Serialization.UnitTests
{
  [TestClass]
  public class XmlUnitTest
  {
    [TestMethod]
    public void Xml2StringTestMethod()
    {
      catalog _catalog = CreateTestData();
      string _catalogString = XmlSerialization.Serialize<catalog>(_catalog, _catalog.StylesheetName);
      catalog _newCatalog = XmlSerialization.Deserialize<catalog>(_catalogString);
      string _newCatalogString = XmlSerialization.Serialize<catalog>(_newCatalog, _catalog.StylesheetName);
      Assert.AreEqual<string>(_catalogString, _newCatalogString);

    }
    [TestMethod]
    public void Xml2FileTestMethod()
    {
      string _fileName = "TestFile.xml";
      catalog _catalog = CreateTestData();
      using (System.IO.FileStream _stream = new System.IO.FileStream(_fileName, System.IO.FileMode.Create))
        XmlSerialization.WriteXmlFile<catalog>(_catalog, _stream, _catalog.StylesheetName);
      catalog _newCatalog;
      using (System.IO.FileStream _stream = new System.IO.FileStream(_fileName, System.IO.FileMode.Open))
        _newCatalog = XmlSerialization.ReadXmlFile<catalog>(_stream);
      string _catalogString = XmlSerialization.Serialize<catalog>(_catalog, _catalog.StylesheetName);
      string _newCatalogString = XmlSerialization.Serialize<catalog>(_newCatalog, _catalog.StylesheetName);
      Assert.AreEqual<string>(_catalogString, _newCatalogString);
    }

    private static catalog CreateTestData()
    {
      CDDescription _cd1 = new CDDescription()
      {
        artist = "Bob Dylan",
        title = "Empire Burlesque",
        country = "USA",
        company = "Columbia",
        price = 10.90M,
        year = 1985,
      };
      CDDescription _cd2 = new CDDescription
      {
        title = "Hide your heart",
        artist = "Bonnie Tyler",
        country = "UK",
        company = "CBS Records",
        price = 9.90M,
        year = 1988
      };
      catalog _catalog = new catalog
      {
        cd = new CDDescription[] { _cd1, _cd2 }
      };
      return _catalog;
    }
  }
}
