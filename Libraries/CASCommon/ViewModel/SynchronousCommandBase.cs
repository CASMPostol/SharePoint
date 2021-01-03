//<summary>
//  Title   : class DelegateCommandBase
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate: 2014-10-13 15:52:44 +0200 (pon., 13 paź 2014) $
//  $Rev: 10857 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/tags/CAS.SharePoint.rel_2_61_7/PR44-SharePoint/Libraries/CASCommon/ViewModel/SynchronousCommandBase.cs $
//  $Id: SynchronousCommandBase.cs 10857 2014-10-13 13:52:44Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.Properties;
using System;
using System.Windows.Input;

namespace CAS.Common.ViewModel
{
  /// <summary>
  /// An <see cref="ICommand" /> whose delegates can be attached for <see cref="Execute" /> and <see cref="CanExecute" />.
  /// </summary>
  /// <typeparam name="T">Parameter type.</typeparam>
  /// <remarks>
  /// The constructor deliberately prevents the use of value types.
  /// Because ICommand takes an object, having a value type for T would cause unexpected behavior when CanExecute(null) is called during XAML initialization for command bindings.
  /// Using default(T) was considered and rejected as a solution because the implementor would not be able to distinguish between a valid and defaulted values.
  /// <para />
  /// Instead, callers should support a value type by using a nullable value type and checking the HasValue property before using the Value property.
  /// <example><code>
  /// public MyClass()
  /// {
  /// this.submitCommand = new DelegateCommand&lt;int?&gt;(this.Submit, this.CanSubmit);
  /// }
  /// private bool CanSubmit(int? customerId)
  /// {
  /// return (customerId.HasValue &amp;&amp; customers.Contains(customerId.Value));
  /// }
  /// </code></example>
  /// </remarks>
  public class SynchronousCommandBase<T> : ICommandWithUpdate
  {

    #region public
    /// <summary>
    /// Creates a new instance of a <see cref="SynchronousCommandBase{T}" />, specifying the execute action.
    /// </summary>
    /// <param name="executeMethod">The <see cref="Action{T}" /> to execute when <see cref="ICommand.Execute" /> is invoked.</param>
    /// <exception cref="System.ArgumentNullException">executeMethod</exception>
    public SynchronousCommandBase(Action<T> executeMethod)
      : this(executeMethod, (o) => true)
    { }
    /// <summary>
    /// Creates a new instance of a <see cref="SynchronousCommandBase{T}" />, specifying both the Execute action and the CanExecute function.
    /// </summary>
    /// <param name="executeMethod">The <see cref="Action{T}" /> to execute when <see cref="ICommand.Execute" /> is invoked.</param>
    /// <param name="canExecuteMethod">The <see cref="Func{T,Bool}" /> to invoked when <see cref="ICommand.CanExecute" /> is invoked.</param>
    /// <exception cref="System.ArgumentNullException">executeMethod</exception>
    public SynchronousCommandBase(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
    {
      if (executeMethod == null || canExecuteMethod == null)
        throw new ArgumentNullException("executeMethod", Resources.DelegateCommandDelegatesCannotBeNull);
      //type checking
      Type genericTypeInfo = typeof(T);
      // DelegateCommand allows object or Nullable<>.  
      // note: Nullable<> is a struct so we cannot use a class constraint.
      if (genericTypeInfo.IsValueType)
      {
        if ((!genericTypeInfo.IsGenericType) || (!typeof(Nullable<>).IsAssignableFrom(genericTypeInfo.GetGenericTypeDefinition())))
        {
          throw new InvalidCastException(Resources.DelegateCommandInvalidGenericPayloadType);
        }
      }
      _executeMethod = executeMethod;
      _canExecuteMethod = canExecuteMethod;
    }
    /// <summary>
    /// Raises <see cref="SynchronousCommandBase{T}.CanExecuteChanged" /> on the UI thread so every command invoker can requery to check if the command can execute.
    /// <remarks>Note that this will trigger the execution of <see cref="SynchronousCommandBase{T}.CanExecute" /> once for each invoker.</remarks>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
    public void RaiseCanExecuteChanged()
    {
      OnCanExecuteChanged();
    }
    #endregion

    #region ICommand
    /// <summary>
    /// Defines the method to be called when the command is invoked.
    /// </summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
    void ICommand.Execute(object parameter)
    {
      Execute((T)parameter);
    }
    /// <summary>
    /// Defines the method that determines whether the command can execute in its current state.
    /// </summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
    /// <returns>
    /// true if this command can be executed; otherwise, false.
    /// </returns>
    bool ICommand.CanExecute(object parameter)
    {
      return CanExecute((T)parameter);
    }
    /// <summary>
    /// Determines if the command can execute with the provided parameter by invoking the <see cref="Func{Object,Bool}"/> supplied during construction.
    /// </summary>
    /// <param name="parameter">The parameter to use when determining if this command can execute.</param>
    /// <returns>Returns <see langword="true"/> if the command can execute.  <see langword="False"/> otherwise.</returns>
    public bool CanExecute(T parameter)
    {
      return _canExecuteMethod == null || _canExecuteMethod(parameter);
    }
    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute.
    /// </summary>
    public event EventHandler CanExecuteChanged;
    /// <summary>
    /// Executes the command with the provided parameter by invoking the <see cref="Action{T}"/> supplied during construction.
    /// </summary>
    /// <param name="parameter"></param>
    public void Execute(T parameter)
    {
      _executeMethod(parameter);
    }
    #endregion

    #region private
    private readonly Action<T> _executeMethod;
    private readonly Func<T, bool> _canExecuteMethod;
    /// <summary>
    /// Raises <see cref="ICommand.CanExecuteChanged"/> on the UI thread so every command invoker can requery <see cref="ICommand.CanExecute"/>.
    /// </summary>
    protected virtual void OnCanExecuteChanged()
    {
      EventHandler _CanExecuteChanged = CanExecuteChanged;
      if (_CanExecuteChanged != null)
        _CanExecuteChanged(this, EventArgs.Empty);
    }
    #endregion

  }
}
