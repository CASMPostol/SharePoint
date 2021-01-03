using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;

namespace CAS.Common.Serialization.UnitTests
{
  [TestClass]
  public class JsonUnitTest
  {
    [TestMethod]
    public void JsonTestMethod()
    {
      Person _pr = new Person("!@#$%^&*()_+1234567890-=", 123456789);
      string _output = JsonSerialization.Serialize<Person>(_pr);
      Person _newPr = JsonSerialization.Deserialize<Person>(_output);
      Assert.AreEqual<string>(_pr.Name, _newPr.Name);
      Assert.AreEqual<int>(_pr.ID, _newPr.ID);
      string _newPersonString = JsonSerialization.Serialize<Person>(_newPr);
      Assert.AreEqual<string>(_output, _newPersonString);
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
