//<summary>
//  Title   : class RegularColumn
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Tools/SPExplorer/SPMetalParameters/SQL/RegularColumn.cs $
//  $Id: RegularColumn.cs 10838 2014-10-10 14:13:54Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>


namespace CAS.SharePoint.Tools.SPExplorer.SPMetalParameters.SQL
{
  /// <summary>
  /// The class <see cref="RegularColumn" /> represents basic information about the column
  /// </summary>
  public class RegularColumn
  {
    internal RegularColumn(ColumnType type, string name, bool nullable, string sqlDataType, int sqlPrecision)
    {
      Type = type;
      ColumnName = name;
      SQLNullable = nullable;
      SQLDataType = sqlDataType;
      SQLPrecision = sqlPrecision;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="RegularColumn"/> class.
    /// </summary>
    /// <param name="type">The type of the column.</param>
    /// <param name="name">The name of the column.</param>
    /// <param name="nullable">if set to <c>true</c> if value is nullable.</param>
    /// <param name="SQLDataType">Type of the SQL data.</param>
    public RegularColumn(ColumnType type, string name, bool nullable, string SQLDataType)
      : this(type, name, nullable, SQLDataType, -1)
    { }
    /// <summary>
    /// Gets the type of the column.
    /// </summary>
    /// <value>
    /// The column type.
    /// </value>
    public virtual ColumnType Type { get; private set; }
    /// <summary>
    /// The column name
    /// </summary>
    public string ColumnName { get; private set; }
    /// <summary>
    /// Gets or sets a value indicating whether the column is nullable].
    /// </summary>
    /// <value>
    /// <c>true</c> if the column is nullable; otherwise, <c>false</c>.
    /// </value>
    public bool SQLNullable { get; private set; }
    /// <summary>
    /// Gets or sets the SQL precision.
    /// </summary>
    /// <value>
    /// The SQL precision.
    /// </value>
    public int SQLPrecision { get; private set; }
    /// <summary>
    /// Gets or sets the type of the SQL data.
    /// </summary>
    /// <value>
    /// The type of the SQL data.
    /// </value>
    public string SQLDataType { get; private set; }
  }

}
