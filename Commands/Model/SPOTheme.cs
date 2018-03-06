#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantManagement;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SharePointPnP.PowerShell.Commands.Model
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

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("palette")]
        public IDictionary<string, string> Palette { get; private set; }

        [JsonProperty("isInverted")]
        public bool IsInverted { get; private set; }
    }
}
#endif