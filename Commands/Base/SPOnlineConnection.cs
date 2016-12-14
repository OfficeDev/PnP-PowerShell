using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.Commands.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Base
{
    public class SPOnlineConnection
    {
        internal string PnPVersionTag { get; set; }
        internal static List<ClientContext> ContextCache { get; set; }
        public static SPOnlineConnection CurrentConnection { get; internal set; }
        public ConnectionType ConnectionType { get; protected set; }
        public int MinimalHealthScore { get; protected set; }
        public int RetryCount { get; protected set; }
        public int RetryWait { get; protected set; }
        public PSCredential PSCredential { get; protected set; }

        public string Url { get; protected set; }

        public string TenantAdminUrl { get; protected set; }

        public ClientContext Context { get; set; }

        public SPOnlineConnection(ClientContext context, ConnectionType connectionType, int minimalHealthScore, int retryCount, int retryWait, PSCredential credential, string url, string tenantAdminUrl, string pnpVersionTag)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            Context = context;
            ConnectionType = connectionType;
            MinimalHealthScore = minimalHealthScore;
            RetryCount = retryCount;
            RetryWait = retryWait;
            PSCredential = credential;
            TenantAdminUrl = tenantAdminUrl;
            ContextCache = new List<ClientContext>();
            ContextCache.Add(context);
            PnPVersionTag = pnpVersionTag;
            Url = (new Uri(url)).AbsoluteUri;
        }

        public void RestoreCachedContext(string url)
        {
            Context = ContextCache.FirstOrDefault(c => c.Url == url);
        }

        internal void CacheContext()
        {
            var c = ContextCache.FirstOrDefault(cc => cc.Url == Context.Url);
            if (c == null)
            {
                ContextCache.Add(Context);
            }
        }

        public ClientContext CloneContext(string url)
        {
            var context = ContextCache.FirstOrDefault(c => c.Url == url);
            if (context == null)
            {
                context = Context.Clone(url);
                ContextCache.Add(context);
            }
            Context = context;
            return context;
        }

        internal static ClientContext GetCachedContext(string url)
        {
            return ContextCache.FirstOrDefault(c => c.Url == url);
        }

        internal static void ClearContextCache()
        {
            ContextCache.Clear();
        }

    }
}
