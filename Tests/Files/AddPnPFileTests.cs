using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Files
{
    [TestClass]
    public class AddFileTests
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
        public void AddPnPFileTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The local file path
				var path = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The destination folder in the site
				var folder = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Name for file
				var fileName = "";
				// From Cmdlet Help: Filename to give the file on SharePoint
				var newFileName = "";
				// This is a mandatory parameter
				// From Cmdlet Help: Stream with the file contents
				var stream = "";
				// From Cmdlet Help: If versioning is enabled, this will check out the file first if it exists, upload the file, then check it in again
				var checkoutVar = "";
				// From Cmdlet Help: The comment added to the checkin
				var checkInComment = "";
				// From Cmdlet Help: Will auto approve the uploaded file
				var approve = "";
				// From Cmdlet Help: The comment added to the approval
				var approveComment = "";
				// From Cmdlet Help: Will auto publish the file
				var publish = "";
				// From Cmdlet Help: The comment added to the publish action
				var publishComment = "";
				var useWebDav = "";
				// From Cmdlet Help: Use the internal names of the fields when specifying field names.
				// Single line of text: -Values @{"Title" = "Title New"}
				// Multiple lines of text: -Values @{"MultiText" = "New text\n\nMore text"}
				// Rich text: -Values @{"MultiText" = "<strong>New</strong> text"}
				// Choice: -Values @{"Choice" = "Value 1"}
				// Number: -Values @{"Number" = "10"}
				// Currency: -Values @{"Number" = "10"}
				// Currency: -Values @{"Currency" = "10"}
				// Date and Time: -Values @{"DateAndTime" = "03/10/2015 14:16"}
				// Lookup (id of lookup value): -Values @{"Lookup" = "2"}
				// Multi value lookup (id of lookup values as array 1): -Values @{"MultiLookupField" = "1","2"}
				// Multi value lookup (id of lookup values as array 2): -Values @{"MultiLookupField" = 1,2}
				// Multi value lookup (id of lookup values as string): -Values @{"MultiLookupField" = "1,2"}
				// Yes/No: -Values @{"YesNo" = $false}
				// Person/Group (id of user/group in Site User Info List or email of the user, separate multiple values with a comma): -Values @{"Person" = "user1@domain.com","21"}
				// Managed Metadata (single value with path to term): -Values @{"MetadataField" = "CORPORATE|DEPARTMENTS|FINANCE"}
				// Managed Metadata (single value with id of term): -Values @{"MetadataField" = "fe40a95b-2144-4fa2-b82a-0b3d0299d818"} with Id of term
				// Managed Metadata (multiple values with paths to terms): -Values @{"MetadataField" = "CORPORATE|DEPARTMENTS|FINANCE","CORPORATE|DEPARTMENTS|HR"}
				// Managed Metadata (multiple values with ids of terms): -Values @{"MetadataField" = "fe40a95b-2144-4fa2-b82a-0b3d0299d818","52d88107-c2a8-4bf0-adfa-04bc2305b593"}
				// Hyperlink or Picture: -Values @{"Hyperlink" = "https://github.com/OfficeDev/, OfficePnp"}
				var values = "";
				// From Cmdlet Help: Use to assign a ContentType to the file
				var contentType = "";

                var results = scope.ExecuteCommand("Add-PnPFile",
					new CommandParameter("Path", path),
					new CommandParameter("Folder", folder),
					new CommandParameter("FileName", fileName),
					new CommandParameter("NewFileName", newFileName),
					new CommandParameter("Stream", stream),
					new CommandParameter("Checkout", checkoutVar),
					new CommandParameter("CheckInComment", checkInComment),
					new CommandParameter("Approve", approve),
					new CommandParameter("ApproveComment", approveComment),
					new CommandParameter("Publish", publish),
					new CommandParameter("PublishComment", publishComment),
					new CommandParameter("UseWebDav", useWebDav),
					new CommandParameter("Values", values),
					new CommandParameter("ContentType", contentType));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            