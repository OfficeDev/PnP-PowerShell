using System;
using System.Management.Automation;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using System.Net;

namespace OfficeDevPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Get, "SPOAuthenticationRealm")]
    [CmdletHelp("Gets the authentication realm for the current web", 
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(
        Code = @"PS:> Get-SPOAuthenticationRealm", 
        Remarks = @"This will get the authentication realm for the current connected site",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-SPOAuthenticationRealm -Url https://contoso.sharepoint.com",
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
                if (e.Response == null)
                {
                }

                var bearerResponseHeader = e.Response.Headers["WWW-Authenticate"];
                if (string.IsNullOrEmpty(bearerResponseHeader))
                {
                }

                const string bearer = "Bearer realm=\"";
                var bearerIndex = bearerResponseHeader.IndexOf(bearer, StringComparison.Ordinal);
                if (bearerIndex < 0)
                {
                }

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
