using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Model
{
    public class GraphObject
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonExtensionData]
        public Dictionary<string,object> Values { get; set; }
    }
}
