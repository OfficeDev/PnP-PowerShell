using System.Management.Automation;
using Microsoft.SharePoint.Client;
using web = Microsoft.SharePoint.Client.Web;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Collections.Generic;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPSubWebs")]
    [CmdletHelp("Returns the subsites of a specific web", 
        Category = CmdletHelpCategory.Webs,
        DetailedDescription = "This command allows returning all subsites located under the current web",
        OutputType = typeof(List<web>),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx")]
    [CmdletExample(Code = @"PS:> Get-PnPSubWebs",
                Remarks = @"Returns all the subsites directly located under the current web",
                SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Get-PnPSubWebs -Recurse",
                Remarks = @"Returns all the subsites directly located under the current web and all the subsites underneath those recursively",
                SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Get-PnPSubWebs -Identity Project1 -Recurse",
                Remarks = @"Returns all the subsites located under the Project1 subsite and all the subsites underneath it recursively",
                SortOrder = 3)]
    public class GetSubWebs : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "Relative Url, Id or instance of a pecific web to list the subsites under")]
        public WebPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "If provided, all subsites underneath the subsites will be added recursively")]
        public SwitchParameter Recurse;

        protected override void ExecuteCmdlet()
        {
            Web web = null;
            if(Identity != null)
            {
                if(Identity.Id != System.Guid.Empty)
                {
                    WriteVerbose($"Retrieving web by Id {Identity.Id}");
                    web = ClientContext.Site.OpenWebById(Identity.Id);
                }
                else if(!string.IsNullOrEmpty(Identity.Url))
                {
                    WriteVerbose($"Retrieving web by Url {Identity.Url}");
                    web = ClientContext.Site.OpenWeb(Identity.Url);
                }
                else if(Identity.Web != null)
                {
                    WriteVerbose("Using web instance passed along");
                    web = Identity.Web;
                }
            }
            else
            {
                WriteVerbose("Using web from current context");
                web = SelectedWeb;
            }

            if(web == null)
            {
                throw new PSArgumentException("Unable to define web from Identity", "Identity");
            }

            var webs = SelectedWeb.Context.LoadQuery(web.Webs);
            SelectedWeb.Context.ExecuteQueryRetry();
            if (!Recurse)
            {
                var subwebProperties = Utilities.PSObjectConverter.ConvertGenericObjects(webs, this);
                WriteObject(subwebProperties, true);
            }
            else
            {
                var subwebs = new List<web>();
                subwebs.AddRange(webs);
                foreach (var subweb in webs)
                {
                    subwebs.AddRange(GetSubWebsInternal(subweb));
                }
                var subwebProperties = Utilities.PSObjectConverter.ConvertGenericObjects(subwebs, this);
                WriteObject(subwebProperties, true);
            }
        }

        private List<web> GetSubWebsInternal(web subweb)
        {
            var subwebs = new List<web>();
            var webs = subweb.Context.LoadQuery(subweb.Webs);
            subweb.Context.ExecuteQueryRetry();
            subwebs.AddRange(webs);
            foreach (var sw in webs)
            {
                subwebs.AddRange(GetSubWebsInternal(sw));
            }
            return subwebs;
        }
    }
}
