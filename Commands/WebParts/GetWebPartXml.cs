using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Text;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.WebParts
{
    [Cmdlet(VerbsCommon.Get, "PnPWebPartXml")]
    [CmdletAlias("Get-SPOWebPartXml")]
    [CmdletHelp("Returns the webpart XML of a webpart registered on a site",
        Category = CmdletHelpCategory.WebParts,
        OutputType = typeof(string))]
    [CmdletExample(
        Code = @"PS:> Get-PnPWebPartXml -ServerRelativePageUrl ""/sites/demo/sitepages/home.aspx"" -Identity a2875399-d6ff-43a0-96da-be6ae5875f82",
        Remarks = @"Returns the webpart XML for a given webpart on a page.", SortOrder = 1)]
    public class GetWebPartXml : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Full server relative url of the webpart page, e.g. /sites/mysite/sitepages/home.aspx")]
        [Alias("PageUrl")]
        public string ServerRelativePageUrl = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "Id or title of the webpart. Use Get-PnPWebPart to retrieve all webpart Ids")]
        public WebPartPipeBind Identity;

        protected override void ExecuteCmdlet()
        {
            var serverRelativeWebUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

            if (!ServerRelativePageUrl.ToLowerInvariant().StartsWith(serverRelativeWebUrl.ToLowerInvariant()))
            {
                ServerRelativePageUrl = UrlUtility.Combine(serverRelativeWebUrl, ServerRelativePageUrl);
            }

            Guid id;
            if (Identity.Id == Guid.Empty)
            {
                var wp = SelectedWeb.GetWebParts(ServerRelativePageUrl).FirstOrDefault(wps => wps.WebPart.Title == Identity.Title);
                if (wp != null)
                {
                    id = wp.Id;
                }
                else
                {
                    throw new Exception($"Web Part with title '{Identity.Title}' cannot be found on page with URL {ServerRelativePageUrl}");
                }
            }
            else
            {
                id = Identity.Id;
            }


            WriteObject(SelectedWeb.GetWebPartXml(id,ServerRelativePageUrl));

            
        }

    }
}
