using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Workflows
{
    [TestClass]
    public class AddWorkflowSubscriptionTests
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
        public void AddPnPWorkflowSubscriptionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The name of the subscription
				var name = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The name of the workflow definition
				var definitionName = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The list to add the workflow to
				var list = "";
				// From Cmdlet Help: Switch if the workflow should be started manually, default value is 'true'
				var startManually = "";
				// From Cmdlet Help: Should the workflow run when an new item is created
				var startOnCreated = "";
				// From Cmdlet Help: Should the workflow run when an item is changed
				var startOnChanged = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The name of the History list
				var historyListName = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The name of the task list
				var taskListName = "";
				var associationValues = "";

                var results = scope.ExecuteCommand("Add-PnPWorkflowSubscription",
					new CommandParameter("Name", name),
					new CommandParameter("DefinitionName", definitionName),
					new CommandParameter("List", list),
					new CommandParameter("StartManually", startManually),
					new CommandParameter("StartOnCreated", startOnCreated),
					new CommandParameter("StartOnChanged", startOnChanged),
					new CommandParameter("HistoryListName", historyListName),
					new CommandParameter("TaskListName", taskListName),
					new CommandParameter("AssociationValues", associationValues));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            