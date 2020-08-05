using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Webs
{
    [TestClass]
    public class InvokeWebActionTests
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
        public void InvokePnPWebActionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Name of list if you only want to handle one specific list and its list items
				var listName = "";
				// From Cmdlet Help: Webs you want to process (for example different site collections), will use Web parameter if not specified
				var webs = "";
				// From Cmdlet Help: Function to be executed on the web. There is one input parameter of type Web
				var webAction = "";
				// From Cmdlet Help: Function to be executed on the web that would determine if WebAction should be invoked, There is one input parameter of type Web and the function should return a boolean value
				var shouldProcessWebAction = "";
				// From Cmdlet Help: Function to be executed on the web, this will trigger after lists and list items have been processed. There is one input parameter of type Web
				var postWebAction = "";
				// From Cmdlet Help: Function to be executed on the web that would determine if PostWebAction should be invoked, There is one input parameter of type Web and the function should return a boolean value
				var shouldProcessPostWebAction = "";
				// From Cmdlet Help: The properties to load for web.
				var webProperties = "";
				// From Cmdlet Help: Function to be executed on the list. There is one input parameter of type List
				var listAction = "";
				// From Cmdlet Help: Function to be executed on the web that would determine if ListAction should be invoked, There is one input parameter of type List and the function should return a boolean value
				var shouldProcessListAction = "";
				// From Cmdlet Help: Function to be executed on the list, this will trigger after list items have been processed. There is one input parameter of type List
				var postListAction = "";
				// From Cmdlet Help: Function to be executed on the web that would determine if PostListAction should be invoked, There is one input parameter of type List and the function should return a boolean value
				var shouldProcessPostListAction = "";
				// From Cmdlet Help: The properties to load for list.
				var listProperties = "";
				// From Cmdlet Help: Function to be executed on the list item. There is one input parameter of type ListItem
				var listItemAction = "";
				// From Cmdlet Help: Function to be executed on the web that would determine if ListItemAction should be invoked, There is one input parameter of type ListItem and the function should return a boolean value
				var shouldProcessListItemAction = "";
				// From Cmdlet Help: The properties to load for list items.
				var listItemProperties = "";
				// From Cmdlet Help: Specify if sub webs will be processed
				var subWebs = "";
				// From Cmdlet Help: Will not output statistics after the operation
				var disableStatisticsOutput = "";
				// From Cmdlet Help: Will skip the counting process; by doing this you will not get an estimated time remaining
				var skipCounting = "";

                var results = scope.ExecuteCommand("Invoke-PnPWebAction",
					new CommandParameter("ListName", listName),
					new CommandParameter("Webs", webs),
					new CommandParameter("WebAction", webAction),
					new CommandParameter("ShouldProcessWebAction", shouldProcessWebAction),
					new CommandParameter("PostWebAction", postWebAction),
					new CommandParameter("ShouldProcessPostWebAction", shouldProcessPostWebAction),
					new CommandParameter("WebProperties", webProperties),
					new CommandParameter("ListAction", listAction),
					new CommandParameter("ShouldProcessListAction", shouldProcessListAction),
					new CommandParameter("PostListAction", postListAction),
					new CommandParameter("ShouldProcessPostListAction", shouldProcessPostListAction),
					new CommandParameter("ListProperties", listProperties),
					new CommandParameter("ListItemAction", listItemAction),
					new CommandParameter("ShouldProcessListItemAction", shouldProcessListItemAction),
					new CommandParameter("ListItemProperties", listItemProperties),
					new CommandParameter("SubWebs", subWebs),
					new CommandParameter("DisableStatisticsOutput", disableStatisticsOutput),
					new CommandParameter("SkipCounting", skipCounting));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            