using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using Resources = PnP.PowerShell.Commands.Properties.Resources;
using System.Management.Automation;
using File = Microsoft.SharePoint.Client.File;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsData.Restore, "PnPFileVersion", DefaultParameterSetName = "Return as file object")]
    [CmdletHelp("Restores a specific file version.",
        Category = CmdletHelpCategory.Files)]
    [CmdletExample(
        Code = @"PS:> Restore-PnPFileVersion -Url Documents/MyDocument.docx -Identity 512",
        Remarks = "Restores the file version with Id 512",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Restore-PnPFileVersion -Url Documents/MyDocument.docx -Identity ""Version 1.0""",
        Remarks = "Restores the file version with label \"Version 1.0\"",
        SortOrder = 2)]
    public class RestoreFileVersion : PnPWebCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Url;

        [Parameter(Mandatory = false)]
        public FileVersionPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "If provided, no confirmation will be requested and the action will be performed")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            var serverRelativeUrl = string.Empty;

            var webUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

            if (!Url.ToLower().StartsWith(webUrl.ToLower()))
            {
                serverRelativeUrl = UrlUtility.Combine(webUrl, Url);
            }
            else
            {
                serverRelativeUrl = Url;
            }

            File file;

#if ONPREMISES
            file = SelectedWeb.GetFileByServerRelativeUrl(serverRelativeUrl);
#else
            file = SelectedWeb.GetFileByServerRelativePath(ResourcePath.FromDecodedUrl(serverRelativeUrl));
#endif

            ClientContext.Load(file, f => f.Exists, f => f.Versions.IncludeWithDefaultProperties(i => i.CreatedBy));
            ClientContext.ExecuteQueryRetry();

            if (file.Exists)
            {
                var versions = file.Versions;

                if (Force || ShouldContinue("Restoring a previous version will overwrite the current version.", Resources.Confirm))
                {
                    if (!string.IsNullOrEmpty(Identity.Label))
                    {
                        versions.RestoreByLabel(Identity.Label);
                        ClientContext.ExecuteQueryRetry();
                        WriteObject("Version restored");
                    }
                    else if (Identity.Id != -1)
                    {
                        var version = versions.GetById(Identity.Id);
                        ClientContext.Load(version);
                        ClientContext.ExecuteQueryRetry();

                        versions.RestoreByLabel(version.VersionLabel);
                        ClientContext.ExecuteQueryRetry();
                        WriteObject("Version restored");
                    }
                }
            }
            else
            {
                throw new PSArgumentException("File not found", nameof(Url));
            }
        }
    }
}


