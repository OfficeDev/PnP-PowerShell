using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.InformationManagement
{

    [Cmdlet(VerbsCommon.Get, "PnPListInformationRightsManagement")]
    [CmdletHelp("Get the site closure status of the site which has a site policy applied", Category = CmdletHelpCategory.InformationManagement)]
    [CmdletExample(
      Code = @"PS:> Get-PnPListInformationRightsManagement -List ""Documents""",
      Remarks = @"Returns Information Rights Management (IRM) settings for the list. See 'Get-Help Set-PnPListInformationRightsManagement -Detailed' for more information about the various values.", SortOrder = 1)]
    public class GetListInformationRightsManagement : PnPWebCmdlet
    {
        [Parameter(Mandatory = true)]
        public ListPipeBind List;

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb, l => l.IrmEnabled, l => l.IrmExpire, l => l.IrmReject);
            if (list == null)
                throw new PSArgumentException($"No list found with id, title or url '{List}'", "List");

            ClientContext.Load(list.InformationRightsManagementSettings);
            ClientContext.ExecuteQueryRetry();

            var irm = new
            {
                InformationRightsManagementEnabled = list.IrmEnabled,
                InformationRightsManagementExpire = list.IrmExpire,
                InformationRightsManagementReject = list.IrmReject,
                list.InformationRightsManagementSettings.AllowPrint,
                list.InformationRightsManagementSettings.AllowScript,
                list.InformationRightsManagementSettings.AllowWriteCopy,
                list.InformationRightsManagementSettings.DisableDocumentBrowserView,
                list.InformationRightsManagementSettings.DocumentAccessExpireDays,
                list.InformationRightsManagementSettings.DocumentLibraryProtectionExpireDate,
                list.InformationRightsManagementSettings.EnableDocumentAccessExpire,
                list.InformationRightsManagementSettings.EnableDocumentBrowserPublishingView,
                list.InformationRightsManagementSettings.EnableGroupProtection,
                list.InformationRightsManagementSettings.EnableLicenseCacheExpire,
                list.InformationRightsManagementSettings.GroupName,
                list.InformationRightsManagementSettings.LicenseCacheExpireDays,
                list.InformationRightsManagementSettings.PolicyDescription,
                list.InformationRightsManagementSettings.PolicyTitle,
#if !ONPREMISES
                list.InformationRightsManagementSettings.TemplateId
#endif
            };
            WriteObject(irm);
        }
    }
}
