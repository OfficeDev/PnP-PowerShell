using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;

namespace SharePointPnP.PowerShell.Commands.Extensions
{
    public static class WebExtensions
    {
        public static Web GetWebById(this Web currentWeb, Guid guid)
        {
            var clientContext = currentWeb.Context as ClientContext;
            var site = clientContext.Site;
            var web = site.OpenWebById(guid);
            web.EnsureProperties(w => w.Url, w => w.Title, w => w.Id, w => w.ServerRelativeUrl);
            return web;
        }

        public static Web GetWebByUrl(this Web currentWeb, string url)
        {
            var clientContext = currentWeb.Context as ClientContext;

            var site = clientContext.Site;
            var web = site.OpenWeb(url);
            web.EnsureProperties(w => w.Url, w => w.Title, w => w.Id, w => w.ServerRelativeUrl);
            return web;
        }

        public static IEnumerable<Web> GetAllWebsRecursive(this Web currentWeb)
        {
            currentWeb.Context.Load(currentWeb, item => item.Webs);
            currentWeb.Context.ExecuteQuery();

            foreach (var subWeb in currentWeb.Webs)
            {
                foreach (var subSubWeb in subWeb.GetAllWebsRecursive())
                {
                    yield return subSubWeb;
                }

                yield return subWeb;
            }
        }
    }
}
