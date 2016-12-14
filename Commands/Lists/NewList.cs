using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.New, "PnPList")]
    [CmdletAlias("New-SPOList")]
    [CmdletHelp("Creates a new list",
        Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
        Code = "PS:> New-PnPList -Title Announcements -Template Announcements",
        SortOrder = 1,
        Remarks = "Create a new announcements list")]
    [CmdletExample(
        Code = @"PS:> New-PnPList -Title ""Demo List"" -Url ""DemoList"" -Template Announcements",
        SortOrder = 2,
        Remarks = "Create a list with a title that is different from the url")]
    public class NewList : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The Title of the list")]
        public string Title;

        [Parameter(Mandatory = true, HelpMessage = "The type of list to create.")]
        public ListTemplateType Template;

        [Parameter(Mandatory = false, HelpMessage = "If set, will override the url of the list.")]
        public string Url = null;

        [Parameter(Mandatory = false, HelpMessage = "Switch parameter if versioning should be enabled")]
        public SwitchParameter EnableVersioning;

        [Parameter(Mandatory = false, HelpMessage = "Obsolete")]
        [Obsolete("Not in use, use OnQuickLaunch parameter instead")]
        public QuickLaunchOptions QuickLaunchOptions;

        [Parameter(Mandatory = false, HelpMessage = "Switch parameter if content types should be enabled on this list")]
        public SwitchParameter EnableContentTypes;

        [Parameter(Mandatory = false, HelpMessage = "Switch parameter if this list should be visible on the QuickLaunch")]
        public SwitchParameter OnQuickLaunch;

        protected override void ExecuteCmdlet()
        {
            var list = SelectedWeb.CreateList(Template, Title, EnableVersioning, true, Url, EnableContentTypes);
            if (OnQuickLaunch)
            {
                list.OnQuickLaunch = true;
                list.Update();
                ClientContext.ExecuteQueryRetry();
            }
        }
    }

}
