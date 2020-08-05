using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Lists
{
    [TestClass]
    public class SetListTests
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
        public void SetPnPListTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The ID, Title or Url of the list.
				var identity = "";
				// From Cmdlet Help: Set to $true to enable content types, set to $false to disable content types
				var enableContentTypes = "";
				// From Cmdlet Help: If used the security inheritance is broken for this list
				var breakRoleInheritance = "";
				// From Cmdlet Help: If used the security inheritance is reset for this list (inherited from parent)
				var resetRoleInheritance = "";
				// From Cmdlet Help: If used the roles are copied from the parent web
				var copyRoleAssignments = "";
				// From Cmdlet Help: If used the unique permissions are cleared from child objects and they can inherit role assignments from this object
				var clearSubscopeVars = "";
				// From Cmdlet Help: The title of the list
				var title = "";
				// From Cmdlet Help: The description of the list
				var description = "";
				// From Cmdlet Help: Hide the list from the SharePoint UI. Set to $true to hide, $false to show.
				var hidden = "";
				// From Cmdlet Help: Enable or disable force checkout. Set to $true to enable, $false to disable.
				var forceCheckoutVar = "";
				// From Cmdlet Help: Set the list experience: Auto, NewExperience or ClassicExperience
				var listExperience = "";
				// From Cmdlet Help: Enable or disable attachments. Set to $true to enable, $false to disable.
				var enableAttachments = "";
				// From Cmdlet Help: Enable or disable folder creation. Set to $true to enable, $false to disable.
				var enableFolderCreation = "";
				// From Cmdlet Help: Enable or disable versioning. Set to $true to enable, $false to disable.
				var enableVersioning = "";
				// From Cmdlet Help: Enable or disable minor versions versioning. Set to $true to enable, $false to disable.
				var enableMinorVersions = "";
				// From Cmdlet Help: Maximum major versions to keep
				var majorVersions = "";
				// From Cmdlet Help: Maximum minor versions to keep
				var minorVersions = "";
				// From Cmdlet Help: Enable or disable whether content approval is enabled for the list. Set to $true to enable, $false to disable.
				var enableModeration = "";

                var results = scope.ExecuteCommand("Set-PnPList",
					new CommandParameter("Identity", identity),
					new CommandParameter("EnableContentTypes", enableContentTypes),
					new CommandParameter("BreakRoleInheritance", breakRoleInheritance),
					new CommandParameter("ResetRoleInheritance", resetRoleInheritance),
					new CommandParameter("CopyRoleAssignments", copyRoleAssignments),
					new CommandParameter("ClearSubscopes", clearSubscopeVars),
					new CommandParameter("Title", title),
					new CommandParameter("Description", description),
					new CommandParameter("Hidden", hidden),
					new CommandParameter("ForceCheckout", forceCheckoutVar),
					new CommandParameter("ListExperience", listExperience),
					new CommandParameter("EnableAttachments", enableAttachments),
					new CommandParameter("EnableFolderCreation", enableFolderCreation),
					new CommandParameter("EnableVersioning", enableVersioning),
					new CommandParameter("EnableMinorVersions", enableMinorVersions),
					new CommandParameter("MajorVersions", majorVersions),
					new CommandParameter("MinorVersions", minorVersions),
					new CommandParameter("EnableModeration", enableModeration));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            