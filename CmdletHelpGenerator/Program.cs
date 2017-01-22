using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System.Runtime.Serialization;
using System.IO;

namespace SharePointPnP.PowerShell.CmdletHelpGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmdlets = new List<CmdletInfo>();
            var inFile = args[0];

            var outFile = args[1];
            var toc = new List<CmdletInfo>();

            // Specify an additional (third) parameter pointing to the Solution folder to generate Markdown. The markdown 
            // will be created in the Documentation folder underneath the solution folder.
            bool generateMarkdown = false;
            string solutionDir = null;
            string spVersion = "Online";
            if (args.Length > 2)
            {
                var configuration = args[2];
                solutionDir = args[3];
                generateMarkdown = true;
                switch (configuration.ToLowerInvariant())
                {
                    case "debug":
                    case "release":
                        {
                            spVersion = "Online";
                            break;
                        }
                    case "debug15":
                    case "release15":
                        {
                            spVersion = "2013";
                            break;

                        }
                    case "debug16":
                    case "release16":
                        {
                            spVersion = "2016";
                            break;
                        }
                }
            }
            var doc = new XDocument(new XDeclaration("1.0", "UTF-8", string.Empty));

            XNamespace ns = "http://msh";
            var helpItems = new XElement(ns + "helpItems", new XAttribute("schema", "maml"));
            doc.Add(helpItems);


            XNamespace maml = "http://schemas.microsoft.com/maml/2004/10";
            XNamespace command = "http://schemas.microsoft.com/maml/dev/command/2004/10";
            XNamespace dev = "http://schemas.microsoft.com/maml/dev/2004/10";

            var mamlNsAttr = new XAttribute(XNamespace.Xmlns + "maml", "http://schemas.microsoft.com/maml/2004/10");
            var commandNsAttr = new XAttribute(XNamespace.Xmlns + "command", "http://schemas.microsoft.com/maml/dev/command/2004/10");
            var devNsAttr = new XAttribute(XNamespace.Xmlns + "dev", "http://schemas.microsoft.com/maml/dev/2004/10");

            var assembly = Assembly.LoadFrom(inFile);
            var types = assembly.GetTypes().Where(t => t.BaseType != null && (t.BaseType.Name.StartsWith("SPO") || t.BaseType.Name.StartsWith("PnP") || t.BaseType.Name == "PSCmdlet")).OrderBy(t => t.Name).ToArray();
            //        System.Diagnostics.Debugger.Launch();

            Console.WriteLine("Generating help and module files");
            var cmdletsToExport = new List<string>();
            var aliasesToCreate = new List<KeyValuePair<string, string>>();

            foreach (var t in types)
            {
                if (t.BaseType == null)
                {
                    return;
                }
                //if (t.BaseType.Name == "SPOCmdlet" || t.BaseType.Name == "PSCmdlet" || t.BaseType.Name == "SPOWebCmdlet" || t.BaseType.Name == "SPOAdminCmdlet" || t.BaseType.Name == "PnPGraphCmdlet" || t.BaseType.Name == "PnPWebRetrievalsCmdlet`1" )
                //{

                    var verb = string.Empty;
                var noun = string.Empty;
                var description = string.Empty;
                var detaileddescription = string.Empty;
                var copyright = string.Empty;
                var version = string.Empty;
                var category = string.Empty;
                var attrs = t.GetCustomAttributes();
                var examples = new List<CmdletExampleAttribute>();
                var relatedLinks = new List<CmdletRelatedLinkAttribute>();
                Type outputType = null;
                var outputTypeLink = string.Empty;
                var outputTypeDescription = string.Empty;
                var aliases = new List<string>();
                var additionalParameters = new List<CmdletAdditionalParameter>();
                // Get info from attributes
                foreach (var attr in attrs)
                {
                    if (attr is CmdletAttribute)
                    {
                        var a = (CmdletAttribute)attr;
                        verb = a.VerbName;
                        noun = a.NounName;

                    }
                    if (attr is CmdletAliasAttribute)
                    {
                        var a = (CmdletAliasAttribute)attr;
                        aliases.Add(a.Alias);
                    }

                    if (attr is CmdletHelpAttribute)
                    {
                        var a = (CmdletHelpAttribute)attr;
                        description = a.Description;
                        copyright = a.Copyright;
                        version = a.Version;
                        detaileddescription = a.DetailedDescription;
                        category = ToEnumString(a.Category);
                        outputType = a.OutputType;
                        outputTypeLink = a.OutputTypeLink;
                        outputTypeDescription = a.OutputTypeDescription;
                    }
                    if (attr is CmdletExampleAttribute)
                    {
                        var a = (CmdletExampleAttribute)attr;
                        examples.Add(a);
                    }
                    if (attr is CmdletRelatedLinkAttribute)
                    {
                        var l = (CmdletRelatedLinkAttribute)attr;
                        relatedLinks.Add(l);
                    }
                    if (attr is CmdletAdditionalParameter)
                    {

                        var a = (CmdletAdditionalParameter)attr;
                        additionalParameters.Add(a);
                    }
                }

                // Store in CmdletInfo structure
                var cmdletInfo = new CmdletInfo(verb, noun);
                cmdletInfo.Description = description;
                cmdletInfo.DetailedDescription = detaileddescription;
                cmdletInfo.Version = version;
                cmdletInfo.Copyright = copyright;
                cmdletInfo.Category = category;
                cmdletInfo.OutputType = outputType;
                cmdletInfo.OutputTypeLink = outputTypeLink;
                cmdletInfo.OutputTypeDescription = outputTypeDescription;
                cmdletInfo.Aliases = aliases;

                if (!string.IsNullOrEmpty(cmdletInfo.Verb) && !string.IsNullOrEmpty(cmdletInfo.Noun))
                {
                    cmdletsToExport.Add(cmdletInfo.FullCommand);
                }

                if (cmdletInfo.Aliases.Any())
                {
                    foreach (var alias in cmdletInfo.Aliases)
                    {
                        aliasesToCreate.Add(new KeyValuePair<string, string>(cmdletInfo.FullCommand, alias));
                    }
                }
                // Build XElement for command
                var commandElement = new XElement(command + "command", mamlNsAttr, commandNsAttr, devNsAttr);
                var detailsElement = new XElement(command + "details");
                commandElement.Add(detailsElement);

                detailsElement.Add(new XElement(command + "name", $"{verb}-{noun}"));
                detailsElement.Add(new XElement(maml + "description", new XElement(maml + "para", description)));
                detailsElement.Add(new XElement(maml + "copyright", new XElement(maml + "para", copyright)));
                detailsElement.Add(new XElement(command + "verb", verb));
                detailsElement.Add(new XElement(command + "noun", noun));
                detailsElement.Add(new XElement(dev + "version", version));

                if (!string.IsNullOrWhiteSpace(detaileddescription))
                {
                    commandElement.Add(new XElement(maml + "description", new XElement(maml + "para", detaileddescription)));
                }
                var syntaxElement = new XElement(command + "syntax");
                commandElement.Add(syntaxElement);

                // Store syntaxes in CmdletInfo structure (if not AllParameterSets), and also in all syntaxItems list
                var fields = GetFields(t);
                //var fields = t.GetFields();
                var syntaxItems = new List<SyntaxItem>();
                foreach (var field in fields)
                {
                    var dontShow = false;
                    foreach (Attribute attr in field.GetCustomAttributes(typeof(ObsoleteAttribute), true))
                    {
                        if (attr is ObsoleteAttribute)
                        {
                            dontShow = true;
                            break;
                        }
                    }

                    foreach (Attribute attr in field.GetCustomAttributes(typeof(ParameterAttribute), true))
                    {
                        if (attr is ParameterAttribute)
                        {
                            var a = (ParameterAttribute)attr;

                            if (!dontShow)
                            {
                                if (a.ParameterSetName != ParameterAttribute.AllParameterSets)
                                {
                                    var cmdletSyntax = cmdletInfo.Syntaxes.FirstOrDefault(c => c.ParameterSetName == a.ParameterSetName);
                                    if (cmdletSyntax == null)
                                    {
                                        cmdletSyntax = new CmdletSyntax();
                                        cmdletSyntax.ParameterSetName = a.ParameterSetName;
                                        cmdletInfo.Syntaxes.Add(cmdletSyntax);
                                    }

                                    cmdletSyntax.Parameters.Add(new CmdletParameterInfo() { Name = field.Name, Description = a.HelpMessage, Position = a.Position, Required = a.Mandatory, Type = field.FieldType.Name });

                                    var syntaxItem = syntaxItems.FirstOrDefault(x => x.Name == a.ParameterSetName);
                                    if (syntaxItem == null)
                                    {
                                        syntaxItem = new SyntaxItem(a.ParameterSetName);
                                        syntaxItems.Add(syntaxItem);
                                    }
                                    syntaxItem.Parameters.Add(new SyntaxItem.Parameter() { Name = field.Name, Description = a.HelpMessage, Position = a.Position, Required = a.Mandatory, Type = field.FieldType.Name });
                                }
                            }
                        }
                    }
                }

                foreach (var additionalParameter in additionalParameters)
                {

                    if (additionalParameter.ParameterSetName != ParameterAttribute.AllParameterSets)
                    {
                        var cmdletSyntax = cmdletInfo.Syntaxes.FirstOrDefault(c => c.ParameterSetName == additionalParameter.ParameterSetName);
                        if (cmdletSyntax == null)
                        {
                            cmdletSyntax = new CmdletSyntax();
                            cmdletSyntax.ParameterSetName = additionalParameter.ParameterSetName;
                            cmdletInfo.Syntaxes.Add(cmdletSyntax);
                        }

                        cmdletSyntax.Parameters.Add(new CmdletParameterInfo()
                        {
                            Name = additionalParameter.ParameterName,
                            Description = additionalParameter.HelpMessage,
                            Position = additionalParameter.Position,
                            Required = additionalParameter.Mandatory,
                            Type = additionalParameter.ParameterType.Name
                        });

                        var syntaxItem = syntaxItems.FirstOrDefault(x => x.Name == additionalParameter.ParameterSetName);
                        if (syntaxItem == null)
                        {
                            syntaxItem = new SyntaxItem(additionalParameter.ParameterSetName);
                            syntaxItems.Add(syntaxItem);
                        }
                        syntaxItem.Parameters.Add(new SyntaxItem.Parameter() { Name = additionalParameter.ParameterName, Description = additionalParameter.HelpMessage, Position = additionalParameter.Position, Required = additionalParameter.Mandatory, Type = additionalParameter.ParameterType.Name });
                    }
                }

                // all parameters
                // Add AllParameterSets to all CmdletInfo syntax sets and syntaxItems sets (first checking there is at least one, i.e. if all are marked AllParameterSets)
                foreach (var field in fields)
                {
                    var dontShow = false;
                    foreach (Attribute attr in field.GetCustomAttributes(typeof(ObsoleteAttribute), true))
                    {
                        if (attr is ObsoleteAttribute)
                        {
                            dontShow = true;
                            break;
                        }
                    }

                    foreach (Attribute attr in field.GetCustomAttributes(typeof(ParameterAttribute), true))
                    {
                        if (attr is ParameterAttribute)
                        {
                            var a = (ParameterAttribute)attr;
                            if (!dontShow)
                            {
                                if (a.ParameterSetName == ParameterAttribute.AllParameterSets)
                                {
                                    if (syntaxItems.Count == 0)
                                    {
                                        syntaxItems.Add(new SyntaxItem(ParameterAttribute.AllParameterSets));
                                    }
                                    foreach (var si in syntaxItems)
                                    {
                                        si.Parameters.Add(new SyntaxItem.Parameter() { Name = field.Name, Description = a.HelpMessage, Position = a.Position, Required = a.Mandatory, Type = field.FieldType.Name });
                                    }

                                    if (cmdletInfo.Syntaxes.Count == 0)
                                    {
                                        cmdletInfo.Syntaxes.Add(new CmdletSyntax() { ParameterSetName = ParameterAttribute.AllParameterSets });
                                    }
                                    foreach (var cmdletSyntax in cmdletInfo.Syntaxes)
                                    {
                                        cmdletSyntax.Parameters.Add(new CmdletParameterInfo() { Name = field.Name, Description = a.HelpMessage, Position = a.Position, Required = a.Mandatory, Type = field.FieldType.Name });
                                    }
                                }
                            }
                        }
                    }
                }



                // Build XElement for parameters from syntaxItems list (note: syntax element is set above)
                foreach (var syntaxItem in syntaxItems)
                {
                    var syntaxItemElement = new XElement(command + "syntaxItem");
                    syntaxElement.Add(syntaxItemElement);

                    syntaxItemElement.Add(new XElement(maml + "name", $"{verb}-{noun}"));
                    foreach (var parameter in syntaxItem.Parameters)
                    {
                        var parameterElement = new XElement(command + "parameter", new XAttribute("required", parameter.Required), new XAttribute("position", parameter.Position > 0 ? parameter.Position.ToString() : "named"));

                        parameterElement.Add(new XElement(maml + "name", parameter.Name));

                        parameterElement.Add(new XElement(maml + "description", new XElement(maml + "para", parameter.Description)));
                        parameterElement.Add(new XElement(command + "parameterValue", new XAttribute("required", parameter.Type != "SwitchParameter"), parameter.Type));

                        syntaxItemElement.Add(parameterElement);
                    }
                }

                // Also store parameters in cmdletInfo.Parameters (all parameters) and XElement parameters
                var parametersElement = new XElement(command + "parameters");
                commandElement.Add(parametersElement);

                foreach (var field in fields)
                {
                    var dontShow = false;
                    foreach (Attribute attr in field.GetCustomAttributes(typeof(ObsoleteAttribute), true))
                    {
                        if (attr is ObsoleteAttribute)
                        {
                            dontShow = true;
                            break;
                        }
                    }
                    foreach (Attribute attr in field.GetCustomAttributes(typeof(ParameterAttribute), true))
                    {
                        if (attr is ParameterAttribute)
                        {
                            var a = (ParameterAttribute)attr;
                            if (!dontShow)
                            {
                                cmdletInfo.Parameters.Add(new CmdletParameterInfo() { Name = field.Name, Description = a.HelpMessage, Position = a.Position, Required = a.Mandatory, Type = field.FieldType.Name });

                                var parameter2Element = new XElement(command + "parameter", new XAttribute("required", a.Mandatory), new XAttribute("position", a.Position > 0 ? a.Position.ToString() : "named"));

                                parameter2Element.Add(new XElement(maml + "name", field.Name));

                                parameter2Element.Add(new XElement(maml + "description", new XElement(maml + "para", a.HelpMessage)));
                                var parameterValueElement = new XElement(command + "parameterValue", field.FieldType.Name, new XAttribute("required", a.Mandatory));
                                parameter2Element.Add(parameterValueElement);

                                var devElement = new XElement(dev + "type");
                                devElement.Add(new XElement(maml + "name", field.FieldType.Name));
                                devElement.Add(new XElement(maml + "uri"));

                                parameter2Element.Add(devElement);

                                parametersElement.Add(parameter2Element);
                            }
                            break;

                        }
                    }
                }

                foreach (var additionalParameter in additionalParameters.GroupBy(o => new { o.ParameterName }).Select(o => o.FirstOrDefault()))
                {
                    cmdletInfo.Parameters.Add(new CmdletParameterInfo()
                    {
                        Name = additionalParameter.ParameterName,
                        Description = additionalParameter.HelpMessage,
                        Position = additionalParameter.Position,
                        Required = additionalParameter.Mandatory,
                        Type = additionalParameter.ParameterType.Name
                    });


                    var parameter2Element = new XElement(command + "parameter", new XAttribute("required", additionalParameter.Mandatory), new XAttribute("position", additionalParameter.Position > 0 ? additionalParameter.Position.ToString() : "named"));

                    parameter2Element.Add(new XElement(maml + "name", additionalParameter.ParameterName));

                    parameter2Element.Add(new XElement(maml + "description", new XElement(maml + "para", additionalParameter.HelpMessage)));
                    var parameterValueElement = new XElement(command + "parameterValue", additionalParameter.ParameterType.Name, new XAttribute("required", additionalParameter.Mandatory));
                    parameter2Element.Add(parameterValueElement);

                    var devElement = new XElement(dev + "type");
                    devElement.Add(new XElement(maml + "name", additionalParameter.ParameterType.Name));
                    devElement.Add(new XElement(maml + "uri"));

                    parameter2Element.Add(devElement);

                    parametersElement.Add(parameter2Element);

                }

                // XElement inputTypes
                commandElement.Add(
                    new XElement(command + "inputTypes",
                        new XElement(command + "inputType",
                            new XElement(dev + "type",
                                new XElement(maml + "name", "String"),
                                new XElement(maml + "uri"),
                                new XElement(maml + "description",
                                    new XElement(maml + "para", "description"))))));
                helpItems.Add(commandElement);

                // XElement return values
                commandElement.Add(
                    new XElement(command + "returnValues",
                        new XElement(command + "returnValue",
                            new XElement(dev + "type",
                                new XElement(maml + "name", "String"),
                                new XElement(maml + "uri"),
                                new XElement(maml + "description",
                                    new XElement(maml + "para", "description"))))));

                // XElement examples
                var examplesElement = new XElement(command + "examples");
                var exampleCount = 1;
                foreach (var exampleAttr in examples.OrderBy(e => e.SortOrder))
                {
                    var example = new XElement(command + "example");
                    var title = $"------------------EXAMPLE {exampleCount}---------------------";
                    example.Add(new XElement(maml + "title", title));
                    example.Add(new XElement(maml + "introduction", new XElement(maml + "para", exampleAttr.Introduction)));
                    example.Add(new XElement(dev + "code", exampleAttr.Code));
                    var remarksElement = new XElement(maml + "remarks", new XElement(maml + "para", exampleAttr.Remarks));
                    remarksElement.Add(new XElement(maml + "para", ""));
                    example.Add(remarksElement);
                    example.Add(new XElement(command + "commandLines",
                        new XElement(command + "commandLine",
                            new XElement(command + "commandText"))));
                    examplesElement.Add(example);
                    exampleCount++;
                }
                commandElement.Add(examplesElement);

                // Related links
                var relatedLinksElement = new XElement(maml + "relatedLinks");
                relatedLinks.Insert(0, new CmdletRelatedLinkAttribute() { Text = "Office 365 Developer Patterns and Practices", Url = "http://aka.ms/officedevpnp" });

                foreach (var link in relatedLinks)
                {
                    var navigationLinksElement = new XElement(maml + "navigationLink");
                    var linkText = new XElement(maml + "linkText");
                    linkText.Value = link.Text + ":";
                    navigationLinksElement.Add(linkText);
                    var uriElement = new XElement(maml + "uri");
                    uriElement.Value = link.Url;
                    navigationLinksElement.Add(uriElement);

                    relatedLinksElement.Add(navigationLinksElement);
                }
                commandElement.Add(relatedLinksElement);

                // Markdown from CmdletInfo
                if (generateMarkdown)
                {
                    GenerateMarkDown(toc, solutionDir, examples, cmdletInfo);
                }
                //              }

            }
            doc.Save(outFile);

            if (generateMarkdown)
            {
                CreateReadme(toc, solutionDir);
            }

            DirectoryInfo di = new DirectoryInfo($"{solutionDir}\\Documentation");
            var mdFiles = di.GetFiles("*.md");

            // Clean up old MD files
            foreach (var mdFile in mdFiles)
            {
                if (mdFile.Name.ToLowerInvariant() != "readme.md")
                {
                    var index = toc.FindIndex(t => $"{t.Verb}{t.Noun}.md" == mdFile.Name);
                    if (index == -1)
                    {
                        mdFile.Delete();
                    }
                }
            }

            // Generate PSM1 file
            var aliasesToExport = new List<string>();
            var psm1Path = $"{new FileInfo(inFile).Directory}\\ModuleFiles\\SharePointPnPPowerShell{spVersion}.psm1";


            if (aliasesToCreate.Any())
            {
                var aliasBuilder = new StringBuilder();

                foreach (var alias in aliasesToCreate.Where(a => !string.IsNullOrEmpty(a.Key)).OrderBy(a => a.Key))
                {
                    var aliasLine = $"Set-Alias -Name {alias.Value} -Value {alias.Key}";
                    aliasBuilder.AppendLine(aliasLine);
                    aliasesToExport.Add(alias.Value);
                }
                Console.WriteLine($"Writing {psm1Path}");
                File.WriteAllText(psm1Path, aliasBuilder.ToString());
            }
            else
            {
                File.WriteAllText(psm1Path, "");
            }

            // Create Module Manifest

            var psd1Path = $"{new FileInfo(inFile).Directory}\\ModuleFiles\\SharePointPnPPowerShell{spVersion}.psd1";
            var cmdletsToExportString = string.Join(",", cmdletsToExport.Select(x => "'" + x + "'"));
            var aliasesToExportString = string.Join(",", aliasesToExport.Select(x => "'" + x + "'"));
            WriteModuleManifest(psd1Path, spVersion, cmdletsToExportString, aliasesToExportString, assembly);
        }

        private static void WriteModuleManifest(string path, string version, string cmdletsToExport, string aliasesToExport, Assembly assembly)
        {
            var manifest = $@"@{{
    ModuleToProcess = 'SharePointPnPPowerShell{version}.psm1'
    NestedModules   = 'SharePointPnP.PowerShell.{version}.Commands.dll'
    ModuleVersion = '{assembly.GetName().Version}'
    Description = 'SharePoint Patterns and Practices PowerShell Cmdlets for SharePoint {version}'
    GUID = '8f1147be-a8e4-4bd2-a705-841d5334edc0'
    Author = 'SharePoint Patterns and Practices'
    CompanyName = 'SharePoint Patterns and Practices'
    DotNetFrameworkVersion = '4.5'
    ProcessorArchitecture = 'None'
    FunctionsToExport = '*'
    CmdletsToExport = {cmdletsToExport}
    VariablesToExport = '*'
    AliasesToExport = {aliasesToExport}
    FormatsToProcess = 'SharePointPnP.PowerShell.{version}.Commands.Format.ps1xml' 
}}";

            Console.WriteLine($"Writing {path}");
            File.WriteAllText(path, manifest, Encoding.UTF8);
        }
        private static List<FieldInfo> GetFields(Type t)
        {
            var fieldInfoList = new List<FieldInfo>();
            foreach (var fieldInfo in t.GetFields())
            {
                fieldInfoList.Add(fieldInfo);
            }
            if (t.BaseType != null && t.BaseType.BaseType != null)
            {
                fieldInfoList.AddRange(GetFields(t.BaseType.BaseType));
            }
            return fieldInfoList;
        }

        private static void CreateReadme(List<CmdletInfo> toc, string solutionDir)
        {
            var originalMd = string.Empty;
            var newMd = string.Empty;

            // Create the readme.md
            var readmePath = $"{solutionDir}\\Documentation\\readme.md";
            if (System.IO.File.Exists(readmePath))
            {
                originalMd = System.IO.File.ReadAllText(readmePath);
            }
            var docBuilder = new StringBuilder();


            docBuilder.AppendFormat("# Cmdlet Documentation #{0}", Environment.NewLine);
            docBuilder.AppendFormat("Below you can find a list of all the available cmdlets. Many commands provide built-in help and examples. Retrieve the detailed help with {0}", Environment.NewLine);
            docBuilder.AppendFormat("{0}```powershell{0}Get-Help Connect-PnPOnline -Detailed{0}```{0}{0}", Environment.NewLine);

            // Get all unique categories
            var categories = toc.Select(c => c.Category).Distinct();

            foreach (var category in categories.OrderBy(c => c))
            {
                docBuilder.AppendFormat("##{0}{1}", category, Environment.NewLine);

                docBuilder.AppendFormat("Cmdlet|Description{0}", Environment.NewLine);
                docBuilder.AppendFormat(":-----|:----------{0}", Environment.NewLine);
                foreach (var cmdletInfo in toc.Where(c => c.Category == category).OrderBy(c => c.Noun))
                {
                    var description = cmdletInfo.Description.Replace("\r\n", " ");
                    docBuilder.AppendFormat("**[{0}]({1}{2}.md)** |{3}{4}", cmdletInfo.FullCommand.Replace("-", "&#8209;"), cmdletInfo.Verb, cmdletInfo.Noun, description, Environment.NewLine);
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

        private static void GenerateMarkDown(List<CmdletInfo> toc, string solutionDir, List<CmdletExampleAttribute> examples, CmdletInfo cmdletInfo)
        {
            var originalMd = string.Empty;
            var newMd = string.Empty;


            if (!string.IsNullOrEmpty(cmdletInfo.Verb) && !string.IsNullOrEmpty(cmdletInfo.Noun))
            {
                string mdFilePath = $"{solutionDir}\\Documentation\\{cmdletInfo.Verb}{cmdletInfo.Noun}.md";
                toc.Add(cmdletInfo);

                if (System.IO.File.Exists(mdFilePath))
                {
                    originalMd = System.IO.File.ReadAllText(mdFilePath);

                }
                var docBuilder = new StringBuilder();

                // Header

                docBuilder.AppendFormat("#{0}{1}", cmdletInfo.FullCommand, Environment.NewLine);

                // Body 

                docBuilder.AppendFormat("{0}{1}", cmdletInfo.Description, Environment.NewLine);
                docBuilder.AppendFormat("##Syntax{0}", Environment.NewLine);
                foreach (var cmdletSyntax in cmdletInfo.Syntaxes.OrderBy(s => s.ParameterSetName))
                {
                    var syntaxText = new StringBuilder();
                    syntaxText.AppendFormat("```powershell\r\n{0} ", cmdletInfo.FullCommand);
                    var cmdletLength = cmdletInfo.FullCommand.Length;
                    var first = true;
                    foreach (var par in cmdletSyntax.Parameters.OrderBy(p => p.Position))
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
                    docBuilder.AppendFormat("```\n\n\n");
                }

                if (!string.IsNullOrEmpty(cmdletInfo.DetailedDescription))
                {
                    docBuilder.Append("##Detailed Description\n");
                    docBuilder.AppendFormat("{0}\n\n", cmdletInfo.DetailedDescription);
                }

                if (cmdletInfo.OutputType != null)
                {
                    docBuilder.Append("##Returns\n");
                    if (!string.IsNullOrEmpty(cmdletInfo.OutputTypeLink))
                    {
                        docBuilder.Append($">[{cmdletInfo.OutputType}]({cmdletInfo.OutputTypeLink})");
                    }
                    else
                    {
                        docBuilder.Append($">{cmdletInfo.OutputType}");
                    }
                    if (!string.IsNullOrEmpty(cmdletInfo.OutputTypeDescription))
                    {
                        docBuilder.Append($"\n\n{cmdletInfo.OutputTypeDescription}");
                    }
                    docBuilder.Append("\n\n");
                }

                docBuilder.Append("##Parameters\n");
                docBuilder.Append("Parameter|Type|Required|Description\n");
                docBuilder.Append("---------|----|--------|-----------\n");
                foreach (var par in cmdletInfo.Parameters.OrderBy(x => x.Name))
                {
                    docBuilder.AppendFormat("|{0}|{1}|{2}|{3}|\n", par.Name, par.Type, par.Required ? "True" : "False", par.Description);
                }
                if (examples.Any())
                    docBuilder.Append("##Examples\n");
                var examplesCount = 1;
                foreach (var example in examples.OrderBy(e => e.SortOrder))
                {
                    docBuilder.AppendFormat("{0}\n", example.Introduction);
                    docBuilder.AppendFormat("###Example {0}\n", examplesCount);
                    docBuilder.AppendFormat("```powershell\n{0}\n```\n", example.Code);
                    docBuilder.AppendFormat("{0}\n", example.Remarks);
                    examplesCount++;
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

        public static string ToEnumString<T>(T type)
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, type);
            try
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                return enumMemberAttribute.Value;
            }
            catch
            {
                return name;
            }
        }

        private class SyntaxItem
        {
            public readonly string Name;
            public readonly List<Parameter> Parameters;

            public SyntaxItem(string name)
            {
                Name = name;
                Parameters = new List<Parameter>();
            }

            public class Parameter
            {
                public bool Required;
                public int Position;
                public string Name;
                public string Description;
                public string Type;
            }
        }

    }
}
