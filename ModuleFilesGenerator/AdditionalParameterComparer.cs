using System.Collections.Generic;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.ModuleFilesGenerator.Model;

namespace PnP.PowerShell.ModuleFilesGenerator
{
    internal class AdditionalParameterComparer : IEqualityComparer<CmdletAdditionalParameter>
    {
        public bool Equals(CmdletAdditionalParameter x, CmdletAdditionalParameter y)
        {
            return x.ParameterName.Equals(y.ParameterName);
        }

        public int GetHashCode(CmdletAdditionalParameter obj)
        {
            return obj.ParameterName.GetHashCode();
        }
    }

}