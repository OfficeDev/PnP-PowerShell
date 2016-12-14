using System;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Get, "PnPListItem", DefaultParameterSetName = "ById")]
    [CmdletAlias("Get-SPOListItem")]
    [CmdletHelp("Retrieves list items",
        Category = CmdletHelpCategory.Lists,
        OutputType = typeof(ListItem),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.listitem.aspx")]
    [CmdletExample(
        Code = "PS:> Get-PnPListItem -List Tasks",
        Remarks = "Retrieves all list items from the Tasks list",
        SortOrder = 1)]
    [CmdletExample(
        Code = "PS:> Get-PnPListItem -List Tasks -Id 1",
        Remarks = "Retrieves the list item with ID 1 from from the Tasks list. This parameter is ignored if the Query parameter is specified.",
        SortOrder = 2)]
    [CmdletExample(
        Code = "PS:> Get-PnPListItem -List Tasks -UniqueId bd6c5b3b-d960-4ee7-a02c-85dc6cd78cc3",
        Remarks = "Retrieves the list item with unique id bd6c5b3b-d960-4ee7-a02c-85dc6cd78cc3 from from the tasks lists. This parameter is ignored if the Query parameter is specified.",
        SortOrder = 3)]
    [CmdletExample(
        Code = "PS:> Get-PnPListItem -List Tasks -Fields \"Title\",\"GUID\"",
        Remarks = "Retrieves all list items, but only includes the values of the Title and GUID fields in the list item object. This parameter is ignored if the Query parameter is specified.",
        SortOrder = 4)]
    [CmdletExample(
        Code = "PS:> Get-PnPListItem -List Tasks -Query \"<View><Query><Where><Eq><FieldRef Name='GUID'/><Value Type='Guid'>bd6c5b3b-d960-4ee7-a02c-85dc6cd78cc3</Value></Eq></Where></Query></View>\"",
        Remarks = "Retrieves all list items based on the CAML query specified.",
        SortOrder = 5)]
    [CmdletExample(
        Code = "PS:> Get-PnPListItem -List Tasks -PageSize 1000",
        Remarks = "Retrieves all list items from the Tasks list in pages of 1000 items. This parameter is ignored if the Query parameter is specified.",
        SortOrder = 6)]
    public class GetListItem : SPOWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The list to query", Position = 0, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "The ID of the item to retrieve", ParameterSetName = "ById")]
        public int Id = -1;

        [Parameter(Mandatory = false, HelpMessage = "The unique id (GUID) of the item to retrieve", ParameterSetName = "ByUniqueId")]
        public GuidPipeBind UniqueId;

        [Parameter(Mandatory = false, HelpMessage = "The CAML query to execute against the list", ParameterSetName = "ByQuery")]
        public string Query;

        [Parameter(Mandatory = false, HelpMessage = "The fields to retrieve. If not specified all fields will be loaded in the returned list object.", ParameterSetName = "AllItems")]
        [Parameter(Mandatory = false, HelpMessage = "TThe fields to retrieve. If not specified all fields will be loaded in the returned list object.", ParameterSetName = "ById")]
        [Parameter(Mandatory = false, HelpMessage = "The fields to retrieve. If not specified all fields will be loaded in the returned list object.", ParameterSetName = "ByUniqueId")]
        public string[] Fields;

        [Parameter(Mandatory = false, HelpMessage = "The number of items to retrieve per page request.", ParameterSetName = "AllItems")]
        public int PageSize = -1;

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);

            if (HasId())
            {
                var listItem = list.GetItemById(Id);
                if (Fields != null)
                {
                    foreach (var field in Fields)
                    {
                        ClientContext.Load(listItem, l => l[field]);
                    }
                }
                else
                {
                    ClientContext.Load(listItem);
                }
                ClientContext.ExecuteQueryRetry();
                WriteObject(listItem);
            }
            else if (HasUniqueId())
            {
                CamlQuery query = new CamlQuery();
                var viewFieldsStringBuilder = new StringBuilder();
                if (HasFields())
                {
                    viewFieldsStringBuilder.Append("<ViewFields>");
                    foreach (var field in Fields)
                    {
                        viewFieldsStringBuilder.AppendFormat("<FieldRef Name='{0}'/>", field);
                    }
                    viewFieldsStringBuilder.Append("</ViewFields>");
                }
                query.ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='GUID'/><Value Type='Guid'>{0}</Value></Eq></Where></Query>{1}</View>", UniqueId.Id, viewFieldsStringBuilder);
                var listItem = list.GetItems(query);
                ClientContext.Load(listItem);
                ClientContext.ExecuteQueryRetry();
                WriteObject(listItem);
            }
            else if (HasCamlQuery())
            {
                CamlQuery query = new CamlQuery { ViewXml = Query };
                var listItems = list.GetItems(query);
                ClientContext.Load(listItems);
                ClientContext.ExecuteQueryRetry();
                WriteObject(listItems, true);
            }
            else
            {
                var query = CamlQuery.CreateAllItemsQuery();
                if (Fields != null)
                {
                    var queryElement = XElement.Parse(query.ViewXml);

                    var viewFields = queryElement.Descendants("ViewFields").FirstOrDefault();
                    if (viewFields != null)
                    {
                        viewFields.RemoveAll();
                    }
                    else
                    {
                        viewFields = new XElement("ViewFields");
                        queryElement.Add(viewFields);
                    }

                    foreach (var field in Fields)
                    {
                        XElement viewField = new XElement("FieldRef");
                        viewField.SetAttributeValue("Name", field);
                        viewFields.Add(viewField);
                    }
                    query.ViewXml = queryElement.ToString();
                }

                if (HasPageSize())
                {
                    var queryElement = XElement.Parse(query.ViewXml);

                    var rowLimit = queryElement.Descendants("RowLimit").FirstOrDefault();
                    if (rowLimit != null)
                    {
                        rowLimit.RemoveAll();
                    }
                    else
                    {
                        rowLimit = new XElement("RowLimit");
                        queryElement.Add(rowLimit);
                    }

                    rowLimit.SetAttributeValue("Paged", "TRUE");
                    rowLimit.SetValue(PageSize);

                    query.ViewXml = queryElement.ToString();
                }

                do
                {
                    var listItems = list.GetItems(query);
                    ClientContext.Load(listItems);
                    ClientContext.ExecuteQueryRetry();
                    WriteObject(listItems, true);

                    query.ListItemCollectionPosition = listItems.ListItemCollectionPosition;
                } while (query.ListItemCollectionPosition != null);
            }
        }

        private bool HasId()
        {
            return Id != -1;
        }

        private bool HasUniqueId()
        {
            return UniqueId != null && UniqueId.Id != Guid.Empty;
        }

        private bool HasCamlQuery()
        {
            return Query != null;
        }

        private bool HasFields()
        {
            return Fields != null;
        }

        private bool HasPageSize()
        {
            return PageSize > 0;
        }
    }
}
