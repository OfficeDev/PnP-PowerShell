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
    [Cmdlet(VerbsCommon.Get, "PnPListItem", DefaultParameterSetName = ParameterSet_ALLITEMS)]
    [CmdletHelp("Retrieves list items",
        Category = CmdletHelpCategory.Lists,
        OutputType = typeof(ListItem),
        OutputTypeLink = "https://docs.microsoft.com/previous-versions/office/sharepoint-server/ee539951(v=office.15)")]
    [CmdletExample(
        Code = "PS:> Get-PnPListItem -List Tasks",
        Remarks = "Retrieves all list items from the Tasks list",
        SortOrder = 1)]
    [CmdletExample(
        Code = "PS:> Get-PnPListItem -List Tasks -Id 1",
        Remarks = "Retrieves the list item with ID 1 from the Tasks list",
        SortOrder = 2)]
    [CmdletExample(
        Code = "PS:> Get-PnPListItem -List Tasks -UniqueId bd6c5b3b-d960-4ee7-a02c-85dc6cd78cc3",
        Remarks = "Retrieves the list item with unique id bd6c5b3b-d960-4ee7-a02c-85dc6cd78cc3 from the tasks lists",
        SortOrder = 3)]
    [CmdletExample(
        Code = "PS:> (Get-PnPListItem -List Tasks -Fields \"Title\",\"GUID\").FieldValues",
        Remarks = "Retrieves all list items, but only includes the values of the Title and GUID fields in the list item object",
        SortOrder = 4)]
    [CmdletExample(
	Code = "PS:> Get-PnPListItem -List Tasks -Query \"<View><ViewFields><FieldRef Name='Title'/><FieldRef Name='Modified'/></ViewFields><Query><Where><Geq><FieldRef Name='Modified'/><Value Type='DateTime'><Today/></Value></Eq></Where></Query></View>\""
        Remarks = "Retrieves all list items modified today, retrieving the columns 'Title' and 'Modified'. When you use -Query, you can add a <ViewFields> clause to retrieve specific columns (since you cannot use -Fields)",
        SortOrder = 5)]
    [CmdletExample(
        Code = "PS:> Get-PnPListItem -List Tasks -PageSize 1000",
        Remarks = "Retrieves all list items from the Tasks list in pages of 1000 items",
        SortOrder = 6)]
    [CmdletExample(
        Code = "PS:> Get-PnPListItem -List Tasks -PageSize 1000 -ScriptBlock { Param($items) $items.Context.ExecuteQuery() } | % { $_.BreakRoleInheritance($true, $true) }",
        Remarks = "Retrieves all list items from the Tasks list in pages of 1000 items and breaks permission inheritance on each item",
        SortOrder = 7)]
    [CmdletExample(
        Code = "PS:> Get-PnPListItem -List Samples -FolderServerRelativeUrl \"/sites/contosomarketing/Lists/Samples/Demo\"",
        Remarks = "Retrieves all list items from the Demo folder in the Samples list located in the contosomarketing site collection",
        SortOrder = 8)]
    public class GetListItem : PnPWebCmdlet
    {
        private const string ParameterSet_BYID = "By Id";
        private const string ParameterSet_BYUNIQUEID = "By Unique Id";
        private const string ParameterSet_BYQUERY = "By Query";
        private const string ParameterSet_ALLITEMS = "All Items";
        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The list to query", Position = 0, ParameterSetName = ParameterAttribute.AllParameterSets)]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "The ID of the item to retrieve", ParameterSetName = ParameterSet_BYID)]
        public int Id = -1;

        [Parameter(Mandatory = false, HelpMessage = "The unique id (GUID) of the item to retrieve", ParameterSetName = ParameterSet_BYUNIQUEID)]
        public GuidPipeBind UniqueId;

        [Parameter(Mandatory = false, HelpMessage = "The CAML query to execute against the list", ParameterSetName = ParameterSet_BYQUERY)]
        public string Query;

        [Parameter(Mandatory = false, HelpMessage = "The server relative URL of a list folder from which results will be returned.", ParameterSetName = ParameterSet_BYQUERY)]
        [Parameter(Mandatory = false, HelpMessage = "The server relative URL of a list folder from which results will be returned.", ParameterSetName = ParameterSet_ALLITEMS)]
        public string FolderServerRelativeUrl;

        [Parameter(Mandatory = false, HelpMessage = "The fields to retrieve. If not specified all fields will be loaded in the returned list object.", ParameterSetName = ParameterSet_ALLITEMS)]
        [Parameter(Mandatory = false, HelpMessage = "The fields to retrieve. If not specified all fields will be loaded in the returned list object.", ParameterSetName = ParameterSet_BYID)]
        [Parameter(Mandatory = false, HelpMessage = "The fields to retrieve. If not specified all fields will be loaded in the returned list object.", ParameterSetName = ParameterSet_BYUNIQUEID)]
        public string[] Fields;

        [Parameter(Mandatory = false, HelpMessage = "The number of items to retrieve per page request.", ParameterSetName = ParameterSet_ALLITEMS)]
		[Parameter(Mandatory = false, HelpMessage = "The number of items to retrieve per page request.", ParameterSetName = ParameterSet_BYQUERY)]
        public int PageSize = -1;

		[Parameter(Mandatory = false, HelpMessage = "The script block to run after every page request.", ParameterSetName = ParameterSet_ALLITEMS)]
		[Parameter(Mandatory = false, HelpMessage = "The script block to run after every page request.", ParameterSetName = ParameterSet_BYQUERY)]
		public ScriptBlock ScriptBlock;

		protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb);
            if (list == null)
                throw new PSArgumentException($"No list found with id, title or url '{List}'", "List");

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
                query.ViewXml = $"<View><Query><Where><Eq><FieldRef Name='GUID'/><Value Type='Guid'>{UniqueId.Id}</Value></Eq></Where></Query>{viewFieldsStringBuilder}</View>";
                var listItem = list.GetItems(query);
                ClientContext.Load(listItem);
                ClientContext.ExecuteQueryRetry();
                WriteObject(listItem);
            }
            else
            {
				CamlQuery query = HasCamlQuery() ? new CamlQuery { ViewXml = Query } : CamlQuery.CreateAllItemsQuery();
                query.FolderServerRelativeUrl = FolderServerRelativeUrl;

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

                    if (ScriptBlock != null)
                    {
						ScriptBlock.Invoke(listItems);
					}

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
