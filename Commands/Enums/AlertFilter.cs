using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Enums
{
    public enum AlertFilter
    {
        AnythingChanges = 0,
        SomeoneElseChangesAnItem = 1,
        SomeoneElseChangesItemCreatedByMe = 2,
        SomeoneElseChangesItemLastModifiedByMe = 3
    }
}
