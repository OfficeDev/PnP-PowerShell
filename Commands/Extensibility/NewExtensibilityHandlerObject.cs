using System.Management.Automation;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Extensibility
{
    [Cmdlet(VerbsCommon.New, "PnPExtensbilityHandlerObject")]
    [CmdletAlias("New-SPOExtensbilityHandlerObject")]
    [CmdletHelp("Creates an ExtensibilityHandler Object, to be used by the Get-SPOProvisioningTemplate cmdlet", 
        Category = CmdletHelpCategory.Features,
        OutputType=typeof(ExtensibilityHandler))]
    [CmdletExample(
        Code = @"
PS:> $handler = New-PnPExtensbilityHandlerObject -Assembly Contoso.Core.Handlers -Type Contoso.Core.Handlers.MyExtensibilityHandler
PS:> Get-PnPProvisioningTemplate -Out NewTemplate.xml -ExtensibilityHandlers $handler",
        Remarks = @"This will create a new ExtensibilityHandler object that is run during extraction of the template", 
        SortOrder = 1)]
  
    public class NewExtensibilityHandlerObject : PSCmdlet
    {
        [Parameter(Mandatory = true, Position=0, ValueFromPipeline=true, HelpMessage = "The full assembly name of the handler")]
        public string Assembly;

        [Parameter(Mandatory = true, HelpMessage = "The type of the handler")]
        public string Type;

        [Parameter(Mandatory = false, HelpMessage = "Any configuration data you want to send to the handler")]
        public string Configuration;

        [Parameter(Mandatory = false, HelpMessage = "If set, the handler will be disabled")]
        public SwitchParameter Disabled;


        protected override void ProcessRecord()
        {
            var handler = new ExtensibilityHandler
            {
                Assembly = Assembly,
                Type = Type,
                Configuration = Configuration,
                Enabled = !Disabled
            };
            WriteObject(handler);
        }

    }
}
