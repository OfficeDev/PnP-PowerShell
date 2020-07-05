using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Linq;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.ModuleFilesGenerator
{
    internal class HelpFileGenerator
    {
        private readonly Assembly _assembly;
        private List<Model.CmdletInfo> _cmdlets;
        private string _outfile;

        private readonly XNamespace maml = "http://schemas.microsoft.com/maml/2004/10";
        private readonly XNamespace command = "http://schemas.microsoft.com/maml/dev/command/2004/10";
        private readonly XNamespace dev = "http://schemas.microsoft.com/maml/dev/2004/10";

        private readonly XAttribute mamlNsAttr = new XAttribute(XNamespace.Xmlns + "maml", "http://schemas.microsoft.com/maml/2004/10");
        private readonly XAttribute commandNsAttr = new XAttribute(XNamespace.Xmlns + "command", "http://schemas.microsoft.com/maml/dev/command/2004/10");
        private readonly XAttribute devNsAttr = new XAttribute(XNamespace.Xmlns + "dev", "http://schemas.microsoft.com/maml/dev/2004/10");

        internal HelpFileGenerator(List<Model.CmdletInfo> cmdlets, Assembly assembly, string outfile)
        {
            _assembly = assembly;
            _cmdlets = cmdlets;
            _outfile = outfile;
        }

        internal void Generate()
        {
            var doc = new XDocument(new XDeclaration("1.0", "UTF-8", String.Empty));
            XNamespace ns = "http://msh";
            var helpItems = new XElement(ns + "helpItems", new XAttribute("schema", "maml"));
            doc.Add(helpItems);

            foreach (var cmdletInfo in _cmdlets)
            {
                var commandElement = GetCommandElement(cmdletInfo);
                commandElement.Add(GetSyntaxElement(cmdletInfo));
                commandElement.Add(GetParametersElement(cmdletInfo));
                //commandElement.Add(GetInputTypesElement());
                commandElement.Add(GetReturnValuesElement(cmdletInfo));
                commandElement.Add(GetExamplesElement(cmdletInfo));
                commandElement.Add(GetRelatedLinksElement(cmdletInfo));
                helpItems.Add(commandElement);
            }

            doc.Save(_outfile);

        }

        private XElement GetCommandElement(Model.CmdletInfo cmdletInfo)
        {
            var commandElement = new XElement(command + "command", mamlNsAttr, commandNsAttr, devNsAttr);
            var detailsElement = new XElement(command + "details");
            commandElement.Add(detailsElement);

            detailsElement.Add(new XElement(command + "name", $"{cmdletInfo.Verb}-{cmdletInfo.Noun}"));

            System.Text.StringBuilder additionalDescriptionInfo = new System.Text.StringBuilder();
            // Supported platforms
            if (cmdletInfo.Platform != "All")
            {
                additionalDescriptionInfo.AppendLine($"* Supported in: {cmdletInfo.Platform}.");
            }

            // API Permissions
            if (cmdletInfo.ApiPermissions.Count > 0)
            {
                additionalDescriptionInfo.AppendLine("* Required API permissions:");

                // Go through each of the API permissions on this cmdlet and build a dictionary with the API type as the key and the requested scope(s) on it as the value
                var requiredOrPermissionByApiDictionary = new Dictionary<string, List<string>>(cmdletInfo.ApiPermissions.Count);
                var requiredAndPermissionByApiDictionary = new Dictionary<string, List<string>>(cmdletInfo.ApiPermissions.Count);
                foreach (var requiredApiPermission in cmdletInfo.ApiPermissions)
                {

                    requiredOrPermissionByApiDictionary = ParseApiPermissions("OrApiPermissions", requiredApiPermission, requiredOrPermissionByApiDictionary);
                    requiredAndPermissionByApiDictionary = ParseApiPermissions("AndApiPermissions", requiredApiPermission, requiredAndPermissionByApiDictionary);

                    //// Through reflection, get the friendly name of the API and the permission scopes. We must use reflection here as generic attribute types do not exist in C# yet. Multiple scopes are returned as a comma-space separated string.
                    //var scopePermission = requiredApiPermission.GetType().GetProperty("OrApiPermissions")?.GetValue(requiredApiPermission, null);

                    //// If we were unable to retrieve the scopes and/or api through reflection, continue with the next permission attribute
                    //if (scopePermission == null || requiredApiPermission.ApiName == null) continue;

                    //// Ensure our dictionary with APIs already has an entry for the current API being processed
                    //if(!requiredPermissionByApiDictionary.ContainsKey(requiredApiPermission.ApiName))
                    //{
                    //    // Add another entry to the dictionary for this API
                    //    requiredPermissionByApiDictionary.Add(requiredApiPermission.ApiName, new List<string>());
                    //}

                    //// The returned scopes are the enum names which use an underscore instead of a dot for the scope names and are comma-space separated if multiple scopes are possible
                    //var scopes = scopePermission.ToString().Replace("_", ".").Split(',');

                    //// Go through each scope
                    //foreach (var scope in scopes)
                    //{
                    //    // Remove the potential trailing space
                    //    var trimmedScope = scope.Trim();

                    //    // If the scope is not known to be required yet, add it to the dictionary with APIs and requested scopes
                    //    if (!requiredPermissionByApiDictionary[requiredApiPermission.ApiName].Contains(trimmedScope))
                    //    {
                    //        requiredPermissionByApiDictionary[requiredApiPermission.ApiName].Add(trimmedScope);
                    //    }
                    //}
                }

                // Loop through all the items in our dictionary with API types and requested scopes on the API so we can add it to the help text output
                foreach (var requiredPermissionDictionaryItem in requiredOrPermissionByApiDictionary)
                {
                    if (requiredPermissionDictionaryItem.Value.Count > 0)
                    {
                        additionalDescriptionInfo.AppendLine($"  * {requiredPermissionDictionaryItem.Key}: {string.Join(" or ", requiredPermissionDictionaryItem.Value.OrderBy(s => s))}");
                    }
                }
                foreach (var requiredPermissionDictionaryItem in requiredAndPermissionByApiDictionary)
                {
                    if (requiredPermissionDictionaryItem.Value.Count > 0)
                    {
                        additionalDescriptionInfo.AppendLine($"  * {requiredPermissionDictionaryItem.Key}: {string.Join(" and ", requiredPermissionDictionaryItem.Value.OrderBy(s => s))}");
                    }
                }

                additionalDescriptionInfo.AppendLine();
            }

            // Notice if the cmdlet is a PnPAdminCmdlet that access to the SharePoint Tenant Admin site is needed
            if (cmdletInfo.CmdletType.BaseType.Name.Equals("PnPAdminCmdlet"))
            {
                additionalDescriptionInfo.AppendLine("* Required SharePoint permissions: Access to the SharePoint Tenant Administration site");
            }

            // Validate if additional information should be prepended to the description
            if (additionalDescriptionInfo.Length > 0)
            {
                // Append an empty line between the additional information and the description itself
                additionalDescriptionInfo.AppendLine();
            }
            detailsElement.Add(new XElement(maml + "description", new XElement(maml + "para", additionalDescriptionInfo.ToString() + cmdletInfo.Description)));
            detailsElement.Add(new XElement(maml + "copyright", new XElement(maml + "para", cmdletInfo.Copyright)));
            detailsElement.Add(new XElement(command + "verb", cmdletInfo.Verb));
            detailsElement.Add(new XElement(command + "noun", cmdletInfo.Noun));
            detailsElement.Add(new XElement(dev + "version", cmdletInfo.Version));

            if (!string.IsNullOrWhiteSpace(cmdletInfo.DetailedDescription))
            {
                commandElement.Add(new XElement(maml + "description", new XElement(maml + "para", cmdletInfo.DetailedDescription)));
            }
            return commandElement;
        }

        private XElement GetSyntaxElement(Model.CmdletInfo cmdletInfo)
        {
            var syntaxElement = new XElement(command + "syntax");

            foreach (var syntaxItem in cmdletInfo.Syntaxes.OrderBy(s => s.Parameters.Count(p => p.Required)))
            {
                var syntaxItemElement = new XElement(command + "syntaxItem");
                syntaxItemElement.Add(new XElement(maml + "name", $"{cmdletInfo.Verb}-{cmdletInfo.Noun}"));
                foreach (var parameter in syntaxItem.Parameters.Distinct(new ParameterComparer()).OrderBy(p => p.Order).ThenBy(p => !p.Required).ThenBy(p => p.Position))
                {
                    var parameterElement = new XElement(command + "parameter", new XAttribute("required", parameter.Required), new XAttribute("position", parameter.Position > 0 ? parameter.Position.ToString() : "named"));
                    parameterElement.Add(new XElement(maml + "name", parameter.Name));

                    parameterElement.Add(new XElement(maml + "description", new XElement(maml + "para", parameter.Description)));
                    parameterElement.Add(new XElement(command + "parameterValue", new XAttribute("required", parameter.Type != "SwitchParameter"), parameter.Type));

                    syntaxItemElement.Add(parameterElement);
                }
                syntaxElement.Add(syntaxItemElement);
            }
            return syntaxElement;
        }

        private Dictionary<string, List<string>> ParseApiPermissions(string propertyName, CmdletApiPermissionBase requiredApiPermission, Dictionary<string, List<string>> requiredPermissionByApiDictionary)
        {
            // Through reflection, get the friendly name of the API and the permission scopes. We must use reflection here as generic attribute types do not exist in C# yet. Multiple scopes are returned as a comma-space separated string.
            var scopePermission = requiredApiPermission.GetType().GetProperty(propertyName)?.GetValue(requiredApiPermission, null);

            // If we were unable to retrieve the scopes and/or api through reflection, continue with the next permission attribute
            if (scopePermission != null && requiredApiPermission.ApiName != null)
            {

                // Ensure our dictionary with APIs already has an entry for the current API being processed
                if (!requiredPermissionByApiDictionary.ContainsKey(requiredApiPermission.ApiName))
                {
                    // Add another entry to the dictionary for this API
                    requiredPermissionByApiDictionary.Add(requiredApiPermission.ApiName, new List<string>());
                }

                // The returned scopes are the enum names which use an underscore instead of a dot for the scope names and are comma-space separated if multiple scopes are possible
                var scopes = scopePermission.ToString().Replace("_", ".").Split(',').Where(s => s != "None");

                // Go through each scope
                foreach (var scope in scopes)
                {
                    // Remove the potential trailing space
                    var trimmedScope = scope.Trim();

                    // If the scope is not known to be required yet, add it to the dictionary with APIs and requested scopes
                    if (!requiredPermissionByApiDictionary[requiredApiPermission.ApiName].Contains(trimmedScope))
                    {
                        requiredPermissionByApiDictionary[requiredApiPermission.ApiName].Add(trimmedScope);
                    }
                }
            }
            return requiredPermissionByApiDictionary;
        }

        private XElement GetParametersElement(Model.CmdletInfo cmdletInfo)
        {
            var parametersElement = new XElement(command + "parameters");

            foreach (var parameter in cmdletInfo.Parameters.Distinct(new ParameterComparer()).OrderBy(p => p.Order).ThenBy(p => p.Name))
            {
                var parameterElement = new XElement(command + "parameter", new XAttribute("required", parameter.Required), new XAttribute("position", parameter.Position > 0 ? parameter.Position.ToString() : "named"));
                parameterElement.Add(new XElement(maml + "name", parameter.Name));

                parameterElement.Add(new XElement(maml + "description", new XElement(maml + "para", parameter.Description)));
                var parameterValueElement = new XElement(command + "parameterValue", parameter.Type, new XAttribute("required", parameter.Required));
                parameterElement.Add(parameterValueElement);

                var devElement = new XElement(dev + "type");
                devElement.Add(new XElement(maml + "name", parameter.Type));
                devElement.Add(new XElement(maml + "uri"));

                parameterElement.Add(devElement);

                parametersElement.Add(parameterElement);
            }
            return parametersElement;
        }

        private XElement GetInputTypesElement(Model.CmdletInfo cmdletInfo)
        {
            return new XElement(command + "inputTypes",
                new XElement(command + "inputType",
                    new XElement(dev + "type",
                        new XElement(maml + "name", ""),
                        new XElement(maml + "uri"),
                        new XElement(maml + "description",
                            new XElement(maml + "para", "")))));
        }

        private XElement GetReturnValuesElement(Model.CmdletInfo cmdletInfo)
        {
            var outputType = "";
            var outputTypeDescription = "";
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
                    else if (cmdletInfo.OutputType.FullName == "GenericObjectNameIdPipeBind`1" || cmdletInfo.OutputType.FullName == "TaxonomyItemPipeBind`1")
                    {
                        outputType = cmdletInfo.OutputType.GenericTypeArguments[0].FullName;
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
            if (!string.IsNullOrEmpty(cmdletInfo.OutputTypeDescription))
            {
                outputTypeDescription = cmdletInfo.OutputTypeDescription;
            }
            return new XElement(command + "returnValues",
                new XElement(command + "returnValue",
                    new XElement(dev + "type",
                        new XElement(maml + "name", outputType),
                        new XElement(maml + "uri"),
                        new XElement(maml + "description",
                            new XElement(maml + "para", outputTypeDescription)))));
        }

        private XElement GetExamplesElement(Model.CmdletInfo cmdletInfo)
        {
            var examplesElement = new XElement(command + "examples");
            var exampleCount = 1;
            foreach (var exampleAttr in cmdletInfo.Examples.OrderBy(e => e.SortOrder))
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
            return examplesElement;
        }

        private XElement GetRelatedLinksElement(Model.CmdletInfo cmdletInfo)
        {
            var relatedLinksElement = new XElement(maml + "relatedLinks");
            cmdletInfo.RelatedLinks.Insert(0, new CmdletRelatedLinkAttribute() { Text = "SharePoint Developer Patterns and Practices", Url = "http://aka.ms/sppnp" });

            foreach (var link in cmdletInfo.RelatedLinks)
            {
                var navigationLinksElement = new XElement(maml + "navigationLink");
                var linkText = new XElement(maml + "linkText") { Value = link.Text + ":" };
                navigationLinksElement.Add(linkText);
                var uriElement = new XElement(maml + "uri") { Value = link.Url };
                navigationLinksElement.Add(uriElement);

                relatedLinksElement.Add(navigationLinksElement);
            }
            return relatedLinksElement;
        }

        #region Helpers
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

        private static string ToEnumString<T>(T type)
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
        #endregion
    }
}
