using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Branding
{
    [Cmdlet(VerbsCommon.Get, "PnPTheme")]
    [CmdletAlias("Get-SPOTheme")]
    [CmdletHelp("Returns the current theme/composed look of the current web.",
        Category = CmdletHelpCategory.Branding,
        OutputType = typeof(OfficeDevPnP.Core.Entities.ThemeEntity))]
    [CmdletExample(
        Code = @"PS:> Get-PnPTheme",
        Remarks = "Returns the current composed look of the current web.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPTheme -DetectCurrentComposedLook",
        Remarks = "Returns the current composed look of the current web, and will try to detect the currently applied composed look based upon the actual site. Without this switch the cmdlet will first check for the presence of a property bag variable called _PnP_ProvisioningTemplateComposedLookInfo that contains composed look information when applied through the provisioning engine or the Set-PnPTheme cmdlet.",
        SortOrder = 2)]
    public class GetTheme : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Specify this switch to not use the PnP Provisioning engine based composed look information but try to detect the current composed look as is.")]
        public SwitchParameter DetectCurrentComposedLook;

        protected override void ExecuteCmdlet()
        {
            if (SelectedWeb.PropertyBagContainsKey("_PnP_ProvisioningTemplateComposedLookInfo") && !DetectCurrentComposedLook)
            {
                try
                {
                    WriteWarning("The information presented here is based upon the fact that the theme has been set using either the PnP Provisioning Engine or using the Set-PnPTheme cmdlet. This information is retrieved from a propertybag value and may differ from the actual site.");
                    var composedLook = JsonConvert.DeserializeObject<ComposedLook>(SelectedWeb.GetPropertyBagValueString("_PnP_ProvisioningTemplateComposedLookInfo", ""));
                    WriteObject(composedLook);
                }
                catch
                {
                    var themeEntity = SelectedWeb.GetCurrentComposedLook();
                    WriteObject(themeEntity);
                }

            }
            else
            {
                var themeEntity = SelectedWeb.GetCurrentComposedLook();
                WriteObject(themeEntity);
            }
        }
    }
}
