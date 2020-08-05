#if !ONPREMISES && !PNPPSCORE
using PnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Management.Automation;
using PnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.Modernization.Framework.Transform;
using PnP.PowerShell.Commands.Utilities;
using System.Reflection;
using SharePointPnP.Modernization.Framework.Cache;
using Microsoft.SharePoint.Client;
using System.Xml.Serialization;
using SharePointPnP.Modernization.Framework;
using System.IO;
using SharePointPnP.Modernization.Framework.Telemetry.Observers;
using SharePointPnP.Modernization.Framework.Publishing;
using PnP.PowerShell.Commands.Base;
using SharePointPnP.Modernization.Framework.Delve;

namespace PnP.PowerShell.Commands.ClientSidePages
{

    [Cmdlet(VerbsData.ConvertTo, "PnPClientSidePage")]
    [CmdletHelp("Converts a classic page (wiki or web part page) into a Client-Side Page",
                Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -Overwrite",
    Remarks = "Converts a wiki/web part page named 'somepage' to a client side page",
    SortOrder = 1)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -Overwrite -WebPartMappingFile c:\contoso\webpartmapping.xml",
    Remarks = "Converts a wiki/web part page named 'somepage' to a client side page using a custom provided mapping file",
    SortOrder = 2)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -Overwrite -AddPageAcceptBanner",
    Remarks = "Converts a wiki/web part page named 'somepage' to a client side page and adds the page accept banner web part on top of the page. This requires that the SPFX solution holding the web part (https://github.com/SharePoint/sp-dev-modernization/blob/master/Solutions/PageTransformationUI/assets/sharepointpnp-pagetransformation-client.sppkg?raw=true) has been installed to the tenant app catalog",
    SortOrder = 3)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -Overwrite -CopyPageMetadata",
    Remarks = "Converts a wiki/web part page named 'somepage' to a client side page, including the copying of the page metadata (if any)",
    SortOrder = 4)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -PublishingPage -Overwrite -TargetWebUrl https://contoso.sharepoint.com/sites/targetmodernsite",
    Remarks = "Converts a publishing page named 'somepage' to a client side page in the https://contoso.sharepoint.com/sites/targetmodernsite site",
    SortOrder = 5)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -PublishingPage -Overwrite -TargetConnection $target",
    Remarks = "Converts a publishing page named 'somepage' to a client side page in the site specified by the TargetConnection connection. This allows to read a page in one environment (on-premises, tenant A) and create in another online location (tenant B)",
    SortOrder = 6)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -Library ""SiteAssets"" -Folder ""Folder1"" -Overwrite",
    Remarks = "Converts a web part page named 'somepage' living inside the SiteAssets library in a folder named folder1 into a client side page",
    SortOrder = 7)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -Folder ""<root>"" -Overwrite",
    Remarks = "Converts a web part page named 'somepage' living inside the root of the site collection (so outside of a library)",
    SortOrder = 8)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -Overwrite -TargetWebUrl https://contoso.sharepoint.com/sites/targetmodernsite",
    Remarks = "Converts a wiki/web part page named 'somepage' to a client side page in the https://contoso.sharepoint.com/sites/targetmodernsite site",
    SortOrder = 9)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -LogType File -LogFolder c:\temp -LogVerbose -Overwrite",
    Remarks = "Converts a wiki/web part page named 'somepage' and creates a log file in c:\\temp using verbose logging",
    SortOrder = 10)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -LogType SharePoint -LogSkipFlush",
    Remarks = "Converts a wiki/web part page named 'somepage' and creates a log file in SharePoint but skip the actual write. Use this option to make multiple ConvertTo-PnPClientSidePage invocations create a single log",
    SortOrder = 11)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""My post title"" -BlogPage -LogType Console -Overwrite -TargetWebUrl https://contoso.sharepoint.com/sites/targetmodernsite",
    Remarks = "Converts a blog page with a title starting with 'my post title' to a client side page in the https://contoso.sharepoint.com/sites/targetmodernsite site",
    SortOrder = 12)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""My post title"" -DelveBlogPage -LogType Console -Overwrite -TargetWebUrl https://contoso.sharepoint.com/sites/targetmodernsite",
    Remarks = "Converts a Delve blog page with a title starting with 'my post title' to a client side page in the https://contoso.sharepoint.com/sites/targetmodernsite site",
    SortOrder = 13)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -PublishingPage -Overwrite -TargetConnection $target -UserMappingFile c:\\temp\user_mapping_file.csv",
    Remarks = "Converts a publishing page named 'somepage' to a client side page in the site specified by the TargetConnection connection. This allows to read a page in on-premises environment and create in another online locations including using specific user mappings between the two environments.",
    SortOrder = 14)]
    public class ConvertToClientSidePage : PnPWebCmdlet
    {
        private static string rootFolder = "<root>";
        private Assembly modernizationAssembly;
        private Assembly sitesCoreAssembly;
       // private Assembly newtonsoftAssembly;

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page to convert")]
        public PagePipeBind Identity;

        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the library containing the page. If SitePages then please omit this parameter")]
        public string Library;

        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "The folder to load the provided page from. If not provided all folders are searched")]
        public string Folder;

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, HelpMessage = "Path and name of the web part mapping file driving the transformation")]
        public string WebPartMappingFile;

        [Parameter(Mandatory = false, HelpMessage = "Overwrites page if already existing")]
        public SwitchParameter Overwrite = false;

        [Parameter(Mandatory = false, HelpMessage = "Created client side page takes name from previous classic page. Classic page gets renamed to previous_<Page>.aspx")]
        public SwitchParameter TakeSourcePageName = false;

        [Parameter(Mandatory = false, HelpMessage = "Replaces a home page with a default stock modern home page")]
        public SwitchParameter ReplaceHomePageWithDefault = false;

        [Parameter(Mandatory = false, HelpMessage = "Adds the page accept banner web part. The actual web part is specified in webpartmapping.xml file")]
        public SwitchParameter AddPageAcceptBanner = false;

        [Parameter(Mandatory = false, HelpMessage = "By default the item level permissions on a page are copied to the created client side page. Use this switch to prevent the copy")]
        public SwitchParameter SkipItemLevelPermissionCopyToClientSidePage = false;

        [Parameter(Mandatory = false, HelpMessage = "If transforming cross site then by default urls in html and summarylinks are rewritten for the target site. Set this flag to prevent that")]
        public SwitchParameter SkipUrlRewriting = false;

        [Parameter(Mandatory = false, HelpMessage = "Set this flag to prevent the default URL rewriting while you still want to do URL rewriting using a custom URL mapping file")]
        public SwitchParameter SkipDefaultUrlRewriting = false;

        [Parameter(Mandatory = false, HelpMessage = "File holding custom URL mapping definitions")]
        public string UrlMappingFile = "";

        [Parameter(Mandatory = false, HelpMessage = "Clears the cache. Can be needed if you've installed a new web part to the site and want to use that in a custom webpartmapping file. Restarting your PS session has the same effect")]
        public SwitchParameter ClearCache = false;

        [Parameter(Mandatory = false, HelpMessage = "Copies the page metadata to the created modern page")]
        public SwitchParameter CopyPageMetadata = false;

        [Parameter(Mandatory = false, HelpMessage = "When an image lives inside a table/list then it's also created as separate image web part underneath that table/list by default. Use this switch set to $false to change that")]
        public SwitchParameter AddTableListImageAsImageWebPart = true;

        [Parameter(Mandatory = false, HelpMessage = "Uses the community script editor (https://github.com/SharePoint/sp-dev-fx-webparts/tree/master/samples/react-script-editor) as replacement for the classic script editor web part")]
        public SwitchParameter UseCommunityScriptEditor = false;

        [Parameter(Mandatory = false, HelpMessage = "By default summarylinks web parts are replaced by QuickLinks, but you can transform to plain html by setting this switch")]
        public SwitchParameter SummaryLinksToHtml = false;

        [Parameter(Mandatory = false, HelpMessage = "Url of the target web that will receive the modern page. Defaults to null which means in-place transformation")]
        public string TargetWebUrl;

        [Parameter(Mandatory = false, HelpMessage = "Allows to generate a transformation log (File | SharePoint)")]
        public ClientSidePageTransformatorLogType LogType = ClientSidePageTransformatorLogType.None;

        [Parameter(Mandatory = false, HelpMessage = "Folder in where the log file will be created (if LogType==File)")]
        public string LogFolder = "";

        [Parameter(Mandatory = false, HelpMessage = "By default each cmdlet invocation will result in a log file, use the -SkipLogFlush to delay the log flushing. The first call without -SkipLogFlush will then write all log entries to a single log")]
        public SwitchParameter LogSkipFlush = false;

        [Parameter(Mandatory = false, HelpMessage = "Configure logging to include verbose log entries")]
        public SwitchParameter LogVerbose = false;

        [Parameter(Mandatory = false, HelpMessage = "Don't publish the created modern page")]
        public SwitchParameter DontPublish = false;

        [Parameter(Mandatory = false, HelpMessage = "Keep the author, editor, created and modified information from the source page (when source page lives in SPO)")]
        public SwitchParameter KeepPageCreationModificationInformation = false;

        [Parameter(Mandatory = false, HelpMessage = "Set's the author of the source page as author in the modern page header (when source page lives in SPO)")]
        public SwitchParameter SetAuthorInPageHeader = false;

        [Parameter(Mandatory = false, HelpMessage = "Post the created, and published, modern page as news")]
        public SwitchParameter PostAsNews = false;

        [Parameter(Mandatory = false, HelpMessage = "Disable comments for the created modern page")]
        public SwitchParameter DisablePageComments = false;

        [Parameter(Mandatory = false, HelpMessage = "I'm transforming a publishing page")]
        public SwitchParameter PublishingPage = false;

        [Parameter(Mandatory = false, HelpMessage = "I'm transforming a blog page")]
        public SwitchParameter BlogPage = false;

        [Parameter(Mandatory = false, HelpMessage = "I'm transforming a Delve blog page")]
        public SwitchParameter DelveBlogPage = false;

        [Parameter(Mandatory = false, HelpMessage = "Transform the possible sub title as topic header on the modern page")]
        public SwitchParameter DelveKeepSubTitle = false;

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, HelpMessage = "Path and name of the page layout mapping file driving the publishing page transformation")]
        public string PageLayoutMapping;

        [Parameter(Mandatory = false, HelpMessage = "Name for the target page (only applies to publishing page transformation)")]
        public string PublishingTargetPageName = "";

        [Parameter(Mandatory = false, HelpMessage = "Name for the target page (only applies when doing cross site page transformation)")]
        public string TargetPageName = "";

        [Parameter(Mandatory = false, HelpMessage = "Folder to create the target page in (will be used in conjunction with auto-generated folders that ensure page name uniqueness)")]
        public string TargetPageFolder = "";

        [Parameter(Mandatory = false, HelpMessage = "When setting a target page folder then the target page folder overrides possibly default folder path (e.g. in the source page lived in a folder) instead of being appended to it")]
        public SwitchParameter TargetPageFolderOverridesDefaultFolder = false;

        [Parameter(Mandatory = false, HelpMessage = "Remove empty sections and columns after transformation of the page")]
        public SwitchParameter RemoveEmptySectionsAndColumns = true;

        [Parameter(Mandatory = false, HelpMessage = "Optional connection to be used by the cmdlet. Retrieve the value for this parameter by either specifying -ReturnConnection on Connect-PnPOnline or by executing Get-PnPConnection.")] // do not remove '#!#99'
        public PnPConnection TargetConnection = null;

        [Parameter(Mandatory = false, HelpMessage = "Disables user mapping during transformation")]
        public SwitchParameter SkipUserMapping = false;

        [Parameter(Mandatory = false, HelpMessage = "Specifies a user mapping file")]
        public string UserMappingFile = "";

        [Parameter(Mandatory = false, HelpMessage = "Specifies a taxonomy term mapping file")]
        public string TermMappingFile = "";

        [Parameter(Mandatory = false, HelpMessage = "Disables term mapping during transformation")]
        public SwitchParameter SkipTermStoreMapping = false;

        [Parameter(Mandatory = false, HelpMessage = "Specifies a LDAP connection string e.g. LDAP://OU=Users,DC=Contoso,DC=local")]
        public string LDAPConnectionString = "";

        protected override void ExecuteCmdlet()
        {
            //Fix loading of modernization framework
            FixLocalAssemblyResolving();
            
            // Load the page to transform
            Identity.Library = this.Library;
            Identity.Folder = this.Folder;
            Identity.BlogPage = this.BlogPage;
            Identity.DelveBlogPage = this.DelveBlogPage;

            if ((this.PublishingPage && this.BlogPage) ||
                (this.PublishingPage && this.DelveBlogPage) ||
                (this.BlogPage && this.DelveBlogPage))
            {
                throw new Exception($"The page is either a blog page, a publishing page or a Delve blog page. Setting PublishingPage, BlogPage and DelveBlogPage to true is not valid.");
            }
            
            ListItem page = null;
            if (this.PublishingPage)
            {
                page = Identity.GetPage(this.ClientContext.Web, CacheManager.Instance.GetPublishingPagesLibraryName(this.ClientContext));
            }
            else if (this.BlogPage)
            {
                // Blogs don't live in other libraries or sub folders
                Identity.Library = null;
                Identity.Folder = null;
                page = Identity.GetPage(this.ClientContext.Web, CacheManager.Instance.GetBlogListName(this.ClientContext));
            }
            else if (this.DelveBlogPage)
            {
                // Blogs don't live in other libraries or sub folders
                Identity.Library = null;
                Identity.Folder = null;
                page = Identity.GetPage(this.ClientContext.Web, "pPg");
            }
            else
            {
                if (this.Folder == null || !this.Folder.Equals(rootFolder, StringComparison.InvariantCultureIgnoreCase))
                {
                    page = Identity.GetPage(this.ClientContext.Web, "sitepages");
                }
            }

            if (page == null && (Folder == null || !this.Folder.Equals(rootFolder, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new Exception($"Page '{Identity?.Name}' does not exist");
            }

            // Publishing specific validation
            if (this.PublishingPage && string.IsNullOrEmpty(this.TargetWebUrl) && TargetConnection == null)
            {
                throw new Exception($"Publishing page transformation is only supported when transformating into another site collection. Use the -TargetWebUrl to specify a modern target site.");
            }

            // Blog specific validation
            if ((this.BlogPage || this.DelveBlogPage) && string.IsNullOrEmpty(this.TargetWebUrl) && TargetConnection == null)
            {
                throw new Exception($"Blog and Delve blog page transformation is only supported when transformating into another site collection. Use the -TargetWebUrl to specify a modern target site.");
            }

            // Load transformation models
            PageTransformation webPartMappingModel = null;
            if (string.IsNullOrEmpty(this.WebPartMappingFile))
            {
                webPartMappingModel = PageTransformator.LoadDefaultWebPartMapping();
                this.WriteVerbose("Using embedded webpartmapping file. Use Export-PnPClientSidePageMapping to get that file in case you want to base your version of the embedded version.");
            }

            // Validate webpartmappingfile
            if (!string.IsNullOrEmpty(this.WebPartMappingFile))
            {
                if (!System.IO.File.Exists(this.WebPartMappingFile))
                {
                    throw new Exception($"Provided webpartmapping file {this.WebPartMappingFile} does not exist");
                }
            }

            if (this.PublishingPage && !string.IsNullOrEmpty(this.PageLayoutMapping) && !System.IO.File.Exists(this.PageLayoutMapping))
            {
                throw new Exception($"Provided pagelayout mapping file {this.PageLayoutMapping} does not exist");
            }

            bool crossSiteTransformation = TargetConnection != null || !string.IsNullOrEmpty(TargetWebUrl);

            // Create target client context (when needed)
            ClientContext targetContext = null;
            if (TargetConnection == null)
            {
                if (!string.IsNullOrEmpty(TargetWebUrl))
                {
                    targetContext = this.ClientContext.Clone(TargetWebUrl);
                }
            }
            else
            {
                targetContext = TargetConnection.Context;
            }


            // Create transformator instance
            PageTransformator pageTransformator = null;
            PublishingPageTransformator publishingPageTransformator = null;
            DelvePageTransformator delvePageTransformator = null;

            if (!string.IsNullOrEmpty(this.WebPartMappingFile))
            {
                // Using custom web part mapping file
                if (this.PublishingPage)
                {
                    if (!string.IsNullOrEmpty(this.PageLayoutMapping))
                    {
                        // Using custom page layout mapping file + default one (they're merged together)
                        publishingPageTransformator = new PublishingPageTransformator(this.ClientContext, targetContext, this.WebPartMappingFile, this.PageLayoutMapping);
                    }
                    else
                    {
                        // Using default page layout mapping file
                        publishingPageTransformator = new PublishingPageTransformator(this.ClientContext, targetContext, this.WebPartMappingFile, null);
                    }
                }
                else if (this.DelveBlogPage)
                {
                    delvePageTransformator = new DelvePageTransformator(this.ClientContext, targetContext, this.WebPartMappingFile);
                }
                else
                {
                    // Use web part mapping file
                    pageTransformator = new PageTransformator(this.ClientContext, targetContext, this.WebPartMappingFile);
                }
            }
            else
            {
                // Using default web part mapping file
                if (this.PublishingPage)
                {
                    if (!string.IsNullOrEmpty(this.PageLayoutMapping))
                    {
                        // Load and validate the custom mapping file
                        PageLayoutManager pageLayoutManager = new PageLayoutManager();
                        var pageLayoutMappingModel = pageLayoutManager.LoadPageLayoutMappingFile(this.PageLayoutMapping);

                        // Using custom page layout mapping file + default one (they're merged together)
                        publishingPageTransformator = new PublishingPageTransformator(this.ClientContext, targetContext, webPartMappingModel, pageLayoutMappingModel);
                    }
                    else
                    {
                        // Using default page layout mapping file
                        publishingPageTransformator = new PublishingPageTransformator(this.ClientContext, targetContext, webPartMappingModel, null);
                    }
                }
                else if (this.DelveBlogPage)
                {
                    delvePageTransformator = new DelvePageTransformator(this.ClientContext, targetContext, webPartMappingModel);
                }
                else
                {
                    // Use web part mapping model loaded from embedded mapping file
                    pageTransformator = new PageTransformator(this.ClientContext, targetContext, webPartMappingModel);
                }
            }

            // Setup logging
            if (this.LogType == ClientSidePageTransformatorLogType.File)
            {
                if (this.PublishingPage)
                {
                    publishingPageTransformator.RegisterObserver(new MarkdownObserver(folder: this.LogFolder, includeVerbose:this.LogVerbose, includeDebugEntries: this.LogVerbose));
                }
                else if (this.DelveBlogPage)
                {
                    delvePageTransformator.RegisterObserver(new MarkdownObserver(folder: this.LogFolder, includeVerbose: this.LogVerbose, includeDebugEntries: this.LogVerbose));
                }
                else
                {
                    pageTransformator.RegisterObserver(new MarkdownObserver(folder: this.LogFolder, includeVerbose: this.LogVerbose, includeDebugEntries: this.LogVerbose));
                }
            }
            else if (this.LogType == ClientSidePageTransformatorLogType.SharePoint)
            {
                if (this.PublishingPage)
                {
                    publishingPageTransformator.RegisterObserver(new MarkdownToSharePointObserver(targetContext ?? this.ClientContext, includeVerbose: this.LogVerbose, includeDebugEntries: this.LogVerbose));
                }
                else if (this.DelveBlogPage)
                {
                    delvePageTransformator.RegisterObserver(new MarkdownToSharePointObserver(targetContext ?? this.ClientContext, includeVerbose: this.LogVerbose, includeDebugEntries: this.LogVerbose));
                }
                else
                {
                    pageTransformator.RegisterObserver(new MarkdownToSharePointObserver(targetContext ?? this.ClientContext, includeVerbose: this.LogVerbose, includeDebugEntries: this.LogVerbose));
                }
            }
            else if (this.LogType == ClientSidePageTransformatorLogType.Console)
            {
                if (this.PublishingPage)
                {
                    publishingPageTransformator.RegisterObserver(new ConsoleObserver(includeDebugEntries: this.LogVerbose));
                }
                else if (this.DelveBlogPage)
                {
                    delvePageTransformator.RegisterObserver(new ConsoleObserver(includeDebugEntries: this.LogVerbose));
                }
                else
                {
                    pageTransformator.RegisterObserver(new ConsoleObserver(includeDebugEntries: this.LogVerbose));
                }
            }

            // Clear the client side component cache
            if (this.ClearCache)
            {
                CacheManager.Instance.ClearAllCaches();
            }

            string serverRelativeClientPageUrl = "";
            if (this.PublishingPage)
            {
                // Setup Transformation information
                PublishingPageTransformationInformation pti = new PublishingPageTransformationInformation(page)
                {
                    Overwrite = this.Overwrite,
                    KeepPageSpecificPermissions = !this.SkipItemLevelPermissionCopyToClientSidePage,
                    PublishCreatedPage = !this.DontPublish,
                    KeepPageCreationModificationInformation = this.KeepPageCreationModificationInformation,
                    PostAsNews = this.PostAsNews,
                    DisablePageComments = this.DisablePageComments,
                    TargetPageName = !string.IsNullOrEmpty(this.PublishingTargetPageName) ? this.PublishingTargetPageName : this.TargetPageName,
                    SkipUrlRewrite = this.SkipUrlRewriting,
                    SkipDefaultUrlRewrite = this.SkipDefaultUrlRewriting,
                    UrlMappingFile = this.UrlMappingFile,
                    AddTableListImageAsImageWebPart = this.AddTableListImageAsImageWebPart,
                    SkipUserMapping = this.SkipUserMapping,
                    UserMappingFile = this.UserMappingFile,
                    LDAPConnectionString = this.LDAPConnectionString,
                    TargetPageFolder = this.TargetPageFolder,
                    TargetPageFolderOverridesDefaultFolder = this.TargetPageFolderOverridesDefaultFolder,
                    TermMappingFile = TermMappingFile,
                    SkipTermStoreMapping = SkipTermStoreMapping,
                    RemoveEmptySectionsAndColumns = this.RemoveEmptySectionsAndColumns
                };

                // Set mapping properties
                pti.MappingProperties["SummaryLinksToQuickLinks"] = (!SummaryLinksToHtml).ToString().ToLower();
                pti.MappingProperties["UseCommunityScriptEditor"] = UseCommunityScriptEditor.ToString().ToLower();

                try
                {
                    serverRelativeClientPageUrl = publishingPageTransformator.Transform(pti);
                }
                finally
                {
                    // Flush log
                    if (this.LogType != ClientSidePageTransformatorLogType.None && this.LogType != ClientSidePageTransformatorLogType.Console && !this.LogSkipFlush)
                    {
                        publishingPageTransformator.FlushObservers();
                    }
                }
            }
            else if (this.DelveBlogPage)
            {
                // Setup Transformation information
                DelvePageTransformationInformation pti = new DelvePageTransformationInformation(page)
                {
                    Overwrite = this.Overwrite,
                    KeepPageSpecificPermissions = !this.SkipItemLevelPermissionCopyToClientSidePage,
                    PublishCreatedPage = !this.DontPublish,
                    KeepPageCreationModificationInformation = this.KeepPageCreationModificationInformation,
                    SetAuthorInPageHeader = this.SetAuthorInPageHeader,
                    PostAsNews = this.PostAsNews,
                    DisablePageComments = this.DisablePageComments,
                    TargetPageName = crossSiteTransformation ? this.TargetPageName : "",
                    SkipUrlRewrite = this.SkipUrlRewriting,
                    SkipDefaultUrlRewrite = this.SkipDefaultUrlRewriting,
                    UrlMappingFile = this.UrlMappingFile,
                    AddTableListImageAsImageWebPart = this.AddTableListImageAsImageWebPart,
                    SkipUserMapping = this.SkipUserMapping,
                    UserMappingFile = this.UserMappingFile,
                    LDAPConnectionString = this.LDAPConnectionString,
                    TargetPageFolder = this.TargetPageFolder,
                    TargetPageFolderOverridesDefaultFolder = this.TargetPageFolderOverridesDefaultFolder,
                    RemoveEmptySectionsAndColumns = this.RemoveEmptySectionsAndColumns
                };

                // Set mapping properties
                pti.MappingProperties["SummaryLinksToQuickLinks"] = (!SummaryLinksToHtml).ToString().ToLower();
                pti.MappingProperties["UseCommunityScriptEditor"] = UseCommunityScriptEditor.ToString().ToLower();

                try
                {
                    serverRelativeClientPageUrl = delvePageTransformator.Transform(pti);
                }
                finally
                {
                    // Flush log
                    if (this.LogType != ClientSidePageTransformatorLogType.None && this.LogType != ClientSidePageTransformatorLogType.Console && !this.LogSkipFlush)
                    {
                        pageTransformator.FlushObservers();
                    }
                }
            }
            else
            {
                Microsoft.SharePoint.Client.File fileToModernize = null;
                if (this.Folder != null && this.Folder.Equals(rootFolder, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Load the page file from the site root folder
                    var webServerRelativeUrl = this.ClientContext.Web.EnsureProperty(p => p.ServerRelativeUrl);
                    fileToModernize = this.ClientContext.Web.GetFileByServerRelativeUrl($"{webServerRelativeUrl}/{this.Identity.Name}");
                    this.ClientContext.Load(fileToModernize);
                    this.ClientContext.ExecuteQueryRetry();
                }

                // Setup Transformation information
                PageTransformationInformation pti = new PageTransformationInformation(page)
                {
                    SourceFile = fileToModernize,
                    Overwrite = this.Overwrite,
                    TargetPageTakesSourcePageName = this.TakeSourcePageName,
                    ReplaceHomePageWithDefaultHomePage = this.ReplaceHomePageWithDefault,
                    KeepPageSpecificPermissions = !this.SkipItemLevelPermissionCopyToClientSidePage,
                    CopyPageMetadata = this.CopyPageMetadata,
                    PublishCreatedPage = !this.DontPublish,
                    KeepPageCreationModificationInformation = this.KeepPageCreationModificationInformation,
                    SetAuthorInPageHeader = this.SetAuthorInPageHeader,
                    PostAsNews = this.PostAsNews,
                    DisablePageComments = this.DisablePageComments,
                    TargetPageName = crossSiteTransformation ? this.TargetPageName : "",
                    SkipUrlRewrite = this.SkipUrlRewriting,
                    SkipDefaultUrlRewrite = this.SkipDefaultUrlRewriting,
                    UrlMappingFile = this.UrlMappingFile,
                    AddTableListImageAsImageWebPart = this.AddTableListImageAsImageWebPart,
                    SkipUserMapping = this.SkipUserMapping,
                    UserMappingFile = this.UserMappingFile,
                    LDAPConnectionString = this.LDAPConnectionString,
                    TargetPageFolder = this.TargetPageFolder,
                    TargetPageFolderOverridesDefaultFolder = this.TargetPageFolderOverridesDefaultFolder,
                    ModernizationCenterInformation = new ModernizationCenterInformation()
                    {
                        AddPageAcceptBanner = this.AddPageAcceptBanner
                    },
                    TermMappingFile = TermMappingFile,
                    SkipTermStoreMapping = SkipTermStoreMapping,
                    RemoveEmptySectionsAndColumns = this.RemoveEmptySectionsAndColumns
                };

                // Set mapping properties
                pti.MappingProperties["SummaryLinksToQuickLinks"] = (!SummaryLinksToHtml).ToString().ToLower();
                pti.MappingProperties["UseCommunityScriptEditor"] = UseCommunityScriptEditor.ToString().ToLower();

                try
                {
                    serverRelativeClientPageUrl = pageTransformator.Transform(pti);
                }
                finally
                {
                    // Flush log
                    if (this.LogType != ClientSidePageTransformatorLogType.None && this.LogType != ClientSidePageTransformatorLogType.Console && !this.LogSkipFlush)
                    {
                        pageTransformator.FlushObservers();
                    }
                }
            }

            // Output the server relative url to the newly created page
            if (!string.IsNullOrEmpty(serverRelativeClientPageUrl))
            {
                WriteObject(serverRelativeClientPageUrl);
            }
        }

        private Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private void FixLocalAssemblyResolving()
        {
            try
            {
                sitesCoreAssembly = Assembly.LoadFrom(Path.Combine(AssemblyDirectory, "OfficeDevPnP.Core.dll"));
                modernizationAssembly = Assembly.LoadFrom(Path.Combine(AssemblyDirectory, "SharePointPnP.Modernization.Framework.dll"));
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_LocalAssemblyResolve;
            }
            catch { }
        }

        private Assembly CurrentDomain_LocalAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("OfficeDevPnP.Core"))
            {
                return sitesCoreAssembly;
            }
            if (args.Name.StartsWith("SharePointPnP.Modernization.Framework"))
            {
                return modernizationAssembly;
            }
            return null;
        }
    }
}
#endif