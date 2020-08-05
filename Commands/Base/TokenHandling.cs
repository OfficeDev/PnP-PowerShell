using Microsoft.SharePoint.Client;
using PnP.PowerShell.Commands.Model;
using System.Web;

namespace PnP.PowerShell.Commands.Base
{
#if !ONPREMISES
    internal static class TokenHandler
    {
        internal static string AcquireToken(string resource, string scope = null)
        {
            GenericToken token = null;
            if (PnPConnection.CurrentConnection == null)
            {
                return null;
            }
            var tenantId = TenantExtensions.GetTenantIdByUrl(PnPConnection.CurrentConnection.Url);
            if (PnPConnection.CurrentConnection.PSCredential != null)
            {
                if (scope == null && !resource.Equals("graph.microsoft.com", System.StringComparison.OrdinalIgnoreCase))
                {
                    // SharePoint token
                    var scopes = new[] { $"https://{resource}//.default" };
                    token = GenericToken.AcquireDelegatedTokenWithCredentials(PnPConnection.PnPManagementShellClientId, scopes, "https://login.microsoftonline.com/organizations/", PnPConnection.CurrentConnection.PSCredential.UserName, PnPConnection.CurrentConnection.PSCredential.Password);
                }
                else
                {

                    token = GenericToken.AcquireDelegatedTokenWithCredentials(PnPConnection.PnPManagementShellClientId, new[] { scope }, "https://login.microsoftonline.com/organizations/", PnPConnection.CurrentConnection.PSCredential.UserName, PnPConnection.CurrentConnection.PSCredential.Password);
                }
            }
            else if (!string.IsNullOrEmpty(PnPConnection.CurrentConnection.ClientId) && !string.IsNullOrEmpty(PnPConnection.CurrentConnection.ClientSecret))
            {
                var clientId = PnPConnection.CurrentConnection.ClientId;
                var clientSecret = HttpUtility.UrlEncode(PnPConnection.CurrentConnection.ClientSecret);
                if (scope == null && !resource.Equals("graph.microsoft.com", System.StringComparison.OrdinalIgnoreCase))
                {
                    // SharePoint token
                    var scopes = new[] { $"https://{resource}//.default" };
                    token = GenericToken.AcquireApplicationToken(tenantId, clientId, "https://login.microsoftonline/organizations/", scopes, clientSecret);
                }
                else
                {
                    token = GenericToken.AcquireApplicationToken(tenantId, clientId, "https://login.microsoftonline.com/organizations/", new[] { scope }, clientSecret);
                }
            }
            if (token != null)
            {
                return token.AccessToken;
            }
            return null;
        }

        //internal static string AcquireToken(string resource, string scope = null)
        //{
        //    if (PnPConnection.CurrentConnection == null)
        //    {
        //        return null;
        //    }

        //    var tenantId = TenantExtensions.GetTenantIdByUrl(PnPConnection.CurrentConnection.Url);

        //    if (tenantId == null) return null;

        //    string body = "";
        //    if (PnPConnection.CurrentConnection.PSCredential != null)
        //    {
        //        var clientId = "31359c7f-bd7e-475c-86db-fdb8c937548e";
        //        var username = PnPConnection.CurrentConnection.PSCredential.UserName;
        //        var password = EncryptionUtility.ToInsecureString(PnPConnection.CurrentConnection.PSCredential.Password);
        //        body = $"grant_type=password&client_id={clientId}&username={username}&password={password}&resource=https://{resource}";
        //    }
        //    else if (!string.IsNullOrEmpty(PnPConnection.CurrentConnection.ClientId) && !string.IsNullOrEmpty(PnPConnection.CurrentConnection.ClientSecret))
        //    {
        //        var clientId = PnPConnection.CurrentConnection.ClientId;
        //        var clientSecret = HttpUtility.UrlEncode(PnPConnection.CurrentConnection.ClientSecret);
        //        body = $"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}&resource=https://{resource}";
        //    }
        //    else
        //    {
        //        throw new System.UnauthorizedAccessException("Specify PowerShell Credentials or AppId and AppSecret");
        //    }

        //    var response = HttpHelper.MakePostRequestForString($"https://login.microsoftonline.com/{tenantId}/oauth2/token", body, "application/x-www-form-urlencoded");
        //    try
        //    {
        //        using (var jdoc = JsonDocument.Parse(response))
        //        {
        //            return jdoc.RootElement.GetProperty("access_token").GetString();
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
    }
#endif
}
