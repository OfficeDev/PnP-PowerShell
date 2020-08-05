using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.ClientSidePages
{
    [TestClass]
    public class AddClientSidePageSectionTests
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
        public void AddPnPClientSidePageSectionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The name of the page
				var page = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Specifies the columns template to use for the section.
				var sectionTemplate = "";
				// From Cmdlet Help: Sets the order of the section. (Default = 1)
				var order = "";
				// From Cmdlet Help: Sets the background of the section (default = 0)
				var zoneEmphasis = "";

                var results = scope.ExecuteCommand("Add-PnPClientSidePageSection",
					new CommandParameter("Page", page),
					new CommandParameter("SectionTemplate", sectionTemplate),
					new CommandParameter("Order", order),
					new CommandParameter("ZoneEmphasis", zoneEmphasis));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            