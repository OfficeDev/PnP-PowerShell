using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Provisioning.Site
{
    [TestClass]
    public class GetProvisioningTemplateTests
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
        public void GetPnPProvisioningTemplateTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// From Cmdlet Help: Filename to write to, optionally including full path
				var outVar = "";
				// From Cmdlet Help: The schema of the output to use, defaults to the latest schema
				var schema = "";
				// From Cmdlet Help: If specified, all term groups will be included. Overrides IncludeSiteCollectionTermGroup.
				var includeAllTermGroups = "";
				// From Cmdlet Help: If specified, all the site collection term groups will be included. Overridden by IncludeAllTermGroups.
				var includeSiteCollectionTermGroup = "";
				// From Cmdlet Help: If specified all site groups will be included.
				var includeSiteGroups = "";
				// From Cmdlet Help: If specified all the managers and contributors of term groups will be included.
				var includeTermGroupsSecurity = "";
				// From Cmdlet Help: If specified the template will contain the current search configuration of the site.
				var includeSearchConfiguration = "";
				// From Cmdlet Help: If specified the files used for masterpages, sitelogo, alternate CSS and the files that make up the composed look will be saved.
				var persistBrandingFiles = "";
				// From Cmdlet Help: If specified the files making up the composed look (background image, font file and color file) will be saved.
				var persistComposedLookFiles = "";
				// From Cmdlet Help: If specified the files used for the publishing feature will be saved.
				var persistPublishingFiles = "";
				// From Cmdlet Help: If specified, out of the box / native publishing files will be saved.
				var includeNativePublishingFiles = "";
				// From Cmdlet Help: If specified hidden lists will be included in the template
				var includeHiddenLists = "";
				// From Cmdlet Help: If specified all client side pages will be included
				var includeAllClientSidePages = "";
				// From Cmdlet Help: During extraction the version of the server will be checked for certain actions. If you specify this switch, this check will be skipped.
				var skipVersionCheck = "";
				// From Cmdlet Help: If specified, resource values for applicable artifacts will be persisted to a resource file
				var persistMultiLanguageResources = "";
				// From Cmdlet Help: If specified, resource files will be saved with the specified prefix instead of using the template name specified. If no template name is specified the files will be called PnP-Resources.<language>.resx. See examples for more info.
				var resourceFilePrefix = "";
				// From Cmdlet Help: Allows you to only process a specific type of artifact in the site. Notice that this might result in a non-working template, as some of the handlers require other artifacts in place if they are not part of what your extracting. For possible values for this parameter visit https://docs.microsoft.com/dotnet/api/officedevpnp.core.framework.provisioning.model.handlers
				var handlers = "";
				// From Cmdlet Help: Allows you to run all handlers, excluding the ones specified.
				var excludeHandlers = "";
				// From Cmdlet Help: Allows you to specify ExtensibilityHandlers to execute while extracting a template.
				var extensibilityHandlers = "";
				// From Cmdlet Help: Allows you to specify ITemplateProviderExtension to execute while extracting a template.
				var templateProviderExtensions = "";
				// From Cmdlet Help: Allows you to specify from which content type group(s) the content types should be included into the template.
				var contentTypeGroups = "";
				// From Cmdlet Help: Overwrites the output file if it exists.
				var force = "";
				// From Cmdlet Help: Exports the template without the use of a base template, causing all OOTB artifacts to be included. Using this switch is generally not required/recommended.
				var noBaseTemplate = "";
				// From Cmdlet Help: The encoding type of the XML file, Unicode is default
				var encoding = "";
				// From Cmdlet Help: It can be used to specify the DisplayName of the template file that will be extracted.
				var templateDisplayName = "";
				// From Cmdlet Help: It can be used to specify the ImagePreviewUrl of the template file that will be extracted.
				var templateImagePreviewUrl = "";
				// From Cmdlet Help: It can be used to specify custom Properties for the template file that will be extracted.
				var templateProperties = "";
				// From Cmdlet Help: Returns the template as an in-memory object, which is an instance of the ProvisioningTemplate type of the PnP Core Component. It cannot be used together with the -Out parameter.
				var outVarputInstance = "";
				// From Cmdlet Help: Specify whether or not content types issued from a content hub should be exported. By default, these content types are included.
				var excludeContentTypesFromSyndication = "";
				// From Cmdlet Help: Specify the lists to extract, either providing their ID or their Title.
				var listsToExtract = "";
				// From Cmdlet Help: Specify a JSON configuration file to configure the extraction progress.
				var configuration = "";

                var results = scope.ExecuteCommand("Get-PnPProvisioningTemplate",
					new CommandParameter("Out", outVar),
					new CommandParameter("Schema", schema),
					new CommandParameter("IncludeAllTermGroups", includeAllTermGroups),
					new CommandParameter("IncludeSiteCollectionTermGroup", includeSiteCollectionTermGroup),
					new CommandParameter("IncludeSiteGroups", includeSiteGroups),
					new CommandParameter("IncludeTermGroupsSecurity", includeTermGroupsSecurity),
					new CommandParameter("IncludeSearchConfiguration", includeSearchConfiguration),
					new CommandParameter("PersistBrandingFiles", persistBrandingFiles),
					new CommandParameter("PersistComposedLookFiles", persistComposedLookFiles),
					new CommandParameter("PersistPublishingFiles", persistPublishingFiles),
					new CommandParameter("IncludeNativePublishingFiles", includeNativePublishingFiles),
					new CommandParameter("IncludeHiddenLists", includeHiddenLists),
					new CommandParameter("IncludeAllClientSidePages", includeAllClientSidePages),
					new CommandParameter("SkipVersionCheck", skipVersionCheck),
					new CommandParameter("PersistMultiLanguageResources", persistMultiLanguageResources),
					new CommandParameter("ResourceFilePrefix", resourceFilePrefix),
					new CommandParameter("Handlers", handlers),
					new CommandParameter("ExcludeHandlers", excludeHandlers),
					new CommandParameter("ExtensibilityHandlers", extensibilityHandlers),
					new CommandParameter("TemplateProviderExtensions", templateProviderExtensions),
					new CommandParameter("ContentTypeGroups", contentTypeGroups),
					new CommandParameter("Force", force),
					new CommandParameter("NoBaseTemplate", noBaseTemplate),
					new CommandParameter("Encoding", encoding),
					new CommandParameter("TemplateDisplayName", templateDisplayName),
					new CommandParameter("TemplateImagePreviewUrl", templateImagePreviewUrl),
					new CommandParameter("TemplateProperties", templateProperties),
					new CommandParameter("OutputInstance", outVarputInstance),
					new CommandParameter("ExcludeContentTypesFromSyndication", excludeContentTypesFromSyndication),
					new CommandParameter("ListsToExtract", listsToExtract),
					new CommandParameter("Configuration", configuration));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            