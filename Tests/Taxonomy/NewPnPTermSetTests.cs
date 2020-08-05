using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Taxonomy
{
    [TestClass]
    public class NewTermSetTests
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
        public void NewPnPTermSetTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The name of the termset.
				var name = "";
				// From Cmdlet Help: The Id to use for the term set; if not specified, or the empty GUID, a random GUID is generated and used.
				var id = "";
				// From Cmdlet Help: The locale id to use for the term set. Defaults to the current locale id.
				var lcid = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Name, id or actually termgroup to create the termset in.
				var termGroup = "";
				// From Cmdlet Help: An e-mail address for term suggestion and feedback. If left blank the suggestion feature will be disabled.
				var contact = "";
				// From Cmdlet Help: Descriptive text to help users understand the intended use of this term set.
				var description = "";
				// From Cmdlet Help: When a term set is closed, only metadata managers can add terms to this term set. When it is open, users can add terms from a tagging application. Not specifying this switch will make the term set closed.
				var isOpenForTermCreation = "";
				// From Cmdlet Help: By default a term set is available to be used by end users and content editors of sites consuming this term set. Specify this switch to turn this off
				var isNotAvailableForTagging = "";
				// From Cmdlet Help: The primary user or group of this term set.
				var owner = "";
				// From Cmdlet Help: People and groups in the organization that should be notified before major changes are made to the term set. You can enter multiple users or groups.
				var stakeHolders = "";
				var customProperties = "";
				// From Cmdlet Help: Term store to check; if not specified the default term store is used.
				var termStore = "";

                var results = scope.ExecuteCommand("New-PnPTermSet",
					new CommandParameter("Name", name),
					new CommandParameter("Id", id),
					new CommandParameter("Lcid", lcid),
					new CommandParameter("TermGroup", termGroup),
					new CommandParameter("Contact", contact),
					new CommandParameter("Description", description),
					new CommandParameter("IsOpenForTermCreation", isOpenForTermCreation),
					new CommandParameter("IsNotAvailableForTagging", isNotAvailableForTagging),
					new CommandParameter("Owner", owner),
					new CommandParameter("StakeHolders", stakeHolders),
					new CommandParameter("CustomProperties", customProperties),
					new CommandParameter("TermStore", termStore));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            