//<summary>
//  Title   : class ControlExtensions
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-04-03 12:57:48 +0200 (pt., 03 kwi 2015) $
//  $Rev: 11553 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePoint/Web/ControlExtensions.cs $
//  $Id: ControlExtensions.cs 11553 2015-04-03 10:57:48Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Linq;
using Microsoft.SharePoint.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace CAS.SharePoint.Web
{
  /// <summary>
  /// The static class containing control extensions functions
  /// </summary>
  public static class ControlExtensions
  {
    /// <summary>
    /// Gets the selected entry from the <see cref="DropDownList"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <param name="dropDown">The drop down.</param>
    /// <param name="entities">The entities.</param>
    /// <returns>TEntity.</returns>
    public static TEntity GetSelected<TEntity>(this DropDownList dropDown, EntityList<TEntity> entities)
      where TEntity : class, IItem
    {
      if (dropDown.SelectedIndex < 0)
        return null;
      return (from _ix in entities where _ix.Id == dropDown.SelectedValue.String2Int() select _ix).FirstOrDefault<TEntity>();
    }
    /// <summary>
    /// Entities the list data source.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <param name="dropDown">The <see cref="DropDownList"/> control to be populated with <paramref name="entries"/>.</param>
    /// <param name="entries">The entities to be added to the <see cref="DropDownList"/>.</param>
    public static void EntityListDataSource<TEntity>(this DropDownList dropDown, IQueryable<TEntity> entries)
      where TEntity : IItem
    {
      dropDown.DataSource = from _tpx in entries orderby _tpx.Title select new { ID = _tpx.Id, Title = _tpx.Title };
      dropDown.DataTextField = "Title";
      dropDown.DataValueField = "ID";
      dropDown.DataBind();
      dropDown.ClearSelection();
    }
    /// <summary>
    /// Selects the item for the element. If item cannot be selected (is not on the list) selection of the <paramref name="dropDown"/> is cleared.
    /// </summary>
    /// <param name="dropDown">The drop down to be updated.</param>
    /// <param name="element">The element pointing out the item to be selected.</param>
    public static void SelectItem4Element(this DropDownList dropDown, IItem element)
    {
      dropDown.ClearSelection();
      if (element != null)
      {
        ListItem _item = dropDown.Items.FindByValue(element.Id.IntToString());
        if (_item != null)
          _item.Selected = true;
      }
    }
    /// <summary>
    /// Selects the specified _DDL.
    /// </summary>
    /// <param name="_ddl">The _DDL.</param>
    /// <param name="_row">The _row.</param>
    public static void Select(this DropDownList _ddl, int _row)
    {
      _ddl.SelectedIndex = -1;
      ListItem _li = _ddl.Items.FindByValue(_row.ToString());
      if (_li == null)
        throw new ApplicationException(String.Format("DropDownList does not contain ListItem with Value = {0}.", _row)) { Source = "ControlExtensions.Select" };
      _li.Selected = true;
    }
    /// <summary>
    /// Converts the text in the <see cref="TextBox"/> to double.
    /// </summary>
    /// <param name="value">The <see cref="TextBox"/> to be converted.</param>
    /// <param name="errors">The errors list collecting messages added by consequence calls.</param>
    /// <returns></returns>
    public static double? TextBox2Double(this TextBox value, List<string> errors)
    {
      string _trimed = value.Text.Trim();
      if (_trimed.IsNullOrEmpty())
        return null;
      double _dv;
      if (Double.TryParse(_trimed, NumberStyles.Any, CultureInfo.CurrentUICulture, out _dv))
        return _dv;
      //TODO _errors.Add( String.Format( "WrongValue".GetLocalizedString(), _value.Text ) );
      errors.Add(String.Format("Wrong Value {0}", value.Text));
      return null;
    }
    /// <summary>
    /// Creates the message as <see cref="Literal"/>.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    public static Literal CreateMessage(string message)
    {
      return new Literal()
      {
        Text = CommonDefinitions.Convert2ErrorMessageFormat(message)
      };
    }
  }
}
