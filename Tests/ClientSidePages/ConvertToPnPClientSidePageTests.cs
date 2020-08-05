using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace SharePointPnP.PowerShell.Tests.ClientSidePages
{
    [TestClass]
    public class ConvertToClientSidePageTests
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
        public void ConvertToPnPClientSidePageTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: The name of the page to convert
				var identity = "";
				// From Cmdlet Help: The name of the library containing the page. If SitePages then please omit this parameter
				var library = "";
				// From Cmdlet Help: The folder to load the provided page from. If not provided all folders are searched
				var folder = "";
				// From Cmdlet Help: Path and name of the web part mapping file driving the transformation
				var webPartMappingFile = "";
				// From Cmdlet Help: Overwrites page if already existing
				var overwrite = "";
				// From Cmdlet Help: Created client side page takes name from previous classic page. Classic page gets renamed to previous_<Page>.aspx
				var takeSourcePageName = "";
				// From Cmdlet Help: Replaces a home page with a default stock modern home page
				var replaceHomePageWithDefault = "";
				// From Cmdlet Help: Adds the page accept banner web part. The actual web part is specified in webpartmapping.xml file
				var addPageAcceptBanner = "";
				// From Cmdlet Help: By default the item level permissions on a page are copied to the created client side page. Use this switch to prevent the copy
				var skipItemLevelPermissionCopyToClientSidePage = "";
				// From Cmdlet Help: If transforming cross site then by default urls in html and summarylinks are rewritten for the target site. Set this flag to prevent that
				var skipUrlRewriting = "";
				// From Cmdlet Help: Set this flag to prevent the default URL rewriting while you still want to do URL rewriting using a custom URL mapping file
				var skipDefaultUrlRewriting = "";
				// From Cmdlet Help: File holding custom URL mapping definitions
				var urlMappingFile = "";
				// From Cmdlet Help: Clears the cache. Can be needed if you've installed a new web part to the site and want to use that in a custom webpartmapping file. Restarting your PS session has the same effect
				var clearCache = "";
				// From Cmdlet Help: Copies the page metadata to the created modern page
				var copyPageMetadata = "";
				// From Cmdlet Help: When an image lives inside a table/list then it's also created as separate image web part underneath that table/list by default. Use this switch set to $false to change that
				var addTableListImageAsImageWebPart = "";
				// From Cmdlet Help: Uses the community script editor (https://github.com/SharePoint/sp-dev-fx-webparts/tree/master/samples/react-script-editor) as replacement for the classic script editor web part
				var useCommunityScriptEditor = "";
				// From Cmdlet Help: By default summarylinks web parts are replaced by QuickLinks, but you can transform to plain html by setting this switch
				var summaryLinksToHtml = "";
				// From Cmdlet Help: Url of the target web that will receive the modern page. Defaults to null which means in-place transformation
				var targetWebUrl = "";
				// From Cmdlet Help: Allows to generate a transformation log (File | SharePoint)
				var logType = "";
				// From Cmdlet Help: Folder in where the log file will be created (if LogType==File)
				var logFolder = "";
				// From Cmdlet Help: By default each cmdlet invocation will result in a log file, use the -SkipLogFlush to delay the log flushing. The first call without -SkipLogFlush will then write all log entries to a single log
				var logSkipFlush = "";
				// From Cmdlet Help: Configure logging to include verbose log entries
				var logVerbose = "";
				// From Cmdlet Help: Don't publish the created modern page
				var dontPublish = "";
				// From Cmdlet Help: Keep the author, editor, created and modified information from the source page (when source page lives in SPO)
				var keepPageCreationModificationInformation = "";
				// From Cmdlet Help: Set's the author of the source page as author in the modern page header (when source page lives in SPO)
				var setAuthorInPageHeader = "";
				// From Cmdlet Help: Post the created, and published, modern page as news
				var postAsNews = "";
				// From Cmdlet Help: Disable comments for the created modern page
				var disablePageComments = "";
				// From Cmdlet Help: I'm transforming a publishing page
				var publishingPage = "";
				// From Cmdlet Help: I'm transforming a blog page
				var blogPage = "";
				// From Cmdlet Help: I'm transforming a Delve blog page
				var delveBlogPage = "";
				// From Cmdlet Help: Transform the possible sub title as topic header on the modern page
				var delveKeepSubTitle = "";
				// From Cmdlet Help: Path and name of the page layout mapping file driving the publishing page transformation
				var pageLayoutVarMapping = "";
				// From Cmdlet Help: Name for the target page (only applies to publishing page transformation)
				var publishingTargetPageName = "";
				// From Cmdlet Help: Name for the target page (only applies when doing cross site page transformation)
				var targetPageName = "";
				// From Cmdlet Help: Folder to create the target page in (will be used in conjunction with auto-generated folders that ensure page name uniqueness)
				var targetPageFolder = "";
				// From Cmdlet Help: When setting a target page folder then the target page folder overrides possibly default folder path (e.g. in the source page lived in a folder) instead of being appended to it
				var targetPageFolderOverridesDefaultFolder = "";
				// From Cmdlet Help: Remove empty sections and columns after transformation of the page
				var removeEmptySectionsAndColumns = "";
				// From Cmdlet Help: Optional connection to be used by the cmdlet. Retrieve the value for this parameter by either specifying -ReturnConnection on Connect-PnPOnline or by executing Get-PnPConnection.
				var targetConnection = "";
				// From Cmdlet Help: Disables user mapping during transformation
				var skipUserMapping = "";
				// From Cmdlet Help: Specifies a user mapping file
				var userMappingFile = "";
				// From Cmdlet Help: Specifies a taxonomy term mapping file
				var termMappingFile = "";
				// From Cmdlet Help: Disables term mapping during transformation
				var skipTermStoreMapping = "";
				// From Cmdlet Help: Specifies a LDAP connection string e.g. LDAP://OU=Users,DC=Contoso,DC=local
				var lDAPConnectionString = "";

                var results = scope.ExecuteCommand("ConvertTo-PnPClientSidePage",
					new CommandParameter("Identity", identity),
					new CommandParameter("Library", library),
					new CommandParameter("Folder", folder),
					new CommandParameter("WebPartMappingFile", webPartMappingFile),
					new CommandParameter("Overwrite", overwrite),
					new CommandParameter("TakeSourcePageName", takeSourcePageName),
					new CommandParameter("ReplaceHomePageWithDefault", replaceHomePageWithDefault),
					new CommandParameter("AddPageAcceptBanner", addPageAcceptBanner),
					new CommandParameter("SkipItemLevelPermissionCopyToClientSidePage", skipItemLevelPermissionCopyToClientSidePage),
					new CommandParameter("SkipUrlRewriting", skipUrlRewriting),
					new CommandParameter("SkipDefaultUrlRewriting", skipDefaultUrlRewriting),
					new CommandParameter("UrlMappingFile", urlMappingFile),
					new CommandParameter("ClearCache", clearCache),
					new CommandParameter("CopyPageMetadata", copyPageMetadata),
					new CommandParameter("AddTableListImageAsImageWebPart", addTableListImageAsImageWebPart),
					new CommandParameter("UseCommunityScriptEditor", useCommunityScriptEditor),
					new CommandParameter("SummaryLinksToHtml", summaryLinksToHtml),
					new CommandParameter("TargetWebUrl", targetWebUrl),
					new CommandParameter("LogType", logType),
					new CommandParameter("LogFolder", logFolder),
					new CommandParameter("LogSkipFlush", logSkipFlush),
					new CommandParameter("LogVerbose", logVerbose),
					new CommandParameter("DontPublish", dontPublish),
					new CommandParameter("KeepPageCreationModificationInformation", keepPageCreationModificationInformation),
					new CommandParameter("SetAuthorInPageHeader", setAuthorInPageHeader),
					new CommandParameter("PostAsNews", postAsNews),
					new CommandParameter("DisablePageComments", disablePageComments),
					new CommandParameter("PublishingPage", publishingPage),
					new CommandParameter("BlogPage", blogPage),
					new CommandParameter("DelveBlogPage", delveBlogPage),
					new CommandParameter("DelveKeepSubTitle", delveKeepSubTitle),
					new CommandParameter("PageLayoutMapping", pageLayoutVarMapping),
					new CommandParameter("PublishingTargetPageName", publishingTargetPageName),
					new CommandParameter("TargetPageName", targetPageName),
					new CommandParameter("TargetPageFolder", targetPageFolder),
					new CommandParameter("TargetPageFolderOverridesDefaultFolder", targetPageFolderOverridesDefaultFolder),
					new CommandParameter("RemoveEmptySectionsAndColumns", removeEmptySectionsAndColumns),
					new CommandParameter("TargetConnection", targetConnection),
					new CommandParameter("SkipUserMapping", skipUserMapping),
					new CommandParameter("UserMappingFile", userMappingFile),
					new CommandParameter("TermMappingFile", termMappingFile),
					new CommandParameter("SkipTermStoreMapping", skipTermStoreMapping),
					new CommandParameter("LDAPConnectionString", lDAPConnectionString));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            