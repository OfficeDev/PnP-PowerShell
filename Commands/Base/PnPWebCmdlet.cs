using System;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.Commands.Extensions;

namespace SharePointPnP.PowerShell.Commands
{
    public abstract class PnPWebCmdlet : PnPCmdlet
    {
        private Web _selectedWeb;


        [Parameter(Mandatory = false, HelpMessage = "The web to apply the command to. Omit this parameter to use the current web.")]
        public WebPipeBind Web = new WebPipeBind();

        protected Web SelectedWeb
        {
            get
            {
                if (_selectedWeb == null)
                {
                    _selectedWeb = GetWeb();
                }
                return _selectedWeb;
            }
        }

        private Web GetWeb()
        {
            Web web = ClientContext.Web;

            if (Web.Id != Guid.Empty)
            {
                web = web.GetWebById(Web.Id);
                SPOnlineConnection.CurrentConnection.CloneContext(web.Url);

                web = SPOnlineConnection.CurrentConnection.Context.Web;
            }
            else if (!string.IsNullOrEmpty(Web.Url))
            {
                web = web.GetWebByUrl(Web.Url);
                SPOnlineConnection.CurrentConnection.CloneContext(web.Url);
                web = SPOnlineConnection.CurrentConnection.Context.Web;
            }
            else if (Web.Web != null)
            {
                web = Web.Web;

                web.EnsureProperty(w => w.Url);

                SPOnlineConnection.CurrentConnection.CloneContext(web.Url);
                web = SPOnlineConnection.CurrentConnection.Context.Web;
            }
            else
            {
                if (SPOnlineConnection.CurrentConnection.Context.Url != SPOnlineConnection.CurrentConnection.Url)
                {
                    SPOnlineConnection.CurrentConnection.RestoreCachedContext(SPOnlineConnection.CurrentConnection.Url);
                }
                web = ClientContext.Web;
            }

            SPOnlineConnection.CurrentConnection.Context.ExecuteQueryRetry();

            return web;
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            if (SPOnlineConnection.CurrentConnection.Context.Url != SPOnlineConnection.CurrentConnection.Url)
            {
                SPOnlineConnection.CurrentConnection.RestoreCachedContext(SPOnlineConnection.CurrentConnection.Url);
            }
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            SPOnlineConnection.CurrentConnection.CacheContext();
        }

    }
}