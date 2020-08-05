using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Webs
{
    [TestClass]
    public class NewWebTests
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
        public void NewPnPWebTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The title of the new web
				var title = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The URL of the new web
				var url = "";
				// From Cmdlet Help: The description of the new web
				var description = "";
				// From Cmdlet Help: The language id of the new web. default = 1033 for English
				var locale = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The site definition template to use for the new web, e.g. STS#0. Use Get-PnPWebTemplates to fetch a list of available templates
				var template = "";
				// From Cmdlet Help: By default the subweb will inherit its security from its parent, specify this switch to break this inheritance
				var breakInheritance = "";
				// From Cmdlet Help: Specifies whether the site inherits navigation.
				var inheritNavigation = "";

                var results = scope.ExecuteCommand("New-PnPWeb",
					new CommandParameter("Title", title),
					new CommandParameter("Url", url),
					new CommandParameter("Description", description),
					new CommandParameter("Locale", locale),
					new CommandParameter("Template", template),
					new CommandParameter("BreakInheritance", breakInheritance),
					new CommandParameter("InheritNavigation", inheritNavigation));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            