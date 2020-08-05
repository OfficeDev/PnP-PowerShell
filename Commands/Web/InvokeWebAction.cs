using Microsoft.SharePoint.Client;
using web = Microsoft.SharePoint.Client.Web;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.InvokeAction;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsLifecycle.Invoke, "PnPWebAction", SupportsShouldProcess = true)]
    [CmdletHelp("Executes operations on web, lists and list items.",
        Category = CmdletHelpCategory.Webs)]
    [CmdletExample(
        Code = @"PS:> Invoke-PnPWebAction -ListAction ${function:ListAction}",
        Remarks = "This will call the function ListAction on all the lists located on the current web.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Invoke-PnPWebAction -ShouldProcessListAction ${function:ShouldProcessList} -ListAction ${function:ListAction}",
        Remarks = "This will call the function ShouldProcessList, if it returns true the function ListAction will then be called. This will occur on all lists located on the current web",
        SortOrder = 2)]
    public class InvokeWebAction : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Webs you want to process (for example different site collections), will use Web parameter if not specified")]
        public web[] Webs;

        [Parameter(Mandatory = false, HelpMessage = "Function to be executed on the web. There is one input parameter of type Web")]
        public Action<web> WebAction;

        [Parameter(Mandatory = false, HelpMessage = "Function to be executed on the web that would determine if " + nameof(WebAction) + " should be invoked, There is one input parameter of type Web and the function should return a boolean value")]
        public Func<web, bool> ShouldProcessWebAction;

        [Parameter(Mandatory = false, HelpMessage = "Function to be executed on the web, this will trigger after lists and list items have been processed. There is one input parameter of type Web")]
        public Action<web> PostWebAction;

        [Parameter(Mandatory = false, HelpMessage = "Function to be executed on the web that would determine if " + nameof(PostWebAction) + " should be invoked, There is one input parameter of type Web and the function should return a boolean value")]
        public Func<web, bool> ShouldProcessPostWebAction;

        [Parameter(Mandatory = false, HelpMessage = "The properties to load for web.")]
        public string[] WebProperties;

        [Parameter(Mandatory = false, HelpMessage = "Name of list if you only want to handle one specific list and its list items")]
        public string ListName { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Function to be executed on the list. There is one input parameter of type List")]
        public Action<List> ListAction;

        [Parameter(Mandatory = false, HelpMessage = "Function to be executed on the web that would determine if " + nameof(ListAction) + " should be invoked, There is one input parameter of type List and the function should return a boolean value")]
        public Func<List, bool> ShouldProcessListAction;

        [Parameter(Mandatory = false, HelpMessage = "Function to be executed on the list, this will trigger after list items have been processed. There is one input parameter of type List")]
        public Action<List> PostListAction;

        [Parameter(Mandatory = false, HelpMessage = "Function to be executed on the web that would determine if " + nameof(PostListAction) + " should be invoked, There is one input parameter of type List and the function should return a boolean value")]
        public Func<List, bool> ShouldProcessPostListAction;

        [Parameter(Mandatory = false, HelpMessage = "The properties to load for list.")]
        public string[] ListProperties;

        [Parameter(Mandatory = false, HelpMessage = "Function to be executed on the list item. There is one input parameter of type ListItem")]
        public Action<ListItem> ListItemAction;

        [Parameter(Mandatory = false, HelpMessage = "Function to be executed on the web that would determine if " + nameof(ListItemAction) + " should be invoked, There is one input parameter of type ListItem and the function should return a boolean value")]
        public Func<ListItem, bool> ShouldProcessListItemAction;

        [Parameter(Mandatory = false, HelpMessage = "The properties to load for list items.")]
        public string[] ListItemProperties;

        [Parameter(Mandatory = false, HelpMessage = "Specify if sub webs will be processed")]
        public SwitchParameter SubWebs;

        [Parameter(Mandatory = false, HelpMessage = "Will not output statistics after the operation")]
        public SwitchParameter DisableStatisticsOutput;

        [Parameter(Mandatory = false, HelpMessage = "Will skip the counting process; by doing this you will not get an estimated time remaining")]
        public SwitchParameter SkipCounting;

        protected override void ExecuteCmdlet()
        {
            if (WebAction == null && ListAction == null && ListItemAction == null && PostWebAction == null && PostListAction == null)
            {
                ThrowTerminatingError(new ErrorRecord(new ArgumentNullException("An action need to be specified"), "0", ErrorCategory.InvalidArgument, null));
                return;
            }

            InvokeActionParameter<web> webActions = new InvokeActionParameter<web>()
            {
                Action = WebAction,
                ShouldProcessAction = ShouldProcessWebAction,
                PostAction = PostWebAction,
                ShouldProcessPostAction = ShouldProcessPostWebAction,
                Properties = WebProperties
            };

            InvokeActionParameter<List> listActions = new InvokeActionParameter<List>()
            {
                Action = ListAction,
                ShouldProcessAction = ShouldProcessListAction,
                PostAction = PostListAction,
                ShouldProcessPostAction = ShouldProcessPostListAction,
                Properties = ListProperties
            };

            InvokeActionParameter<ListItem> listItemActions = new InvokeActionParameter<ListItem>()
            {
                Action = ListItemAction,
                ShouldProcessAction = ShouldProcessListItemAction,
                Properties = WebProperties
            };

            InvokeAction.InvokeWebAction invokeAction;
            if (string.IsNullOrEmpty(ListName))
            {
                IEnumerable<web> websToProcess;
                if (Webs == null || Webs.Length == 0)
                    websToProcess = new[] { SelectedWeb };
                else
                    websToProcess = Webs;

                invokeAction = new InvokeAction.InvokeWebAction(this, websToProcess, SubWebs.ToBool(), webActions, listActions, listItemActions, SkipCounting.ToBool());
            }
            else
            {
                invokeAction = new InvokeAction.InvokeWebAction(this, SelectedWeb, ListName, webActions, listActions, listItemActions, SkipCounting.ToBool());
            }

            InvokeWebActionResult result = invokeAction.StartProcessAction();

            if (!DisableStatisticsOutput)
            {
                WriteObject(result.ToDataTable());
            }

            WriteObject(result);
        }
    }
}