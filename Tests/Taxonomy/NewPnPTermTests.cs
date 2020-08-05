using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Taxonomy
{
    [TestClass]
    public class NewTermTests
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
        public void NewPnPTermTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The name of the term.
				var name = "";
				// From Cmdlet Help: The Id to use for the term; if not specified, or the empty GUID, a random GUID is generated and used.
				var id = "";
				// From Cmdlet Help: The locale id to use for the term. Defaults to the current locale id.
				var lcid = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The termset to add the term to.
				var termSet = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The termgroup to create the term in.
				var termGroup = "";
				// From Cmdlet Help: Descriptive text to help users understand the intended use of this term.
				var description = "";
				// From Cmdlet Help: Custom Properties
				var customProperties = "";
				// From Cmdlet Help: Custom Properties
				var localCustomProperties = "";
				// From Cmdlet Help: Term store to check; if not specified the default term store is used.
				var termStore = "";

                var results = scope.ExecuteCommand("New-PnPTerm",
					new CommandParameter("Name", name),
					new CommandParameter("Id", id),
					new CommandParameter("Lcid", lcid),
					new CommandParameter("TermSet", termSet),
					new CommandParameter("TermGroup", termGroup),
					new CommandParameter("Description", description),
					new CommandParameter("CustomProperties", customProperties),
					new CommandParameter("LocalCustomProperties", localCustomProperties),
					new CommandParameter("TermStore", termStore));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            