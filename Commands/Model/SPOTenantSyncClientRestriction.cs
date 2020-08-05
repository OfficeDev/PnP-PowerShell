#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using System;
using System.Collections.Generic;

namespace PnP.PowerShell.Commands.Model
{
    /// <summary>
    /// Returned properties for <see cref="GetPnPTenantSyncClientRestriction"/>
    /// </summary>
    public class SPOTenantSyncClientRestriction
    {
        /// <summary>
        /// Indication if Mac sync clients, the Beta version and the new sync client (OneDrive.exe), should be blocked
        /// </summary>
        public bool BlockMacSync { get; private set; }

        /// <summary>
        /// The domain GUID to add to the safe recipient list. Requires a minimum of 1 domain GUID to be enabled. The maximum number of domain GUIDs allowed are 125.
        /// </summary>
        public List<Guid> AllowedDomainList { get; private set; }

        /// <summary>
        /// Blocks certain file types from syncing with the new sync client (OneDrive.exe)
        /// </summary>
        public List<string> ExcludedFileExtensions { get; private set; }

        /// <summary>
        /// Indicates if the usage of the old Groove sync client is allowed (true) or not (false)
        /// </summary>
        public bool OptOutOfGrooveBlock { get; private set; }

        /// <summary>
        /// Indicates if the user is asked to upgrade to the new Groove client, but can still use the old client (true) of if the user is forced to upgrade (false)
        /// </summary>
        public bool OptOutOfGrooveSoftBlock { get; private set; }

        /// <summary>
        /// Specifies if the Report Problem Dialog is disabled or not
        /// </summary>
        public bool DisableReportProblemDialog { get; private set; }

        /// <summary>
        /// Indicates if a sync restriction is set on the tenant
        /// </summary>
        public bool TenantRestrictionEnabled { get; private set; }

        /// <summary>
        /// Instantiates a new SPOTenantSyncClientRestriction instance
        /// </summary>
        /// <param name="tenant">Tenant instance to get the properties from to fill this instance</param>
        public SPOTenantSyncClientRestriction(Tenant tenant)
        {
            AllowedDomainList = new List<Guid>(tenant.AllowedDomainListForSyncClient);
            BlockMacSync = tenant.BlockMacSync;
            ExcludedFileExtensions = new List<string>(tenant.ExcludedFileExtensionsForSyncClient);
            OptOutOfGrooveBlock = tenant.OptOutOfGrooveBlock;
            OptOutOfGrooveSoftBlock = tenant.OptOutOfGrooveSoftBlock;
            DisableReportProblemDialog = tenant.DisableReportProblemDialog;
            TenantRestrictionEnabled = tenant.IsUnmanagedSyncClientForTenantRestricted;
        }
    }
}
#endif