using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.WebParts
{
    [TestClass]
    public class AddClientSideWebPartTests
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
        public void AddPnPClientSideWebPartTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The name of the page.
				var page = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Defines a default web part type to insert.
				var defaultWebPartType = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Specifies the component instance or Id to add.
				var component = "";
				// From Cmdlet Help: The properties of the web part
				var webPartProperties = "";
				// From Cmdlet Help: Sets the order of the web part control. (Default = 1)
				var order = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Sets the section where to insert the web part control.
				var section = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Sets the column where to insert the web part control.
				var column = "";

                var results = scope.ExecuteCommand("Add-PnPClientSideWebPart",
					new CommandParameter("Page", page),
					new CommandParameter("DefaultWebPartType", defaultWebPartType),
					new CommandParameter("Component", component),
					new CommandParameter("WebPartProperties", webPartProperties),
					new CommandParameter("Order", order),
					new CommandParameter("Section", section),
					new CommandParameter("Column", column));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            