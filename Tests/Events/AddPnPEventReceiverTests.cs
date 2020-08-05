using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Events
{
    [TestClass]
    public class AddEventReceiverTests
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
        public void AddPnPEventReceiverTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: The list object or name where the remote event receiver needs to be added. If omitted, the remote event receiver will be added to the web.
				var list = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The name of the remote event receiver
				var name = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The URL of the remote event receiver web service
				var url = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The type of the event receiver like ItemAdded, ItemAdding. See https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.eventreceivertype.aspx for the full list of available types.
				var eventReceiverType = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The synchronization type: Asynchronous or Synchronous
				var synchronization = "";
				// From Cmdlet Help: The sequence number where this remote event receiver should be placed
				var sequenceNumber = "";
				// From Cmdlet Help: Overwrites the output file if it exists.
				var force = "";

                var results = scope.ExecuteCommand("Add-PnPEventReceiver",
					new CommandParameter("List", list),
					new CommandParameter("Name", name),
					new CommandParameter("Url", url),
					new CommandParameter("EventReceiverType", eventReceiverType),
					new CommandParameter("Synchronization", synchronization),
					new CommandParameter("SequenceNumber", sequenceNumber),
					new CommandParameter("Force", force));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            