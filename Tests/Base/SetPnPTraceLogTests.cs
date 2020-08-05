using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Base
{
    [TestClass]
    public class SetTraceLogTests
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
        public void SetPnPTraceLogTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Turn on tracing to log file
				var on = "";
				// From Cmdlet Help: The path and filename of the file to write the trace log to.
				var logFile = "";
				// From Cmdlet Help: Turn on console trace output.
				var writeToConsole = "";
				// From Cmdlet Help: The level of events to capture. Possible values are 'Debug', 'Error', 'Warning', 'Information'. Defaults to 'Information'.
				var level = "";
				// From Cmdlet Help: If specified the trace log entries will be delimited with this value.
				var delimiter = "";
				// From Cmdlet Help: Indents in the tracelog will be with this amount of characters. Defaults to 4.
				var indentSize = "";
				// From Cmdlet Help: Auto flush the trace log. Defaults to true.
				var autoFlush = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Turn off tracing to log file.
				var off = "";

                var results = scope.ExecuteCommand("Set-PnPTraceLog",
					new CommandParameter("On", on),
					new CommandParameter("LogFile", logFile),
					new CommandParameter("WriteToConsole", writeToConsole),
					new CommandParameter("Level", level),
					new CommandParameter("Delimiter", delimiter),
					new CommandParameter("IndentSize", indentSize),
					new CommandParameter("AutoFlush", autoFlush),
					new CommandParameter("Off", off));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            