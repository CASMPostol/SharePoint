//_______________________________________________________________
//  Title   : DataAttribute - derived from Microsoft.SharePoint.Linq
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2015-01-23 17:02:00 +0100 (pt., 23 sty 2015) $
//  $Rev: 11255 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/DataAttribute.cs $
//  $Id: DataAttribute.cs 11255 2015-01-23 16:02:00Z mpostol $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System;

namespace Microsoft.SharePoint.Linq
{  
  /// <summary>
  /// Provides two optional properties commonly used by attributes on properties (of entity classes) that are mapped to list fields (columns) or list properties.
  /// </summary>
  [AttributeUsage( AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false )]
  public abstract class DataAttribute: Attribute
  { 
    /// <summary>
    ///  Initializes a new instance of the Microsoft.SharePoint.Linq.DataAttribute class.
    /// </summary>
    protected DataAttribute(){}   
    /// <summary>
    /// Gets or sets the internal name of the list field (column) or list property.
    /// </summary>
    /// <value>
    /// A System.String that identifies the name of the field or property.
    /// </value>
    public string Name { get; set; }  
    /// <summary>
    /// Gets or sets a value that indicates whether the column on the list is read-only.
    /// </summary>
    /// <value>
    ///    true, if the column is read-only; otherwise, false.
    /// </value>
    public bool ReadOnly { get; set; }    
    /// <summary>
    /// Gets or sets the field member of the entity class that stores the value of the property to which the attribute is applied.
    /// </summary>
    /// <value>
    /// A System.String that represents the name of the field member.
    /// </value>
    public string Storage { get; set; }
  }
}
