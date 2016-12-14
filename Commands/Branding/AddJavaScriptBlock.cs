using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Enums;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Add, "PnPJavaScriptBlock")]
    [CmdletAlias("Add-SPOJavaScriptBlock")]
    [CmdletHelp("Adds a link to a JavaScript snippet/block to a web or site collection",
        DetailedDescription = "Specify a scope as 'Site' to add the custom action to all sites in a site collection.",
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = "PS:> Add-PnPJavaScriptBlock -Name myAction -script '<script>Alert(\"This is my Script block\");</script>' -Sequence 9999 -Scope Site",
                Remarks = "Add a JavaScript code block  to all pages within the current site collection under the name myAction and at order 9999",
                SortOrder = 1)]
    [CmdletExample(Code = "PS:> Add-PnPJavaScriptBlock -Name myAction -script '<script>Alert(\"This is my Script block\");</script>'",
                Remarks = "Add a JavaScript code block  to all pages within the current web under the name myAction",
                SortOrder = 2)]
    public class AddJavaScriptBlock : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The name of the script block. Can be used to identify the script with other cmdlets or coded solutions")]
        [Alias("Key")]
        public string Name = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The javascript block to add to the specified scope")]
        public string Script = null;

        [Parameter(Mandatory = false, HelpMessage = "A sequence number that defines the order on the page")]
        public int Sequence = 0;

        [Parameter(Mandatory = false)]
        [Obsolete("Use Scope instead")]
        [Alias("AddToSite")]
        public SwitchParameter SiteScoped;

        [Parameter(Mandatory = false, HelpMessage = "The scope of the script to add to. Either Web or Site, defaults to Web. 'All' is not valid for this command.")]
        public CustomActionScope Scope = CustomActionScope.Web;

        protected override void ExecuteCmdlet()
        {
            // Following code to handle deprecated parameter
            CustomActionScope setScope;

            if (MyInvocation.BoundParameters.ContainsKey("SiteScoped"))
            {
                setScope = CustomActionScope.Site;
            }
            else
            {
                setScope = Scope;
            }

            if (setScope != CustomActionScope.All)
            {
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
            else
            {
                WriteError(new ErrorRecord(new Exception("Scope parameter can only be set to Web or Site"),"INCORRECTVALUE",ErrorCategory.InvalidArgument, this));
            }
        }
    }
}
