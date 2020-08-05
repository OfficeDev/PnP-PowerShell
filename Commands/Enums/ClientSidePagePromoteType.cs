#if !SP2013 && !SP2016
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.ClientSidePages
{
    public enum ClientSidePagePromoteType
    {
        None = 0,
        HomePage = 1,
        NewsArticle = 2,
        Template = 3
    }
}
#endif
