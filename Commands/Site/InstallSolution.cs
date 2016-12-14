using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using File = System.IO.File;

namespace SharePointPnP.PowerShell.Commands.Site
{
    [Cmdlet(VerbsLifecycle.Install, "PnPSolution")]
    [CmdletAlias("Install-SPOSolution")]
    [CmdletHelp("Installs a sandboxed solution to a site collection. WARNING! This method can delete your composed look gallery due to the method used to activate the solution. We recommend you to only to use this cmdlet if you are okay with that.",
        Category = CmdletHelpCategory.Sites)]
    public class InstallSolution : SPOCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage="ID of the solution, from the solution manifest")]
        public GuidPipeBind PackageId;

        [Parameter(Mandatory = true, HelpMessage="Path to the sandbox solution package (.WSP) file")]
        public string SourceFilePath;

        [Parameter(Mandatory = false, HelpMessage="Optional major version of the solution, defaults to 1")]
        public int MajorVersion = 1;

        [Parameter(Mandatory = false, HelpMessage="Optional minor version of the solution, defaults to 0")]
        public int MinorVersion = 0;

        protected override void ExecuteCmdlet()
        {
            if (File.Exists(SourceFilePath))
            {
                ClientContext.Site.InstallSolution(PackageId.Id, SourceFilePath, MajorVersion, MinorVersion);
            }
            else
            {
                throw new Exception("File does not exist");
            }
        }
    }
}
