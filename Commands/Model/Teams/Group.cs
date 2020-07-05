using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Model.Teams
{
    public class Group
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string MailNickname { get; set; }
        public string Description { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public GroupVisibility Visibility { get; set; }

        [JsonProperty("owners@odata.bind")]
        public List<string> Owners { get; set; }

        [JsonProperty("members@odata.bind")]
        public List<string> Members { get; set; }

        public string Classification { get; set; }

        public bool MailEnabled { get; set; }

        public List<string> GroupTypes { get; set; }

        public bool? SecurityEnabled { get; set; }
    }
}
