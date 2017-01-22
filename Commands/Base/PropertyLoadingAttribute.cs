using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Base
{
    public class PropertyLoadingAttribute : Attribute
    {
        public int Depth { get; set; } = 1;
    }
}
