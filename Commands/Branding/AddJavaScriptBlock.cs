using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.PowerShell.Commands.Enums;
using System;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Add, "SPOJavaScriptBlock")]
    [CmdletHelp("Adds a link to a JavaScript snippet/block to a web or site collection",
        DetailedDescription = "Specify a scope as 'Site' to add the custom action to all sites in a site collection.",
        Category = CmdletHelpCategory.Branding)]
    public class AddJavaScriptBlock : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The name of the script block. Can be used to identiy the script with other cmdlets or coded solutions")]
        [Alias("Key")]
        public string Name = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The javascript block to add")]
        public string Script = null;

        [Parameter(Mandatory = false)]
        public int Sequence = 0;

        [Parameter(Mandatory = false)]
        [Obsolete("Use Scope instead")]
        [Alias("AddToSite")]
        public SwitchParameter SiteScoped;

        [Parameter(Mandatory = false, HelpMessage = "The scope of the script to add to. Either Web or Site, defaults to Web.")]
        public CustomActionScope Scope = CustomActionScope.Web;

        protected override void ExecuteCmdlet()
        {
            // Following code to handle desprecated parameter
            CustomActionScope setScope;

            if (MyInvocation.BoundParameters.ContainsKey("SiteScoped"))
            {
                setScope = CustomActionScope.Site;
            }
            else
            {
                setScope = Scope;
            }

            if (setScope == CustomActionScope.Web)
            {
                SelectedWeb.AddJsBlock(Name, Script, Sequence);
            }
            else
            {
                var site = ClientContext.Site;
                site.AddJsBlock(Name, Script, Sequence);
            }
        }
    }
}
