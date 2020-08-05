using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Taxonomy
{
    [TestClass]
    public class ExportTaxonomyTests
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
        public void ExportPnPTaxonomyTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: If specified, will export the specified termset only
				var termSetId = "";
				// From Cmdlet Help: If specified will include the ids of the taxonomy items in the output. Format: <label>;#<guid>
				var includeID = "";
				// From Cmdlet Help: File to export the data to.
				var path = "";
				// From Cmdlet Help: Term store to export; if not specified the default term store is used.
				var termStoreName = "";
				// From Cmdlet Help: Overwrites the output file if it exists.
				var force = "";
				// From Cmdlet Help: The path delimiter to be used, by default this is '|'
				var delimiter = "";
				// From Cmdlet Help: Specify the language code for the exported terms
				var lcid = "";
				// From Cmdlet Help: Defaults to Unicode
				var encoding = "";

                var results = scope.ExecuteCommand("Export-PnPTaxonomy",
					new CommandParameter("TermSetId", termSetId),
					new CommandParameter("IncludeID", includeID),
					new CommandParameter("Path", path),
					new CommandParameter("TermStoreName", termStoreName),
					new CommandParameter("Force", force),
					new CommandParameter("Delimiter", delimiter),
					new CommandParameter("Lcid", lcid),
					new CommandParameter("Encoding", encoding));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            