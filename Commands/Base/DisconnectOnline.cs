using PnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using PnP.PowerShell.Commands.Provider;
using PnP.PowerShell.Commands.Model;

namespace PnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommunications.Disconnect, "PnPOnline")]
    [CmdletHelp("Disconnects the context",
        DetailedDescription = "Disconnects the current context and requires you to build up a new connection in order to use the Cmdlets again. Using Connect-PnPOnline to connect to a different site has the same effect.",
        SupportedPlatform = CmdletSupportedPlatform.All,
        Category = CmdletHelpCategory.Base)]
    [CmdletExample(
        Code = @"PS:> Disconnect-PnPOnline",
        Remarks = @"This will clear out all active tokens",
        SortOrder = 1)]
    public class DisconnectOnline : PSCmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = "Connection to be used by cmdlet")]
        public PnPConnection Connection = null;

        protected override void ProcessRecord()
        {
            // If no specific connection has been passed in, take the connection from the current context
            if (Connection == null)
            {
                Connection = PnPConnection.CurrentConnection;
            }
#if !ONPREMISES
            if (Connection?.Certificate != null)
            {
#if !PNPPSCORE
                if (Connection != null && Connection.DeleteCertificateFromCacheOnDisconnect)
                {
                    PnPConnectionHelper.CleanupCryptoMachineKey(Connection.Certificate);
                }
#endif
                Connection.Certificate = null;
            }
#endif
            var success = false;
            if (Connection != null)
            {
                success = DisconnectProvidedService(Connection);
            }
            else
            {
                success = DisconnectCurrentService();
            }
            if (!success)
            {
                throw new InvalidOperationException(Properties.Resources.NoConnectionToDisconnect);
            }

            // clear credentials
            PnPConnection.CurrentConnection = null;

            var provider = SessionState.Provider.GetAll().FirstOrDefault(p => p.Name.Equals(SPOProvider.PSProviderName, StringComparison.InvariantCultureIgnoreCase));
            if (provider != null)
            {
                //ImplementingAssembly was introduced in Windows PowerShell 5.0.
#if !PNPPSCORE
                var drives = Host.Version.Major >= 5 ? provider.Drives.Where(d => d.Provider.Module.ImplementingAssembly.FullName == Assembly.GetExecutingAssembly().FullName) : provider.Drives;
#else
                var drives = Host.Version.Major >= 5 ? provider.Drives.Where(d => d.Provider.Module.Name == Assembly.GetExecutingAssembly().FullName) : provider.Drives;
#endif
                foreach (var drive in drives)
                {
                    SessionState.Drive.Remove(drive.Name, true, "Global");
                }
            }
        }

        internal static bool DisconnectProvidedService(PnPConnection connection)
        {
            //connection.ClearTokens();
            Environment.SetEnvironmentVariable("PNPPSHOST", string.Empty);
            Environment.SetEnvironmentVariable("PNPPSSITE", string.Empty);
            if (connection == null)
            {
                return false;
            }
            //GraphToken.ClearCaches();
            //OfficeManagementApiToken.ClearCaches();
            GenericToken.ClearCaches();
            connection.Context = null;
            connection = null;
            return true;
        }

        internal static bool DisconnectCurrentService()
        {
            Environment.SetEnvironmentVariable("PNPPSHOST", string.Empty);
            Environment.SetEnvironmentVariable("PNPPSSITE", string.Empty);

            if (PnPConnection.CurrentConnection == null)
            {
                return false;
            }
            else
            {
                PnPConnection.CurrentConnection.ClearTokens();
                PnPConnection.CurrentConnection.Context = null;
                PnPConnection.CurrentConnection = null;


                return true;
            }
        }
    }
}
