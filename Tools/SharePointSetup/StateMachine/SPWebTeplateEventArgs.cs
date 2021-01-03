using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CAS.SharePoint.Setup.Package;
using Microsoft.SharePoint;

namespace CAS.SharePoint.Setup.StateMachine
{
  internal class SPWebTeplateEventArgs: ProgressChangedEventArgs
  {
    public SPWebTeplateEventArgs( SPWebTemplateCollection collection, Action onSelected,  int progressPercentage, string userState ) :
      base( progressPercentage, userState )
    {
      WebTemplates = collection;
      SPWebTeplateSelected = onSelected;
    }
    internal SPWebTemplateCollection WebTemplates{ get; private set; }
    internal Action SPWebTeplateSelected { get; private set; }
  }
}
