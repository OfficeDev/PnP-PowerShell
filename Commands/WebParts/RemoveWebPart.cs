using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WebParts;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.WebParts
{
    [Cmdlet(VerbsCommon.Remove, "PnPWebPart")]
    [CmdletAlias("Remove-SPOWebPart")]
    [CmdletHelp("Removes a webpart from a page",
        Category = CmdletHelpCategory.WebParts)]
    public class RemoveWebPart : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "ID")]
        public GuidPipeBind Identity;

        [Parameter(Mandatory = true, ParameterSetName = "NAME")]
        [Alias("Name")]
        public string Title = string.Empty;

        [Parameter(Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets)]
        [Alias("PageUrl")]
        public string ServerRelativePageUrl = string.Empty;

        protected override void ExecuteCmdlet()
        {
            var serverRelativeWebUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

            if (!ServerRelativePageUrl.ToLowerInvariant().StartsWith(serverRelativeWebUrl.ToLowerInvariant()))
            {
                ServerRelativePageUrl = UrlUtility.Combine(serverRelativeWebUrl, ServerRelativePageUrl);
            }


            if (ParameterSetName == "NAME")
            {
                SelectedWeb.DeleteWebPart(ServerRelativePageUrl, Title);
            }
            else
            {
                var wps = SelectedWeb.GetWebParts(ServerRelativePageUrl);
                var wp = from w in wps where w.Id == Identity.Id select w;
                var webPartDefinitions = wp as WebPartDefinition[] ?? wp.ToArray();
                if(webPartDefinitions.Any())
                {
                    webPartDefinitions.FirstOrDefault().DeleteWebPart();
                    ClientContext.ExecuteQueryRetry();
                }
            }
        }
    }
}
