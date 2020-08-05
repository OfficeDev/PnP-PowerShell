using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.WebParts
{
    [Cmdlet(VerbsCommon.Set, "PnPWebPartProperty")]
    [CmdletHelp("Sets a web part property",
        Category = CmdletHelpCategory.WebParts)]
    [CmdletExample(
        Code = @"PS:> Set-PnPWebPartProperty -ServerRelativePageUrl /sites/demo/sitepages/home.aspx -Identity ccd2c98a-c9ae-483b-ae72-19992d583914 -Key ""Title"" -Value ""New Title"" ",
        Remarks = "Sets the title property of the web part.",
        SortOrder = 1)]
    public class SetWebPartProperty : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Full server relative url of the web part page, e.g. /sites/demo/sitepages/home.aspx")]
        [Alias("PageUrl")]
        public string ServerRelativePageUrl = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "The Guid of the web part")]
        public GuidPipeBind Identity;

        [Parameter(Mandatory = true, HelpMessage = "Name of a single property to be set")]
        public string Key = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = "Value of the property to be set")]
        public PSObject Value = string.Empty;

        protected override void ExecuteCmdlet()
        {
            var serverRelativeWebUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);

            if (!ServerRelativePageUrl.ToLowerInvariant().StartsWith(serverRelativeWebUrl.ToLowerInvariant()))
            {
                ServerRelativePageUrl = UrlUtility.Combine(serverRelativeWebUrl, ServerRelativePageUrl);
            }

            if (Value.BaseObject is string)
            {
                SelectedWeb.SetWebPartProperty(Key, Value.ToString(), Identity.Id, ServerRelativePageUrl);
            }
            else if (Value.BaseObject is int)
            {
                SelectedWeb.SetWebPartProperty(Key, (int)Value.BaseObject, Identity.Id, ServerRelativePageUrl);
            } else if (Value.BaseObject is bool)
            {
                SelectedWeb.SetWebPartProperty(Key, (bool)Value.BaseObject, Identity.Id, ServerRelativePageUrl);
            }
            else
            {
                ThrowTerminatingError(new ErrorRecord(new Exception("Type of value is not supported. Has to be of type string, int or bool"), "UNSUPPORTEDTYPE",ErrorCategory.InvalidType, this));
            }
        }
    }
}
