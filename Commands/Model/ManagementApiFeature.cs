
using System.Text.Json.Serialization;

namespace PnP.PowerShell.Commands.Model
{
    /// <summary>
    /// Information regarding one Office 365 service feature
    /// </summary>
    public class ManagementApiFeature
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("DisplayName")]
        public string DisplayName { get; set; }
    }
}
