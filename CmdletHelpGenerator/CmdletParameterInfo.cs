namespace SharePointPnP.PowerShell.CmdletHelpGenerator
{
    public class CmdletParameterInfo
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public bool Required { get; set; }

        public string ParameterSetName { get; set; }

        public int Position { get; set; }
    }
}
