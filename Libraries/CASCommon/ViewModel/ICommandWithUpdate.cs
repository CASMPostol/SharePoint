using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CAS.Common.ViewModel
{
  /// <summary>
  /// Defines a command that support update request of the <see cref="ICommand.CanExecute" />
  /// </summary>
  public interface ICommandWithUpdate : ICommand
  {
    /// <summary>
    /// Raises <see cref="ICommand.CanExecuteChanged" /> on the UI thread so every command invoker can requery to check if the command can execute.
    /// <remarks>Note that this will trigger the execution of <see cref="ICommand.CanExecute" /> once for each invoker.</remarks>
    /// </summary>
    void RaiseCanExecuteChanged();
  }
}
