using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.ContentTypes
{
    [Cmdlet(VerbsCommon.Remove, "PnPContentType")]
    [CmdletAlias("Remove-SPOContentType")]
    [CmdletHelp("Removes a content type from a web", 
        Category = CmdletHelpCategory.ContentTypes)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPContentType -Identity ""Project Document""",
        Remarks = @"This will remove a content type called ""Project Document"" from the current web",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPContentType -Identity ""Project Document"" -Force",
        Remarks = @"This will remove a content type called ""Project Document"" from the current web with force",
        SortOrder = 2)]
    public class RemoveContentType : PnPWebCmdlet
    {

        [Parameter(Mandatory = true, Position=0, ValueFromPipeline=true, HelpMessage="The name or ID of the content type to remove")]
        public ContentTypePipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Specifying the Force parameter will skip the confirmation question.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (Force || ShouldContinue(Resources.RemoveContentType, Resources.Confirm))
            {
                ContentType ct = null;
                if (Identity.ContentType != null)
                {
                    ct = Identity.ContentType;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Identity.Id))
                    {
                        ct = SelectedWeb.GetContentTypeById(Identity.Id);
                    }
                    else if (!string.IsNullOrEmpty(Identity.Name))
                    {
                        ct = SelectedWeb.GetContentTypeByName(Identity.Name);
                    }
                }
                if(ct != null)
                {
                    ct.DeleteObject();
                    ClientContext.ExecuteQueryRetry();
                }

            }
        }
    }
}
