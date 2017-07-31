using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPWeb")]
    [CmdletHelp("Returns the current web object",
        Category = CmdletHelpCategory.Webs,
        OutputType = typeof(Web),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx")]
    public class GetWeb : PnPRetrievalsCmdlet<Web>
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0)]
        public WebPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            Web web = null;
            if (Identity == null)
            {
                WriteVerbose("Listing current web");
                web = ClientContext.Web;
            }
            else
            {
                if (Identity.Id != Guid.Empty)
                {
                    WriteVerbose($"Retrieving web through Id {Identity.Id}");
                    web = ClientContext.Site.OpenWebById(Identity.Id);
                }
                else if (Identity.Web != null)
                {
                    WriteVerbose("Received web instance");
                    web = Identity.Web;
                }
                else if (Identity.Url != null)
                {
                    WriteVerbose($"Retrieving web through Url {Identity.Url}");
                    web = ClientContext.Site.OpenWeb(Identity.Url);
                }
            }

            if(web == null)
            {
                throw new ArgumentException("Unable to define web to retrieve", "Identity");
            }
            ClientContext.Load(web);
            ClientContext.ExecuteQueryRetry();

            var webProperties = Utilities.PSObjectConverter.ConvertGenericObject(web, this);
            WriteObject(webProperties);
        }
    }
}
