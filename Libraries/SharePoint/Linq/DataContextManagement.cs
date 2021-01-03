//<summary>
//  Title   : class DataContextManagement
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2015-02-25 16:12:08 +0100 (śr., 25 lut 2015) $
//  $Rev: 11412 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/SharePoint/Linq/DataContextManagement.cs $
//  $Id: DataContextManagement.cs 11412 2015-02-25 15:12:08Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;
using System.Web.UI;
using Microsoft.SharePoint.Linq;

namespace CAS.SharePoint.Linq
{
  /// <summary>
  /// Interface IPreRender - contains the event that Occurs after the object is loaded but prior to rendering.
  /// </summary>
  public interface IPreRender
  {
    /// <summary>
    /// Occurs after the object is loaded but prior to rendering.
    /// </summary>
    event EventHandler PreRender;
  }
  /// <summary>
  /// class DataContextManagement used to create Linq entities. 
  /// </summary>
  /// <typeparam name="type">The type of the ype.</typeparam>
  public class DataContextManagement<type> : IDisposable
    where type : DataContext, new()
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DataContextManagement{type}"/> class.
    /// </summary>
    /// <param name="_parent">The _parent.</param>
    public DataContextManagement(UserControl _parent)
    {
      _parent.PreRender += new EventHandler(_parent_PreRender);
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="DataContextManagement{type}" /> class and adds event handler to dispose this object if it is not longer needed.
    /// </summary>
    /// <param name="preRender">An instance that implements <see cref="IPreRender"/>.</param>
    public DataContextManagement(IPreRender preRender)
    {
      preRender.PreRender += new EventHandler(_parent_PreRender);
    }
    /// <summary>
    /// Creates singleton <see cref="DataContextManagement&lt;type&gt;"/> if not created already and returns it to the caller.
    /// </summary>
    /// <value>
    /// An object derived from <see cref="DataContextManagement&lt;type&gt;"/>.
    /// </value>
    /// <exception cref="System.ObjectDisposedException">The object DataContext has been already disposed.</exception>
    public type DataContext
    {
      get
      {
        if (disposed)
          throw new ObjectDisposedException("The object DataContext has been already disposed.");
        if (_DataContext == null)
          _DataContext = new type();
        return _DataContext;
      }
    }
    private void _parent_PreRender(object sender, EventArgs e)
    {
      Dispose();
    }
    #region IDisposable
    // Other managed resource this class uses.
    private type _DataContext = null;
    // Track whether Dispose has been called.
    private bool disposed = false;
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      // This object will be cleaned up by the Dispose method.
      // Therefore, you should call GC.SupressFinalize to
      // take this object off the finalization queue
      // and prevent finalization code for this object
      // from executing a second time.
      GC.SuppressFinalize(this);
    }
    // Dispose(bool disposing) executes in two distinct scenarios.
    // If disposing equals true, the method has been called directly
    // or indirectly by a user's code. Managed and unmanaged resources
    // can be disposed.
    // If disposing equals false, the method has been called by the
    // runtime from inside the finalizer and you should not reference
    // other objects. Only unmanaged resources can be disposed.
    private void Dispose(bool disposing)
    {
      // Check to see if Dispose has already been called.
      if (!this.disposed)
      {
        // If disposing equals true, dispose all managed
        // and unmanaged resources.
        if (disposing && _DataContext != null)
        {
          // Dispose managed resources.
          _DataContext.Dispose();
        }
        // Call the appropriate methods to clean up unmanaged resources here.
        // If disposing is false, only the following code is executed.
        // Note disposing has been done.
        disposed = true;
      }
    }
    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// <see cref="DataContextManagement&lt;type&gt;"/> is reclaimed by garbage collection.
    /// </summary>
    /// <remarks>Use C# destructor syntax for finalization code. This destructor will run only if the Dispose method does not get called. It gives your base class the opportunity to finalize. 
    /// Do not provide destructors in types derived from this class.
    /// </remarks>
    ~DataContextManagement()
    {
      // Do not re-create Dispose clean-up code here.
      // Calling Dispose(false) is optimal in terms of
      // readability and maintainability.
      Dispose(false);
    }
    #endregion
  }
}
