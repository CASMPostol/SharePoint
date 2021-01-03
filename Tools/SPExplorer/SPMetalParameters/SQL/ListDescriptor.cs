//<summary>
//  Title   : class ListDescriptor
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Tools/SPExplorer/SPMetalParameters/SQL/ListDescriptor.cs $
//  $Id: ListDescriptor.cs 10838 2014-10-10 14:13:54Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Linq;

namespace CAS.SharePoint.Tools.SPExplorer.SPMetalParameters.SQL
{
  /// <summary>
  /// Type of the column enumerator
  /// </summary>
  public enum ColumnType
  {
    /// <summary>
    /// The regular
    /// </summary>
    Regular,
    /// <summary>
    /// The private key
    /// </summary>
    PrivateKey,
    /// <summary>
    /// The foreign key
    /// </summary>
    ForeignKey
  }
  /// <summary>
  /// List descriptor providing information about this list instance
  /// </summary>
  public class ListDescriptor
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="ListDescriptor"/> class.
    /// </summary>
    /// <param name="name">The name of the list.</param>
    public ListDescriptor(string name)
    {
      SQLTableName = name.SQLName();
    }
    /// <summary>
    /// Gets the columns of represented list.
    /// </summary>
    /// <value>
    /// The columns.
    /// </value>
    public RegularColumn[] Columns
    {
      get { return b_Columns; }
      set
      {
        b_Columns = value;
        PFColumnName = value.Where<RegularColumn>(_clx => _clx.Type == ColumnType.PrivateKey).FirstOrDefault<RegularColumn>().GetName();
      }
    }
    /// <summary>
    /// Gets the name of the SQL table.
    /// </summary>
    /// <value>
    /// The name of the SQL table.
    /// </value>
    public string SQLTableName { get; private set; }
    /// <summary>
    /// Gets the name of the pf column.
    /// </summary>
    /// <value>
    /// The name of the pf column.
    /// </value>
    public string PFColumnName { get; private set; }
    private RegularColumn[] b_Columns;

  }
}
