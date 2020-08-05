using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace SharePointPnP.PowerShell.Tests.ClientSidePages
{
    [TestClass]
    public class ExportClientSidePageMappingTests
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
        public void ExportPnPClientSidePageMappingTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Exports the builtin web part mapping file
				var builtInWebPartMapping = "";
				// From Cmdlet Help: Exports the builtin pagelayout mapping file (only needed for publishing page transformation)
				var builtInPageLayoutVarMapping = "";
				// From Cmdlet Help: Analyzes the pagelayouts in the current publishing portal and exports them as a pagelayout mapping file
				var customPageLayoutVarMapping = "";
				// From Cmdlet Help: The name of the publishing page to export a page layout mapping file for
				var publishingPage = "";
				// From Cmdlet Help: Set this flag if you also want to analyze the OOB page layouts...typically these are covered via the default mapping, but if you've updated these page layouts you might want to analyze them again
				var analyzeOOBPageLayoutVars = "";
				// From Cmdlet Help: The folder to created the mapping file(s) in
				var folder = "";
				// From Cmdlet Help: Overwrites existing mapping files
				var overwrite = "";
				// From Cmdlet Help: Outputs analyser logging to the console
				var logging = "";

                var results = scope.ExecuteCommand("Export-PnPClientSidePageMapping",
					new CommandParameter("BuiltInWebPartMapping", builtInWebPartMapping),
					new CommandParameter("BuiltInPageLayoutMapping", builtInPageLayoutVarMapping),
					new CommandParameter("CustomPageLayoutMapping", customPageLayoutVarMapping),
					new CommandParameter("PublishingPage", publishingPage),
					new CommandParameter("AnalyzeOOBPageLayouts", analyzeOOBPageLayoutVars),
					new CommandParameter("Folder", folder),
					new CommandParameter("Overwrite", overwrite),
					new CommandParameter("Logging", logging));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            