using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.CmdletHelpGenerator
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

        public string FullCommand => $"{Verb}-{Noun}";

        public string Category { get; set; }

        public CmdletInfo(string verb, string noun)
        {
            Verb = verb;
            Noun = noun;
            Parameters = new List<CmdletParameterInfo>();
            Syntaxes = new List<CmdletSyntax>();
            Aliases = new List<string>();
        }
    }
}
