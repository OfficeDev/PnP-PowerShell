using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Base
{
    public class PropertyLoadingAttribute : Attribute
    {
        public int Depth { get; set; } = 1;
    }
}
