using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;

namespace PnP.PowerShell.Commands.ContentTypes
{
    [Cmdlet(VerbsCommon.Get, "PnPContentType")]
    [CmdletHelp("Retrieves a content type",
        Category = CmdletHelpCategory.ContentTypes,
        OutputType = typeof(ContentType),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.contenttype.aspx")]
    [CmdletExample(
        Code = @"PS:> Get-PnPContentType ",
        Remarks = @"This will get a listing of all available content types within the current web",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPContentType -InSiteHierarchy",
        Remarks = @"This will get a listing of all available content types within the site collection",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPContentType -Identity ""Project Document""",
        Remarks = @"This will get the content type with the name ""Project Document"" within the current context",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-PnPContentType -List ""Documents""",
        Remarks = @"This will get a listing of all available content types within the list ""Documents""",
        SortOrder = 4)]
    public class GetContentType : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "Name or ID of the content type to retrieve")]
        public ContentTypePipeBind Identity;
        [Parameter(Mandatory = false, ValueFromPipeline = true, HelpMessage = "List to query")]
        public ListPipeBind List;
        [Parameter(Mandatory = false, ValueFromPipeline = false, HelpMessage = "Search site hierarchy for content types")]
        public SwitchParameter InSiteHierarchy;

        protected override void ExecuteCmdlet()
        {

            if (Identity != null)
            {
                if (List == null)
                {
                    ContentType ct;
                    if (!string.IsNullOrEmpty(Identity.Id))
                    {
                        ct = SelectedWeb.GetContentTypeById(Identity.Id, InSiteHierarchy.IsPresent);
                    }
                    else
                    {
                        ct = SelectedWeb.GetContentTypeByName(Identity.Name, InSiteHierarchy.IsPresent);
                    }
                    if (ct != null)
                    {

                        WriteObject(ct);
                    }
                }
                else
                {
                    List list = List.GetList(SelectedWeb);
                    if (list == null)
                        throw new PSArgumentException($"No list found with id, title or url '{List}'", "List");

                    ClientContext.ExecuteQueryRetry();

                    if (!string.IsNullOrEmpty(Identity.Id))
                    {
                        if(list.ContentTypeExistsById(Identity.Id))
                        {
                            var cts = list.GetContentTypeById(Identity.Id);
                            ClientContext.Load(cts);
                            ClientContext.ExecuteQueryRetry();
                            WriteObject(cts, false);
                        }
                        else
                        {
                            WriteError(new ErrorRecord(new ArgumentException(String.Format("Content Type Id: {0} does not exist in the list: {1}", Identity.Id, list.Title)), "CONTENTTYPEDOESNOTEXIST", ErrorCategory.InvalidArgument, this));
                            
                        }
                    }
                    else if (!string.IsNullOrEmpty(Identity.Name))
                    {
                        if (list.ContentTypeExistsByName(Identity.Name))
                        {
                            var cts = list.GetContentTypeByName(Identity.Name);
                            ClientContext.Load(cts);
                            ClientContext.ExecuteQueryRetry();
                            WriteObject(cts, false);
                        }
                        else
                        {
                            WriteError(new ErrorRecord(new ArgumentException(String.Format("Content Type Name: {0} does not exist in the list: {1}", Identity.Name, list.Title)), "CONTENTTYPEDOESNOTEXIST", ErrorCategory.InvalidArgument, this));
                        
                        }
                     
                    }
                }
            }
            else
            {
                if (List == null)
                {
                    var cts = (InSiteHierarchy.IsPresent) ? ClientContext.LoadQuery(SelectedWeb.AvailableContentTypes) : ClientContext.LoadQuery(SelectedWeb.ContentTypes);
                    ClientContext.ExecuteQueryRetry();

                    WriteObject(cts, true);
                }
                else
                {
                    List list = List.GetList(SelectedWeb);
                    if (list == null)
                        throw new PSArgumentException($"No list found with id, title or url '{List}'", "List");
                    var cts = ClientContext.LoadQuery(list.ContentTypes.Include(ct => ct.Id, ct => ct.Name, ct => ct.StringId, ct => ct.Group));
                    ClientContext.ExecuteQueryRetry();
                    WriteObject(cts, true);
                }
            }
        }
    }
}

