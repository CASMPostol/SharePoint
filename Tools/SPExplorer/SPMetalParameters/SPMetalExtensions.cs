//<summary>
//  Title   : SPMetalExtensions helper class
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-10 16:13:54 +0200 (pt., 10 paź 2014) $
//  $Rev: 10838 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Tools/SPExplorer/SPMetalParameters/SPMetalExtensions.cs $
//  $Id: SPMetalExtensions.cs 10838 2014-10-10 14:13:54Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace CAS.SharePoint.Tools.SPExplorer.SPMetalParameters
{

  internal static class SPMetalExtensions
  {
    internal static string SQLName(this string name)
    {
      name = name.Replace(" ", "");
      if ("Procedure, Format, Order, Group, No, External, Status".Contains(name))
        return string.Format("SP{0}", name);
      else
        return name;
    }
    /// <summary>
    /// Determines whether the provided <see cref="SPFieldType"/> type is supported and can be converted to SQL type.
    /// </summary>
    /// <param name="type">The type to validate.</param>
    internal static bool IsSPFieldTypeSupported(this SPFieldType type)
    {
      return m_ConveribleTypes.ContainsKey(type);
    }
    internal static string SQLDataType(this SPFieldType type)
    {
      if (m_ConveribleTypes.ContainsKey(type))
        return m_ConveribleTypes[type];
      return String.Empty;
    }
    /// <summary>
    /// Contains all types that can be converted to SQL equivalent type
    /// </summary>
    private static Dictionary<SPFieldType, string> m_ConveribleTypes = new Dictionary<SPFieldType, string>
    {
      { SPFieldType.AllDayEvent,  "BIT"},
      { SPFieldType.Attachments,  "BIT"},
      { SPFieldType.Boolean,  "BIT"},
      { SPFieldType.CrossProjectLink,  "BIT"},
      { SPFieldType.Recurrence,  "BIT"},
      { SPFieldType.Calculated,  "FLOAT"},
      { SPFieldType.Counter,  "INT"},
      { SPFieldType.Integer,  "INT"},
      { SPFieldType.Lookup,  "INT"},
      { SPFieldType.User,  "NVARCHAR(max)"},
      { SPFieldType.Currency, "FLOAT"},
      { SPFieldType.Number,  "FLOAT"},
      { SPFieldType.DateTime, "DATETIME"},
      { SPFieldType.Note, "NVARCHAR(max)"},
      { SPFieldType.Text, "NVARCHAR(max)"},
      { SPFieldType.File, "NVARCHAR(max)"},
      { SPFieldType.Choice, "NVARCHAR(max)"}
    };
  }
}
