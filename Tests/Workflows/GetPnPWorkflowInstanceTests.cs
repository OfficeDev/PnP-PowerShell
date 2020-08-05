using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Workflows
{
    [TestClass]
    public class GetWorkflowInstanceTests
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
        public void GetPnPWorkflowInstanceTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The List for which workflow instances should be retrieved
				var list = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The List Item for which workflow instances should be retrieved
				var listItem = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The workflow subscription for which workflow instances should be retrieved
				var workflowSubscription = "";

                var results = scope.ExecuteCommand("Get-PnPWorkflowInstance",
					new CommandParameter("List", list),
					new CommandParameter("ListItem", listItem),
					new CommandParameter("WorkflowSubscription", workflowSubscription));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            