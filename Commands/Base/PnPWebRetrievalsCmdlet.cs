using System;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Extensions;

namespace PnP.PowerShell.Commands
{
    /// <summary>
    /// Inherit from this base class if the PowerShell commandlet should allow switching the webcontext to a subsite of the current context for the duration of the execution of the command by specifying the -Web argument
    /// </summary>
    /// <typeparam name="TType">Type of object which will be returned in the output</typeparam>
    [CmdletAdditionalParameter(ParameterType = typeof(string[]), ParameterName = "Includes", HelpMessage = "Specify properties to include when retrieving objects from the server.")]
    public abstract class PnPWebRetrievalsCmdlet<TType> : PnPRetrievalsCmdlet<TType> where TType : ClientObject
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