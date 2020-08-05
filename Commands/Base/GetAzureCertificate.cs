using System.Management.Automation;
using PnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using PnP.PowerShell.Commands.Utilities;

namespace PnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "PnPAzureCertificate", DefaultParameterSetName = "SELF")]
    [CmdletHelp(@"Get PEM values and manifest settings for an existing certificate (.pfx) for use when using CSOM via an app-only ADAL application.

See https://github.com/SharePoint/PnP-PowerShell/tree/master/Samples/SharePoint.ConnectUsingAppPermissions for a sample on how to get started.

KeyCredentials contains the ADAL app manifest sections.

Certificate contains the PEM encoded certificate.

PrivateKey contains the PEM encoded private key of the certificate.",
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(
        Code = @"PS:> Get-PnPAzureCertificate -CertificatePath ""mycert.pfx""",
        Remarks = @"This will output PEM values and ADAL app manifest settings for the certificate mycert.pfx.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPAzureCertificate -CertificatePath ""mycert.pfx"" -CertificatePassword (ConvertTo-SecureString -String ""YourPassword"" -AsPlainText -Force)",
        Remarks = @"This will output PEM values and ADAL app manifest settings for the certificate mycert.pfx which has the password YourPassword.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPAzureCertificate -CertificatePath ""mycert.cer"" | clip",
        Remarks = "Output the JSON snippet which needs to be replaced in the application manifest file and copies it to the clipboard",
        SortOrder = 3)]
    public class GetPnPAdalCertificate : PSCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Path to the certificate (*.pfx)")]
        public string CertificatePath;

        [Parameter(Mandatory = false, HelpMessage = "Password to the certificate (*.pfx)")]
        public SecureString CertificatePassword;

        protected override void ProcessRecord()
        {
            if (!Path.IsPathRooted(CertificatePath))
            {
                CertificatePath = Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, CertificatePath);
            }
            var certificate = new X509Certificate2(CertificatePath, CertificatePassword, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
            var rawCert = certificate.GetRawCertData();
            var base64Cert = Convert.ToBase64String(rawCert);
            var rawCertHash = certificate.GetCertHash();
            var base64CertHash = Convert.ToBase64String(rawCertHash);
            var keyId = Guid.NewGuid();

            var template = @"
{{
    ""customKeyIdentifier"": ""{0}"",
    ""keyId"": ""{1}"",
    ""type"": ""AsymmetricX509Cert"",
    ""usage"": ""Verify"",
    ""value"":  ""{2}""
}}
";
            var manifestEntry = string.Format(template, base64CertHash, keyId, base64Cert);

            var record = new PSObject();
            record.Properties.Add(new PSVariableProperty(new PSVariable("Subject", certificate.Subject)));
            record.Properties.Add(new PSVariableProperty(new PSVariable("ValidFrom", certificate.NotBefore)));
            record.Properties.Add(new PSVariableProperty(new PSVariable("ValidTo", certificate.NotAfter)));
            record.Properties.Add(new PSVariableProperty(new PSVariable("Thumbprint", certificate.Thumbprint)));

            record.Properties.Add(new PSVariableProperty(new PSVariable("KeyCredentials", manifestEntry)));
            record.Properties.Add(new PSVariableProperty(new PSVariable("Certificate", CertificateHelper.CertificateToBase64(certificate))));
            record.Properties.Add(new PSVariableProperty(new PSVariable("PrivateKey", CertificateHelper.PrivateKeyToBase64(certificate))));

            WriteObject(record);
        }
    }
}
