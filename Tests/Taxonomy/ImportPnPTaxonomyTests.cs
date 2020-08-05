using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Taxonomy
{
    [TestClass]
    public class ImportTaxonomyTests
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
        public void ImportPnPTaxonomyTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: An array of strings describing termgroup, termset, term, subterms using a default delimiter of '|'.
				var terms = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Specifies a file containing terms per line, in the format as required by the Terms parameter.
				var path = "";
				var lcid = "";
				// From Cmdlet Help: Term store to import to; if not specified the default term store is used.
				var termStoreName = "";
				// From Cmdlet Help: The path delimiter to be used, by default this is '|'
				var delimiter = "";
				// From Cmdlet Help: If specified, terms that exist in the termset, but are not in the imported data, will be removed.
				var synchronizeDeletions = "";

                var results = scope.ExecuteCommand("Import-PnPTaxonomy",
					new CommandParameter("Terms", terms),
					new CommandParameter("Path", path),
					new CommandParameter("Lcid", lcid),
					new CommandParameter("TermStoreName", termStoreName),
					new CommandParameter("Delimiter", delimiter),
					new CommandParameter("SynchronizeDeletions", synchronizeDeletions));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            