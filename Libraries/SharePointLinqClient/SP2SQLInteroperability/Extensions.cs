//_______________________________________________________________
//  Title   : CAS.SharePoint.Client.SP2SQLInteroperability.Extensions
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate: 2015-01-28 15:08:14 +0100 (śr., 28 sty 2015) $
//  $Rev: 11268 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePointLinqClient/SP2SQLInteroperability/Extensions.cs $
//  $Id: Extensions.cs 11268 2015-01-28 14:08:14Z mpostol $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAS.SharePoint.Client.SP2SQLInteroperability
{
  /// <summary>
  /// Class Extensions. - static class providing extension for this namespace
  /// </summary>
  public static class Extensions
  {
    
    /// <summary>
    /// Determines whether the specified <paramref name="start"/> is latter than expected.
    /// </summary>
    /// <param name="start">The start data to compare.</param>
    /// <param name="delay">The delay in days.</param>
    /// <returns><c>true</c> if the specified start data is latter then <see cref="DateTime.Today"/>- <paramref name="delay"/>; otherwise, <c>false</c>.</returns>
    public static bool IsLatter(this DateTime? start, int delay)
    {
      return start.HasValue ? start + TimeSpan.FromDays(delay) < DateTime.Today : false;
    }
    /// <summary>
    /// Adds the <paramref name="item"/> to the list if it is not null.
    /// </summary>
    /// <typeparam name="T">Type of the list items.</typeparam>
    /// <param name="list">The list containing a collection of <paramref name="item"/>.</param>
    /// <param name="item">The item to be added.</param>
    public static void AddIfNotNull<T>(this List<T> list, T item)
      where T : class
    {
      if (item == null)
        return;
      list.Add(item);
    }
    /// <summary>
    /// Adds the <paramref name="key"/> to the list if not already done.
    /// </summary>
    /// <typeparam name="TKey">The type of the.<paramref name="key"/>.</typeparam>
    /// <param name="list">The list containing collection of <typeparamref name="TKey"/>.</param>
    /// <param name="key">The key to be added.</param>
    public static void AddIfNew<TKey>(this List<TKey> list, TKey key)
    {
      if (list.Contains(key))
        return;
      list.Add(key);
    }
    /// <summary>
    /// Submitting changes to SHarePoint and SQL.
    /// </summary>
    /// <param name="spedc">The SharePoint entities.</param>
    /// <param name="sqledc">The SQL entities.</param>
    /// <param name="progress">The delegate to inform about the operation progress.</param>
    public static void SubmitChanges(Microsoft.SharePoint.Linq.DataContext spedc, System.Data.Linq.DataContext sqledc, ProgressChangedEventHandler progress)
    {
      progress(null, new ProgressChangedEventArgs(1, "Submitting changes to SQL"));
      sqledc.SubmitChanges();
      progress(null, new ProgressChangedEventArgs(1, "Submitting changes to SharePoint"));
      spedc.SubmitChanges();
    }

  }
}
