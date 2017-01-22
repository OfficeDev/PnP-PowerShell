using System;
using System.Linq;
using Microsoft.Online.SharePoint.TenantAdministration;
using SharePointPnP.PowerShell.Commands.Enums;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.Base
{
    // TO BE REMOVED IN APRIL 2017 RELEASE
    [Obsolete("Use PnPAdminCmdlet instead. This class will be removed in the April 2017 release.")]
    public abstract class SPOAdminCmdlet : PnPCmdlet
    {
        private Tenant _tenant;
        private Uri _baseUri;

        public Tenant Tenant
        {
            get
            {
                if (_tenant == null)
                {
                    _tenant = new Tenant(ClientContext);

                }
                return _tenant;
            }
        }

        public Uri BaseUri => _baseUri;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (SPOnlineConnection.CurrentConnection == null)
            {
                throw new InvalidOperationException(Resources.NoConnection);
            }
            if (ClientContext == null)
            {
                throw new InvalidOperationException(Resources.NoConnection);
            }

            SPOnlineConnection.CurrentConnection.CacheContext();

            if (SPOnlineConnection.CurrentConnection.TenantAdminUrl != null && SPOnlineConnection.CurrentConnection.ConnectionType == ConnectionType.O365)
            {
                var uri = new Uri(SPOnlineConnection.CurrentConnection.Url);
                var uriParts = uri.Host.Split('.');
                if (uriParts[0].ToLower().EndsWith("-admin"))
                {
                    _baseUri =
                        new Uri(
                            $"{uri.Scheme}://{uriParts[0].ToLower().Replace("-admin", "")}.{string.Join(".", uriParts.Skip(1))}{(!uri.IsDefaultPort ? ":" + uri.Port : "")}");
                }
                else
                {
                    _baseUri = new Uri($"{uri.Scheme}://{uri.Authority}");
                }
                SPOnlineConnection.CurrentConnection.CloneContext(SPOnlineConnection.CurrentConnection.TenantAdminUrl);
            }
            else
            {
                Uri uri = new Uri(ClientContext.Url);
                var uriParts = uri.Host.Split('.');
                if (!uriParts[0].EndsWith("-admin") &&
                    SPOnlineConnection.CurrentConnection.ConnectionType == ConnectionType.O365)
                {
                    _baseUri = new Uri($"{uri.Scheme}://{uri.Authority}");

                    var adminUrl = $"https://{uriParts[0]}-admin.{string.Join(".", uriParts.Skip(1))}";

                    SPOnlineConnection.CurrentConnection.Context =
                        SPOnlineConnection.CurrentConnection.CloneContext(adminUrl);
                }
                else if(SPOnlineConnection.CurrentConnection.ConnectionType == ConnectionType.TenantAdmin)
                {
                    _baseUri =
                       new Uri(
                           $"{uri.Scheme}://{uriParts[0].ToLower().Replace("-admin", "")}.{string.Join(".", uriParts.Skip(1))}{(!uri.IsDefaultPort ? ":" + uri.Port : "")}");

                }
            }
        }

        protected override void EndProcessing()
        {
            SPOnlineConnection.CurrentConnection.RestoreCachedContext(SPOnlineConnection.CurrentConnection.Url);
        }
    }
}
