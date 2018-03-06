using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Set, "PnPList")]
    [CmdletHelp("Updates list settings",
         Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
         Code = @"Set-PnPList -Identity ""Demo List"" -EnableContentTypes $true",
         Remarks = "Switches the Enable Content Type switch on the list",
         SortOrder = 1)]
    [CmdletExample(
         Code = @"Set-PnPList -Identity ""Demo List"" -Hidden $true",
         Remarks = "Hides the list from the SharePoint UI.",
         SortOrder = 2)]
    [CmdletExample(
         Code = @"Set-PnPList -Identity ""Demo List"" -EnableVersioning $true",
         Remarks = "Turns on major versions on a list",
         SortOrder = 3)]
    [CmdletExample(
         Code = @"Set-PnPList -Identity ""Demo List"" -EnableVersioning $true -MajorVersions 20",
         Remarks = "Turns on major versions on a list and sets the maximum number of Major Versions to keep to 20.",
         SortOrder = 4)]
    [CmdletExample(
         Code = @"Set-PnPList -Identity ""Demo Library"" -EnableVersioning $true -EnableMinorVersions $true -MajorVersions 20 -MinorVersions 5",
         Remarks = "Turns on major versions on a document library and sets the maximum number of Major versions to keep to 20 and sets the maximum of Minor versions to 5.",
         SortOrder = 5)]
    [CmdletExample(
        Code = @"Set-PnPList -Identity ""Demo List"" -EnableAttachments $true",
        Remarks = "Turns on attachments on a list",
        SortOrder = 6)]
    public class SetList : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The ID, Title or Url of the list.")]
        public ListPipeBind Identity;

        [Parameter(Mandatory = false,
             HelpMessage = "Set to $true to enable content types, set to $false to disable content types")]
        public bool
            EnableContentTypes;

        [Parameter(Mandatory = false, HelpMessage = "If used the security inheritance is broken for this list")]
        public
            SwitchParameter BreakRoleInheritance;

        [Parameter(Mandatory = false, HelpMessage = "If used the roles are copied from the parent web")]
        public
            SwitchParameter CopyRoleAssignments;

        [Parameter(Mandatory = false,
             HelpMessage =
                 "If used the unique permissions are cleared from child objects and they can inherit role assignments from this object"
         )]
        public SwitchParameter ClearSubscopes;

        [Parameter(Mandatory = false, HelpMessage = "The title of the list")]
        public string Title = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Hide the list from the SharePoint UI. Set to $true to hide, $false to show.")]
        public bool Hidden;

        [Parameter(Mandatory = false, HelpMessage = "Enable or disable attachments. Set to $true to enable, $false to disable.")]
        public bool EnableAttachments;

        [Parameter(Mandatory = false, HelpMessage = "Enable or disable versioning. Set to $true to enable, $false to disable.")]
        public bool EnableVersioning;

        [Parameter(Mandatory = false, HelpMessage = "Enable or disable minor versions versioning. Set to $true to enable, $false to disable.")]
        public bool EnableMinorVersions;

        [Parameter(Mandatory = false, HelpMessage = "Maximum major versions to keep")]
        public uint MajorVersions = 10;

        [Parameter(Mandatory = false, HelpMessage = "Maximum minor versions to keep")]
        public uint MinorVersions = 10;

        [Parameter(Mandatory = false, HelpMessage = "Enable or disable whether content approval is enabled for the list. Set to $true to enable, $false to disable.")]
        public bool EnableModeration;

        protected override void ExecuteCmdlet()
        {
            var list = Identity.GetList(SelectedWeb);

            if (list != null)
            {
                list.EnsureProperties(l => l.EnableAttachments, l => l.EnableVersioning, l => l.EnableMinorVersions, l => l.Hidden, l => l.EnableModeration, l => l.BaseType);

                var enableVersioning = list.EnableVersioning;
                var enableMinorVersions = list.EnableMinorVersions;
                var hidden = list.Hidden;
                var enableAttachments = list.EnableAttachments;

                var isDirty = false;
                if (BreakRoleInheritance)
                {
                    list.BreakRoleInheritance(CopyRoleAssignments, ClearSubscopes);
                    isDirty = true;
                }

                if (!string.IsNullOrEmpty(Title))
                {
                    list.Title = Title;
                    isDirty = true;
                }

                if (MyInvocation.BoundParameters.ContainsKey("Hidden") && Hidden != list.Hidden)
                {
                    list.Hidden = Hidden;
                    isDirty = true;
                }

                if (MyInvocation.BoundParameters.ContainsKey("EnableContentTypes") && list.ContentTypesEnabled != EnableContentTypes)
                {
                    list.ContentTypesEnabled = EnableContentTypes;
                    isDirty = true;
                }

                if (MyInvocation.BoundParameters.ContainsKey("EnableVersioning") && EnableVersioning != enableVersioning)
                {
                    list.EnableVersioning = EnableVersioning;
                    isDirty = true;
                }

                if (MyInvocation.BoundParameters.ContainsKey("EnableMinorVersions") && EnableMinorVersions != enableMinorVersions)
                {
                    list.EnableMinorVersions = EnableMinorVersions;
                    isDirty = true;
                }

                if (MyInvocation.BoundParameters.ContainsKey("EnableModeration") && list.EnableModeration != EnableModeration)
                {
                    list.EnableModeration = EnableModeration;
                    isDirty = true;
                }

                if (MyInvocation.BoundParameters.ContainsKey("EnableAttachments") && EnableAttachments != enableAttachments)
                {
                    list.EnableAttachments = EnableAttachments;
                    isDirty = true;
                }

                if (isDirty)
                {
                    list.Update();
                    ClientContext.ExecuteQueryRetry();
                }
                isDirty = false;

                if (list.EnableVersioning)
                {
                    // list or doclib?

                    if (list.BaseType == BaseType.DocumentLibrary)
                    {
                        if (MyInvocation.BoundParameters.ContainsKey("MajorVersions"))
                        {
                            list.MajorVersionLimit = (int)MajorVersions;
                            isDirty = true;
                        }

                        if (MyInvocation.BoundParameters.ContainsKey("MinorVersions") && list.EnableMinorVersions)
                        {
                            list.MajorWithMinorVersionsLimit = (int)MinorVersions;
                            isDirty = true;
                        }
                    }
                    else
                    {
                        if (MyInvocation.BoundParameters.ContainsKey("MajorVersions"))
                        {
                            list.MajorVersionLimit = (int)MajorVersions;
                            isDirty = true;
                        }
                    }
                }
                if (isDirty)
                {
                    list.Update();
                    ClientContext.ExecuteQueryRetry();
                }
            }
        }
    }
}