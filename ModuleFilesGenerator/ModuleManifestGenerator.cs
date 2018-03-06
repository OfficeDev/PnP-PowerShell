using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.ModuleFilesGenerator
{
    internal class ModuleManifestGenerator
    {
        private string _assemblyPath;
        private string _configurationName;
        private List<Model.CmdletInfo> _cmdlets;
        private Version _assemblyVersion;

        public ModuleManifestGenerator(List<Model.CmdletInfo> cmdlets, string assemblyPath, string configurationName, Version assemblyVersion)
        {
            _cmdlets = cmdlets;
            _assemblyPath = assemblyPath;
            _configurationName = configurationName;
            _assemblyVersion = assemblyVersion;
        }
        internal void Generate()
        {
#if NETCOREAPP2_0
            var spVersion = "Core";
#else
            var spVersion = string.Empty;
            switch (_configurationName.ToLowerInvariant())
            {
                case "debug":
                case "release":
                    {
                        spVersion = "Online";
                        break;
                    }
                case "debug15":
                case "release15":
                    {
                        spVersion = "2013";
                        break;

                    }
                case "debug16":
                case "release16":
                    {
                        spVersion = "2016";
                        break;
                    }
            }
#endif
            // Generate PSM1 file
            var aliasesToExport = new List<string>();
            foreach (var cmdlet in _cmdlets.Where(c => c.Aliases.Any()))
            {
                foreach (var alias in cmdlet.Aliases)
                {
                    aliasesToExport.Add(alias);
                }
            }

            // Create Module Manifest
#if !NETCOREAPP2_0
            var psd1Path = $"{new FileInfo(_assemblyPath).Directory}\\ModuleFiles\\SharePointPnPPowerShell{spVersion}.psd1";
#else
            var psd1Path = $"{new FileInfo(_assemblyPath).Directory}\\ModuleFiles\\SharePointPnPPowerShellCore.psd1";
#endif
            var cmdletsToExportString = string.Join(",", _cmdlets.Select(c => "'" + c.FullCommand + "'"));
            string aliasesToExportString = null;
            if (aliasesToExport.Any())
            {
                aliasesToExportString = string.Join(",", aliasesToExport.Select(x => "'" + x + "'"));
            }
            WriteModuleManifest(psd1Path, spVersion, cmdletsToExportString, aliasesToExportString);
        }

        private void WriteModuleManifest(string path, string spVersion, string cmdletsToExport, string aliasesToExport)
        {
            var aliases = "";
            //if (aliasesToExport != null)
            //{
            //    aliases = $"{Environment.NewLine}AliasesToExport = {aliasesToExport}";
            //}
#if !NETCOREAPP2_0
            var manifest = $@"@{{
    RootModule = 'SharePointPnP.PowerShell.{spVersion}.Commands.dll'
    ModuleVersion = '{_assemblyVersion}'
    Description = 'SharePoint Patterns and Practices PowerShell Cmdlets for SharePoint {spVersion}'
    GUID = '8f1147be-a8e4-4bd2-a705-841d5334edc0'
    Author = 'SharePoint Patterns and Practices'
    CompanyName = 'SharePoint Patterns and Practices'
    DotNetFrameworkVersion = '4.5'
    ProcessorArchitecture = 'None'
    FunctionsToExport = '*'
    CmdletsToExport = {cmdletsToExport}
    VariablesToExport = '*'
    AliasesToExport = '*'
    FormatsToProcess = 'SharePointPnP.PowerShell.{spVersion}.Commands.Format.ps1xml' 
    PrivateData = @{{
        PSData = @{{
            ProjectUri = 'https://aka.ms/sppnp'
            IconUri = 'https://raw.githubusercontent.com/SharePoint/PnP-PowerShell/master/Commands/Resources/pnp.ico'
        }}
    }}
}}";
#else
            var manifest = $@"@{{
    RootModule = 'SharePointPnP.PowerShell.Core.dll'
    ModuleVersion = '{_assemblyVersion}'
    Description = 'SharePoint Patterns and Practices PowerShell Cmdlets for SharePoint Online'
    GUID = '0b0430ce-d799-4f3b-a565-f0dca1f31e17'
    Author = 'SharePoint Patterns and Practices'
    CompanyName = 'SharePoint Patterns and Practices'
    PowerShellVersion = '5.0'
    ProcessorArchitecture = 'None'
    FunctionsToExport = '*'
    CmdletsToExport = {cmdletsToExport}
    VariablesToExport = '*'
    AliasesToExport = '*'
    FormatsToProcess = 'SharePointPnP.PowerShell.{spVersion}.Format.ps1xml' 
    PrivateData = @{{
        PSData = @{{
            ProjectUri = 'https://aka.ms/sppnp'
            IconUri = 'https://raw.githubusercontent.com/SharePoint/PnP-PowerShell/master/Commands/Resources/pnp.ico'
        }}
    }}
}}";
#endif
            File.WriteAllText(path, manifest, Encoding.UTF8);
        }
    }
}
