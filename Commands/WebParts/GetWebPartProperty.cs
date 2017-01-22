using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.WebParts
{
    [Cmdlet(VerbsCommon.Get, "PnPWebPartProperty")]
    [CmdletAlias("Get-SPOWebPartProperty")]
    [CmdletHelp("Returns a web part property", 
        Category = CmdletHelpCategory.WebParts)]
    [CmdletExample(
        Code = @"PS:> Get-PnPWebPartProperty -ServerRelativePageUrl /sites/demo/sitepages/home.aspx -Identity ccd2c98a-c9ae-483b-ae72-19992d583914",
        Remarks = "Returns all properties of the webpart.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPWebPartProperty -ServerRelativePageUrl /sites/demo/sitepages/home.aspx -Identity ccd2c98a-c9ae-483b-ae72-19992d583914 -Key ""Title""",
        Remarks = "Returns the title property of the webpart.",
        SortOrder = 2)]
    public class GetWebPartProperty : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Full server relative URL of the webpart page, e.g. /sites/mysite/sitepages/home.aspx")]
        [Alias("PageUrl")]
        public string ServerRelativePageUrl = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The id of the webpart")]
        public GuidPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Name of a single property to be returned")]
        public string Key;

        protected override void ExecuteCmdlet()
        {
            var serverRelativeWebUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

            if (!ServerRelativePageUrl.ToLowerInvariant().StartsWith(serverRelativeWebUrl.ToLowerInvariant()))
            {
                ServerRelativePageUrl = UrlUtility.Combine(serverRelativeWebUrl, ServerRelativePageUrl);
            }


            var properties = SelectedWeb.GetWebPartProperties(Identity.Id, ServerRelativePageUrl);
            var values = properties.FieldValues.Select(x => new PropertyBagValue() { Key = x.Key, Value = x.Value });
            if (!string.IsNullOrEmpty(Key))
            {
                var value = values.FirstOrDefault(v => v.Key == Key);
                if (value != null)
                {
                    WriteObject(value.Value);
                }
            }
            else
            {
                WriteObject(values, true);
            }
        }



    }
}
