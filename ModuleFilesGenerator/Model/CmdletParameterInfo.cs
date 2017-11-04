using System.Collections.Generic;

namespace SharePointPnP.PowerShell.ModuleFilesGenerator.Model
{
    public class CmdletParameterInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public bool Required { get; set; }

        public string ParameterSetName { get; set; }

        public int Position { get; set; }

        public List<string> Aliases { get; set; }
        public bool ValueFromPipeline { get; internal set; }

        public int Order { get; set; }
        
        public CmdletParameterInfo()
        {
            this.Aliases = new List<string>();
        }
    }
}
