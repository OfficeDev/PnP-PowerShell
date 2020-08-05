using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Model.Teams
{
    public class Group
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string MailNickname { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GroupVisibility Visibility { get; set; }

        [JsonPropertyName("owners@odata.bind")]
        public List<string> Owners { get; set; }

        [JsonPropertyName("members@odata.bind")]
        public List<string> Members { get; set; }

        public string Classification { get; set; }

        public bool MailEnabled { get; set; }

        public List<string> GroupTypes { get; set; }

        public bool? SecurityEnabled { get; set; }
    }
}
