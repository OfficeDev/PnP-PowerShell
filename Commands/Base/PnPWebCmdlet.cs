using System;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.Commands.Extensions;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands
{
    public abstract class PnPWebCmdlet : PnPSharePointCmdlet
    {
        private Web _selectedWeb;

        [Parameter(Mandatory = false, HelpMessage = "This parameter allows you to optionally apply the cmdlet action to a subweb within the current web. In most situations this parameter is not required and you can connect to the subweb using Connect-PnPOnline instead. Specify the GUID, server relative url (i.e. /sites/team1) or web instance of the web to apply the command to. Omit this parameter to use the current web.")]
        [PnPParameter(Order = 99)]
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
                PnPConnection.CurrentConnection.CloneContext(web.Url);

                web = PnPConnection.CurrentConnection.Context.Web;
            }
            else if (!string.IsNullOrEmpty(Web.Url))
            {
                web = web.GetWebByUrl(Web.Url);
                PnPConnection.CurrentConnection.CloneContext(web.Url);
                web = PnPConnection.CurrentConnection.Context.Web;
            }
            else if (Web.Web != null)
            {
                web = Web.Web;

                web.EnsureProperty(w => w.Url);

                PnPConnection.CurrentConnection.CloneContext(web.Url);
                web = PnPConnection.CurrentConnection.Context.Web;
            }
            else
            {
                if (PnPConnection.CurrentConnection.Context.Url != PnPConnection.CurrentConnection.Url)
                {
                    PnPConnection.CurrentConnection.RestoreCachedContext(PnPConnection.CurrentConnection.Url);
                }
                web = ClientContext.Web;
            }

            PnPConnection.CurrentConnection.Context.ExecuteQueryRetry();

            return web;
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            if (PnPConnection.CurrentConnection.Context.Url != PnPConnection.CurrentConnection.Url)
            {
                PnPConnection.CurrentConnection.RestoreCachedContext(PnPConnection.CurrentConnection.Url);
            }
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            PnPConnection.CurrentConnection.CacheContext();
        }
    }
}