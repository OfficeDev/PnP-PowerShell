using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Taxonomy
{
    [TestClass]
    public class ImportTermSetTests
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
        public void ImportPnPTermSetTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Group to import the term set to; an error is returned if the group does not exist.
				var groupName = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Local path to the file containing the term set to import, in the standard format (as the 'sample import file' available in the Term Store Administration).
				var path = "";
				// From Cmdlet Help: GUID to use for the term set; if not specified, or the empty GUID, a random GUID is generated and used.
				var termSetId = "";
				// From Cmdlet Help: If specified, the import will remove any terms (and children) previously in the term set but not in the import file; default is to leave them.
				var synchronizeDeletions = "";
				// From Cmdlet Help: Whether the term set should be marked open; if not specified, then the existing setting is not changed.
				var isOpen = "";
				// From Cmdlet Help: Contact for the term set; if not specified, the existing setting is retained.
				var contact = "";
				// From Cmdlet Help: Owner for the term set; if not specified, the existing setting is retained.
				var owner = "";
				// From Cmdlet Help: Term store to import into; if not specified the default term store is used.
				var termStoreName = "";

                var results = scope.ExecuteCommand("Import-PnPTermSet",
					new CommandParameter("GroupName", groupName),
					new CommandParameter("Path", path),
					new CommandParameter("TermSetId", termSetId),
					new CommandParameter("SynchronizeDeletions", synchronizeDeletions),
					new CommandParameter("IsOpen", isOpen),
					new CommandParameter("Contact", contact),
					new CommandParameter("Owner", owner),
					new CommandParameter("TermStoreName", termStoreName));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            