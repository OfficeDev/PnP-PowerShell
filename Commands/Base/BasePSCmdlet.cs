using System;
using System.IO;
using System.Management.Automation;
using System.Reflection;

namespace PnP.PowerShell.Commands.Base
{
    /// <summary>
    /// Base class for all the PnP Cmdlets
    /// </summary>
    public class BasePSCmdlet : PSCmdlet
    {
        private static Assembly newtonsoftAssembly;
        private static Assembly systemBuffersAssembly;
        private static Assembly systemRuntimeCompilerServicesUnsafeAssembly;
        private static Assembly systemThreadingTasksExtensionsAssembly;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            FixAssemblyResolving();

            // Throw warning if an old *-SPO* cmdlet is being used
            if (MyInvocation.InvocationName.ToUpper().IndexOf("-SPO", StringComparison.Ordinal) > -1)
            {
                WriteWarning($"PnP Cmdlets starting with the SPO Prefix have been deprecated since the June 2017 release. Please update your scripts and use {MyInvocation.MyCommand.Name} instead.");
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        }

        private void FixAssemblyResolving()
        {
            if (BasePSCmdlet.newtonsoftAssembly == null)
            {
                newtonsoftAssembly = GetAssembly("Newtonsoft.Json.dll");
            }
            if (systemBuffersAssembly == null)
            {
                systemBuffersAssembly = GetAssembly("System.Buffers.dll");
            }
            if (systemRuntimeCompilerServicesUnsafeAssembly == null)
            {
                systemRuntimeCompilerServicesUnsafeAssembly = GetAssembly("System.Runtime.CompilerServices.Unsafe.dll");
            }
            if (systemThreadingTasksExtensionsAssembly == null)
            {
                systemThreadingTasksExtensionsAssembly = GetAssembly("System.Threading.Tasks.Extensions.dll");
            }

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private Assembly GetAssembly(string assemblyName)
        {
            Assembly assembly = null;
            var assemblyPath = Path.Combine(AssemblyDirectoryFromLocation, assemblyName);
            if (File.Exists(assemblyPath))
            {
                assembly = Assembly.LoadFrom(assemblyPath);
            }
            else
            {
                var codebasePath = Path.Combine(AssemblyDirectoryFromCodeBase, assemblyName);
                assembly = Assembly.LoadFrom(codebasePath);
            }
            return assembly;
        }

        private string AssemblyDirectoryFromLocation
        {
            get
            {
                var location = Assembly.GetExecutingAssembly().Location;
                var escapedLocation = Uri.UnescapeDataString(location);
                return Path.GetDirectoryName(escapedLocation);
            }
        }

        private string AssemblyDirectoryFromCodeBase
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("NewtonSoft.Json", StringComparison.InvariantCultureIgnoreCase))
            {
                return newtonsoftAssembly;
            }
            if (args.Name.StartsWith("System.Buffers", StringComparison.InvariantCultureIgnoreCase))
            {
                return systemBuffersAssembly;
            }
            if (args.Name.StartsWith("System.Runtime.CompilerServices.Unsafe", StringComparison.InvariantCultureIgnoreCase))
            {
                return systemRuntimeCompilerServicesUnsafeAssembly;
            }
            if (args.Name.StartsWith("System.Threading.Tasks.Extensions", StringComparison.InvariantCultureIgnoreCase))
            {
                return systemThreadingTasksExtensionsAssembly;
            }
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName == args.Name)
                {
                    return assembly;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if a parameter with the provided name has been provided in the execution command
        /// </summary>
        /// <param name="parameterName">Name of the parameter to validate if it has been provided in the execution command</param>
        /// <returns>True if a parameter with the provided name is present, false if it is not</returns>
        public bool ParameterSpecified(string parameterName)
        {
            return MyInvocation.BoundParameters.ContainsKey(parameterName);
        }

        protected virtual void ExecuteCmdlet()
        { }

        protected override void ProcessRecord()
        {
            ExecuteCmdlet();
        }
    }
}
