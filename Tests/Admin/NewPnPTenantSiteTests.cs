using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Admin
{
    [TestClass]
    public class NewTenantSiteTests
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
        public void NewPnPTenantSiteTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Specifies the title of the new site collection
				var title = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Specifies the full URL of the new site collection. It must be in a valid managed path in the company's site. For example, for company contoso, valid managed paths are https://contoso.sharepoint.com/sites and https://contoso.sharepoint.com/teams.
				var url = "";
				// From Cmdlet Help: Specifies the description of the new site collection. Setting a value for this parameter will override the Wait parameter as we have to set the description after the site has been created.
				var description = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Specifies the user name of the site collection's primary owner. The owner must be a user instead of a security group or an email-enabled security group.
				var owner = "";
				// From Cmdlet Help: Specifies the language of this site collection. For more information, see Locale IDs supported by SharePoint at https://github.com/pnp/PnP-PowerShell/wiki/Supported-LCIDs-by-SharePoint. To get the list of supported languages on a SharePoint environment use: Get-PnPAvailableLanguage.
				var lcid = "";
				// From Cmdlet Help: Specifies the site collection template type. Use the Get-PnPWebTemplates cmdlet to get the list of valid templates. If no template is specified, one can be added later. The Template and LocaleId parameters must be a valid combination as returned from the Get-PnPWebTemplates cmdlet.
				var template = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Use Get-PnPTimeZoneId to retrieve possible timezone values
				var timeZone = "";
				// From Cmdlet Help: Specifies the quota for this site collection in Sandboxed Solutions units. This value must not exceed the company's aggregate available Sandboxed Solutions quota. The default value is 0. For more information, see Resource Usage Limits on Sandboxed Solutions in SharePoint 2010 : http://msdn.microsoft.com/en-us/library/gg615462.aspx.
				var resourceQuota = "";
				// From Cmdlet Help: Specifies the warning level for the resource quota. This value must not exceed the value set for the ResourceQuota parameter
				var resourceQuotaWarningLevel = "";
				// From Cmdlet Help: Specifies the storage quota for this site collection in megabytes. This value must not exceed the company's available quota.
				var storageQuota = "";
				// From Cmdlet Help: Specifies the warning level for the storage quota in megabytes. This value must not exceed the values set for the StorageQuota parameter
				var storageQuotaWarningLevel = "";
				// From Cmdlet Help: Specifies if any existing site with the same URL should be removed from the recycle bin
				var removeDeletedSite = "";
				var wait = "";
				// From Cmdlet Help: Do not ask for confirmation.
				var force = "";

                var results = scope.ExecuteCommand("New-PnPTenantSite",
					new CommandParameter("Title", title),
					new CommandParameter("Url", url),
					new CommandParameter("Description", description),
					new CommandParameter("Owner", owner),
					new CommandParameter("Lcid", lcid),
					new CommandParameter("Template", template),
					new CommandParameter("TimeZone", timeZone),
					new CommandParameter("ResourceQuota", resourceQuota),
					new CommandParameter("ResourceQuotaWarningLevel", resourceQuotaWarningLevel),
					new CommandParameter("StorageQuota", storageQuota),
					new CommandParameter("StorageQuotaWarningLevel", storageQuotaWarningLevel),
					new CommandParameter("RemoveDeletedSite", removeDeletedSite),
					new CommandParameter("Wait", wait),
					new CommandParameter("Force", force));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            