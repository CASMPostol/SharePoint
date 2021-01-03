using CAS.SharePoint.Client.WebReferences;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace CAS.SharePoint.Client.WebService.UnitTests
{
  [TestClass]
  public class SharePointWebServiceClientUnitTest
  {
    [TestMethod]
    /// <summary>
    /// <see cref="VersioningInformation"/> test method - used to get an example of the response.
    /// </summary>
    public void VersioningInformationTestMethod()
    {
      //WebServiceContext _vi = new WebServiceContext(@"http://casas:11227/sites/ipr/_vti_bin/Lists.asmx");
      //for (int i = 0; i < 2; i++)
      //{
      //  string[] _fields = new string[] { "Modified", "Disposal2BatchIndex", "Title", "_UIVersionString", "Created" }; // { "ID", "No",  };
      //  List<Version> _res = _vi.GetVersionDescriptor(ListName, 1, _fields.ToList<string>());
      //}
      Assert.Inconclusive("Removed code - it is obsolete");
    }
    [TestMethod]
    public void VersionsTitleTestMethod()
    {
      XmlDocument doc = new XmlDocument();
      doc.LoadXml
        (@"<Version Title='Disposal: Cartons of material 0004068275' Modified='2013-09-19T06:40:27Z' Editor='10;#Wojcik,, Ireneusz,#JTICORP\gstwojcii,#Ireneusz.Wojcik@jti.com,#Ireneusz.Wojcik@jti.com,#Wojcik,, Ireneusz' />");
      Version _newOne = new Version(ListName, ListID, "Title", doc.FirstChild);
      Assert.AreEqual<int>(ListID, _newOne.ItemID);
      Assert.AreEqual<string>(ListName, _newOne.ListName);
      Assert.AreEqual<string>("Disposal: Cartons of material 0004068275", _newOne.Value);
      Assert.AreEqual<System.DateTime>(new System.DateTime(2013, 09, 19, 06, 40, 27), _newOne.Modified);
      Assert.AreEqual<string>(@"10;#Wojcik,, Ireneusz,#JTICORP\gstwojcii,#Ireneusz.Wojcik@jti.com,#Ireneusz.Wojcik@jti.com,#Wojcik,, Ireneusz", _newOne.Editor);
    }
    private const string ListName = "Disposal";
    private const int ListID = 49;
  }

}
