using System.Text.Json.Serialization;

namespace PnP.PowerShell.Commands.Utilities.REST
{
    public class MetadataType
    {
        [JsonPropertyName("type")]
        public string _typename { get; set; }

        public MetadataType(string typename)
        {
            this._typename = typename;
        }
    }
}