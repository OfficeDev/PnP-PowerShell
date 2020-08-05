#if !ONPREMISES && !PNPPSCORE
using SharePointPnP.Modernization.Framework.Cache;
using PnP.PowerShell.CmdletHelpAttributes;
using System;
using System.IO;
using System.Management.Automation;
using System.Reflection;

namespace PnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsData.Save, "PnPClientSidePageConversionLog")]
    [CmdletHelp("Persists the current client side page conversion log data to the loggers linked to the last used page transformation run. Needs to be used in conjunction with the -LogSkipFlush flag on the ConvertTo-PnPClientSidePage cmdlet",
                Category = CmdletHelpCategory.ClientSidePages, SupportedPlatform = CmdletSupportedPlatform.Online)]
    [CmdletExample(Code = @"PS:> Save-PnPClientSidePageConversionLog",
                   Remarks = "Persists the current client side page conversion log data to the loggers linked to the last used page transformation run. Needs to be used in conjunction with the -LogSkipFlush flag on the ConvertTo-PnPClientSidePage cmdlet",
                   SortOrder = 1)]
    public class SaveClientSidePageConversionLog : PnPWebCmdlet
    {
        private Assembly modernizationAssembly;
        private Assembly sitesCoreAssembly;
        //private Assembly newtonsoftAssembly;

        protected override void ExecuteCmdlet()
        {
            //Fix loading of modernization framework
            FixLocalAssemblyResolving();

            // Get last used transformator instance from cache
            var transformator = CacheManager.Instance.GetLastUsedTransformator();

            if (transformator != null)
            {
                transformator.FlushObservers();
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