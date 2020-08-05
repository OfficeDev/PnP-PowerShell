using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.ModuleFilesGenerator
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
#if NETCOREAPP3_0
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
                case "debug19":
                case "release19":
                    {
                        spVersion = "2019";
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
            var psd1Path = $"{new FileInfo(_assemblyPath).Directory}\\ModuleFiles\\SharePointPnPPowerShell{spVersion}.psd1";

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
#if !NETCOREAPP3_0
            var manifest = $@"@{{
    RootModule = 'PnP.PowerShell.{spVersion}.Commands.dll'
    ModuleVersion = '{_assemblyVersion}'
    Description = 'Microsoft 365 Patterns and Practices PowerShell Cmdlets for SharePoint {spVersion}'
    GUID = '8f1147be-a8e4-4bd2-a705-841d5334edc0'
    Author = 'Microsoft 365 Patterns and Practices'
    CompanyName = 'Microsoft 365 Patterns and Practices'
    DotNetFrameworkVersion = '4.6.1'
    ProcessorArchitecture = 'None'
    FunctionsToExport = '*'
    CmdletsToExport = {cmdletsToExport}
    VariablesToExport = '*'
    AliasesToExport = '*'
    FormatsToProcess = 'PnP.PowerShell.{spVersion}.Commands.Format.ps1xml' 
    PrivateData = @{{
        PSData = @{{
            ProjectUri = 'https://aka.ms/sppnp'
            IconUri = 'https://github.com/pnp/media/blob/19f8d40f400f9f0d766a8efd69496d8f536b853f/parker/ms/300w/parker-ms-300.png'
        }}
    }}
}}";
#else
            var manifest = $@"@{{
    RootModule = 'PnP.PowerShell.Core.dll'
    ModuleVersion = '{_assemblyVersion}'
    Description = 'Microsoft 365 Patterns and Practices PowerShell Cmdlets for SharePoint Online'
    GUID = '0b0430ce-d799-4f3b-a565-f0dca1f31e17'
    Author = 'Microsoft 365 Patterns and Practices'
    CompanyName = 'Microsoft 365 Patterns and Practices'
    PowerShellVersion = '6.0'
    ProcessorArchitecture = 'None'
    FunctionsToExport = '*'
    CmdletsToExport = {cmdletsToExport}
    VariablesToExport = '*'
    AliasesToExport = '*'
    FormatsToProcess = 'PnP.PowerShell.{spVersion}.Format.ps1xml' 
    PrivateData = @{{
        PSData = @{{
            ProjectUri = 'https://aka.ms/sppnp'
            IconUri = 'https://raw.githubusercontent.com/pnp/media/master/optimized/pnp-projects/blue/png/pnp-powershell-300.png'
        }}
    }}
}}";
#endif
            File.WriteAllText(path, manifest, Encoding.UTF8);
        }
    }
}
