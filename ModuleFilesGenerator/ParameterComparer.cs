using System.Collections.Generic;
using PnP.PowerShell.ModuleFilesGenerator.Model;

namespace PnP.PowerShell.ModuleFilesGenerator
{
    internal class ParameterComparer : IEqualityComparer<CmdletParameterInfo>
    {
        public bool Equals(CmdletParameterInfo x, CmdletParameterInfo y)
        {
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(CmdletParameterInfo obj)
        {
            return obj.Name.GetHashCode();
        }
    }

}