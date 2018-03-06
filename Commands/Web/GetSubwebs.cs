using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using SharePointPnP.PowerShell.Commands.Extensions;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPSubWebs")]
    [CmdletHelp("Returns the subwebs of the current web", 
        Category = CmdletHelpCategory.Webs,
        OutputType = typeof(Web),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPSubWebs",
        Remarks = "Retrieves all subsites of the current context returning the Id, Url, Title and ServerRelativeUrl of each subsite in the output",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSubWebs -Recurse",
        Remarks = "Retrieves all subsites of the current context and all of their nested child subsites returning the Id, Url, Title and ServerRelativeUrl of each subsite in the output",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSubWebs -Recurse -Includes ""WebTemplate"",""Description"" | Select ServerRelativeUrl, WebTemplate, Description",
        Remarks = "Retrieves all subsites of the current context and shows the ServerRelativeUrl, WebTemplate and Description properties in the resulting output",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSubWebs -Identity Team1 -Recurse",
        Remarks = "Retrieves all subsites of the subsite Team1 and all of its nested child subsites returning the Id, Url, Title and ServerRelativeUrl of each subsite in the output",
        SortOrder = 4)]
    public class GetSubWebs : PnPWebRetrievalsCmdlet<Web>
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "If provided, only the subsite with the provided Id, GUID or the Web instance will be returned")]
        public WebPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "If provided, recursion through all subsites and their childs will take place to return them as well")]
        public SwitchParameter Recurse;

        protected override void ExecuteCmdlet()
        {
            DefaultRetrievalExpressions = new Expression<Func<Web, object>>[] { w => w.Id, w => w.Url, w => w.Title, w => w.ServerRelativeUrl };

            Web parentWeb = SelectedWeb;
            if (Identity != null)
            {
                if (Identity.Id != Guid.Empty)
                {
                    parentWeb = parentWeb.GetWebById(Identity.Id);
                }
                else if (Identity.Web != null)
                {
                    parentWeb = Identity.Web;
                }
                else if (Identity.Url != null)
                {
                    parentWeb = parentWeb.GetWebByUrl(Identity.Url);
                }
            }

            var allWebs = GetSubWebsInternal(parentWeb.Webs, Recurse);
            WriteObject(allWebs, true);
        }

        private List<Web> GetSubWebsInternal(WebCollection subsites, bool recurse)
        {
            var subwebs = new List<Web>();

            // Retrieve the subsites in the provided webs collection
            subsites.EnsureProperties(new Expression<Func<WebCollection, object>>[] { wc => wc.Include(w => w.Id) });

            foreach (var subsite in subsites)
            {
                // Retrieve all the properties for this particular subsite
                subsite.EnsureProperties(RetrievalExpressions);
                subwebs.Add(subsite);

                if (recurse)
                {
                    // As the Recurse flag has been set, recurse this method for it's child web collection
                    subwebs.AddRange(GetSubWebsInternal(subsite.Webs, recurse));
                }
            }

            return subwebs;
        }
    }
}
