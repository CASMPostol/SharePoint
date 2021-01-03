//<summary>
//  Title   : interface IEntityRef
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-01-21 14:28:33 +0100 (śr., 21 sty 2015) $
//  $Rev: 11245 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/IEntityRef.cs $
//  $Id: IEntityRef.cs 11245 2015-01-21 13:28:33Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using Microsoft.SharePoint.Client;

namespace Microsoft.SharePoint.Linq
{
  /// <summary>
  /// Interface IEntityRef - provides internal operation for the lookup property represented by the <see cref="EntityRef{TEntity}"/>
  /// </summary>
  internal interface IEntityRef
  {

    /// <summary>
    /// Assigns new value to the entity.
    /// </summary>
    /// <param name="entity">The referenced entity.</param>
    void SetEntity(object entity);
    /// <summary>
    /// Sets the lookup field - operation executed once after creating new entity and attaching it or getting new value from SharePoint.
    /// </summary>
    /// <param name="value">The value containing information from SharePoint field.</param>
    /// <param name="dataContext">The data context.</param>
    /// <param name="listName">Name of the list.</param>
    void SetLookup(FieldLookupValue value, DataContext dataContext, string listName);
    /// <summary>
    /// Gets the lookup information to be assigned to the lookup field.
    /// </summary>
    /// <param name="listName">Name of the list.</param>
    /// <returns>FieldLookupValue.</returns>
    FieldLookupValue GetLookup(string listName);
  
  }
}
