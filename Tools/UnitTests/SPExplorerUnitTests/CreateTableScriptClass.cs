//<summary>
//  Title   : class CreateTableScriptClass
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-11-27 16:16:44 +0100 (czw., 27 lis 2014) $
//  $Rev: 10999 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/UnitTests/SPExplorerUnitTests/CreateTableScriptClass.cs $
//  $Id: CreateTableScriptClass.cs 10999 2014-11-27 15:16:44Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Serialization;
using CAS.SharePoint.Tools.SPExplorer.SQLGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SPExplorerUnitTests
{
  [TestClass]
  public class CreateTableScriptClass
  {
    [TestMethod]
    public void CreateTableScriptTestMethod()
    {
      string _privateKeyColumnName = "PrivateKeyColumnName";
      CreateTableScript _gen = new CreateTableScript()
      {
        ColumnDefinitions = GetColumnDefinitions(_privateKeyColumnName),
        PrivateKeyColumnName = _privateKeyColumnName,
        TableName = "TableName",
      };
      _gen.ConstrainDefinitions = GetConstrainDefinitions(_gen);
      Encoding _encoder = Encoding.ASCII;
      byte[] _resBytes = _encoder.GetBytes(_gen.TransformText());
      SHA1 _sha1 = SHA1.Create();
      byte[] _resHash = _sha1.ComputeHash(_resBytes);
      string _arrayContent = JsonSerializer.Serialize<byte[]>(_resHash); //to be used to define m_ResultString content - just copy/pased internal conmtent
      Assert.IsTrue(m_ResultString.Length == _resHash.Length);
      for (int i = 0; i < m_ResultString.Length; i++)
        Assert.AreEqual<byte>(m_ResultString[i], _resHash[i]);
    }
    private byte[] m_ResultString = new byte[] { 53, 201, 4, 231, 253, 140, 78, 209, 197, 79, 205, 41, 33, 233, 37, 194, 54, 219, 212, 87 };
    private List<CreateTableScript.ColumDescriptor> GetColumnDefinitions(string privateKeyColumnName)
    {
      List<CreateTableScript.ColumDescriptor> _ret = new List<CreateTableScript.ColumDescriptor>();
      _ret.Add(
        new CreateTableScript.ColumDescriptor()
        {
          ColumnName = privateKeyColumnName,
          DataType = "DataType",
          Nullable = false,
          Precision = -1
        }
      );
      for (int i = 0; i < 5; i++)
      {
        _ret.Add
         (
            new CreateTableScript.ColumDescriptor()
            {
              ColumnName = String.Format("ColumnName{0}", i),
              DataType = "DataType",
              Nullable = false,
              Precision = -1
            }
         );
      };
      return _ret;
    }
    private static List<CreateTableScript.ConstrainDescriptor> GetConstrainDefinitions(CreateTableScript parent)
    {
      List<CreateTableScript.ConstrainDescriptor> _ret = new List<CreateTableScript.ConstrainDescriptor>();
      for (int i = 0; i < 5; i++)
      {
        _ret.Add
         (
          new CreateTableScript.ConstrainDescriptor(parent)
          {
            FGColumnName = String.Format("FGColumnName{0}", i),
            ReferencedColumn = String.Format("ReferencedColumn{0}", i),
            ReferencedTableName = String.Format("ReferencedTableName{0}", i),
          }
         );
      };
      return _ret;
    }
  }
}
