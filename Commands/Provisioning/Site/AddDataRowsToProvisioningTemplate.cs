using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Text.RegularExpressions;
using SPSite = Microsoft.SharePoint.Client.Site;

namespace PnP.PowerShell.Commands.Provisioning.Site
{
    [Cmdlet(VerbsCommon.Add, "PnPDataRowsToProvisioningTemplate")]
    [CmdletHelp("Adds datarows to a list inside a PnP Provisioning Template",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Add-PnPDataRowsToProvisioningTemplate -Path template.pnp -List 'PnPTestList' -Query '<View></View>' -Fields 'Title','Choice'",
       Remarks = "Adds datarows from the provided list to the PnP Provisioning Template at the provided location",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Add-PnPDataRowsToProvisioningTemplate -Path template.pnp -List 'PnPTestList' -Query '<View></View>' -Fields 'Title','Choice' -IncludeSecurity",
      Remarks = "Adds datarows from the provided list to the PnP Provisioning Template at the provided location",
       SortOrder = 2)]
    public class AddDataRowsToProvisioningTemplate : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename of the .PNP Open XML site template to read from, optionally including full path.")]
        public string Path;

        [Parameter(Mandatory = true, HelpMessage = "The list to query")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "The CAML query to execute against the list. Defaults to all items.")]
        public string Query;

        [Parameter(Mandatory = false, HelpMessage = "The fields to retrieve. If not specified all fields will be loaded in the returned list object.")]
        public string[] Fields;

        [Parameter(Mandatory = false, Position = 5, HelpMessage = "A switch to include ObjectSecurity information.")]
        public SwitchParameter IncludeSecurity;

        [Parameter(Mandatory = false, Position = 4, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while loading the template.")]
        public ITemplateProviderExtension[] TemplateProviderExtensions;

        [Parameter(Mandatory = false, HelpMessage = "If set, this switch will try to tokenize the values with web and site related tokens")]
        public SwitchParameter TokenizeUrls;

        private readonly static FieldType[] _unsupportedFieldTypes =
        {
            FieldType.Attachments,
            FieldType.Computed
        };

        protected override void ExecuteCmdlet()
        {
            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }

            var template = ReadProvisioningTemplate
                    .LoadProvisioningTemplateFromFile(Path,
                    TemplateProviderExtensions, (e) =>
                    {
                        WriteError(new ErrorRecord(e, "TEMPLATENOTVALID", ErrorCategory.SyntaxError, null));
                    });

            if (template == null)
            {
                throw new ApplicationException("Invalid template file!");
            }
            //We will remove a list if it's found so we can get the list

            ListInstance listInstance = template.Lists.Find(l => l.Title == List.Title);
            if (listInstance == null)
            {
                throw new ApplicationException("List does not exist in the template file!");
            }

            List spList = List.GetList(SelectedWeb);
            ClientContext.Load(spList, l => l.RootFolder, l => l.HasUniqueRoleAssignments);
            ClientContext.ExecuteQueryRetry();

            if (TokenizeUrls.IsPresent)
            {
                ClientContext.Load(ClientContext.Web, w => w.Url, w => w.ServerRelativeUrl, w => w.Id);
                ClientContext.Load(ClientContext.Site, s => s.Url, s => s.ServerRelativeUrl, s => s.Id);
                ClientContext.Load(ClientContext.Web.Lists, lists => lists.Include(l=>l.Title, l => l.RootFolder.ServerRelativeUrl));
            }

            CamlQuery query = new CamlQuery();

            var viewFieldsStringBuilder = new StringBuilder();
            if (Fields != null)
            {
                viewFieldsStringBuilder.Append("<ViewFields>");
                foreach (var field in Fields)
                {
                    viewFieldsStringBuilder.AppendFormat("<FieldRef Name='{0}'/>", field);
                }
                viewFieldsStringBuilder.Append("</ViewFields>");
            }

            query.ViewXml = string.Format("<View>{0}{1}</View>", Query, viewFieldsStringBuilder);
            var listItems = spList.GetItems(query);

            ClientContext.Load(listItems, lI => lI.Include(l => l.HasUniqueRoleAssignments, l => l.ContentType.StringId));
            ClientContext.ExecuteQueryRetry();

            Microsoft.SharePoint.Client.FieldCollection fieldCollection = spList.Fields;
            ClientContext.Load(fieldCollection, fs => fs.Include(f => f.InternalName, f => f.FieldTypeKind, f => f.ReadOnlyField));
            ClientContext.ExecuteQueryRetry();

            var rows = new DataRowCollection(template);
            foreach (var listItem in listItems)
            {
                //Make sure we don't pull Folders.. Of course this won't work
                if (listItem.ServerObjectIsNull == false)
                {
                    ClientContext.Load(listItem);
                    ClientContext.ExecuteQueryRetry();
                    if (!(listItem.FileSystemObjectType == FileSystemObjectType.Folder))
                    {
                        DataRow row = new DataRow();
                        if (IncludeSecurity && listItem.HasUniqueRoleAssignments)
                        {
                            row.Security.ClearSubscopes = true;
                            row.Security.CopyRoleAssignments = false;

                            var roleAssignments = listItem.RoleAssignments;
                            ClientContext.Load(roleAssignments);
                            ClientContext.ExecuteQueryRetry();

                            ClientContext.Load(roleAssignments, r => r.Include(a => a.Member.LoginName, a => a.Member, a => a.RoleDefinitionBindings));
                            ClientContext.ExecuteQueryRetry();

                            foreach (var roleAssignment in roleAssignments)
                            {
                                var principalName = roleAssignment.Member.LoginName;
                                var roleBindings = roleAssignment.RoleDefinitionBindings;
                                foreach (var roleBinding in roleBindings)
                                {
                                    row.Security.RoleAssignments.Add(new OfficeDevPnP.Core.Framework.Provisioning.Model.RoleAssignment() { Principal = principalName, RoleDefinition = roleBinding.Name });
                                }
                            }
                        }
                        if (Fields != null)
                        {
                            foreach (var fieldName in Fields)
                            {
                                Microsoft.SharePoint.Client.Field dataField = fieldCollection.FirstOrDefault(f => f.InternalName == fieldName);

                                if (dataField != null)
                                {
                                    var defaultFieldValue = GetFieldValueAsText(ClientContext.Web, listItem, dataField);
                                    if (TokenizeUrls.IsPresent)
                                    {
                                        defaultFieldValue = Tokenize(defaultFieldValue, ClientContext.Web, ClientContext.Site);
                                    }

                                    row.Values.Add(fieldName, defaultFieldValue);
                                }
                            }
                        }
                        else
                        {
                            //All fields are added except readonly fields and unsupported field type
                            var fieldsToExport = fieldCollection.AsEnumerable()
                                .Where(f => !f.ReadOnlyField && !_unsupportedFieldTypes.Contains(f.FieldTypeKind));
                            foreach (var field in fieldsToExport)
                            {
                                var fldKey = (from f in listItem.FieldValues.Keys where f == field.InternalName select f).FirstOrDefault();
                                if (!string.IsNullOrEmpty(fldKey))
                                {
                                    var fieldValue = GetFieldValueAsText(ClientContext.Web, listItem, field);
                                    if (TokenizeUrls.IsPresent)
                                    {
                                        fieldValue = Tokenize(fieldValue, ClientContext.Web, ClientContext.Site);
                                    }
                                    row.Values.Add(field.InternalName, fieldValue);
                                }
                            }
                        }

                        rows.Add(row);
                    }
                }
            }
            template.Lists.Remove(listInstance);
            listInstance.DataRows.AddRange(rows);
            template.Lists.Add(listInstance);

            var outFileName = System.IO.Path.GetFileName(Path);
            var outPath = new FileInfo(Path).DirectoryName;

            var fileSystemConnector = new FileSystemConnector(outPath, "");
            var formatter = XMLPnPSchemaFormatter.LatestFormatter;
            var extension = new FileInfo(Path).Extension.ToLowerInvariant();
            if (extension == ".pnp")
            {
                XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(new OpenXMLConnector(Path, fileSystemConnector));
                var templateFileName = outFileName.Substring(0, outFileName.LastIndexOf(".", StringComparison.Ordinal)) + ".xml";

                provider.SaveAs(template, templateFileName, formatter, TemplateProviderExtensions);
            }
            else
            {
                XMLTemplateProvider provider = new XMLFileSystemTemplateProvider(Path, "");
                provider.SaveAs(template, Path, formatter, TemplateProviderExtensions);
            }
        }

        private string GetFieldValueAsText(Web web, ListItem listItem, Microsoft.SharePoint.Client.Field field)
        {
            var rawValue = listItem[field.InternalName];
            if (rawValue == null) return null;

            switch (field.FieldTypeKind)
            {
                case FieldType.Geolocation:
                    var geoValue = (FieldGeolocationValue)rawValue;
                    return $"{geoValue.Altitude},{geoValue.Latitude},{geoValue.Longitude},{geoValue.Measure}";
                case FieldType.URL:
                    var urlValue = (FieldUrlValue)rawValue;
                    return $"{urlValue.Url},{urlValue.Description}";
                case FieldType.Lookup:
                    var strVal = rawValue as string;
                    if(strVal != null)
                    {
                        return strVal;
                    }
                    var singleLookupValue = rawValue as FieldLookupValue;
                    if (singleLookupValue != null)
                    {
                        return singleLookupValue.LookupId.ToString();
                    }
                    var multipleLookupValue = rawValue as FieldLookupValue[];
                    if (multipleLookupValue != null)
                    {
                        return string.Join(",", multipleLookupValue.Select(lv => lv.LookupId));
                    }
                    throw new Exception("Invalid data in field");
                case FieldType.User:
                    var singleUserValue = rawValue as FieldUserValue;
                    if (singleUserValue != null)
                    {
                        return GetLoginName(web, singleUserValue.LookupId);
                    }
                    var multipleUserValue = rawValue as FieldUserValue[];
                    if (multipleUserValue != null)
                    {
                        return string.Join(",", multipleUserValue.Select(lv => GetLoginName(web,lv.LookupId)));
                    }
                    throw new Exception("Invalid data in field");
                case FieldType.MultiChoice:
                    var multipleChoiceValue = rawValue as string[];
                    if (multipleChoiceValue != null)
                    {
                        return string.Join(";#", multipleChoiceValue);
                    }
                    return Convert.ToString(rawValue);
                default:
                    return Convert.ToString(rawValue);
            }
        }

        private Dictionary<Guid, Dictionary<int, string>> _webUserCache = new Dictionary<Guid, Dictionary<int, string>>();
        private string GetLoginName(Web web, int userId)
        {
            if (!_webUserCache.ContainsKey(web.Id)) _webUserCache.Add(web.Id, new Dictionary<int, string>());
            if (!_webUserCache[web.Id].ContainsKey(userId))
            {
                var user = web.GetUserById(userId);
                web.Context.Load(user, u => u.LoginName);
                web.Context.ExecuteQueryRetry();
                _webUserCache[web.Id].Add(userId, user.LoginName);
            }
            return _webUserCache[web.Id][userId];
        }

        private static string Tokenize(string input, Web web, SPSite site)
        {
            if (string.IsNullOrEmpty(input)) return input;
            //foreach (var list in lists)
            //{
            //    input = input.ReplaceCaseInsensitive(web.Url.TrimEnd('/') + "/" + list.GetWebRelativeUrl(), "{listurl:" + Regex.Escape(list.Title) + "}");
            //    input = input.ReplaceCaseInsensitive(list.RootFolder.ServerRelativeUrl, "{listurl:" + Regex.Escape(list.Title)+ "}");
            //}
            input = input.ReplaceCaseInsensitive(web.Url, "{site}");
            input = input.ReplaceCaseInsensitive(web.ServerRelativeUrl, "{site}");
            input = input.ReplaceCaseInsensitive(web.Id.ToString(), "{siteid}");
            input = input.ReplaceCaseInsensitive(site.ServerRelativeUrl, "{sitecollection}");
            input = input.ReplaceCaseInsensitive(site.Id.ToString(), "{sitecollectionid}");
            input = input.ReplaceCaseInsensitive(site.Url, "{sitecollection}");

            return input;
        }
    }
}
