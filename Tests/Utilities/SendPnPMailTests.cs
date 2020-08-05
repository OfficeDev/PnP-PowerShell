using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Utilities
{
    [TestClass]
    public class SendMailTests
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
        public void SendPnPMailTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				var server = "";
				// From Cmdlet Help: If using from address, you also have to provide a password
				var from = "";
				// From Cmdlet Help: If using a password, you also have to provide the associated from address
				var password = "";
				// This is a mandatory parameter
				// From Cmdlet Help: List of recipients
				var to = "";
				// From Cmdlet Help: List of recipients on CC
				var cc = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Subject of the email
				var subject = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Body of the email. Accepts simple HTML as `<h1></h1>`, `<br/>` etc.
				var body = "";

                var results = scope.ExecuteCommand("Send-PnPMail",
					new CommandParameter("Server", server),
					new CommandParameter("From", from),
					new CommandParameter("Password", password),
					new CommandParameter("To", to),
					new CommandParameter("Cc", cc),
					new CommandParameter("Subject", subject),
					new CommandParameter("Body", body));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            