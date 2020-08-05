using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Base
{
    [TestClass]
    public class NewPnPAdalCertificateTests
    {
        #region Test Setup/CleanUp
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            // This runs on class level once before all tests run
            //using (var ctx = TestCommon.CreateClientContext())
            //{
            //}
        }

        [ClassCleanup]
        public static void Cleanup(TestContext testContext)
        {
            // This runs on class level once
            //using (var ctx = TestCommon.CreateClientContext())
            //{
            //}
        }

        [TestInitialize]
        public void Initialize()
        {
            using (var scope = new PSTestScope())
            {
                // Example
                // scope.ExecuteCommand("cmdlet", new CommandParameter("param1", prop));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var scope = new PSTestScope())
            {
                try
                {
                    // Do Test Setup - Note, this runs PER test
                }
                catch (Exception)
                {
                    // Describe Exception
                }
            }
        }
        #endregion

        #region Scaffolded Cmdlet Tests
        //TODO: This is a scaffold of the cmdlet - complete the unit test
        //[TestMethod]
        public void NewPnPAzureCertificateTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Common Name (e.g. server FQDN or YOUR name) [pnp.contoso.com]
				var commonName = "";
				// From Cmdlet Help: Country Name (2 letter code)
				var country = "";
				// From Cmdlet Help: State or Province Name (full name)
				var state = "";
				// From Cmdlet Help: Locality Name (eg, city)
				var locality = "";
				// From Cmdlet Help: Organization Name (eg, company)
				var organization = "";
				// From Cmdlet Help: Organizational Unit Name (eg, section)
				var organizationUnit = "";
				// From Cmdlet Help: Filename to write to, optionally including full path (.pfx)
				var outVar = "";
				// From Cmdlet Help: Filename to write to, optionally including full path (.pfx)
				var outVarPfx = "";
				// From Cmdlet Help: Filename to write to, optionally including full path (.cer)
				var outVarCert = "";
				// From Cmdlet Help: Number of years until expiration (default is 10, max is 30)
				var validYears = "";
				// From Cmdlet Help: Optional certificate password
				var certificatePassword = "";

                var results = scope.ExecuteCommand("New-PnPAzureCertificate",
					new CommandParameter("CommonName", commonName),
					new CommandParameter("Country", country),
					new CommandParameter("State", state),
					new CommandParameter("Locality", locality),
					new CommandParameter("Organization", organization),
					new CommandParameter("OrganizationUnit", organizationUnit),
					new CommandParameter("Out", outVar),
					new CommandParameter("OutPfx", outVarPfx),
					new CommandParameter("OutCert", outVarCert),
					new CommandParameter("ValidYears", validYears),
					new CommandParameter("CertificatePassword", certificatePassword));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            