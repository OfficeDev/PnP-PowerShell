using System;
using System.Management.Automation;
using System.Security.Cryptography.X509Certificates;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPAzureADManifestKeyCredentials")]
    [CmdletAlias("Get-SPOAzureADManifestKeyCredentials")]
    [CmdletHelp("Creates the JSON snippet that is required for the manifest JSON file for Azure WebApplication / WebAPI apps", 
        Category = CmdletHelpCategory.Base,
        OutputType=typeof(string),
        OutputTypeDescription = "Outputs a JSON formatted string")]
    [CmdletExample(
        Code = @"PS:> Get-PnPAzureADManifestKeyCredentials -CertPath .\mycert.cer",
        Remarks = "Output the JSON snippet which needs to be replaced in the application manifest file", 
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPAzureADManifestKeyCredentials -CertPath .\mycert.cer | Set-Clipboard",
        Remarks = "Output the JSON snippet which needs to be replaced in the application manifest file and copies it to the clipboard",
        SortOrder = 2)]
    public class GetAzureADManifestKeyCredentials : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"Specifies the path to the certificate like .\mycert.cer")]
        public string CertPath;

        protected override void ProcessRecord()
        {
            if (!System.IO.Path.IsPathRooted(CertPath))
            {
                CertPath = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, CertPath);
            }
            var cert = new X509Certificate2(CertPath);

            var rawCert = cert.GetRawCertData();

            var base64Cert = Convert.ToBase64String(rawCert);

            var rawCertHash = cert.GetCertHash();

            var base64CertHash = Convert.ToBase64String(rawCertHash);

            var keyId = Guid.NewGuid().ToString();

            var output = $"\"keyCredentials\": [\n\t{{\n\t\t\"customKeyIdentifier\": \"{base64CertHash}\",\n\t\t\"keyId\": \"{keyId}\",\n\t\t\"type\": \"AsymmetricX509Cert\",\n\t\t\"usage\": \"Verify\",\n\t\t\"value\": \"{base64Cert}\"\n\t}}\n],";

            WriteObject(output);

        }
    }
}
