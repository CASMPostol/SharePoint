using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;
using CAS.SharePoint.Serialization;

namespace CAS.SharePoin.UnitTests
{
  [TestClass]
  public class JsonUnitTest
  {
    [TestMethod]
    public void JsonTestMethod()
    {
      Person _pr = new Person("!@#$%^&*()_+1234567890-=", 123456789);
      string _output = JsonSerializer.Serialize<Person>(_pr);
      Person _newPr = JsonSerializer.Deserialize<Person>(_output);
      Assert.AreEqual<string>(_pr.Name, _newPr.Name);
      Assert.AreEqual<int>(_pr.ID, _newPr.ID);
    }
    //Test data
    // Set the Name and Namespace properties to new values.
    [DataContract(Name = "Customer", Namespace = "http://www.contoso.com")]
    class Person
    {
      // To implement the IExtensibleDataObject interface, you must also 
      // implement the ExtensionData property. 
      private ExtensionDataObject extensionDataObjectValue;
      public ExtensionDataObject ExtensionData
      {
        get
        {
          return extensionDataObjectValue;
        }
        set
        {
          extensionDataObjectValue = value;
        }
      }

      [DataMember(Name = "CustName")]
      internal string Name;
      [DataMember(Name = "CustID")]
      internal int ID;
      public Person(string newName, int newID)
      {
        Name = newName;
        ID = newID;
      }
    }

  }
}
