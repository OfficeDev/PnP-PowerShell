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

        [Parameter(Mandatory = false, HelpMessage = "Enable or disable versioning. Set to $true to enable, $false to disable.")]
        public bool EnableVersioning;

        [Parameter(Mandatory = false, HelpMessage = "Enable or disable minor versions versioning. Set to $true to enable, $false to disable.")]
        public bool EnableMinorVersions;

        [Parameter(Mandatory = false, HelpMessage = "Maximum major versions to keep")]
        [ValidateRange(0, int.MaxValue)]
        public uint MajorVersions = 0;

        [Parameter(Mandatory = false, HelpMessage = "Maximum minor versions to keep")]
        [ValidateRange(0, int.MaxValue)]
        public uint MinorVersions = 0;

        [Parameter(Mandatory = false, HelpMessage = "Enable or disable content approval (moderation). Set to $true to enable, $false to disable.")]
        public bool EnableModeration;

        [Parameter(Mandatory = false, HelpMessage = "Set to $true to prevent this list from being crawled, or $false to allow the crawler.")]
        public bool NoCrawl;

        protected override void ExecuteCmdlet()
        {
            var list = Identity.GetList(SelectedWeb);
            
            if (list != null)
            {
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

                if (MyInvocation.BoundParameters.ContainsKey(nameof(Hidden)) && Hidden != list.Hidden)
                {
                    list.Hidden = Hidden;
                    isDirty = true;
                }

                if (MyInvocation.BoundParameters.ContainsKey(nameof(EnableContentTypes)) && list.ContentTypesEnabled != EnableContentTypes)
                {
                    list.ContentTypesEnabled = EnableContentTypes;
                    isDirty = true;
                }

                list.EnsureProperties(l => l.EnableVersioning, l => l.EnableMinorVersions, l => l.Hidden);

                var enableVersioning = list.EnableVersioning;
                var enableMinorVersions = list.EnableMinorVersions;
                var hidden = list.Hidden;

                if (MyInvocation.BoundParameters.ContainsKey(nameof(EnableVersioning)) && EnableVersioning != enableVersioning)
                {
                    list.EnableVersioning = EnableVersioning;
                    isDirty = true;
                }

                if (MyInvocation.BoundParameters.ContainsKey(nameof(EnableMinorVersions)) && EnableMinorVersions != enableMinorVersions)
                {
                    list.EnableMinorVersions = EnableMinorVersions;
                    isDirty = true;
                }

                if (MyInvocation.BoundParameters.ContainsKey(nameof(EnableModeration)))
                {
                    list.EnableModeration = EnableModeration;
                    isDirty = true;
                }

                if (MyInvocation.BoundParameters.ContainsKey(nameof(NoCrawl)))
                {
                    list.NoCrawl = NoCrawl;
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
                    if (MyInvocation.BoundParameters.ContainsKey(nameof(MajorVersions)))
                    {
                        list.MajorVersionLimit = (int)MajorVersions;
                        isDirty = true;
                    }

                    if (MyInvocation.BoundParameters.ContainsKey(nameof(MinorVersions)) && list.EnableMinorVersions)
                    {
                        if (list.BaseType == BaseType.DocumentLibrary)
                        {
                            list.MajorWithMinorVersionsLimit = (int)MinorVersions;
                            isDirty = true;
                        }
                        else
                        {
                            WriteWarning("Cannot set minor version retention because list is not a document library. Value will be ignored.");
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