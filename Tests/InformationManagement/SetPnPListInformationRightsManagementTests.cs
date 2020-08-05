using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.InformationManagement
{
    [TestClass]
    public class SetListInformationRightsManagementTests
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
        public void SetPnPListInformationRightsManagementTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The list to set Information Rights Management (IRM) settings for.
				var list = "";
				// From Cmdlet Help: Specifies whether Information Rights Management (IRM) is enabled for the list.
				var enable = "";
				// From Cmdlet Help: Specifies whether Information Rights Management (IRM) expiration is enabled for the list.
				var enableExpiration = "";
				// From Cmdlet Help: Specifies whether Information Rights Management (IRM) rejection is enabled for the list.
				var enableRejection = "";
				// From Cmdlet Help: Sets a value indicating whether the viewer can print the downloaded document.
				var allowPrint = "";
				// From Cmdlet Help: Sets a value indicating whether the viewer can run a script on the downloaded document.
				var allowScript = "";
				// From Cmdlet Help: Sets a value indicating whether the viewer can write on a copy of the downloaded document.
				var allowWriteCopy = "";
				// From Cmdlet Help: Sets a value indicating whether to block Office Web Application Companion applications (WACs) from showing this document.
				var disableDocumentBrowserView = "";
				// From Cmdlet Help: Sets the number of days after which the downloaded document will expire.
				var documentAccessExpireDays = "";
				// From Cmdlet Help: Sets the date after which the Information Rights Management (IRM) protection of this document library will stop.
				var documentLibraryProtectionExpireDate = "";
				// From Cmdlet Help: Sets a value indicating whether the downloaded document will expire.
				var enableDocumentAccessExpire = "";
				// From Cmdlet Help: Sets a value indicating whether to enable Office Web Application Companion applications (WACs) to publishing view.
				var enableDocumentBrowserPublishingView = "";
				// From Cmdlet Help: Sets a value indicating whether the permission of the downloaded document is applicable to a group.
				var enableGroupProtection = "";
				// From Cmdlet Help: Sets whether a user must verify their credentials after some interval.
				var enableLicenseCacheExpire = "";
				// From Cmdlet Help: Sets the number of days that the application that opens the document caches the IRM license. When these elapse, the application will connect to the IRM server to validate the license.
				var licenseCacheExpireDays = "";
				// From Cmdlet Help: Sets the group name (email address) that the permission is also applicable to.
				var groupName = "";
				// From Cmdlet Help: Sets the permission policy description.
				var policyDescription = "";
				// From Cmdlet Help: Sets the permission policy title.
				var policyTitle = "";
				var templateId = "";

                var results = scope.ExecuteCommand("Set-PnPListInformationRightsManagement",
					new CommandParameter("List", list),
					new CommandParameter("Enable", enable),
					new CommandParameter("EnableExpiration", enableExpiration),
					new CommandParameter("EnableRejection", enableRejection),
					new CommandParameter("AllowPrint", allowPrint),
					new CommandParameter("AllowScript", allowScript),
					new CommandParameter("AllowWriteCopy", allowWriteCopy),
					new CommandParameter("DisableDocumentBrowserView", disableDocumentBrowserView),
					new CommandParameter("DocumentAccessExpireDays", documentAccessExpireDays),
					new CommandParameter("DocumentLibraryProtectionExpireDate", documentLibraryProtectionExpireDate),
					new CommandParameter("EnableDocumentAccessExpire", enableDocumentAccessExpire),
					new CommandParameter("EnableDocumentBrowserPublishingView", enableDocumentBrowserPublishingView),
					new CommandParameter("EnableGroupProtection", enableGroupProtection),
					new CommandParameter("EnableLicenseCacheExpire", enableLicenseCacheExpire),
					new CommandParameter("LicenseCacheExpireDays", licenseCacheExpireDays),
					new CommandParameter("GroupName", groupName),
					new CommandParameter("PolicyDescription", policyDescription),
					new CommandParameter("PolicyTitle", policyTitle),
					new CommandParameter("TemplateId", templateId));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            