using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Taxonomy
{
    [TestClass]
    public class NewTermGroupTests
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
        public void NewPnPTermGroupTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Name of the taxonomy term group to create.
				var name = "";
				// From Cmdlet Help: GUID to use for the term group; if not specified, or the empty GUID, a random GUID is generated and used.
				var id = "";
				// From Cmdlet Help: Description to use for the term group.
				var description = "";
				// From Cmdlet Help: Term store to add the group to; if not specified the default term store is used.
				var termStore = "";

                var results = scope.ExecuteCommand("New-PnPTermGroup",
					new CommandParameter("Name", name),
					new CommandParameter("Id", id),
					new CommandParameter("Description", description),
					new CommandParameter("TermStore", termStore));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            