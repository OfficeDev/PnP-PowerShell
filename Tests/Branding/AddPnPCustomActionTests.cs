using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Branding
{
    [TestClass]
    public class AddCustomActionTests
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
        public void AddPnPCustomActionTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The name of the custom action
				var name = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The title of the custom action
				var title = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The description of the custom action
				var description = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The group where this custom action needs to be added like 'SiteActions'
				var group = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The actual location where this custom action need to be added like 'CommandUI.Ribbon'
				var location = "";
				// From Cmdlet Help: Sequence of this CustomAction being injected. Use when you have a specific sequence with which to have multiple CustomActions being added to the page.
				var sequence = "";
				// From Cmdlet Help: The URL, URI or ECMAScript (JScript, JavaScript) function associated with the action
				var url = "";
				// From Cmdlet Help: The URL of the image associated with the custom action
				var imageUrl = "";
				// From Cmdlet Help: XML fragment that determines user interface properties of the custom action
				var commandUIExtension = "";
				// From Cmdlet Help: The identifier of the object associated with the custom action.
				var registrationId = "";
				// From Cmdlet Help: A string array that contain the permissions needed for the custom action
				var rights = "";
				// From Cmdlet Help: Specifies the type of object associated with the custom action
				var registrationType = "";
				// From Cmdlet Help: The scope of the CustomAction to add to. Either Web or Site; defaults to Web. 'All' is not valid for this command.
				var scopeVar = "";
				// This is a mandatory parameter
				// From Cmdlet Help: The Client Side Component Id of the custom action
				var clientSideComponentId = "";
				// From Cmdlet Help: The Client Side Component Properties of the custom action. Specify values as a json string : "{Property1 : 'Value1', Property2: 'Value2'}"
				var clientSideComponentProperties = "";
				// From Cmdlet Help: The Client Side Host Properties of the custom action. Specify values as a json string : "{'preAllocatedApplicationCustomizerTopHeight': '50', 'preAllocatedApplicationCustomizerBottomHeight': '50'}"
				var clientSideHostProperties = "";

                var results = scope.ExecuteCommand("Add-PnPCustomAction",
					new CommandParameter("Name", name),
					new CommandParameter("Title", title),
					new CommandParameter("Description", description),
					new CommandParameter("Group", group),
					new CommandParameter("Location", location),
					new CommandParameter("Sequence", sequence),
					new CommandParameter("Url", url),
					new CommandParameter("ImageUrl", imageUrl),
					new CommandParameter("CommandUIExtension", commandUIExtension),
					new CommandParameter("RegistrationId", registrationId),
					new CommandParameter("Rights", rights),
					new CommandParameter("RegistrationType", registrationType),
					new CommandParameter("Scope", scopeVar),
					new CommandParameter("ClientSideComponentId", clientSideComponentId),
					new CommandParameter("ClientSideComponentProperties", clientSideComponentProperties),
					new CommandParameter("ClientSideHostProperties", clientSideHostProperties));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            