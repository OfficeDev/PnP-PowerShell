using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Taxonomy
{
    [TestClass]
    public class GetTermTests
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
        public void GetPnPTermTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The Id or Name of a Term
				var identity = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Name of the termset to check.
				var termSet = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Name of the termgroup to check.
				var termGroup = "";
				// From Cmdlet Help: Term store to check; if not specified the default term store is used.
				var termStore = "";
				// From Cmdlet Help: Find the first term recursively matching the label in a term hierarchy.
				var recursive = "";
				// From Cmdlet Help: Includes the hierarchy of child terms if available
				var includeChildTerms = "";

                var results = scope.ExecuteCommand("Get-PnPTerm",
					new CommandParameter("Identity", identity),
					new CommandParameter("TermSet", termSet),
					new CommandParameter("TermGroup", termGroup),
					new CommandParameter("TermStore", termStore),
					new CommandParameter("Recursive", recursive),
					new CommandParameter("IncludeChildTerms", includeChildTerms));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            