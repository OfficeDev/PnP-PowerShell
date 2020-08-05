using System.Text.Json.Serialization;

namespace PnP.PowerShell.Commands.Model
{
    /// <summary>
    /// Information regarding the current status of a specific feature in an Office 365 service
    /// </summary>
    public class ManagementApiFeatureStatus
    {
        [JsonPropertyName("FeatureDisplayName")]
        public string FeatureDisplayName { get; set; }

        [JsonPropertyName("FeatureName")]
        public string FeatureName { get; set; }

        [JsonPropertyName("FeatureServiceStatusDisplayName")]
        public string FeatureServiceStatusDisplayName { get; set; }

        [JsonPropertyName("FeatureServiceStatus")]
        public string FeatureServiceStatus { get; set; }
    }
}
