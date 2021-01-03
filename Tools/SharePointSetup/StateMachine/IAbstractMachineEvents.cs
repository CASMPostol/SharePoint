using System;
using System.IO;
using System.IO.Packaging;
using CAS.SharePoint.Setup.Package;

namespace CAS.SharePoint.Setup.StateMachine
{
  interface IAbstractMachineEvents
  {
    /// <summary>
    /// Cancels this instance.
    /// </summary>
    void Cancel();
    /// <summary>
    /// Exceptions the specified exception.
    /// </summary>
    /// <param name="exception">The exception.</param>
    void Exception( Exception exception );
    /// <summary>
    /// New package properties.
    /// </summary>
    /// <param name="packageProperties">The package properties.</param>
    void NewPackageProperties( InstalationPackageProperties packageProperties );
    /// <summary>
    /// Go to the next step.
    /// </summary>
    void Next();
    /// <summary>
    /// Go to previous step.
    /// </summary>
    void Previous();
    /// <summary>
    /// Uninstalls the application.
    /// </summary>
    void Uninstall();
  }
}
