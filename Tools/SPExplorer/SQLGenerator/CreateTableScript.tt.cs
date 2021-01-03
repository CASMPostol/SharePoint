//<summary>
//  Title   : CreateTableScript
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-12-10 16:16:26 +0100 (śr., 10 gru 2014) $
//  $Rev: 11083 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Tools/SPExplorer/SQLGenerator/CreateTableScript.tt.cs $
//  $Id: CreateTableScript.tt.cs 11083 2014-12-10 15:16:26Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;

namespace CAS.SharePoint.Tools.SPExplorer.SQLGenerator
{
  public partial class CreateTableScript
  {

    public class ColumDescriptor
    {
      public string ColumnName;
      public String DataType;
      public int Precision = -1;
      public bool Nullable = true;
      public override string ToString()
      {
        string _dataTypePart = Precision <= 0 ? DataType : String.Format("({0})", Precision);
        string _NullablePart = Nullable ? "NULL" : "NOT NULL";
        return String.Format("{0,-24} {1,-15} {2}", "[" + ColumnName + "]", _dataTypePart, _NullablePart);
      }
    }
    public class ConstrainDescriptor
    {
      public ConstrainDescriptor(CreateTableScript parent)
      {
        m_Parent = parent;
      }
      public string ReferencedTableName;
      public string ReferencedColumn;
      public string FGColumnName;
      public override string ToString()
      {
        return String.Format("CONSTRAINT [FK_{0}_{1}] FOREIGN KEY ([{2}]) REFERENCES [dbo].[{1}] ([{3}])", m_Parent.TableName, ReferencedTableName, ReferencedColumn, FGColumnName);
      }
      private CreateTableScript m_Parent = null;
    }
    public string TableName = "TableName";
    public List<ColumDescriptor> ColumnDefinitions = new List<ColumDescriptor>();
    public List<ConstrainDescriptor> ConstrainDefinitions = new List<ConstrainDescriptor>();
    public string PrivateKeyColumnName;

  }
}
