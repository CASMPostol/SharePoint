//<summary>
//  Title   : interface IEntityListItemsCollection
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-01-09 16:00:57 +0100 (pt., 09 sty 2015) $
//  $Rev: 11188 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/IEntityListItemsCollection.cs $
//  $Id: IEntityListItemsCollection.cs 11188 2015-01-09 15:00:57Z mpostol $
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.SharePoint.Linq
{
  internal interface IEntityListItemsCollection
  {

    /// <summary>
    /// Submit the changes.
    /// </summary>
    /// <param name="executeQuery">The action used to execute query.</param>
    void SubmitChanges(Action<ProgressChangedEventArgs, Action> executeQuery);
    /// <summary>
    /// Gets the field lookup value.
    /// </summary>
    /// <param name="fieldLookupValue">The field lookup value.</param>
    /// <returns>Return the value.</returns>
    object GetFieldLookupValue(FieldLookupValue fieldLookupValue );
    /// <summary>
    /// Gets the field lookup value.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>FieldLookupValue.</returns>
    FieldLookupValue GetFieldLookupValue( Object entity );
    /// <summary>
    /// Gets the entities for reverse lookup.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="parentItemId">The parent item identifier.</param>
    /// <returns>Dictionary&lt;System.Int32, Object&gt;.</returns>
    Dictionary<int, Object> GetEntitiesForReverseLookup(string fieldName, int parentItemId);
    /// <summary>
    /// Gets the index of the <paramref name="item"/> using reflection and <see cref="CAS.SharePoint.Client.Linq2SP.StorageItemsList"/>.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    int GetIndex(object item); 

  }
}
