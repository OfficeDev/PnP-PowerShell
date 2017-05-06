using System;
using System.Reflection;

namespace SharePointPnP.PowerShell.ModuleFilesGenerator
{
    public class GenerateModuleFiles
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Generating Module Files");
            var assemblyPath = args[0];
            var configurationName = args[1];
            var solutionDir = args[2];
        
            try
            {
                Assembly cmdletAssembly = Assembly.LoadFrom(assemblyPath);

                var analyzer = new CmdletsAnalyzer(cmdletAssembly);

                var cmdlets = analyzer.Analyze();

                var helpFileGenerator = new HelpFileGenerator(cmdlets, cmdletAssembly, $"{assemblyPath}-help.xml");
                helpFileGenerator.Generate();

                var markDownGenerator = new MarkDownGenerator(cmdlets, solutionDir);
                markDownGenerator.Generate();

                var moduleManifestGenerator = new ModuleManifestGenerator(cmdlets, assemblyPath, configurationName, cmdletAssembly.GetName().Version);
                moduleManifestGenerator.Generate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 1;
            }
            return 0;
        }
    }
}
