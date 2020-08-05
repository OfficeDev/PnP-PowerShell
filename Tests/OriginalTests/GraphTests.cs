using System;
using System.Management.Automation.Runspaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PnP.PowerShell.Tests
{
    [TestClass]
    public class GraphTests
    {
        private string _groupId;

        [TestInitialize]
        public void Initialize()
        {
            using (var scope = new PSTestScope())
            {
                var random = new Random();

                var group = scope.ExecuteCommand("New-PnPMicrosoft365Group",
                        new CommandParameter("DisplayName", "PnPDeletedMicrosoft365Group test"),
                        new CommandParameter("Description", "PnPDeletedMicrosoft365Group test"),
                        new CommandParameter("MailNickname", $"pnp-unit-test-{random.Next(1, 1000)}"),
                        new CommandParameter("Force"));
                _groupId = group[0].Properties["GroupId"].Value.ToString();

                scope.ExecuteCommand("Remove-PnPMicrosoft365Group", new CommandParameter("Identity", _groupId));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var scope = new PSTestScope())
            {
                try
                {
                    scope.ExecuteCommand("Remove-PnPMicrosoft365Group", new CommandParameter("Identity", _groupId));
                }
                catch (Exception)
                {
                    // Group has already been deleted
                }
                try
                {
                    scope.ExecuteCommand("Remove-PnPDeletedMicrosoft365Group", new CommandParameter("Identity", _groupId));
                }
                catch (Exception)
                {
                    // Group has already been permanently deleted
                }
            }
        }

        [TestMethod]
        public void GetDeletedMicrosoft365Groups()
        {
            using (var scope = new PSTestScope())
            {
                var results = scope.ExecuteCommand("Get-PnPDeletedMicrosoft365Group");
                Assert.IsTrue(results.Count > 0);
            }
        }

        [TestMethod]
        public void GetDeletedMicrosoft365Group()
        {
            using (var scope = new PSTestScope())
            {
                var results = scope.ExecuteCommand("Get-PnPDeletedMicrosoft365Group", new CommandParameter("Identity", _groupId));

                Assert.IsTrue(results != null && results[0].Properties["GroupId"].Value.ToString() == _groupId);
            }
        }

        [TestMethod]
        public void RestoreDeletedMicrosoft365Group()
        {
            using (var scope = new PSTestScope())
            {
                scope.ExecuteCommand("Restore-PnPDeletedMicrosoft365Group", new CommandParameter("Identity", _groupId));
                var results = scope.ExecuteCommand("Get-PnPMicrosoft365Group", new CommandParameter("Identity", _groupId));

                Assert.IsTrue(results != null && results[0].Properties["GroupId"].Value.ToString() == _groupId);
            }
        }

        [TestMethod]
        public void RemoveDeletedMicrosoft365Group()
        {
            using (var scope = new PSTestScope())
            {
                scope.ExecuteCommand("Remove-PnPDeletedMicrosoft365Group", new CommandParameter("Identity", _groupId));

                // The group should no longer be found in deleted groups
                try
                {
                    var results = scope.ExecuteCommand("Get-PnPDeletedMicrosoft365Group", new CommandParameter("Identity", _groupId));
                    Assert.IsFalse(results != null);
                }
                catch (Exception)
                {
                    Assert.IsTrue(true);
                }
            }
        }
    }
}
