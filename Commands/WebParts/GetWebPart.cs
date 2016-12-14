using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WebParts;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.WebParts
{
    [Cmdlet(VerbsCommon.Get, "PnPWebPart")]
    [CmdletAlias("Get-SPOWebPart")]
    [CmdletHelp("Returns a webpart definition object",
        Category = CmdletHelpCategory.WebParts,
        OutputType=typeof(IEnumerable<WebPartDefinition>),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.webparts.webpartdefinition.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPWebPart -ServerRelativePageUrl ""/sites/demo/sitepages/home.aspx""",
        Remarks = @"Returns all webparts defined on the given page.", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPWebPart -ServerRelativePageUrl ""/sites/demo/sitepages/home.aspx"" -Identity a2875399-d6ff-43a0-96da-be6ae5875f82",
        Remarks = @"Returns a specific webpart defined on the given page.", SortOrder = 2)]
    public class GetWebPart : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Full server relative URL of the webpart page, e.g. /sites/mysite/sitepages/home.aspx")]
        [Alias("PageUrl")]
        public string ServerRelativePageUrl = string.Empty;

        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        public WebPartPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var serverRelativeWebUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

            if (!ServerRelativePageUrl.ToLowerInvariant().StartsWith(serverRelativeWebUrl.ToLowerInvariant()))
            {
                ServerRelativePageUrl = UrlUtility.Combine(serverRelativeWebUrl, ServerRelativePageUrl);
            }


            var definitions = SelectedWeb.GetWebParts(ServerRelativePageUrl);

            if (Identity != null)
            {
                if (Identity.Id != Guid.Empty)
                {
                    var wpfound = from wp in definitions where wp.Id == Identity.Id select wp;
                    var webPartDefinitions = wpfound as WebPartDefinition[] ?? wpfound.ToArray();
                    if (webPartDefinitions.Any())
                    {
                        WriteObject(webPartDefinitions.FirstOrDefault());

                    }
                }
                else if (!string.IsNullOrEmpty(Identity.Title))
                {
                    var wpfound = from wp in definitions where wp.WebPart.Title == Identity.Title select wp;
                    var webPartDefinitions = wpfound as WebPartDefinition[] ?? wpfound.ToArray();
                    if (webPartDefinitions.Any())
                    {
                        WriteObject(webPartDefinitions.FirstOrDefault());
                    }
                }
            }
            else
            {
                WriteObject(definitions, true);
            }
        }
    }
}
