//<summary>
//  Title   : Name of Application
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2015-01-23 17:02:00 +0100 (pt., 23 sty 2015) $
//  $Rev: 11255 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePointLinqClient/Linq2SP/StorageItemsList.cs $
//  $Id: StorageItemsList.cs 11255 2015-01-23 16:02:00Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Collections.Generic;

namespace CAS.SharePoint.Client.Linq2SP
{
  /// <summary>
  /// <see cref="List{T}"/> of Storage items <see cref="StorageItem"/>
  /// </summary>
  internal class StorageItemsList : Dictionary<string, StorageItem>
  {
    internal int GetId<TEntity>(TEntity entity)
      where TEntity : class
    {
      return (int)IdStorage.GetValue<int>(entity);
    }
    internal StorageItem IdStorage { get; set; }
  }
}
