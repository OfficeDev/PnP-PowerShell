using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Base
{
    [TestClass]
    public class InitializePowerShellAuthenticationTests
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
        public void InitializePnPPowerShellAuthenticationTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The name of the Azure AD Application to create
				var applicationName = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The identifier of your tenant, e.g. mytenant.onmicrosoft.com
				var tenant = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Password for the certificate being created
				var certificatePath = "";
				// From Cmdlet Help: Common Name (e.g. server FQDN or YOUR name). defaults to 'pnp.contoso.com'
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
				// From Cmdlet Help: Number of years until expiration (default is 10, max is 30)
				var validYears = "";
				// From Cmdlet Help: Optional certificate password
				var certificatePassword = "";
				// From Cmdlet Help: Folder to create certificate files in (.CER and .PFX)
				var outVarPath = "";
				// From Cmdlet Help: Local Certificate Store to add the certificate to
				var store = "";

                var results = scope.ExecuteCommand("Initialize-PnPPowerShellAuthentication",
					new CommandParameter("ApplicationName", applicationName),
					new CommandParameter("Tenant", tenant),
					new CommandParameter("CertificatePath", certificatePath),
					new CommandParameter("CommonName", commonName),
					new CommandParameter("Country", country),
					new CommandParameter("State", state),
					new CommandParameter("Locality", locality),
					new CommandParameter("Organization", organization),
					new CommandParameter("OrganizationUnit", organizationUnit),
					new CommandParameter("ValidYears", validYears),
					new CommandParameter("CertificatePassword", certificatePassword),
					new CommandParameter("OutPath", outVarPath),
					new CommandParameter("Store", store));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            