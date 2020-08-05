#if !ONPREMISES
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using Microsoft.Online.SharePoint.TenantManagement;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using OfficeDevPnP.Core.Utilities;

namespace PnP.PowerShell.Commands.UserProfiles
{
    [Cmdlet(VerbsCommon.New, "PnPUPABulkImportJob")]
    [CmdletHelp(@"Submit up a new user profile bulk import job.", "See https://docs.microsoft.com/sharepoint/dev/solution-guidance/bulk-user-profile-update-api-for-sharepoint-online for information on the API and how the bulk import process works.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.UserProfiles)]
    [CmdletExample(
        Code = @"PS:> @"" 
 {
  ""value"": [
    {
      ""IdName"": ""mikaels@contoso.com"",
      ""Department"": ""PnP"",
    },
	{
      ""IdName"": ""vesaj@contoso.com"",
      ""Department"": ""PnP"",
    }    
  ]
}
""@ > profiles.json

PS:> New-PnPUPABulkImportJob -Folder ""Shared Documents"" -Path profiles.json -IdProperty ""IdName"" -UserProfilePropertyMapping @{""Department""=""Department""}",
        Remarks = @"This will submit a new user profile bulk import job to SharePoint Online.", SortOrder = 1)]
    public class NewUPABulkImportJob : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Site or server relative URL of the folder to where you want to store the import job file.")]
        public string Folder;

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "The local file path.")]
        public string Path = string.Empty;

        [Parameter(Mandatory = true, Position = 2, HelpMessage = "Specify user profile property mapping between the import file and UPA property names.")]
        public Hashtable UserProfilePropertyMapping;

        [Parameter(Mandatory = true, Position = 3, HelpMessage = "The name of the identifying property in your file.")]
        public string IdProperty;

        [Parameter(Mandatory = false, Position = 4, HelpMessage = "The type of profile identifier (Email/CloudId/PrincipalName). Defaults to Email.")]
        public ImportProfilePropertiesUserIdType IdType = ImportProfilePropertiesUserIdType.Email;

        protected override void ExecuteCmdlet()
        {
            if (string.IsNullOrWhiteSpace(Path))
            {
                throw new InvalidEnumArgumentException(@"Path cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(IdProperty))
            {
                throw new InvalidEnumArgumentException(@"IdProperty cannot be empty.");
            }
            
            var webCtx = ClientContext.Clone(PnPConnection.CurrentConnection.Url);
            var web = webCtx.Web;
            var webServerRelativeUrl = web.EnsureProperty(w => w.ServerRelativeUrl);
            if (!Folder.ToLower().StartsWith(webServerRelativeUrl))
            {
                Folder = UrlUtility.Combine(webServerRelativeUrl, Folder);
            }
            if (!web.DoesFolderExists(Folder))
            {
                throw new InvalidOperationException($"Folder {Folder} does not exist.");
            }
            var folder = web.GetFolderByServerRelativeUrl(Folder);

            var fileName = System.IO.Path.GetFileName(Path);
            File file = folder.UploadFile(fileName, Path, true);

            
            var o365 = new Office365Tenant(ClientContext);
            var propDictionary = UserProfilePropertyMapping.Cast<DictionaryEntry>().ToDictionary(kvp => (string)kvp.Key, kvp => (string)kvp.Value);
            var url = new Uri(webCtx.Url).GetLeftPart(UriPartial.Authority) + file.ServerRelativeUrl;
            var id = o365.QueueImportProfileProperties(IdType, IdProperty, propDictionary, url);
            ClientContext.ExecuteQueryRetry();

            var job = o365.GetImportProfilePropertyJob(id.Value);
            ClientContext.Load(job);
            ClientContext.ExecuteQueryRetry();
            WriteObject(job);
        }
    }
}
#endif
