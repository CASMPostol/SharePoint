using System;
using System.Data;
using System.Web.UI.WebControls.WebParts;

namespace CAS.SharePoint.Web
{
  /// <summary>
  /// Interconnection Data
  /// </summary>
  /// <typeparam name="DerivedType">The type of the derived type.</typeparam>
  public abstract class InterconnectionData<DerivedType>: EventArgs
    where DerivedType: InterconnectionData<DerivedType>
  {
    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="InterconnectionData&lt;DerivedType&gt;"/> class.
    /// </summary>
    public InterconnectionData() { }
    /// <summary>
    /// Sets the row data.
    /// </summary>
    /// <param name="_connector">The _connector.</param>
    /// <param name="_update">The _update.</param>
    public void SetRowData( IWebPartRow _connector, EventHandler<DerivedType> _update )
    {
      m_Update = _update;
      _connector.GetRowData( SetData );
    }
    /// <summary>
    /// Gets the field value.
    /// </summary>
    /// <param name="_name">The _name.</param>
    /// <returns></returns>
    public string GetFieldValue( string _name )
    {
      if ( Row == null )
        return String.Empty;
      string _val = Row[ _name ] as String;
      if ( _val == null )
        return String.Empty;
      return _val;
    }
    #endregion

    #region private
    private void SetData( object _data )
    {
      Row = _data as DataRowView;
      m_Update( this, (DerivedType)this );
    }
    private EventHandler<DerivedType> m_Update;
    private DataRowView Row { get; set; }
    #endregion
  }
}
