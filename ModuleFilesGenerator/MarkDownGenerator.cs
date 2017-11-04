using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharePointPnP.PowerShell.ModuleFilesGenerator
{
    internal class MarkDownGenerator
    {
        private List<Model.CmdletInfo> _cmdlets;
        private string _solutionDir;
        private const string extension = "md";
        internal MarkDownGenerator(List<Model.CmdletInfo> cmdlets, string solutionDir)
        {
            _cmdlets = cmdlets;
            _solutionDir = solutionDir;
        }

        internal void Generate()
        {
            GenerateCmdletDocs();
            GenerateMappingJson();
            GenerateTOC();
            GenerateMSDNTOC();

            DirectoryInfo di = new DirectoryInfo($"{_solutionDir}\\Documentation");
            var mdFiles = di.GetFiles("*.md");

            // Clean up old MD files
            foreach (var mdFile in mdFiles)
            {
                if (mdFile.Name.ToLowerInvariant() != $"readme.{extension}")
                {
                    var index = _cmdlets.FindIndex(t => $"{t.Verb}-{t.Noun}.{extension}" == mdFile.Name);
                    if (index == -1)
                    {
                        mdFile.Delete();
                    }
                }
            }
        }

        private void GenerateCmdletDocs()
        {

            foreach (var cmdletInfo in _cmdlets)
            {
                var originalMd = string.Empty;
                var newMd = string.Empty;


                if (!string.IsNullOrEmpty(cmdletInfo.Verb) && !string.IsNullOrEmpty(cmdletInfo.Noun))
                {
                    string mdFilePath = $"{_solutionDir}\\Documentation\\{cmdletInfo.Verb}-{cmdletInfo.Noun}.{extension}";

                    if (System.IO.File.Exists(mdFilePath))
                    {
                        originalMd = System.IO.File.ReadAllText(mdFilePath);

                    }
                    var docBuilder = new StringBuilder();

                    // Header
                    var platform = "SharePoint Server 2013, SharePoint Server 2016, SharePoint Online";
                    if (cmdletInfo.Platform != "All")
                    {
                        platform = cmdletInfo.Platform;
                    }
                    docBuilder.Append($@"---{Environment.NewLine}external help file:{Environment.NewLine}applicable: {platform}{Environment.NewLine}schema: 2.0.0{Environment.NewLine}---{Environment.NewLine}");

                    docBuilder.Append($"# {cmdletInfo.FullCommand}{Environment.NewLine}{Environment.NewLine}");

                    // Body 

                    //if (cmdletInfo.Platform != "All")
                    //{
                    //    docBuilder.Append($"## SYNOPSIS{Environment.NewLine}{cmdletInfo.Description}{Environment.NewLine}{Environment.NewLine}>Only available for {cmdletInfo.Platform}{Environment.NewLine}{Environment.NewLine}");
                    //}
                    //else
                    //{
                    docBuilder.Append($"## SYNOPSIS{Environment.NewLine}{cmdletInfo.Description}{Environment.NewLine}{Environment.NewLine}");
                    //}

                    if (cmdletInfo.Syntaxes.Any())
                    {
                        docBuilder.Append($"## SYNTAX {Environment.NewLine}{Environment.NewLine}");
                        foreach (var cmdletSyntax in cmdletInfo.Syntaxes.OrderBy(s => s.Parameters.Count(p => p.Required)))
                        {
                            if (cmdletSyntax.ParameterSetName != "__AllParameterSets")
                            {
                                docBuilder.Append($"### {cmdletSyntax.ParameterSetName}{Environment.NewLine}");
                            }
                            var syntaxText = new StringBuilder();
                            syntaxText.AppendFormat("```powershell\r\n{0} ", cmdletInfo.FullCommand);
                            var cmdletLength = cmdletInfo.FullCommand.Length;
                            var first = true;
                            foreach (var par in cmdletSyntax.Parameters.Distinct(new ParameterComparer()).OrderBy(p => p.Order).ThenBy(p => !p.Required).ThenBy(p => p.Position))
                            {
                                if (first)
                                {
                                    first = false;
                                }
                                else
                                {
                                    syntaxText.Append(new string(' ', cmdletLength + 1));
                                }
                                if (!par.Required)
                                {
                                    syntaxText.Append("[");
                                }
                                if (par.Type.StartsWith("Int"))
                                {
                                    par.Type = "Int";
                                }
                                if (par.Type == "SwitchParameter")
                                {
                                    syntaxText.AppendFormat("-{0} [<{1}>]", par.Name, par.Type);
                                }
                                else
                                {
                                    syntaxText.AppendFormat("-{0} <{1}>", par.Name, par.Type);
                                }
                                if (!par.Required)
                                {
                                    syntaxText.Append("]");
                                }
                                syntaxText.Append("\r\n");
                            }
                            // Add All ParameterSet ones
                            docBuilder.Append(syntaxText);
                            docBuilder.Append($"```{Environment.NewLine}{Environment.NewLine}");
                        }
                    }
                    if (!string.IsNullOrEmpty(cmdletInfo.DetailedDescription))
                    {
                        docBuilder.Append($"## DESCRIPTION{Environment.NewLine}");
                        docBuilder.Append($"{cmdletInfo.DetailedDescription}{Environment.NewLine}{Environment.NewLine}");
                    }

                    if (cmdletInfo.Examples.Any())
                    {
                        docBuilder.Append($"## EXAMPLES{Environment.NewLine}{Environment.NewLine}");
                        var examplesCount = 1;
                        foreach (var example in cmdletInfo.Examples.OrderBy(e => e.SortOrder))
                        {

                            docBuilder.Append($"### ------------------EXAMPLE {examplesCount}------------------{Environment.NewLine}");
                            if (!string.IsNullOrEmpty(example.Introduction))
                            {
                                docBuilder.Append($"{example.Introduction}{Environment.NewLine}");
                            }
                            docBuilder.Append($"```powershell{Environment.NewLine}{example.Code}{Environment.NewLine}```{Environment.NewLine}{Environment.NewLine}");
                            docBuilder.Append($"{example.Remarks}{Environment.NewLine}{Environment.NewLine}");
                            examplesCount++;
                        }
                    }

                    if (cmdletInfo.Parameters.Any())
                    {
                        docBuilder.Append($"## PARAMETERS{Environment.NewLine}{Environment.NewLine}");

                        foreach (var parameter in cmdletInfo.Parameters.OrderBy(x => x.Order).ThenBy(x => x.Name).Distinct(new ParameterComparer()))
                        {
                            if (parameter.Type.StartsWith("Int"))
                            {
                                parameter.Type = "Int";
                            }
                            docBuilder.Append($"### -{parameter.Name}{Environment.NewLine}");
                            docBuilder.Append($"{parameter.Description}{Environment.NewLine}{Environment.NewLine}");
                            docBuilder.Append($"```yaml{Environment.NewLine}");
                            docBuilder.Append($"Type: {parameter.Type}{Environment.NewLine}");
                            if (parameter.ParameterSetName == "__AllParameterSets")
                            {
                                parameter.ParameterSetName = "(All)";
                            }
                            docBuilder.Append($"Parameter Sets: { parameter.ParameterSetName}{Environment.NewLine}");
                            if (parameter.Aliases.Any())
                            {
                                docBuilder.Append($"Aliases: {string.Join(",", parameter.Aliases)}{Environment.NewLine}");
                            }
                            docBuilder.Append(Environment.NewLine);
                            docBuilder.Append($"Required: {parameter.Required}{Environment.NewLine}");
                            docBuilder.Append($"Position: {(parameter.Position == int.MinValue ? "Named" : parameter.Position.ToString())}{Environment.NewLine}");
                            docBuilder.Append($"Accept pipeline input: {parameter.ValueFromPipeline}{Environment.NewLine}");
                            docBuilder.Append($"```{Environment.NewLine}{Environment.NewLine}");
                        }
                    }

                    if (cmdletInfo.OutputType != null)
                    {
                        docBuilder.Append($"## OUTPUTS{Environment.NewLine}{Environment.NewLine}");
                        var outputType = "";
                        if (cmdletInfo.OutputType != null)
                        {
                            if (cmdletInfo.OutputType.IsGenericType)
                            {
                                if (cmdletInfo.OutputType.GetGenericTypeDefinition() == typeof(List<>) || cmdletInfo.OutputType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                                {
                                    if (cmdletInfo.OutputType.GenericTypeArguments.Any())
                                    {
                                        outputType = $"List<{cmdletInfo.OutputType.GenericTypeArguments[0].FullName}>";
                                    }
                                    else
                                    {
                                        outputType = cmdletInfo.OutputType.FullName;
                                    }
                                }
                                else
                                {
                                    outputType = cmdletInfo.OutputType.FullName;
                                }
                            }
                            else
                            {
                                outputType = cmdletInfo.OutputType.FullName;
                            }
                        }
                        if (!string.IsNullOrEmpty(cmdletInfo.OutputTypeLink))
                        {
                            docBuilder.Append($"### [{outputType}]({cmdletInfo.OutputTypeLink})");
                        }
                        else
                        {
                            docBuilder.Append($"### {outputType}");
                        }
                        if (!string.IsNullOrEmpty(cmdletInfo.OutputTypeDescription))
                        {
                            docBuilder.Append($"\n\n{cmdletInfo.OutputTypeDescription}");
                        }
                        docBuilder.Append("\n\n");
                    }

                    if (cmdletInfo.RelatedLinks.Any())
                    {
                        docBuilder.Append($"# RELATED LINKS{Environment.NewLine}{Environment.NewLine}");
                        foreach (var link in cmdletInfo.RelatedLinks)
                        {
                            docBuilder.Append($"[{link.Text}]({link.Url})");
                        }
                    }

                    newMd = docBuilder.ToString();

                    var dmp = new DiffMatchPatch.diff_match_patch();

                    var diffResults = dmp.diff_main(newMd, originalMd);

                    foreach (var result in diffResults)
                    {
                        if (result.operation != DiffMatchPatch.Operation.EQUAL)
                        {
                            System.IO.File.WriteAllText(mdFilePath, docBuilder.ToString());
                            break;
                        }
                    }
                }
            }
        }

        private void GenerateMappingJson()
        {
            var groups = new Dictionary<string, string>();
            foreach (var cmdletInfo in _cmdlets)
            {
                groups.Add($"{cmdletInfo.FullCommand}", cmdletInfo.Category);
            }

            var json = JsonConvert.SerializeObject(groups);

            var mappingFolder = $"{_solutionDir}\\Documentation\\Mapping";
            if (!System.IO.Directory.Exists(mappingFolder))
            {
                System.IO.Directory.CreateDirectory(mappingFolder);
            }

            var mappingPath = $"{_solutionDir}\\Documentation\\Mapping\\groupMapping.json";
            System.IO.File.WriteAllText(mappingPath, json);
        }

        private void GenerateTOC()
        {
            var originalMd = string.Empty;
            var newMd = string.Empty;

            // Create the readme.md
            var readmePath = $"{_solutionDir}\\Documentation\\readme.{extension}";
            if (System.IO.File.Exists(readmePath))
            {
                originalMd = System.IO.File.ReadAllText(readmePath);
            }
            var docBuilder = new StringBuilder();


            docBuilder.AppendFormat("# Cmdlet Documentation #{0}", Environment.NewLine);
            docBuilder.AppendFormat("Below you can find a list of all the available cmdlets. Many commands provide built-in help and examples. Retrieve the detailed help with {0}", Environment.NewLine);
            docBuilder.AppendFormat("{0}```powershell{0}Get-Help Connect-PnPOnline -Detailed{0}```{0}{0}", Environment.NewLine);

            // Get all unique categories
            var categories = _cmdlets.Where(c => !string.IsNullOrEmpty(c.Category)).Select(c => c.Category).Distinct();

            foreach (var category in categories.OrderBy(c => c))
            {
                docBuilder.AppendFormat("## {0}{1}", category, Environment.NewLine);

                docBuilder.AppendFormat("Cmdlet|Description|Platforms{0}", Environment.NewLine);
                docBuilder.AppendFormat(":-----|:----------|:--------{0}", Environment.NewLine);
                foreach (var cmdletInfo in _cmdlets.Where(c => c.Category == category).OrderBy(c => c.Noun))
                {
                    var description = cmdletInfo.Description != null ? cmdletInfo.Description.Replace("\r\n", " ") : "";
                    docBuilder.AppendFormat("**[{0}]({1}-{2}.md)** |{3}|{4}{5}", cmdletInfo.FullCommand.Replace("-", "&#8209;"), cmdletInfo.Verb, cmdletInfo.Noun, description, cmdletInfo.Platform, Environment.NewLine);
                }
            }

            newMd = docBuilder.ToString();
            DiffMatchPatch.diff_match_patch dmp = new DiffMatchPatch.diff_match_patch();

            var diffResults = dmp.diff_main(newMd, originalMd);

            foreach (var result in diffResults)
            {
                if (result.operation != DiffMatchPatch.Operation.EQUAL)
                {
                    System.IO.File.WriteAllText(readmePath, docBuilder.ToString());
                }
            }
        }

        private void GenerateMSDNTOC()
        {
            var originalTocMd = string.Empty;
            var newTocMd = string.Empty;

            var msdnDocPath = $"{_solutionDir}\\Documentation\\MSDN";
            if (!Directory.Exists(msdnDocPath))
            {
                Directory.CreateDirectory(msdnDocPath);
            }

            // Generate the landing page
            var landingPagePath = $"{msdnDocPath}\\PnP-PowerShell-Overview.{extension}";
            GenerateMSDNLandingPage(landingPagePath);

            // TOC.md generation
            var tocPath = $"{msdnDocPath}\\TOC.{extension}";
            if (System.IO.File.Exists(tocPath))
            {
                originalTocMd = System.IO.File.ReadAllText(tocPath);
            }
            var docBuilder = new StringBuilder();


            docBuilder.AppendFormat("# [SharePoint PnP PowerShell reference](PnP-PowerShell-Overview.md){0}", Environment.NewLine);

            // Get all unique categories
            var categories = _cmdlets.Where(c => !string.IsNullOrEmpty(c.Category)).Select(c => c.Category).Distinct();

            foreach (var category in categories.OrderBy(c => c))
            {
                var categoryMdPage = $"{category.Replace(" ", "")}-category.{extension}";
                var categoryMdPath = $"{msdnDocPath}\\{categoryMdPage}";

                // Add section reference to TOC
                docBuilder.AppendFormat("## [{0}]({1}){2}", category, categoryMdPage, Environment.NewLine);

                var categoryCmdlets = _cmdlets.Where(c => c.Category == category).OrderBy(c => c.Noun);

                // Generate category MD
                GenerateMSDNCategory(category, categoryMdPath, categoryCmdlets);

                // Link cmdlets to TOC
                foreach (var cmdletInfo in categoryCmdlets)
                {
                    var description = cmdletInfo.Description != null ? cmdletInfo.Description.Replace("\r\n", " ") : "";
                    docBuilder.AppendFormat("### [{0}]({1}-{2}.md){3}", cmdletInfo.FullCommand, cmdletInfo.Verb, cmdletInfo.Noun, Environment.NewLine);
                }
            }

            newTocMd = docBuilder.ToString();
            DiffMatchPatch.diff_match_patch dmp = new DiffMatchPatch.diff_match_patch();

            var diffResults = dmp.diff_main(newTocMd, originalTocMd);

            foreach (var result in diffResults)
            {
                if (result.operation != DiffMatchPatch.Operation.EQUAL)
                {
                    System.IO.File.WriteAllText(tocPath, docBuilder.ToString());
                }
            }
        }

        private void GenerateMSDNLandingPage(string landingPagePath)
        {
            var originalLandingPageMd = string.Empty;
            var newLandingPageMd = string.Empty;

            if (System.IO.File.Exists(landingPagePath))
            {
                originalLandingPageMd = System.IO.File.ReadAllText(landingPagePath);
            }
            var docBuilder = new StringBuilder();

            // read base file from disk
            var assemblyPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;

            string baseLandingPage = System.IO.File.ReadAllText(Path.Combine(assemblyPath, "landingpage.md"));

            // Get all unique categories
            var categories = _cmdlets.Where(c => !string.IsNullOrEmpty(c.Category)).Select(c => c.Category).Distinct();

            foreach (var category in categories.OrderBy(c => c))
            {
                docBuilder.Append("\n\n");
                docBuilder.AppendFormat("### {0} {1}", category, Environment.NewLine);
                docBuilder.AppendFormat("Cmdlet|Description|Platform{0}", Environment.NewLine);
                docBuilder.AppendFormat(":-----|:----------|:-------{0}", Environment.NewLine);

                var categoryCmdlets = _cmdlets.Where(c => c.Category == category).OrderBy(c => c.Noun);

                foreach (var cmdletInfo in categoryCmdlets)
                {
                    var description = cmdletInfo.Description != null ? cmdletInfo.Description.Replace("\r\n", " ") : "";
                    docBuilder.AppendFormat("**[{0}]({1}-{2}.md)** |{3}|{4}{5}", cmdletInfo.FullCommand.Replace("-", "&#8209;"), cmdletInfo.Verb, cmdletInfo.Noun, description, cmdletInfo.Platform, Environment.NewLine);
                }
            }

            string dynamicLandingPage = docBuilder.ToString();
            newLandingPageMd = baseLandingPage.Replace("---cmdletdata---", dynamicLandingPage);

            DiffMatchPatch.diff_match_patch dmp = new DiffMatchPatch.diff_match_patch();

            var diffResults = dmp.diff_main(newLandingPageMd, originalLandingPageMd);

            foreach (var result in diffResults)
            {
                if (result.operation != DiffMatchPatch.Operation.EQUAL)
                {
                    System.IO.File.WriteAllText(landingPagePath, newLandingPageMd);
                }
            }
        }

        private void GenerateMSDNCategory(string category, string categoryMdPath, IOrderedEnumerable<Model.CmdletInfo> cmdlets)
        {
            var originalCategoryMd = string.Empty;
            var newCategoryMd = string.Empty;

            if (System.IO.File.Exists(categoryMdPath))
            {
                originalCategoryMd = System.IO.File.ReadAllText(categoryMdPath);
            }
            var docBuilder = new StringBuilder();
            docBuilder.AppendFormat("# {0} {1}", category, Environment.NewLine);
            docBuilder.AppendFormat("Cmdlet|Description|Platform{0}", Environment.NewLine);
            docBuilder.AppendFormat(":-----|:----------|:-------{0}", Environment.NewLine);
            foreach (var cmdletInfo in cmdlets)
            {
                var description = cmdletInfo.Description != null ? cmdletInfo.Description.Replace("\r\n", " ") : "";
                docBuilder.AppendFormat("**[{0}]({1}-{2}.md)** |{3}|{4}{5}", cmdletInfo.FullCommand.Replace("-", "&#8209;"), cmdletInfo.Verb, cmdletInfo.Noun, description, cmdletInfo.Platform, Environment.NewLine);
            }

            newCategoryMd = docBuilder.ToString();
            DiffMatchPatch.diff_match_patch dmp = new DiffMatchPatch.diff_match_patch();

            var diffResults = dmp.diff_main(newCategoryMd, originalCategoryMd);

            foreach (var result in diffResults)
            {
                if (result.operation != DiffMatchPatch.Operation.EQUAL)
                {
                    System.IO.File.WriteAllText(categoryMdPath, docBuilder.ToString());
                }
            }
        }


    }
}
