using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PnP.PowerShell.Commands.Model
{
    /// <summary>
    /// Information regarding one Office 365 service
    /// </summary>
    public class ManagementApiService
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("DisplayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("Features")]
        public IEnumerable<ManagementApiFeature> Features { get; set; }
    }
}
