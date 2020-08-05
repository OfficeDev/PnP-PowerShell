#if !ONPREMISES && !PNPPSCORE
using PnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Management.Automation;
using PnP.PowerShell.Commands.Utilities;
using System.Reflection;
using Microsoft.SharePoint.Client;
using System.IO;
using SharePointPnP.Modernization.Framework.Publishing;
using PnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.Modernization.Framework.Cache;
using SharePointPnP.Modernization.Framework.Telemetry.Observers;
using SharePointPnP.Modernization.Framework.Transform;

namespace PnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsData.Export, "PnPClientSidePageMapping")]
    [CmdletHelp("Get's the built-in maping files or a custom mapping file for your publishing portal page layouts. These mapping files are used to tailor the page transformation experience.",
                Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(Code = @"PS:> Export-PnPClientSidePageMapping -BuiltInPageLayoutMapping -CustomPageLayoutMapping -Folder c:\\temp -Overwrite",
                   Remarks = "Exports the built in page layout mapping and analyzes the current site's page layouts and exports these to files in folder c:\\temp",
                   SortOrder = 1)]
    [CmdletExample(Code = @"PS:> Export-PnPClientSidePageMapping -CustomPageLayoutMapping -PublishingPage mypage.aspx -Folder c:\\temp -Overwrite",
                   Remarks = "Analyzes the page layout of page mypage.aspx and exports this to a file in folder c:\\temp",
                   SortOrder = 2)]
    [CmdletExample(Code = @"PS:> Export-PnPClientSidePageMapping -BuiltInWebPartMapping -Folder c:\\temp -Overwrite",
                   Remarks = "Exports the built in webpart mapping to a file in folder c:\\temp. Use this a starting basis if you want to tailer the web part mapping behavior.",
                   SortOrder = 3)]
    public class ExportClientSidePageMapping : PnPWebCmdlet
    {
        private Assembly modernizationAssembly;
        private Assembly sitesCoreAssembly;

        [Parameter(Mandatory = false, HelpMessage = "Exports the builtin web part mapping file")]
        public SwitchParameter BuiltInWebPartMapping = false;

        [Parameter(Mandatory = false, HelpMessage = "Exports the builtin pagelayout mapping file (only needed for publishing page transformation)")]
        public SwitchParameter BuiltInPageLayoutMapping = false;

        [Parameter(Mandatory = false, HelpMessage = "Analyzes the pagelayouts in the current publishing portal and exports them as a pagelayout mapping file")]
        public SwitchParameter CustomPageLayoutMapping = false;

        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the publishing page to export a page layout mapping file for")]
        public PagePipeBind PublishingPage;

        [Parameter(Mandatory = false, HelpMessage = "Set this flag if you also want to analyze the OOB page layouts...typically these are covered via the default mapping, but if you've updated these page layouts you might want to analyze them again")]
        public SwitchParameter AnalyzeOOBPageLayouts = false;

        [Parameter(Mandatory = false, HelpMessage = "The folder to created the mapping file(s) in")]
        public string Folder;

        [Parameter(Mandatory = false, HelpMessage = "Overwrites existing mapping files")]
        public SwitchParameter Overwrite = false;

        [Parameter(Mandatory = false, HelpMessage = "Outputs analyser logging to the console")]
        public SwitchParameter Logging = false;


        protected override void ExecuteCmdlet()
        {
            //Fix loading of modernization framework
            FixLocalAssemblyResolving();

            // Configure folder to export
            string folderToExportTo = Environment.CurrentDirectory;
            if (!string.IsNullOrEmpty(this.Folder))
            {
                if (!Directory.Exists(this.Folder))
                {
                    throw new Exception($"Folder '{this.Folder}' does not exist");
                }

                folderToExportTo = this.Folder;
            }

            // Export built in web part mapping
            if (this.BuiltInWebPartMapping)
            {
                string fileName = Path.Combine(folderToExportTo, "webpartmapping.xml");

                if (System.IO.File.Exists(fileName) && !Overwrite)
                {
                    Console.WriteLine($"Skipping the export from the built-in webpart mapping file {fileName} as this already exists. Use the -Overwrite flag to overwrite if needed.");
                }
                else
                {
                    // Load the default one from resources into a model, no need for persisting this file
                    string webpartMappingFileContents = PageTransformator.LoadDefaultWebPartMappingFile();
                    System.IO.File.WriteAllText(fileName, webpartMappingFileContents);
                }
            }

            // Export built in page layout mapping
            if (this.BuiltInPageLayoutMapping)
            {
                string fileName = Path.Combine(folderToExportTo, "pagelayoutmapping.xml");

                if (System.IO.File.Exists(fileName) && !Overwrite)
                {
                    Console.WriteLine($"Skipping the export from the built-in pagelayout mapping file {fileName} as this already exists. Use the -Overwrite flag to overwrite if needed.");
                }
                else
                {
                    // Load the default one from resources into a model, no need for persisting this file
                    string pageLayoutMappingFileContents = PublishingPageTransformator.LoadDefaultPageLayoutMappingFile(); 
                    System.IO.File.WriteAllText(fileName, pageLayoutMappingFileContents);
                }
            }

            // Export custom page layout mapping
            if (this.CustomPageLayoutMapping)
            {
                if (!this.ClientContext.Web.IsPublishingWeb())
                {
                    throw new Exception("The -CustomPageLayoutMapping parameter only works for publishing sites.");
                }

                ListItem page = null;

                if (PublishingPage != null)
                {
                    page = PublishingPage.GetPage(this.ClientContext.Web, CacheManager.Instance.GetPublishingPagesLibraryName(this.ClientContext));
                }

                Guid siteId = this.ClientContext.Site.EnsureProperty(p => p.Id);
                
                string fileName = $"custompagelayoutmapping-{siteId.ToString()}.xml";

                if (page != null)
                {
                    fileName = $"custompagelayoutmapping-{siteId.ToString()}-{page.FieldValues["FileLeafRef"].ToString().ToLower().Replace(".aspx", "")}.xml";
                }

                if (System.IO.File.Exists(Path.Combine(folderToExportTo, fileName)) && !Overwrite)
                {
                    Console.WriteLine($"Skipping the export from the custom pagelayout mapping file {Path.Combine(folderToExportTo, fileName)} as this already exists. Use the -Overwrite flag to overwrite if needed.");
                }
                else
                {
                    var analyzer = new PageLayoutAnalyser(this.ClientContext);

                    if (Logging)
                    {
                        analyzer.RegisterObserver(new ConsoleObserver(false));
                    }

                    if (page != null)
                    {
                        analyzer.AnalysePageLayoutFromPublishingPage(page);
                    }
                    else
                    {
                        analyzer.AnalyseAll(!this.AnalyzeOOBPageLayouts);
                    }

                    analyzer.GenerateMappingFile(folderToExportTo, fileName);
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