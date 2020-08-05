using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.ClientSidePages
{
    [TestClass]
    public class AddClientSideTextTests
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
        public void AddPnPClientSideTextTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The name of the page.
				var page = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Specifies the text to display in the text area.
				var text = "";
				// From Cmdlet Help: Sets the order of the text control. (Default = 1)
				var order = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Sets the section where to insert the text control.
				var section = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Sets the column where to insert the text control.
				var column = "";

                var results = scope.ExecuteCommand("Add-PnPClientSideText",
					new CommandParameter("Page", page),
					new CommandParameter("Text", text),
					new CommandParameter("Order", order),
					new CommandParameter("Section", section),
					new CommandParameter("Column", column));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            