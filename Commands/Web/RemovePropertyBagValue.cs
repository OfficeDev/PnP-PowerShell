using Microsoft.SharePoint.Client;
using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Utilities;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Remove, "PnPPropertyBagValue", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    [CmdletAlias("Remove-SPOPropertyBagValue")]
    [CmdletHelp("Removes a value from the property bag",
        Category = CmdletHelpCategory.Webs)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPPropertyBagValue -Key MyKey",
        Remarks = "This will remove the value with key MyKey from the current web property bag",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPPropertyBagValue -Key MyKey -Folder /MyFolder",
        Remarks = "This will remove the value with key MyKey from the folder MyFolder which is located in the root folder of the current web",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Remove-PnPPropertyBagValue -Key MyKey -Folder /",
        Remarks = "This will remove the value with key MyKey from the root folder of the current web",
        SortOrder = 3)]
    public class RemovePropertyBagValue : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Key of the property bag value to be removed")]
        public string Key;

        [Parameter(Mandatory = false, HelpMessage = "Site relative url of the folder. See examples for use.")]
        public string Folder;

        [Parameter(Mandatory = false)]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            if (string.IsNullOrEmpty(Folder))
            {
                if (SelectedWeb.PropertyBagContainsKey(Key))
                {
                    if (Force || ShouldContinue(string.Format(Properties.Resources.Delete0, Key), Properties.Resources.Confirm))
                    {
                        SelectedWeb.RemovePropertyBagValue(Key);
                    }
                }
            }
            else
            {
                SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

                var folderUrl = UrlUtility.Combine(SelectedWeb.ServerRelativeUrl, Folder);
                var folder = SelectedWeb.GetFolderByServerRelativeUrl(folderUrl);

                folder.EnsureProperty(f => f.Properties);

                if (folder.Properties.FieldValues.ContainsKey(Key))
                {
                    if (Force || ShouldContinue(string.Format(Properties.Resources.Delete0, Key), Properties.Resources.Confirm))
                    {

                        folder.Properties[Key] = null;
                        folder.Properties.FieldValues.Remove(Key);
                        folder.Update();
                        ClientContext.ExecuteQueryRetry();
                    }
                }
            }
        }
    }
}
