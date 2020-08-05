using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Site
{
    [TestClass]
    public class SetAuditingTests
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
        public void SetPnPAuditingTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Enable all audit flags
				var enableAll = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Disable all audit flags
				var disableAll = "";
				// From Cmdlet Help: Set the retention time
				var retentionTime = "";
				// From Cmdlet Help: Trim the audit log
				var trimAuditLog = "";
				// From Cmdlet Help: Audit editing items
				var editItems = "";
				// From Cmdlet Help: Audit checking out or checking in items
				var checkOutCheckInItems = "";
				// From Cmdlet Help: Audit moving or copying items to another location in the site.
				var moveCopyItems = "";
				// From Cmdlet Help: Audit deleting or restoring items
				var deleteRestoreItems = "";
				// From Cmdlet Help: Audit editing content types and columns
				var editContentTypesColumns = "";
				// From Cmdlet Help: Audit searching site content
				var searchContent = "";
				// From Cmdlet Help: Audit editing users and permissions
				var editUsersPermissions = "";

                var results = scope.ExecuteCommand("Set-PnPAuditing",
					new CommandParameter("EnableAll", enableAll),
					new CommandParameter("DisableAll", disableAll),
					new CommandParameter("RetentionTime", retentionTime),
					new CommandParameter("TrimAuditLog", trimAuditLog),
					new CommandParameter("EditItems", editItems),
					new CommandParameter("CheckOutCheckInItems", checkOutCheckInItems),
					new CommandParameter("MoveCopyItems", moveCopyItems),
					new CommandParameter("DeleteRestoreItems", deleteRestoreItems),
					new CommandParameter("EditContentTypesColumns", editContentTypesColumns),
					new CommandParameter("SearchContent", searchContent),
					new CommandParameter("EditUsersPermissions", editUsersPermissions));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            