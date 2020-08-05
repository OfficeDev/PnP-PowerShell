using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Provisioning.Site
{
    [TestClass]
    public class ApplyProvisioningTemplateTests
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
        public void ApplyPnPProvisioningTemplateTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Path to the xml or pnp file containing the provisioning template.
				var path = "";
				// From Cmdlet Help: ID of the template to use from the xml file containing the provisioning template. If not specified and multiple ProvisioningTemplate elements exist, the last one will be used.
				var templateId = "";
				// From Cmdlet Help: Root folder where resources/files that are being referenced in the template are located. If not specified the same folder as where the provisioning template is located will be used.
				var resourceFolder = "";
				// From Cmdlet Help: Specify this parameter if you want to overwrite and/or create properties that are known to be system entries (starting with vti_, dlc_, etc.)
				var overwriteSystemPropertyBagValues = "";
				// From Cmdlet Help: Ignore duplicate data row errors when the data row in the template already exists.
				var ignoreDuplicateDataRowErrors = "";
				// From Cmdlet Help: If set content types will be provisioned if the target web is a subweb.
				var provisionContentTypesToSubWebs = "";
				// From Cmdlet Help: If set fields will be provisioned if the target web is a subweb.
				var provisionFieldsToSubWebs = "";
				// From Cmdlet Help: Override the RemoveExistingNodes attribute in the Navigation elements of the template. If you specify this value the navigation nodes will always be removed before adding the nodes in the template
				var clearNavigation = "";
				// From Cmdlet Help: Allows you to specify parameters that can be referred to in the template by means of the {parameter:<Key>} token. See examples on how to use this parameter.
				var parameters = "";
				// From Cmdlet Help: Allows you to only process a specific part of the template. Notice that this might fail, as some of the handlers require other artifacts in place if they are not part of what your applying. Visit https://docs.microsoft.com/dotnet/api/officedevpnp.core.framework.provisioning.model.handlers for possible values.
				var handlers = "";
				// From Cmdlet Help: Allows you to run all handlers, excluding the ones specified.
				var excludeHandlers = "";
				// From Cmdlet Help: Allows you to specify ExtensbilityHandlers to execute while applying a template
				var extensibilityHandlers = "";
				// From Cmdlet Help: Allows you to specify ITemplateProviderExtension to execute while applying a template.
				var templateProviderExtensions = "";
				// From Cmdlet Help: Allows you to provide an in-memory instance of the ProvisioningTemplate type of the PnP Core Component. When using this parameter, the -Path parameter refers to the path of any supporting file for the template.
				var inputInstance = "";

                var results = scope.ExecuteCommand("Apply-PnPProvisioningTemplate",
					new CommandParameter("Path", path),
					new CommandParameter("TemplateId", templateId),
					new CommandParameter("ResourceFolder", resourceFolder),
					new CommandParameter("OverwriteSystemPropertyBagValues", overwriteSystemPropertyBagValues),
					new CommandParameter("IgnoreDuplicateDataRowErrors", ignoreDuplicateDataRowErrors),
					new CommandParameter("ProvisionContentTypesToSubWebs", provisionContentTypesToSubWebs),
					new CommandParameter("ProvisionFieldsToSubWebs", provisionFieldsToSubWebs),
					new CommandParameter("ClearNavigation", clearNavigation),
					new CommandParameter("Parameters", parameters),
					new CommandParameter("Handlers", handlers),
					new CommandParameter("ExcludeHandlers", excludeHandlers),
					new CommandParameter("ExtensibilityHandlers", extensibilityHandlers),
					new CommandParameter("TemplateProviderExtensions", templateProviderExtensions),
					new CommandParameter("InputInstance", inputInstance));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            