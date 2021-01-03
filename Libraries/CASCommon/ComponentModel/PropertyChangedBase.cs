//<summary>
//  Title   : class PropertyChangedBase
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-12-10 16:16:26 +0100 (śr., 10 gru 2014) $
//  $Rev: 11083 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/CASCommon/ComponentModel/PropertyChangedBase.cs $
//  $Id: PropertyChangedBase.cs 11083 2014-12-10 15:16:26Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.ComponentModel;

namespace CAS.Common.ComponentModel
{
  /// <summary>
  /// Provides basic implementation for the <see cref="INotifyPropertyChanged"/>
  /// </summary>
  public class PropertyChangedBase : INotifyPropertyChanged
  {

    #region INotifyPropertyChanged Members
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="PropertyChangedBase"/> is modified.
    /// </summary>
    /// <value>
    ///   <c>true</c> if modified; otherwise, <c>false</c>.
    /// </value>
    public bool Modified
    {
      get
      {
        return b_Modified;
      }
      set
      {
        RaiseHandler<bool>(value, ref b_Modified, "Modified", this);
      }
    }
    #endregion

    #region private
    /// <summary>
    /// Called when the interface is modified.
    /// </summary>
    protected virtual void OnModified() { }
    /// <summary>
    /// The method sets a new value in a variable and then executes the event handler if the new value
    /// differs from the old one. Used to easily implement INotifyPropeprtyChanged.
    /// </summary>
    /// <typeparam name="T">The type of values being handled (usually the type of the property).</typeparam>
    /// <param name="newValue">The new value to set.</param>
    /// <param name="oldValue">The old value to replace (and the value holder).</param>
    /// <param name="propertyName">The property's name as required by <see cref="System.ComponentModel.PropertyChangedEventArgs"/>.</param>
    /// <param name="sender">The object to be appointed as the executioner of the handler.</param>
    /// <returns>A boolean value that indicates if the new value was truly different from the old value according to <code>object.Equals()</code>.</returns>
    protected virtual bool RaiseHandler<T>(T newValue, ref T oldValue, string propertyName, object sender)
    {
      bool changed = !Object.Equals(oldValue, newValue);
      if (changed)
      {
        //Save the new value. 
        oldValue = newValue;
        PropertyChangedEventHandler handler = this.PropertyChanged;
        //Raise the event 
        if (propertyName != "Modified")
          Modified = true;
        else
        {
          b_Modified = true;
          if (handler != null)
            handler(sender, new PropertyChangedEventArgs("Modified"));
        }
        if (!m_InModified)
        {
          m_InModified = true;
          OnModified();
          m_InModified = false;
        }
        if (handler != null)
          handler(sender, new PropertyChangedEventArgs(propertyName));
      }
      //Signal what happened. 
      return changed;
    }
    private bool b_Modified = false;
    private bool m_InModified = false;
    #endregion

  }
}
