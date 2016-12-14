using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsCommon.Set, "PnPAuditing")]
    [CmdletAlias("Set-SPOAuditing")]
    [CmdletHelp("Set Auditing setting for a site",
        Category = CmdletHelpCategory.Sites)]
    [CmdletExample(
        Code = @"PS:> Set-PnPAuditing -EnableAll",
        Remarks = "Enables all auditing settings for the current site",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPAuditing -DisableAll",
        Remarks = @"Disables all auditing settings for the current site
                    This also disables the automatic trimming of the audit log",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Set-PnPAuditing -RetentionTime 7",
        Remarks = "Sets the audit log trimming to 7 days, this also enables the automatic trimming of the audit log",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Set-PnPAuditing -TrimAuditLog",
        Remarks = "Enables the automatic trimming of the audit log",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Set-PnPAuditing -RetentionTime 7 -CheckOutCheckInItems -MoveCopyItems -SearchContent",
        Remarks = @"Sets the audit log trimming to 7 days, this also enables the automatic trimming of the audit log.
                    Do auditing for:
                    - Checking out or checking in items
                    - Moving or copying items to another location in the site
                    - Searching site content",
        SortOrder = 5)]
    public class SetAuditing : SPOCmdlet
    {
        [Parameter(Mandatory = false, ParameterSetName ="EnableAll")]
        public SwitchParameter EnableAll;
        [Parameter(Mandatory = false, ParameterSetName = "DisableAll")]
        public SwitchParameter DisableAll;

        [Parameter(Mandatory = false, ParameterSetName = "Other")]
        [Parameter(ParameterSetName = "EnableAll")]
        public int RetentionTime = -1;
        [Parameter(Mandatory = false, ParameterSetName = "Other")]
        [Parameter(ParameterSetName = "EnableAll")]
        public SwitchParameter TrimAuditLog;

        //Editing items
        //AuditMaskType.Update
        [Parameter(Mandatory = false, ParameterSetName = "Other")]
        public SwitchParameter EditItems;

        //Checking out or checking in items
        //AuditMaskType.CheckOut and AuditMaskType.CheckIn
        [Parameter(Mandatory = false, ParameterSetName = "Other")]
        public SwitchParameter CheckOutCheckInItems;

        //Moving or copying items to another location in the site
        //AuditMaskType.Copy and AuditMaskType.Move
        [Parameter(Mandatory = false, ParameterSetName = "Other")]
        public SwitchParameter MoveCopyItems;

        //Deleting or restoring items
        //AuditMaskType.Undelete and AuditMaskType.ObjectDelete
        [Parameter(Mandatory = false, ParameterSetName = "Other")]
        public SwitchParameter DeleteRestoreItems;

        //Editing content types and columns
        //AuditMaskType.SchemaChange and AuditMaskType.ProfileChange
        [Parameter(Mandatory = false, ParameterSetName = "Other")]
        public SwitchParameter EditContentTypesColumns;

        //Searching site content
        //AuditMaskType.Search
        [Parameter(Mandatory = false, ParameterSetName = "Other")]
        public SwitchParameter SearchContent;

        //Editing users and permissions
        //AuditMaskType.SecurityChange
        [Parameter(Mandatory = false, ParameterSetName = "Other")]
        public SwitchParameter EditUsersPermissions;

        protected override void ExecuteCmdlet()
        {
            var updateAudit = false;
            var audit = ClientContext.Site.Audit;
            var auditFlags = audit.AuditFlags;
            ClientContext.Load(audit);
            ClientContext.ExecuteQueryRetry();

            if (EnableAll)
            {
                auditFlags = AuditMaskType.All;
                updateAudit = true;
            }
            if(DisableAll)
            {
                auditFlags = AuditMaskType.None;
                updateAudit = true;
                ClientContext.Site.TrimAuditLog = false;
            }
            if(RetentionTime != -1)
            {
                ClientContext.Site.AuditLogTrimmingRetention = RetentionTime;
                ClientContext.Site.TrimAuditLog = true;
            }
            if(TrimAuditLog.IsPresent)
            {
                ClientContext.Site.TrimAuditLog = true;
            }

            //set the events to audit
            //AuditMaskType.Update
            if (EditItems.IsPresent) { auditFlags = auditFlags | AuditMaskType.Update; updateAudit = true; }

            //AuditMaskType.CheckOut and AuditMaskType.CheckIn
            if(CheckOutCheckInItems.IsPresent) { auditFlags = auditFlags | AuditMaskType.CheckOut | AuditMaskType.CheckIn; updateAudit = true; }

            //AuditMaskType.Copy and AuditMaskType.Move
            if (MoveCopyItems.IsPresent) { auditFlags = auditFlags | AuditMaskType.Copy | AuditMaskType.Move; updateAudit = true; }

            //AuditMaskType.Undelete and AuditMaskType.ObjectDelete
            if (DeleteRestoreItems.IsPresent) { auditFlags = auditFlags | AuditMaskType.Undelete | AuditMaskType.ObjectDelete; updateAudit = true; }

            //AuditMaskType.SchemaChange and AuditMaskType.ProfileChange
            if (EditContentTypesColumns.IsPresent) { auditFlags = auditFlags | AuditMaskType.SchemaChange | AuditMaskType.ProfileChange; updateAudit = true; }

            //AuditMaskType.Search
            if (SearchContent.IsPresent) { auditFlags = auditFlags | AuditMaskType.Search; updateAudit = true; }

            //AuditMaskType.SecurityChange
            if (EditUsersPermissions.IsPresent) { auditFlags = auditFlags | AuditMaskType.SecurityChange; updateAudit = true; }

            if(updateAudit)
            {
                ClientContext.Site.Audit.AuditFlags = auditFlags;
                ClientContext.Site.Audit.Update();
            }

            //Commit the changes
            ClientContext.ExecuteQueryRetry();
        }
    }
}
