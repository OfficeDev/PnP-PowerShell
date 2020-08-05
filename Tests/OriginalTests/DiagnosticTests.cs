using System.Linq;
using System.Management.Automation.Runspaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PnP.PowerShell.Tests
{
    [TestClass]
    public class DiagnosticTests
    {
        [TestMethod]
        public void MeasureResponseTime()
        {
            using (var scope = new PSTestScope(true))
            {
                var result = scope.ExecuteCommand("Measure-PnPResponseTime", new CommandParameter("Count", "1"));
                Assert.IsTrue(result.Any());
                Assert.IsTrue(result.First().BaseObject.GetType() == typeof(Commands.Diagnostic.ResponseTimeStatistics));

                var statistics = result.First().BaseObject as Commands.Diagnostic.ResponseTimeStatistics;
                Assert.AreEqual(1, statistics.Count);
                Assert.IsTrue(statistics.Average > 0);
                Assert.AreEqual(statistics.Max, statistics.Average);
                Assert.AreEqual(statistics.Min, statistics.Average);
                Assert.AreEqual(0, statistics.StandardDeviation);
                Assert.AreEqual(0, statistics.TruncatedAverage);
            }
        }
    }
}
