using System;
using Microsoft.SharePoint.Client;

namespace OfficeDevPnP.PowerShell.Commands
{
    public static class WebExtensions
    {
        public static Web GetWebById(this Web currentWeb, Guid guid)
        {
            var clientContext = currentWeb.Context as ClientContext;
            Site site = clientContext.Site;
            Web web = site.OpenWebById(guid);
            web.EnsureProperties(w => w.Url, w => w.Title, w => w.Id, w => w.ServerRelativeUrl);
            return web;
        }

        public static Web GetWebByUrl(this Web currentWeb, string url)
        {
            var clientContext = currentWeb.Context as ClientContext;
            Site site = clientContext.Site;
            Web web = site.OpenWeb(url);
            web.EnsureProperties(w => w.Url, w => w.Title, w => w.Id, w => w.ServerRelativeUrl);
            return web;
        }


    }
}
