#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using System;
using Resources = PnP.PowerShell.Commands.Properties.Resources;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class HubSitePipeBind
    {
        /// <summary>
        /// Unique identifier of the hub site
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Url of the hub site
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Site collection instance representing the hub site
        /// </summary>
        public Microsoft.SharePoint.Client.Site Site { get; }

        /// <summary>
        /// HubSiteProperties of the hub site
        /// </summary>
        private HubSiteProperties _hubSiteProperties;

        public HubSitePipeBind()
        {
            Id = Guid.Empty;
            Url = string.Empty;
            Site = null;
        }

        /// <summary>
        /// Creates a new HubSitePipeBind based on the Url of a hub site or dhe Id of a hub site
        /// </summary>
        /// <param name="identity">Url or Id of a hub site</param>
        public HubSitePipeBind(string identity)
        {
            if (string.IsNullOrEmpty(identity))
            {
                throw new ArgumentException(nameof(Url));
            }
            if (Guid.TryParse(identity, out Guid hubSiteId))
            {
                Id = hubSiteId;
            }
            else
            {
                Uri uri;
                try
                {
                    uri = new Uri(identity);
                }
                catch (UriFormatException)
                {
                    throw new ArgumentException(nameof(Url));
                }
                Url = identity;
            }
        }

        public HubSitePipeBind(Microsoft.SharePoint.Client.Site site)
        {
            site.EnsureProperties(s => s.Url, s => s.Id);
            Id = site.Id;
            Url = site.Url;
            Site = site;
        }

        public HubSitePipeBind(HubSiteProperties properties)
        {
            Id = properties.ID;
            Url = properties.SiteUrl;
            _hubSiteProperties = properties;
        }

        /// <summary>
        /// Gets the HubSiteProperties of the Hub site in this pipebind
        /// </summary>
        /// <param name="tenant">Tenant instance to use to retrieve the HubSiteProperties of the Hub in this pipe bind</param>
        /// <exception cref="Exception">Thrown when the HubSiteProperties cannot be retrieved</exception>
        /// <returns>HubSiteProperties of the Hub site in this pipebind</returns>
        public HubSiteProperties GetHubSite(Tenant tenant)
        {
            if(_hubSiteProperties != null)
            {
                return _hubSiteProperties;
            }
            else if(Id != Guid.Empty)
            {
                _hubSiteProperties = tenant.GetHubSitePropertiesById(Id);
                return _hubSiteProperties;
            }
            else if(Url != null)
            {
                _hubSiteProperties = tenant.GetHubSitePropertiesByUrl(Url);
                return _hubSiteProperties;
            }
            throw new Exception(Resources.SiteNotFound);
        }
    }
}
#endif