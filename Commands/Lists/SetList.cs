using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Set, "PnPList")]
    [CmdletAlias("Set-SPOList")]
    [CmdletHelp("Updates list settings",
        Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
        Code = @"Set-PnPList -Identity ""Demo List"" -EnableContentTypes $true", 
        Remarks = "Switches the Enable Content Type switch on the list",
        SortOrder = 1)]
    public class SetList : SPOWebCmdlet
    {
        [Parameter(Mandatory=true, HelpMessage = "The ID, Title or Url of the list.")]
        public ListPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Set to $true to enable content types, set to $false to disable content types")]
        public bool EnableContentTypes;

        [Parameter(Mandatory = false, HelpMessage = "If used the security inheritance is broken for this list")]
        public SwitchParameter BreakRoleInheritance;

        [Parameter(Mandatory = false, HelpMessage = "If used the roles are copied from the parent web")]
        public SwitchParameter CopyRoleAssignments;

        [Parameter(Mandatory = false, HelpMessage = "If used the unique permissions are cleared from child objects and they can inherit role assignments from this object")]
        public SwitchParameter ClearSubscopes;

        [Parameter(Mandatory = false, HelpMessage = "The title of the list")]
        public string Title;

        [Parameter(Mandatory = false, HelpMessage = "The description of the list")]
        public string Description;

        [Parameter(Mandatory = false, HelpMessage = "Enable attachments for the list, set to $false to disable")]
        public SwitchParameter EnableAttachments;

        [Parameter(Mandatory = false, HelpMessage = "Enable folder creation for the list, set to $false to disable")]
        public SwitchParameter EnableFolderCreation;

        [Parameter(Mandatory = false, HelpMessage = "Enable minor versions for the list, set to $false to disable")]
        public SwitchParameter EnableMinorVersions;

        [Parameter(Mandatory = false, HelpMessage = "Enable content approval for the list, set to $false to disable")]
        public SwitchParameter EnableModeration;

        [Parameter(Mandatory = false, HelpMessage = "Enable versioning for the list, set to $false to disable")]
        public SwitchParameter EnableVersioning;

        protected override void ExecuteCmdlet()
        {
            var list = Identity.GetList(SelectedWeb);

            if(list != null)
            {
                foreach (var key in MyInvocation.BoundParameters.Keys)
                {
                    switch (key)
                    {
                        case "BreakRoleInheritance":
                            list.BreakRoleInheritance(CopyRoleAssignments, ClearSubscopes);
                            break;

                        case "Description":
                            list.Description = Description;
                            break;

                        case "EnableAttachments":
                            list.EnableAttachments = EnableAttachments;
                            break;

                        case "EnableContentTypes":
                            list.ContentTypesEnabled = EnableContentTypes;
                            break;

                        case "EnableFolderCreation":
                            list.EnableFolderCreation = EnableFolderCreation;
                            break;

                        case "EnableMinorVersions":
                            list.EnableMinorVersions = EnableMinorVersions;
                            break;

                        case "EnableModeration":
                            list.EnableModeration = EnableModeration;
                            break;

                        case "EnableVersioning":
                            list.EnableVersioning = EnableVersioning;
                            break;

                        case "Title":
                            list.Title = Title;
                            break;
                    } // switch
                } // keys

                list.Update();
                ClientContext.ExecuteQueryRetry(); 
            }
        }
    }
}
