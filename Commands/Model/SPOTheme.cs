#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantManagement;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PnP.PowerShell.Commands.Model
{
    public class SPOTheme
    {
        internal SPOTheme(ThemeProperties themeProps)
        {
            if (themeProps != null)
            {
                this.Name = themeProps.Name;
                this.Palette = themeProps.Palette;
                this.IsInverted = themeProps.IsInverted;
            }
        }

        internal SPOTheme(string name, IDictionary<string, string> palette, bool isInverted)
        {
            this.Name = name;
            this.Palette = palette;
            this.IsInverted = isInverted;
        }

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonPropertyName("palette")]
        public IDictionary<string, string> Palette { get; private set; }

        [JsonPropertyName("isInverted")]
        public bool IsInverted { get; private set; }
    }
}
#endif