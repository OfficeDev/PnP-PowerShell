using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.RecordsManagement
{
    [Cmdlet(VerbsCommon.Get, "PnPListRecordDeclaration")]
    [CmdletHelp("Returns the manual record declaration settings for a list",
        Category = CmdletHelpCategory.RecordsManagement, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Get-PnPListRecordDeclaration -List ""Documents""",
        Remarks = @"Returns the record declaration setting for the list ""Documents""",
        SortOrder = 1)]
      
    public class GetListRecordDeclaration : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The list to retrieve the record declaration settings for")]
        public ListPipeBind List;

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);

            var d = new
            {
                ManualRecordDeclaration = list.GetListManualRecordDeclaration(),
                AutoRecordDeclaration = list.GetListAutoRecordDeclaration()
            };
            WriteObject(d);
        }
    }
}
