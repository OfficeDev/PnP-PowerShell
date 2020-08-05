using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;
using File = Microsoft.SharePoint.Client.File;
using Resources = PnP.PowerShell.Commands.Properties.Resources;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Remove, "PnPFileVersion", DefaultParameterSetName = "Return as file object")]
    [CmdletHelp("Removes all or a specific file version.",
        Category = CmdletHelpCategory.Files)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPFileVersion -Url Documents/MyDocument.docx -Identity 512",
        Remarks = "Removes the file version with Id 512",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPFileVersion -Url Documents/MyDocument.docx -Identity ""Version 1.0""",
        Remarks = "Removes the file version with label \"Version 1.0\"",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPFileVersion -Url Documents/MyDocument.docx -All",
        Remarks = "Removes all file versions",
        SortOrder = 2)]
    public class RemoveFileVersion : PnPWebCmdlet
    {
        private const string ParameterSetName_BYID = "By Id";
        private const string ParameterSetName_ALL = "All";

        [Parameter(Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string Url;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSetName_ALL)]
        public SwitchParameter All;

        [Parameter(Mandatory = false, ParameterSetName = ParameterSetName_BYID)]
        public FileVersionPipeBind Identity;

        [Parameter(Mandatory = false, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public SwitchParameter Recycle;

        [Parameter(Mandatory = false, HelpMessage = "If provided, no confirmation will be requested and the action will be performed", ParameterSetName = ParameterAttribute.AllParameterSets)]
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

                switch (ParameterSetName)
                {
                    case ParameterSetName_ALL:
                        {
                            if (Force || ShouldContinue("Remove all versions?", Resources.Confirm))
                            {
                                versions.DeleteAll();
                                ClientContext.ExecuteQueryRetry();
                            }
                            break;
                        }
                    case ParameterSetName_BYID:
                        {
                            if (Force || ShouldContinue("Remove a version?", Resources.Confirm))
                            {
                                if (!string.IsNullOrEmpty(Identity.Label))
                                {
                                    versions.DeleteByLabel(Identity.Label);
                                    ClientContext.ExecuteQueryRetry();
                                }
                                else if (Identity.Id != -1)
                                {
                                    versions.DeleteByID(Identity.Id);
                                    ClientContext.ExecuteQueryRetry();
                                }
                            }
                            break;
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


