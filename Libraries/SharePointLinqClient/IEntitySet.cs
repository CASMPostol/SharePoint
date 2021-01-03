//<summary>
//  Title   : internal interface IEntitySet
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2014-09-05 20:47:03 +0200 (pt., 05 wrz 2014) $
//  $Rev: 10768 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePointLinqClient/IEntitySet.cs $
//  $Id: IEntitySet.cs 10768 2014-09-05 18:47:03Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace Microsoft.SharePoint.Linq
{
  internal interface IEntitySet
  {
    /// <summary>
    /// Gets the values for the EntitySet.
    /// </summary>
    /// <param name="dataContext">The data context.</param>
    /// <param name="association">The association information from reflexion.</param>
    /// <param name="id">The identifier of the entity..</param>
    void LoadValues(DataContext dataContext, AssociationAttribute association, int id);
  }
}
