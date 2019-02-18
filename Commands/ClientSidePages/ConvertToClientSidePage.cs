#if !ONPREMISES
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Management.Automation;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.Modernization.Framework.Transform;
using SharePointPnP.PowerShell.Commands.Utilities;
using System.Reflection;
using SharePointPnP.Modernization.Framework.Cache;
using Microsoft.SharePoint.Client;

namespace SharePointPnP.PowerShell.Commands.ClientSidePages
{

    [Cmdlet(VerbsData.ConvertTo, "PnPClientSidePage")]
    [CmdletHelp("Converts a classic page (wiki or web part page) into a Client-Side Page",
                Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -Overwrite",
    Remarks = "Converts a wiki page named 'somepage' to a client side page",
    SortOrder = 1)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -Overwrite -WebPartMappingFile c:\contoso\webpartmapping.xml",
    Remarks = "Converts a wiki page named 'somepage' to a client side page using a custom provided mapping file",
    SortOrder = 2)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -Overwrite -AddPageAcceptBanner",
    Remarks = "Converts a wiki page named 'somepage' to a client side page and adds the page accept banner web part on top of the page. This requires that the SPFX solution holding the web part (https://github.com/SharePoint/sp-dev-modernization/blob/master/Solutions/PageTransformationUI/assets/sharepointpnp-pagetransformation-client.sppkg?raw=true) has been installed to the tenant app catalog.",
    SortOrder = 3)]
    [CmdletExample(
    Code = @"PS:> ConvertTo-PnPClientSidePage -Identity ""somepage.aspx"" -Overwrite -CopyPageMetadata",
    Remarks = "Converts a wiki page named 'somepage' to a client side page, including the copying of the page metadata (if any)",
    SortOrder = 4)]
    public class ConvertToClientSidePage : PnPWebCmdlet
    {
        private Assembly modernizationAssembly;
        private Assembly sitesCoreAssembly;
        private Assembly newtonsoftAssembly;

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page to convert")]
        public PagePipeBind Identity;

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true, HelpMessage = "Path and name of the web part mapping file driving the transformation", ParameterSetName = "WebPartMappingFile")]
        public string WebPartMappingFile;

        [Parameter(Mandatory = false, HelpMessage = "Overwrites page if already existing")]
        public SwitchParameter Overwrite = false;

        [Parameter(Mandatory = false, HelpMessage = "Created client side page takes name from previous classic page. Classic page gets renamed to previous_<Page>.aspx")]
        public SwitchParameter TakeSourcePageName = false;

        [Parameter(Mandatory = false, HelpMessage = "Replaces a home page with a default stock modern home page")]
        public SwitchParameter ReplaceHomePageWithDefault = false;

        [Parameter(Mandatory = false, HelpMessage = "Adds the page accept banner web part. The actual web part is specified in webpartmapping.xml file")]
        public SwitchParameter AddPageAcceptBanner = false;

        [Parameter(Mandatory = false, HelpMessage = "By default the item level permissions on a page are copied to the created client side page. Use this switch to prevent the copy.")]
        public SwitchParameter SkipItemLevelPermissionCopyToClientSidePage = false;

        [Parameter(Mandatory = false, HelpMessage = "Clears the cache. Can be needed if you've installed a new web part to the site and want to use that in a custom webpartmapping file. Restarting your PS session has the same effect.")]
        public SwitchParameter ClearCache = false;

        [Parameter(Mandatory = false, HelpMessage = "Copies the page metadata to the created modern page.")]
        public SwitchParameter CopyPageMetadata = false;

        protected override void ExecuteCmdlet()
        {
            string tempPath = null;

            try
            {
                //Fix loading of modernization framework
                FixAssemblyResolving();

                // Load the page to transform
                var page = Identity.GetPage(this.ClientContext.Web);

                if (page == null)
                {
                    throw new Exception($"Page '{Identity?.Name}' does not exist");
                }

                if (string.IsNullOrEmpty(this.WebPartMappingFile))
                {
                    // Load the default one from resources
                    string webpartMappingFileContents = WebPartMappingLoader.LoadFile("SharePointPnP.PowerShell.Commands.ClientSidePages.webpartmapping.xml");

                    // Save the file to a temp location
                    tempPath = System.IO.Path.GetTempFileName();
                    System.IO.File.WriteAllText(tempPath, webpartMappingFileContents);
                    this.WebPartMappingFile = tempPath;

                    this.WriteVerbose("Using embedded webpartmapping file (https://github.com/SharePoint/PnP-PowerShell/blob/master/Commands/ClientSidePages/webpartmapping.xml)");
                }

                // Validate webpartmappingfile
                if (!string.IsNullOrEmpty(this.WebPartMappingFile))
                {
                    if (!System.IO.File.Exists(this.WebPartMappingFile))
                    {
                        throw new Exception($"Provided webpartmapping file {this.WebPartMappingFile} does not exist");
                    }
                }

                // Create transformator instance
                PageTransformator pageTransformator = new PageTransformator(this.ClientContext, this.WebPartMappingFile);

                // Setup Transformation information
                PageTransformationInformation pti = new PageTransformationInformation(page)
                {
                    Overwrite = this.Overwrite,
                    TargetPageTakesSourcePageName = this.TakeSourcePageName,
                    ReplaceHomePageWithDefaultHomePage = this.ReplaceHomePageWithDefault,
                    KeepPageSpecificPermissions = !this.SkipItemLevelPermissionCopyToClientSidePage,
                    CopyPageMetadata = this.CopyPageMetadata,
                    ModernizationCenterInformation = new ModernizationCenterInformation()
                    {
                        AddPageAcceptBanner = this.AddPageAcceptBanner
                    },
                };

                // Clear the client side component cache
                if (this.ClearCache)
                {
                    CacheManager.Instance.ClearClientSideComponents();
                    CacheManager.Instance.ClearFieldsToCopy();
                    CacheManager.Instance.ClearBaseTemplate();
                }

                string serverRelativeClientPageUrl = pageTransformator.Transform(pti);

                // Output the server relative url to the newly created page
                if (!string.IsNullOrEmpty(serverRelativeClientPageUrl))
                {
                    WriteObject(serverRelativeClientPageUrl);
                }
            }
            finally
            {
                if (!string.IsNullOrEmpty(tempPath) && System.IO.File.Exists(tempPath))
                {
                    System.IO.File.Delete(tempPath);
                }
            }
        }

        private string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        private void FixAssemblyResolving()
        {
            try
            {
                newtonsoftAssembly = Assembly.LoadFrom(System.IO.Path.Combine(AssemblyDirectory, "NewtonSoft.Json.dll"));
                sitesCoreAssembly = Assembly.LoadFrom(System.IO.Path.Combine(AssemblyDirectory, "OfficeDevPnP.Core.dll"));
                modernizationAssembly = Assembly.LoadFrom(System.IO.Path.Combine(AssemblyDirectory, "SharePointPnP.Modernization.Framework.dll"));
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            }
            catch { }
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("OfficeDevPnP.Core"))
            {
                return sitesCoreAssembly;
            }
            if (args.Name.StartsWith("Newtonsoft.Json"))
            {
                return newtonsoftAssembly;
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