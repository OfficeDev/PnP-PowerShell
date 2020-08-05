using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Diagnostic
{
    [TestClass]
    public class MeasureResponseTimeTests
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
        public void MeasurePnPResponseTimeTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				var url = "";
				// From Cmdlet Help: Number of probe requests to send
				var count = "";
				// From Cmdlet Help: Number of warm up requests to send before start calculating statistics
				var warmUp = "";
				// From Cmdlet Help: Idle timeout between requests to avoid request throttling
				var timeoutVar = "";
				// From Cmdlet Help: Number of buckets in histogram in output statistics
				var histogram = "";
				// From Cmdlet Help: Response time measurement mode. RoundTrip - measures full request round trip. SPRequestDuration - measures server processing time only, based on SPRequestDuration HTTP header. Latency - difference between RoundTrip and SPRequestDuration
				var mode = "";

                var results = scope.ExecuteCommand("Measure-PnPResponseTime",
					new CommandParameter("Url", url),
					new CommandParameter("Count", count),
					new CommandParameter("WarmUp", warmUp),
					new CommandParameter("Timeout", timeoutVar),
					new CommandParameter("Histogram", histogram),
					new CommandParameter("Mode", mode));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            