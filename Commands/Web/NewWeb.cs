using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.New, "PnPWeb")]
    [CmdletHelp("Creates a new subweb under the current web",
        Category = CmdletHelpCategory.Webs,
        OutputType = typeof(Web),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.web.aspx")]
    [CmdletExample(
        Code = @"PS:> New-PnPWeb -Title ""Project A Web"" -Url projectA -Description ""Information about Project A"" -Locale 1033 -Template ""STS#0""", 
        Remarks = "Creates a new subweb under the current web with URL projectA", 
        SortOrder = 1)]
    public class NewWeb : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage="The title of the new web")]
        public string Title;

        [Parameter(Mandatory = true, HelpMessage="The URL of the new web")]
        public string Url;

        [Parameter(Mandatory = false, HelpMessage="The description of the new web")]
        public string Description = string.Empty;

        [Parameter(Mandatory = false, HelpMessage="The language id of the new web. default = 1033 for English")]
        public int Locale = 1033;

        [Parameter(Mandatory = true, HelpMessage= "The site definition template to use for the new web, e.g. STS#0. Use Get-PnPWebTemplates to fetch a list of available templates")]
        public string Template = string.Empty;

        [Parameter(Mandatory = false, HelpMessage="By default the subweb will inherit its security from its parent, specify this switch to break this inheritance")]
        public SwitchParameter BreakInheritance = false;

        [Parameter(Mandatory = false, HelpMessage="Specifies whether the site inherits navigation.")]
        public SwitchParameter InheritNavigation = true;
        protected override void ExecuteCmdlet()
        {
            var web = SelectedWeb.CreateWeb(Title, Url, Description, Template, Locale, !BreakInheritance,InheritNavigation);
            ClientContext.Load(web, w => w.Id, w => w.Url, w => w.ServerRelativeUrl);
            ClientContext.ExecuteQueryRetry();
            WriteObject(web);
        }

    }
}
