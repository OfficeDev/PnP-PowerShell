using System.Management.Automation;
using Microsoft.SharePoint.Client;
using web = Microsoft.SharePoint.Client.Web;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System;
using SharePointPnP.PowerShell.Commands.Extensions;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Remove, "PnPWeb")]
    [CmdletAlias("Remove-SPOWeb")]
    [CmdletHelp("Removes a subweb in the current web",
        Category = CmdletHelpCategory.Webs)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPWeb -Url projectA",
        Remarks = "Remove a web",
        SortOrder = 1)]

    [CmdletExample(
        Code = @"PS:> Remove-PnPWeb -Identity 5fecaf67-6b9e-4691-a0ff-518fc9839aa0",
        Remarks = "Remove a web specified by its ID",
        SortOrder = 2)]

    [CmdletExample(
        Code = @"PS:> Get-PnPSubWebs | Remove-PnPWeb -Force",
        Remarks = "Remove all subwebs and do not ask for confirmation",
        SortOrder = 2)]


    public class RemoveWeb : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The site relative url of the web, e.g. 'Subweb1'", ParameterSetName = "ByUrl")]
        public string Url;

        [Parameter(Mandatory = true, HelpMessage = "Identity/Id/Web object to delete", ParameterSetName = "ByIdentity", ValueFromPipeline = true)]
        public WebPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Do not ask for confirmation to delete the subweb", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (ParameterSetName == "ByIdentity")
            {
                web web;
                if (Identity.Id != Guid.Empty)
                {
                    web = ClientContext.Web.GetWebById(Identity.Id);
                    web.EnsureProperty(w => w.Title);
                    if (Force || ShouldContinue(string.Format(Properties.Resources.RemoveWeb0, web.Title), Properties.Resources.Confirm))
                    {
                        web.DeleteObject();
                        web.Context.ExecuteQueryRetry();
                    }
                }
                else if (Identity.Web != null)
                {
                    Identity.Web.EnsureProperty(w => w.Title);
                    if (Force || ShouldContinue(string.Format(Properties.Resources.RemoveWeb0, Identity.Web.Title), Properties.Resources.Confirm))
                    {
                        Identity.Web.DeleteObject();
                        Identity.Web.Context.ExecuteQueryRetry();
                    }
                }
                else if (Identity.Url != null)
                {
                    web = ClientContext.Web.GetWebByUrl(Identity.Url);
                    web.EnsureProperty(w => w.Title);
                    if (Force || ShouldContinue(string.Format(Properties.Resources.RemoveWeb0, Identity.Web.Title), Properties.Resources.Confirm))
                    {
                        web.DeleteObject();
                        web.Context.ExecuteQueryRetry();
                    }
                }

            }
            else {
                var web = SelectedWeb.GetWeb(Url);
                web.EnsureProperty(w => w.Title);
                if (Force || ShouldContinue(string.Format(Properties.Resources.RemoveWeb0, web.Title), Properties.Resources.Confirm))
                {
                    SelectedWeb.DeleteWeb(Url);
                    ClientContext.ExecuteQueryRetry();
                }
            }
        }
    }
}