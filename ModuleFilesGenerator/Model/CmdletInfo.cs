using System;
using System.Collections.Generic;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.ModuleFilesGenerator.Model
{
    public class CmdletInfo
    {
        public string Verb { get; set; }
        public string Noun { get; set; }

        public string Description { get; set; }

        public string DetailedDescription { get; set; }

        public List<CmdletParameterInfo> Parameters { get; set; }

        public string Version { get; set; }

        public string Copyright { get; set; }

        public List<CmdletSyntax> Syntaxes { get; set; }

        public Type OutputType { get; set; }
        public string OutputTypeDescription { get; set; }
        public string OutputTypeLink { get; set; }

        public List<string> Aliases { get; set; }

        public List<CmdletExampleAttribute> Examples { get; set; }

        public List<CmdletRelatedLinkAttribute> RelatedLinks { get; set; }

        public List<CmdletAdditionalParameter> AdditionalParameters { get; set; }
        public string FullCommand => $"{Verb}-{Noun}";

        public string Category { get; set; }

        public Type CmdletType { get; set; }

        public string Platform { get; set; }

        public CmdletInfo()
        {
            Parameters = new List<CmdletParameterInfo>();
            Syntaxes = new List<CmdletSyntax>();
            Aliases = new List<string>();
            Examples = new List<CmdletExampleAttribute>();
            RelatedLinks = new List<CmdletRelatedLinkAttribute>();
            AdditionalParameters = new List<CmdletAdditionalParameter>();
        }
    }
}
