using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Admin
{
    [TestClass]
    public class SetTenantSyncClientRestrictionTests
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
        public void SetPnPTenantSyncClientRestrictionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Block Mac sync clients-- the Beta version and the new sync client (OneDrive.exe). The values for this parameter are $true and $false. The default value is $false.
				var blockMacSync = "";
				// From Cmdlet Help: Specifies if the Report Problem Dialog is disabled or not.
				var disableReportProblemDialog = "";
				// From Cmdlet Help: Sets the domain GUID to add to the safe recipient list. Requires a minimum of 1 domain GUID. The maximum number of domain GUIDs allowed are 125. I.e. 634c71f6-fa83-429c-b77b-0dba3cb70b93,4fbc735f-0ac2-48ba-b035-b1ae3a480887.
				var domainGuids = "";
				// From Cmdlet Help: Enables the feature to block sync originating from domains that are not present in the safe recipients list.
				var enable = "";
				// From Cmdlet Help: Blocks certain file types from syncing with the new sync client (OneDrive.exe). Provide as one string separating the extensions using a semicolon (;). I.e. "docx;pptx"
				var excludedFileExtensions = "";
				// From Cmdlet Help: Controls whether or not a tenant's users can sync OneDrive for Business libraries with the old OneDrive for Business sync client. The valid values are OptOut, HardOptin, and SoftOptin.
				var grooveBlockOption = "";

                var results = scope.ExecuteCommand("Set-PnPTenantSyncClientRestriction",
					new CommandParameter("BlockMacSync", blockMacSync),
					new CommandParameter("DisableReportProblemDialog", disableReportProblemDialog),
					new CommandParameter("DomainGuids", domainGuids),
					new CommandParameter("Enable", enable),
					new CommandParameter("ExcludedFileExtensions", excludedFileExtensions),
					new CommandParameter("GrooveBlockOption", grooveBlockOption));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            