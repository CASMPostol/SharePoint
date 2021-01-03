//<summary>
//  Title   : class ForeignKeyColumn
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Tools/SPExplorer/SPMetalParameters/SQL/ForeignKeyColumn.cs $
//  $Id: ForeignKeyColumn.cs 10838 2014-10-10 14:13:54Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.SharePoint.Tools.SPExplorer.SPMetalParameters.SQL
{

  /// <summary>
  /// Class representing foreign key column
  /// </summary>
  public class ForeignKeyColumn : RegularColumn
  {
    internal ForeignKeyColumn(string name, bool nullable, string sqlDataType)
      : base(ColumnType.ForeignKey, name, nullable, sqlDataType, -1)
    { }
    /// <summary>
    /// The referenced table name
    /// </summary>
    public string ReferencedTableName;
    /// <summary>
    /// The referenced column - the private key of the destination table.
    /// </summary>
    public string ReferencedColumn;

  }
}
