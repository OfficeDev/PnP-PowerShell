#if !PNPPSCORE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Utilities
{
    internal static class IsolatedStorage
    {
        public static void InitializeIsolatedStorage()
        {
            var useNewEvidence = false;
            try
            {
                var usfdAttempt1 = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForDomain(); // this will fail when the current AppDomain Evidence is instantiated via COM or in PowerShell
            }
            catch (Exception)
            {
                useNewEvidence = true;
            }

            if (useNewEvidence)
            {
                var replacementEvidence = new System.Security.Policy.Evidence();
                replacementEvidence.AddHostEvidence(new System.Security.Policy.Zone(System.Security.SecurityZone.MyComputer));

                var currentAppDomain = System.Threading.Thread.GetDomain();
                var securityIdentityField = currentAppDomain.GetType().GetField("_SecurityIdentity", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                securityIdentityField.SetValue(currentAppDomain, replacementEvidence);
            }
        }
    }
}
#endif