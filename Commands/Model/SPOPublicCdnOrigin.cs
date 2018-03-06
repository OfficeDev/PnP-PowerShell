#if !ONPREMISES
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Model
{
    public class SPOPublicCdnOrigin
    {
        public string Id { get; internal set; }

        public string Url { get; internal set; }

        public SPOPublicCdnOrigin(string id, string url)
        {
            Id = id;
            Url = url;
        }
    }
}
#endif