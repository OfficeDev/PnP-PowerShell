using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using OfficeDevPnP.Core.Utilities;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPPropertyBag")]
    [CmdletHelp("Returns the property bag values.",
        Category = CmdletHelpCategory.Webs,
        OutputType = typeof(PropertyBagValue))]
    [CmdletExample(
       Code = @"PS:> Get-PnPPropertyBag",
       Remarks = "This will return all web property bag values",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Get-PnPPropertyBag -Key MyKey",
       Remarks = "This will return the value of the key MyKey from the web property bag",
       SortOrder = 2)]
    [CmdletExample(
       Code = @"PS:> Get-PnPPropertyBag -Folder /MyFolder",
       Remarks = "This will return all property bag values for the folder MyFolder which is located in the root of the current web",
       SortOrder = 3)]
    [CmdletExample(
       Code = @"PS:> Get-PnPPropertyBag -Folder /MyFolder -Key vti_mykey",
       Remarks = "This will return the value of the key vti_mykey from the folder MyFolder which is located in the root of the current web",
       SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Get-PnPPropertyBag -Folder / -Key vti_mykey",
        Remarks = "This will return the value of the key vti_mykey from the root folder of the current web",
        SortOrder = 5)]
    public class GetPropertyBag : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ValueFromPipeline = true, HelpMessage = "Key that should be looked up")]
        public string Key = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Site relative url of the folder. See examples for use.")]
        public string Folder = string.Empty;

        protected override void ExecuteCmdlet()
        {
            if (string.IsNullOrEmpty(Folder))
            {
                if (!string.IsNullOrEmpty(Key))
                {
                    WriteObject(SelectedWeb.GetPropertyBagValueString(Key, string.Empty));
                }
                else
                {
                    SelectedWeb.EnsureProperty(w => w.AllProperties);
                    
                    var values = SelectedWeb.AllProperties.FieldValues.Select(x => new PropertyBagValue() { Key = x.Key, Value = x.Value });
                    WriteObject(values, true);
                }
            }
            else
            {
                // Folder Property Bag

                SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
                
                var folderUrl = UrlUtility.Combine(SelectedWeb.ServerRelativeUrl, Folder);
#if ONPREMISES
                var folder = SelectedWeb.GetFolderByServerRelativeUrl(folderUrl);
#else
                var folder = SelectedWeb.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(folderUrl));
#endif
                folder.EnsureProperty(f => f.Properties);
                
                if (!string.IsNullOrEmpty(Key))
                {
                    var value = folder.Properties.FieldValues.FirstOrDefault(x => x.Key == Key);
                    WriteObject(value.Value, true);
                }
                else
                {
                    var values = folder.Properties.FieldValues.Select(x => new PropertyBagValue() { Key = x.Key, Value = x.Value });
                    WriteObject(values, true);
                }

            }
        }
    }

    public class PropertyBagValue
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
