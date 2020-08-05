using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.ClientSidePages
{
    [TestClass]
    public class AddClientSidePageTests
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
        public void AddPnPClientSidePageTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Specifies the name of the page.
				var name = "";
				// From Cmdlet Help: Specifies the layout type of the page.
				var layoutVarType = "";
				// From Cmdlet Help: Allows to promote the page for a specific purpose (HomePage | NewsPage)
				var promoteAs = "";
				// From Cmdlet Help: Specify either the name, ID or an actual content type.
				var contentType = "";
				// From Cmdlet Help: Enables or Disables the comments on the page
				var commentsEnabled = "";
				// From Cmdlet Help: Publishes the page once it is saved. Applicable to libraries set to create major and minor versions.
				var publish = "";
				// From Cmdlet Help: Type of layout used for the header
				var headerLayoutVarType = "";
				// From Cmdlet Help: Sets the message for publishing the page.
				var publishMessage = "";

                var results = scope.ExecuteCommand("Add-PnPClientSidePage",
					new CommandParameter("Name", name),
					new CommandParameter("LayoutType", layoutVarType),
					new CommandParameter("PromoteAs", promoteAs),
					new CommandParameter("ContentType", contentType),
					new CommandParameter("CommentsEnabled", commentsEnabled),
					new CommandParameter("Publish", publish),
					new CommandParameter("HeaderLayoutType", headerLayoutVarType),
					new CommandParameter("PublishMessage", publishMessage));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            