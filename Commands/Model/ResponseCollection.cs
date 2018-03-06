using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SharePointPnP.PowerShell.Commands.Model
{
    public class ResponseCollection<T>
    {
        [JsonProperty("value")]
        public List<T> Items { get; set; }

        [JsonProperty("odata.nextLink")]
        public string NextLink { get; set; }
    }
}