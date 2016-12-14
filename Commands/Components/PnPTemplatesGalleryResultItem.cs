using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SharePointPnP.PowerShell.Commands.Enums;

namespace SharePointPnP.PowerShell.Commands.Components
{
    /// <summary>
    /// Class that represents an item of the result from the PnPTemplatesGallery search
    /// </summary>
    public class PnPTemplatesGalleryResultItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Abstract { get; set; }

        public string ImageUrl { get; set; }

        public string TemplatePnPUrl { get; set; }

        public string SEO { get; set; }

        public string BaseTemplate { get; set; }

        public double Rating { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TargetScope Scopes { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TargetPlatform Platforms { get; set; }
    }
}
