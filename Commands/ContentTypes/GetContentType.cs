using OfficeDevPnP.PowerShell.Commands.Base.PipeBinds;
using Microsoft.SharePoint.Client;
using System.Management.Automation;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "SPOContentType")]
    [CmdletHelp("Retrieves a content type",
        Category = CmdletHelpCategory.ContentTypes)]
    [CmdletExample(
        Code = @"PS:> Get-SPOContentType ",
        Remarks = @"This will get a listing of all available content types within the current web",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-SPOContentType -InSiteHierarchy",
        Remarks = @"This will get a listing of all available content types within the site collection",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-SPOContentType -Identity ""Project Document""",
        Remarks = @"This will get a listing of content types within the current context",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-SPOContentType -List ""Documents""",
        Remarks = @"This will get a listing of all available content types within the list ""Documents""",
        SortOrder = 4)]
    public class GetContentType : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "Name or ID of the content type to retrieve")]
        public ContentTypePipeBind Identity;
        [Parameter(Mandatory = false, Position = 1, ValueFromPipeline = true, HelpMessage = "List to query")]
        public ListPipeBind List;
        [Parameter(Mandatory = false, Position = 1, ValueFromPipeline = false, HelpMessage = "Search site hierarchy for content types")]
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

                    ClientContext.ExecuteQueryRetry();
                    if (!string.IsNullOrEmpty(Identity.Id))
                    {
                        var cts = ClientContext.LoadQuery(list.ContentTypes.Include(ct => ct.Id, ct => ct.Name, ct => ct.StringId, ct => ct.Group).Where(ct => ct.StringId == Identity.Id));
                        ClientContext.ExecuteQueryRetry();
                        WriteObject(cts, true);
                    }
                    else
                    {
                        var cts = ClientContext.LoadQuery(list.ContentTypes.Include(ct => ct.Id, ct => ct.Name, ct => ct.StringId, ct => ct.Group).Where(ct => ct.Name == Identity.Name));
                        ClientContext.ExecuteQueryRetry();
                        WriteObject(cts, true);
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
                } else
                {
                    List list = List.GetList(SelectedWeb);
                    var cts = ClientContext.LoadQuery(list.ContentTypes.Include(ct => ct.Id, ct => ct.Name, ct => ct.StringId, ct => ct.Group));
                    ClientContext.ExecuteQueryRetry();
                    WriteObject(cts, true);
                }
            }
        }
    }
}

