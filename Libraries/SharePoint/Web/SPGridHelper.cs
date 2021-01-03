using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;

namespace CAS.SharePoint.Web
{
  /// <summary>
  /// <see cref="SPGridView"/> helper functionality.
  /// </summary>
  public static class SPGridHelper
  {
    /// <summary>
    /// Handles the RowCreated event of the CustomGridView control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="GridViewRowEventArgs" /> instance containing the event data.</param>
    public static void CustomGridView_RowCreated( Object sender, GridViewRowEventArgs e )
    {
      GridView _sender = sender as GridView;
      // Use the RowType property to determine whether the row being created is the header row.  
      if ( e.Row.RowType == DataControlRowType.Header )
      {
        // Call the GetSortColumnIndex helper method to determine 
        // the index of the column being sorted. 
        int sortColumnIndex = GetSortColumnIndex( _sender );
        if ( sortColumnIndex != -1 )
        {
          // Call the AddSortImage helper method to add 
          // a sort direction image to the appropriate 
          // column header. 
          AddSortImage( _sender, sortColumnIndex, e.Row );
        }
      }
    }

    // This is a helper method used to determine the index of the 
    // column being sorted. If no column is being sorted, -1 is returned. 
    private static int GetSortColumnIndex( GridView control )
    {

      // Iterate through the Columns collection to determine the index 
      // of the column being sorted. 
      foreach ( DataControlField field in control.Columns )
      {
        if ( field.SortExpression == control.SortExpression )
        {
          return control.Columns.IndexOf( field );
        }
      }
      return -1;
    }
    // This is a helper method used to add a sort direction 
    // image to the header of the column being sorted. 
    private static void AddSortImage( GridView control, int columnIndex, GridViewRow headerRow )
    {
      // Create the sorting image based on the sort direction.
      Image sortImage = new Image();
      if ( control.SortDirection == SortDirection.Ascending )
      {
        sortImage.ImageUrl = "~/Images/Ascending.jpg";
        sortImage.AlternateText = "Ascending Order";
      }
      else
      {
        sortImage.ImageUrl = "~/Images/Descending.jpg";
        sortImage.AlternateText = "Descending Order";
      }
      // Add the image to the appropriate header cell.
      headerRow.Cells[ columnIndex ].Controls.Add( sortImage );
    }

  }
}
