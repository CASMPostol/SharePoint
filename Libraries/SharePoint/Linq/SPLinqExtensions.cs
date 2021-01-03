//_______________________________________________________________
//  Title   : SPLinqExtensions
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2015-04-03 12:57:48 +0200 (pt., 03 kwi 2015) $
//  $Rev: 11553 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePoint/Linq/SPLinqExtensions.cs $
//  $Id: SPLinqExtensions.cs 11553 2015-04-03 10:57:48Z mpostol $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using Microsoft.SharePoint.Linq;
using System;
using System.Linq;

namespace CAS.SharePoint.Linq
{
  /// <summary>
  /// Class SPLinqExtensions - helpers of the SharePoint Linq.
  /// </summary>
  public static class SPLinqExtensions
  {

    /// <summary>
    /// The ID column name
    /// </summary>
    public const string IDColumnName = "ID";
    /// <summary>
    /// The title column name
    /// </summary>
    public const string TitleColumnName = "Title";
    /// <summary>
    /// The ID property name
    /// </summary>
    public const string IDPropertyName = "Id";
    /// <summary>
    /// The title property name
    /// </summary>
    public const string TitlePropertyName = "Title";
    /// <summary>
    /// Try to get an <see cref="IItem"/> at the index <paramref name="id"/>.
    /// </summary>
    /// <typeparam name="t">The type of the list entries.</typeparam>
    /// <param name="list">The list to be queried.</param>
    /// <param name="id">The <see cref="IItem"/> identifier.</param>
    /// <exception cref="ApplicationException">Element cannot be found.</exception>
    /// <returns>An instance of the <typeparamref name="t"/> for the selected index.</returns>
    public static t TryGetAtIndex<t>(this EntityList<t> list, string id)
      where t : class, IItem
    {
      if (id.IsNullOrEmpty())
        return null;
      return GetAtIndex<t>(list, id);
    }
    /// <summary>
    /// Get an <see cref="IItem"/> at the index <paramref name="id"/>.
    /// </summary>
    /// <typeparam name="t">The type of the list entries.</typeparam>
    /// <param name="list">The list to be queried.</param>
    /// <param name="id">The <see cref="IItem"/> identifier.</param>
    /// <exception cref="ApplicationException"><paramref name="id"/> is null or element cannot be found.</exception>
    /// <returns>An instance of the <typeparamref name="t"/> for the selected index.</returns>
    public static t GetAtIndex<t>(this EntityList<t> list, string id)
      where t : class, IItem
    {
      int? _index = id.String2Int();
      if (!_index.HasValue)
        throw new ApplicationException(typeof(t).Name + " index is null");
      try
      {
        return GetAtIndex<t>(list, _index.Value);
      }
      catch (Exception)
      {
        throw new ApplicationException(String.Format("{0} cannot be found at specified index {1}", typeof(t).Name, _index.Value));
      }
    }
    /// <summary>
    /// Gets an entity at index.
    /// </summary>
    /// <typeparam name="t">The type of the list entries.</typeparam>
    /// <param name="list">The list to be queried.</param>
    /// <param name="id">The <see cref="IItem"/> identifier.</param>
    /// <returns>An instance of the <typeparamref name="t"/> for the selected index.</returns>
    public static t GetAtIndex<t>(this EntityList<t> list, int id)
      where t : class, IItem
    {
      return (from idx in list where idx.Id == id select idx).First();
    }
    /// <summary>
    /// Finds an entity at <paramref name="id"/>
    /// </summary>
    /// <typeparam name="t">The type of the list entries.</typeparam>
    /// <param name="list">The list to be queried.</param>
    /// <param name="id">The <see cref="IItem"/> identifier.</param>
    /// <returns>An instance of the <typeparamref name="t"/> for the selected index.</returns>
    public static t FindAtIndex<t>(this EntityList<t> list, string id)
      where t : class, IItem
    {
      int? _index = id.String2Int();
      if (!_index.HasValue)
        return null;
      try
      {
        return (from idx in list where idx.Id == _index.Value select idx).FirstOrDefault();
      }
      catch (Exception)
      {
        return null;
      }
    }

  }
}

