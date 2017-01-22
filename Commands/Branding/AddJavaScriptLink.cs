using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Enums;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Add, "PnPJavaScriptLink")]
    [CmdletAlias("Add-SPOJavaScriptLink")]
    [CmdletHelp("Adds a link to a JavaScript file to a web or sitecollection",
        Category = CmdletHelpCategory.Branding)]
    [CmdletExample(Code = "PS:> Add-PnPJavaScriptLink -Name jQuery -Url https://code.jquery.com/jquery.min.js -Sequence 9999 -Scope Site",
                Remarks = "Injects a reference to the latest v1 series jQuery library to all pages within the current site collection under the name jQuery and at order 9999",
                SortOrder = 1)]
    [CmdletExample(Code = "PS:> Add-PnPJavaScriptLink -Name jQuery -Url https://code.jquery.com/jquery.min.js",
                Remarks = "Injects a reference to the latest v1 series jQuery library to all pages within the current web under the name jQuery",
                SortOrder = 2)]
    public class AddJavaScriptLink : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Name under which to register the JavaScriptLink")]
        [Alias("Key")]
        public string Name = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "URL to the JavaScript file to inject")]
        public string[] Url = null;

        [Parameter(Mandatory = false, HelpMessage = "Sequence of this JavaScript being injected. Use when you have a specific sequence with which to have JavaScript files being added to the page. I.e. jQuery library first and then jQueryUI.")]
        public int Sequence = 0;

        [Parameter(Mandatory = false)]
        [Obsolete("Use Scope")]
        [Alias("AddToSite")]
        public SwitchParameter SiteScoped;

        [Parameter(Mandatory = false, HelpMessage = "Defines if this JavaScript file will be injected to every page within the current site collection or web. All is not allowed in for this command. Default is web.")]
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

            switch (setScope)
            {
                case CustomActionScope.Web:
                    SelectedWeb.AddJsLink(Name, Url, Sequence);
                    break;

                case CustomActionScope.Site:
                    ClientContext.Site.AddJsLink(Name, Url, Sequence);
                    break;

                case CustomActionScope.All:
                    ThrowTerminatingError(new ErrorRecord(new Exception("Scope parameter can only be set to Web or Site"), "INCORRECTVALUE", ErrorCategory.InvalidArgument, this));
                    break;
            }
        }
    }
}
