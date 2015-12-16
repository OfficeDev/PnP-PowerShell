using Microsoft.PowerShell.Commands;
using OfficeDevPnP.PowerShell.Commands.Base;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace OfficeDevPnP.PowerShell.Tests
{
    public class PSTestScope : IDisposable
    {
        private Runspace _runSpace;

        public string SiteUrl { get; set; }
        public string CredentialManagerEntry { get; set; }
        public string Realm { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }

        public PSTestScope(bool connect = true)
        {
            SiteUrl = ConfigurationManager.AppSettings["SPODevSiteUrl"];
            CredentialManagerEntry = ConfigurationManager.AppSettings["SPOCredentialManagerLabel"];
            Realm = ConfigurationManager.AppSettings["Realm"];
            AppId = ConfigurationManager.AppSettings["AppId"];
            AppSecret = ConfigurationManager.AppSettings["AppSecret"];

            var iss = InitialSessionState.CreateDefault();
            if (connect)
            {
                SessionStateCmdletEntry ssce = new SessionStateCmdletEntry("Connect-SPOnline", typeof(ConnectSPOnline), null);

                iss.Commands.Add(ssce);
            }
            _runSpace = RunspaceFactory.CreateRunspace(iss);

            _runSpace.Open();


            if (connect)
            {
                var pipeLine = _runSpace.CreatePipeline();
                Command cmd = new Command("connect-sponline");
                cmd.Parameters.Add("Url", SiteUrl);
                if (!string.IsNullOrEmpty(CredentialManagerEntry))
                {
                    // Use Windows Credential Manager to authenticate
                    cmd.Parameters.Add("Credentials", CredentialManagerEntry);
                }
                else
                {                    
                    if (!string.IsNullOrEmpty(Realm) && !string.IsNullOrEmpty("AppId") && !string.IsNullOrEmpty("AppSecret"))
                    {
                        // Use oAuth Token to authenticate
                        cmd.Parameters.Add("Realm", Realm);
                        cmd.Parameters.Add("AppId", AppId);
                        cmd.Parameters.Add("AppSecret", AppSecret);
                    }
                }
                pipeLine.Commands.Add(cmd);
                pipeLine.Invoke();
            }
        }


        public Collection<PSObject> ExecuteCommand(string cmdletString)
        {
            return ExecuteCommand(cmdletString, null);
        }

        public Collection<PSObject> ExecuteCommand(string cmdletString, params CommandParameter[] parameters)
        {
            var pipeLine = _runSpace.CreatePipeline();
            Command cmd = new Command(cmdletString);
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
            pipeLine.Commands.Add(cmd);
            return pipeLine.Invoke();

        }

        public void Dispose()
        {
            //if (_powerShell != null)
            //{
            //    _powerShell.Dispose();
            //}
            if (_runSpace != null)
            {
                _runSpace.Dispose();
            }
        }
    }
}
