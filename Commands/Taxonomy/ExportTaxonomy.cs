using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using SharePointPnP.PowerShell.Commands.Enums;
using File = System.IO.File;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsData.Export, "PnPTaxonomy", SupportsShouldProcess = true)]
    [CmdletAlias("Export-SPOTaxonomy")]
    [CmdletHelp("Exports a taxonomy to either the output or to a file.",
        Category = CmdletHelpCategory.Taxonomy)]
    [CmdletExample
        (Code = @"PS:> Export-PnPTaxonomy",
        Remarks = "Exports the full taxonomy to the standard output",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Export-PnPTaxonomy -Path c:\output.txt",
        Remarks = "Exports the full taxonomy the file output.txt",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Export-PnPTaxonomy -Path c:\output.txt -TermSet f6f43025-7242-4f7a-b739-41fa32847254 ",
        Remarks = "Exports the term set with the specified id",
        SortOrder = 3)]
    public class ExportTaxonomy : PnPCmdlet
    {
        [Parameter(Mandatory = false, ParameterSetName = "TermSet", HelpMessage = "If specified, will export the specified termset only")]
        public GuidPipeBind TermSetId = new GuidPipeBind();

        [Parameter(Mandatory = false, HelpMessage = "If specified will include the ids of the taxonomy items in the output. Format: <label>;#<guid>")]
        public SwitchParameter IncludeID = false;

        [Parameter(Mandatory = false, HelpMessage = "File to export the data to.")]
        public string Path;

        [Parameter(Mandatory = false, ParameterSetName = "TermSet", HelpMessage = "Term store to export; if not specified the default term store is used.")]
        public string TermStoreName;

        [Parameter(Mandatory = false, HelpMessage = "Overwrites the output file if it exists.")]
        public SwitchParameter Force;

        [Parameter(Mandatory = false, HelpMessage = "The path delimiter to be used, by default this is '|'")]
        public string Delimiter = "|";

        [Parameter(Mandatory = false, HelpMessage = "Defaults to Unicode")]
        public Encoding Encoding = Encoding.Unicode;


        protected override void ExecuteCmdlet()
        {
            List<string> exportedTerms;
            if (ParameterSetName == "TermSet")
            {
                if (Delimiter != "|" && Delimiter == ";#")
                {
                    throw new Exception("Restricted delimiter specified");
                }
                if (!string.IsNullOrEmpty(TermStoreName))
                {
                    var taxSession = TaxonomySession.GetTaxonomySession(ClientContext);
                    var termStore = taxSession.TermStores.GetByName(TermStoreName);
                    exportedTerms = ClientContext.Site.ExportTermSet(TermSetId.Id, IncludeID, termStore, Delimiter);
                }
                else
                {
                    exportedTerms = ClientContext.Site.ExportTermSet(TermSetId.Id, IncludeID, Delimiter);
                }
            }
            else
            {
                exportedTerms = ClientContext.Site.ExportAllTerms(IncludeID, Delimiter);
            }

            if (Path == null)
            {
                WriteObject(exportedTerms);
            }
            else
            {
                if (!System.IO.Path.IsPathRooted(Path))
                {
                    Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                }

                System.Text.Encoding textEncoding = System.Text.Encoding.Unicode;
                switch (Encoding)
                {
                    case Encoding.ASCII:
                        {
                            textEncoding = System.Text.Encoding.ASCII;
                            break;
                        }

                    case Encoding.BigEndianUnicode:
                        {
                            textEncoding = System.Text.Encoding.BigEndianUnicode;
                            break;
                        }
                    case Encoding.UTF32:
                        {
                            textEncoding = System.Text.Encoding.UTF32;
                            break;
                        }
                    case Encoding.UTF7:
                        {
                            textEncoding = System.Text.Encoding.UTF7;
                            break;
                        }
                    case Encoding.UTF8:
                        {
                            textEncoding = System.Text.Encoding.UTF8;
                            break;
                        }
                    case Encoding.Unicode:
                        {
                            textEncoding = System.Text.Encoding.Unicode;
                            break;
                        }

                }

                if (File.Exists(Path))
                {
                    if (Force || ShouldContinue(string.Format(Resources.File0ExistsOverwrite, Path), Resources.Confirm))
                    {
                        File.WriteAllLines(Path, exportedTerms, textEncoding);
                    }
                }
                else
                {
                    File.WriteAllLines(Path, exportedTerms, textEncoding);
                }
            }
        }

    }
}
