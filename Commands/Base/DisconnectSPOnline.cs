using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using SharePointPnP.PowerShell.Commands.Provider;

namespace SharePointPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommunications.Disconnect, "PnPOnline")]
    [CmdletAlias("Disconnect-SPOnline")]
    [CmdletHelp("Disconnects the context", 
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(
        Code = @"PS:> Disconnect-PnPOnline", 
        Remarks = @"This will disconnect you from the server.",
        SortOrder = 1)]
    public class DisconnectSPOnline : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!DisconnectCurrentService())
                throw new InvalidOperationException(Properties.Resources.NoConnectionToDisconnect);

            var provider = SessionState.Provider.GetAll().FirstOrDefault(p => p.Name.Equals("SPO", StringComparison.InvariantCultureIgnoreCase));
            if (provider != null)
            {
                var drives = provider.Drives.Where(d => d.Provider.Module.ImplementingAssembly.FullName == Assembly.GetExecutingAssembly().FullName);
                foreach (var drive in drives)
                {
                    var spoDrive = drive as SPODriveInfo;
                    spoDrive.ClearState();
                    SessionState.Drive.Remove(drive.Name, true, "Global");
                }
            }
        }

        internal static bool DisconnectCurrentService()
        {
            if (SPOnlineConnection.CurrentConnection == null)
                return false;
            SPOnlineConnection.CurrentConnection = null;
            return true;
        }
    }
}
