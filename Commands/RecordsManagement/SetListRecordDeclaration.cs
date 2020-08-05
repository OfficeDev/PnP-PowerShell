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
    [Cmdlet(VerbsCommon.Set, "PnPListRecordDeclaration")]
    [CmdletHelp("Sets the manual record declaration settings for a list",
        Category = CmdletHelpCategory.RecordsManagement,
        Description = @"The RecordDeclaration parameter supports 4 values:

AlwaysAllowManualDeclaration
NeverAllowManualDeclaration
UseSiteCollectionDefaults
",
        SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
        Code = @"PS:> Set-PnPListRecordDeclaration -List ""Documents"" -ManualRecordDeclaration NeverAllowManualDeclaration",
        Remarks = "Sets the manual record declaration to never allow",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPListRecordDeclaration -List ""Documents"" -AutoRecordDeclaration $true",
        Remarks = "Turns on auto record declaration for the list",
        SortOrder = 2)]

    public class SetListRecordDeclaration : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"The List to set the manual record declaration settings for")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = @"Defines the manual record declaration setting for the lists")]
        public EcmListManualRecordDeclaration? ManualRecordDeclaration;

        [Parameter(Mandatory = false, HelpMessage = @"Defines if you want to set auto record declaration on the list")]
        public bool? AutoRecordDeclaration;
        
        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);

            if (ManualRecordDeclaration.HasValue)
            {
                list.SetListManualRecordDeclaration(ManualRecordDeclaration.Value);
            }

            if(AutoRecordDeclaration.HasValue)
            {
                list.SetListAutoRecordDeclaration(AutoRecordDeclaration.Value);
            }
        }

    }
}
