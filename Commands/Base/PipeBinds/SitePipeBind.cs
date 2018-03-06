using Microsoft.SharePoint.Client;
using System;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class SitePipeBind
    {
        private readonly Guid _id;
        private readonly string _url;
        private readonly Microsoft.SharePoint.Client.Site _site;

        public SitePipeBind()
        {
            _id = Guid.Empty;
            _url = string.Empty;
            _site = null;
        }

        public SitePipeBind(string url)
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

        public SitePipeBind(Microsoft.SharePoint.Client.Site site)
        {
            site.EnsureProperties(s => s.Url);
            _url = site.Url;
            _site = site;
        }

        public string Url => _url;

        public Microsoft.SharePoint.Client.Site Site => _site;
        
    }
}
