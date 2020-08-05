using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell.Commands;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using File = System.IO.File;

namespace PnP.PowerShell.Commands.Provider.SPOProxy
{
    [Cmdlet(CmdletVerb, CmdletNoun, DefaultParameterSetName = "Path", SupportsShouldProcess = true, SupportsTransactions = true)]
    [CmdletHelp("Proxy cmdlet for using Copy-Item between SharePoint provider and FileSystem provider", Category = CmdletHelpCategory.Files)]
    public class SPOProxyCopyItem : SPOProxyCmdletBase
    {
        public const string CmdletVerb = "Copy";

        internal override string CmdletType => CmdletVerb;

        [Parameter]
        public override SwitchParameter Recurse { get; set; }

        protected override void ProcessRecord()
        {
            SPOProxyImplementation.CopyMoveImplementation(this);
        }
    }
}
