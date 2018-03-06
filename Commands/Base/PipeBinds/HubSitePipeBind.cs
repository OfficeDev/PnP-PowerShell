#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using System;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class HubSitePipeBind
    {
        private readonly Guid _id;
        private readonly string _url;
        private readonly Microsoft.SharePoint.Client.Site _site;

        public HubSitePipeBind()
        {
            _id = Guid.Empty;
            _url = string.Empty;
            _site = null;
        }

        public HubSitePipeBind(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("Url");
            }
            Uri uri;
            try
            {
                uri = new Uri(url);
            }
            catch (UriFormatException)
            {
                throw new ArgumentException("Url");
            }
            _url = url;
        }

        public HubSitePipeBind(Microsoft.SharePoint.Client.Site site)
        {
            site.EnsureProperties(s => s.Url);
            _url = site.Url;
            _site = site;
        }

        public HubSitePipeBind(HubSiteProperties properties)
        {
            _url = properties.SiteUrl;
        }

        public string Url => _url;

        public Microsoft.SharePoint.Client.Site Site => _site;
        
    }
}
#endif