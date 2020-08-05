using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.SharePoint.Client;

namespace PnP.PowerShell.Commands.Extensions
{
    public static class WebExtensions
    {
        public static Web GetWebById(this Web currentWeb, Guid guid, Expression<Func<Web, object>>[] expressions = null)
        {
            var clientContext = currentWeb.Context as ClientContext;
            var site = clientContext.Site;
            var web = site.OpenWebById(guid);
            if (expressions != null)
            {
                web.EnsureProperties(expressions);
            }
            else
            {
                web.EnsureProperties(w => w.Url, w => w.Title, w => w.Id, w => w.ServerRelativeUrl);
            }
            return web;
        }

        public static Web GetWebByUrl(this Web currentWeb, string url, Expression<Func<Web, object>>[] expressions = null)
        {
            var clientContext = currentWeb.Context as ClientContext;

            var site = clientContext.Site;
            var web = site.OpenWeb(url);
            if (expressions != null)
            {
                web.EnsureProperties(expressions);
            }
            else
            {
                web.EnsureProperties(w => w.Url, w => w.Title, w => w.Id, w => w.ServerRelativeUrl);
            }
            return web;
        }

        public static IEnumerable<Web> GetAllWebsRecursive(this Web currentWeb, Expression<Func<Web, object>>[] expressions = null)
        {
            List<Expression<Func<Web, object>>> exps = new List<Expression<Func<Web, object>>>();
            if (expressions != null) exps.AddRange(expressions);

            exps.Add(item => item.Webs);

            currentWeb.Context.Load(currentWeb, exps.ToArray());
            currentWeb.Context.ExecuteQueryRetry();

            foreach (var subWeb in currentWeb.Webs)
            {
                foreach (var subSubWeb in subWeb.GetAllWebsRecursive(expressions))
                {
                    yield return subSubWeb;
                }

                yield return subWeb;
            }
        }
    }
}
