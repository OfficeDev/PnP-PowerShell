using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.ClientSidePages
{
    [TestClass]
    public class SetClientSideWebPartTests
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
        public void SetPnPClientSideWebPartTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The name of the page
				var page = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The identity of the web part. This can be the web part instance id or the title of a web part
				var identity = "";
				// From Cmdlet Help: Sets the internal title of the web part. Notice that this will NOT set a visible title.
				var title = "";
				// From Cmdlet Help: Sets the properties as a JSON string.
				var propertiesJson = "";

                var results = scope.ExecuteCommand("Set-PnPClientSideWebPart",
					new CommandParameter("Page", page),
					new CommandParameter("Identity", identity),
					new CommandParameter("Title", title),
					new CommandParameter("PropertiesJson", propertiesJson));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            