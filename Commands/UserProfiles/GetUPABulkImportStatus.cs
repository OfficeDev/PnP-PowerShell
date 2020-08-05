#if !ONPREMISES
using System.Management.Automation;
using Microsoft.Online.SharePoint.TenantManagement;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.UserProfiles
{
    [Cmdlet(VerbsCommon.Get, "PnPUPABulkImportStatus")]
    [CmdletHelp(@"Get user profile bulk import status.",
        "Retrieve information about the status of submitted user profile bulk upload jobs.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.UserProfiles)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUPABulkImportStatus",
        Remarks = @"This will list the status of all submitted user profile bulk import jobs.", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUPABulkImportStatus -IncludeErrorDetails",
        Remarks = @"This will list the status of all submitted user profile bulk import jobs, and if it contains an error it will include the error log messages if present.", SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUPABulkImportStatus -JobId <guid>",
        Remarks = @"This will list the status for the specified import job.", SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-PnPUPABulkImportStatus -JobId <guid> -IncludeErrorDetails",
        Remarks = @"This will list the status for the specified import job, and if it contains an error it will include the error log messages if present.", SortOrder = 4)]

    public class GetUPABulkImportStatus : PnPAdminCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, HelpMessage = "The instance id of the job")]
        public GuidPipeBind JobId;

        [Parameter(Mandatory = false, ValueFromPipeline = true, HelpMessage = "Include error log details")]
        public SwitchParameter IncludeErrorDetails;

        protected override void ExecuteCmdlet()
        {
            var o365 = new Office365Tenant(ClientContext);

            if (ParameterSpecified(nameof(JobId)))
            {
                var job = o365.GetImportProfilePropertyJob(JobId.Id);
                ClientContext.Load(job);
                ClientContext.ExecuteQueryRetry();

                GetErrorInfo(job);
                WriteObject(job);
            }
            else
            {
                ImportProfilePropertiesJobStatusCollection jobs = o365.GetImportProfilePropertyJobs();
                ClientContext.Load(jobs);
                ClientContext.ExecuteQueryRetry();
                foreach (var job in jobs)
                {
                    GetErrorInfo(job);
                }
                WriteObject(jobs);
            }
        }

        private void GetErrorInfo(ImportProfilePropertiesJobInfo job)
        {
            if (job.Error != ImportProfilePropertiesJobError.NoError && IncludeErrorDetails == true)
            {
                var webUrl = Web.GetWebUrlFromPageUrl(ClientContext, job.LogFolderUri);
                ClientContext.ExecuteQueryRetry();
                string relativePath = job.LogFolderUri.Replace(webUrl.Value, "");
                var webCtx = ClientContext.Clone(webUrl.Value);
                if (webCtx.Web.DoesFolderExists(relativePath))
                {
                    var folder = webCtx.Web.GetFolderByServerRelativeUrl(relativePath);
                    var files = folder.Files;
                    webCtx.Load(folder);
                    webCtx.Load(files);
                    webCtx.ExecuteQueryRetry();
                    string message = string.Empty;
                    foreach (var logFile in files)
                        message += "\r\n" + webCtx.Web.GetFileAsString(logFile.ServerRelativeUrl);
                    job.ErrorMessage = message;
                }
            }
        }
    }
}
#endif