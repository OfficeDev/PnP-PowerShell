using System;
using System.IO;
using System.Management.Automation;
using System.Reflection;

namespace SharePointPnP.PowerShell.Commands.Base
{
    public class BasePSCmdlet : PSCmdlet
    {

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            FixAssemblyResolving();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        }

        private void FixAssemblyResolving()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
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
    }
}
