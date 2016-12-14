using System;
using System.Management.Automation;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System.Net;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "PnPAuthenticationRealm")]
    [CmdletAlias("Get-SPOAuthenticationRealm")]
    [CmdletHelp("Gets the authentication realm for the current web", 
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(
        Code = @"PS:> Get-PnPAuthenticationRealm", 
        Remarks = @"This will get the authentication realm for the current connected site",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPAuthenticationRealm -Url https://contoso.sharepoint.com",
        Remarks = @"This will get the authentication realm for https://contoso.sharepoint.com",
        SortOrder = 2)]
    public class GetAuthenticationRealm : SPOCmdlet
    {

        [Parameter(Mandatory = false, Position=0, ValueFromPipeline=true, HelpMessage = "Specifies the URL of the site")]
        public string Url;

        protected override void ProcessRecord()
        {
            if(string.IsNullOrEmpty(Url))
            {
                Url = ClientContext.Url;
            }
            WebRequest request = WebRequest.Create(new Uri(Url) + "/_vti_bin/client.svc");
            request.Headers.Add("Authorization: Bearer ");

            try
            {
                using (request.GetResponse())
                {
                }
            }
            catch (WebException e)
            {
                var bearerResponseHeader = e.Response.Headers["WWW-Authenticate"];

                const string bearer = "Bearer realm=\"";
                var bearerIndex = bearerResponseHeader.IndexOf(bearer, StringComparison.Ordinal);

                var realmIndex = bearerIndex + bearer.Length;

                if (bearerResponseHeader.Length >= realmIndex + 36)
                {
                    var targetRealm = bearerResponseHeader.Substring(realmIndex, 36);

                    Guid realmGuid;

                    if (Guid.TryParse(targetRealm, out realmGuid))
                    {
                        WriteObject(targetRealm);
                    }
                }
            }
        }


    }
}
