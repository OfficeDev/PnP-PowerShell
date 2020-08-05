#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System.Management.Automation;
using System;
using System.Collections.Generic;
using Resources = PnP.PowerShell.Commands.Properties.Resources;
using PnP.PowerShell.Commands.Model;

namespace PnP.PowerShell.Commands.Admin
{
    [Cmdlet(VerbsCommon.Set, "PnPTenantSyncClientRestriction", DefaultParameterSetName = ParameterAttribute.AllParameterSets)]
    [CmdletHelp(@"Sets organization-level sync client restriction properties",
        DetailedDescription = @"Sets organization-level sync client restriction properties such as BlockMacSync, OptOutOfGroveBlock, and DisableReportProblemDialog.

You must have the SharePoint Online admin or Global admin role to run the cmdlet.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTenantSyncClientRestriction -BlockMacSync:$false",
        Remarks = @"This example blocks access to Mac sync clients for OneDrive file synchronization", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-SPOTenantSyncClientRestriction  -ExcludedFileExtensions ""pptx;docx;xlsx""",
        Remarks = @"This example blocks syncing of PowerPoint, Word, and Excel file types using the new sync client (OneDrive.exe).", SortOrder = 2)]
    public class SetTenantSyncClientRestriction : PnPAdminCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Block Mac sync clients-- the Beta version and the new sync client (OneDrive.exe). The values for this parameter are $true and $false. The default value is $false.")]
        public SwitchParameter BlockMacSync;

        [Parameter(Mandatory = false, HelpMessage = "Specifies if the Report Problem Dialog is disabled or not.")]
        public SwitchParameter DisableReportProblemDialog;

        [Parameter(Mandatory = false, HelpMessage = "Sets the domain GUID to add to the safe recipient list. Requires a minimum of 1 domain GUID. The maximum number of domain GUIDs allowed are 125. I.e. 634c71f6-fa83-429c-b77b-0dba3cb70b93,4fbc735f-0ac2-48ba-b035-b1ae3a480887.")]
        public List<Guid> DomainGuids;

        [Parameter(Mandatory = false, HelpMessage = "Enables the feature to block sync originating from domains that are not present in the safe recipients list.")]
        public SwitchParameter Enable;

        [Parameter(Mandatory = false, HelpMessage = @"Blocks certain file types from syncing with the new sync client (OneDrive.exe). Provide as one string separating the extensions using a semicolon (;). I.e. ""docx;pptx""")]
        public List<string> ExcludedFileExtensions;

        [Parameter(Mandatory = false, HelpMessage = "Controls whether or not a tenant's users can sync OneDrive for Business libraries with the old OneDrive for Business sync client. The valid values are OptOut, HardOptin, and SoftOptin.")]
        public Enums.GrooveBlockOption GrooveBlockOption;

        protected override void ExecuteCmdlet()
        {
            ClientContext.Load(Tenant);
            ClientContext.ExecuteQueryRetry();

            if (ParameterSpecified(nameof(DomainGuids)))
            {
                Tenant.AllowedDomainListForSyncClient = new List<Guid>(DomainGuids);
            }

            Tenant.BlockMacSync = BlockMacSync.ToBool();
            Tenant.IsUnmanagedSyncClientForTenantRestricted = Enable.ToBool();
            Tenant.DisableReportProblemDialog = DisableReportProblemDialog.ToBool();

            if (ParameterSpecified(nameof(ExcludedFileExtensions)))
            {
                Tenant.ExcludedFileExtensionsForSyncClient = ExcludedFileExtensions;
            }
            
            if(ParameterSpecified(nameof(GrooveBlockOption)))
            {
                switch (GrooveBlockOption)
                {
                    case Enums.GrooveBlockOption.OptOut:
                        Tenant.OptOutOfGrooveBlock = true;
                        Tenant.OptOutOfGrooveSoftBlock = true;
                        break;

                    case Enums.GrooveBlockOption.HardOptin:
                        Tenant.OptOutOfGrooveBlock = false;
                        Tenant.OptOutOfGrooveSoftBlock = true;
                        break;

                    case Enums.GrooveBlockOption.SoftOptin:
                        Tenant.OptOutOfGrooveBlock = true;
                        Tenant.OptOutOfGrooveSoftBlock = false;
                        break;

                    default:
                        throw new PSArgumentException(string.Format(Resources.GrooveBlockOptionNotSupported, nameof(GrooveBlockOption), GrooveBlockOption), nameof(GrooveBlockOption));
                }
            }
            ClientContext.ExecuteQueryRetry();
        }
    }
}
#endif