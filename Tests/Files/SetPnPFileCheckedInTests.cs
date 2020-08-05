using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Files
{
    [TestClass]
    public class SetFileCheckedInTests
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
        public void SetPnPFileCheckedInTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The server relative url of the file to check in
				var url = "";
				// From Cmdlet Help: The check in type to use. Defaults to Major
				var checkinType = "";
				// From Cmdlet Help: The check in comment
				var comment = "";
				// From Cmdlet Help: Approve file
				var approve = "";

                var results = scope.ExecuteCommand("Set-PnPFileCheckedIn",
					new CommandParameter("Url", url),
					new CommandParameter("CheckinType", checkinType),
					new CommandParameter("Comment", comment),
					new CommandParameter("Approve", approve));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            