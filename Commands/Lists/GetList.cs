using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Get, "PnPList")]
    [CmdletAlias("Get-SPOList")]
    [CmdletHelp("Returns a List object",
        Category = CmdletHelpCategory.Lists,
        OutputType = typeof(List),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.list.aspx")]
    [CmdletExample(
        Code = "PS:> Get-PnPList",
        Remarks = "Returns all lists in the current web",
        SortOrder = 1)]
    [CmdletExample(
        Code = "PS:> Get-PnPList -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe",
        Remarks = "Returns a list with the given id.",
        SortOrder = 2)]
    [CmdletExample(
        Code = "PS:> Get-PnPList -Identity Lists/Announcements",
        Remarks = "Returns a list with the given url.",
        SortOrder = 3)]
    public class GetList : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, name or Url (Lists/MyList) of the list.")]
        public ListPipeBind Identity;

        [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 1, HelpMessage = "Additional Properties to retrieve for the List item(s) to be returned")]
        [ValidateSet("ContentTypes", "DataSource", "EventReceivers", "Fields", "SchemaXml")]
        public string[] Includes;

        protected override void ExecuteCmdlet()
        {
            if (Identity != null)
            {
                var list = Identity.GetList(SelectedWeb);

                if (Includes.Length > 0)
                {
                    foreach (var include in Includes)
                    {
                        switch (include)
                        {
                            case "ContentTypes":
                                ClientContext.Load(list, l => l.ContentTypes);
                                break;
                            case "DataSource":
                                ClientContext.Load(list, l => l.DataSource);
                                break;
                            case "EventReceivers":
                                ClientContext.Load(list, l => l.EventReceivers);
                                break;
                            case "Fields":
                                ClientContext.Load(list, l => l.Fields);
                                break;
                            case "SchemaXml":
                                ClientContext.Load(list, l => l.SchemaXml);
                                break;
                            default: break;
                        }
                    }
                    ClientContext.ExecuteQueryRetry();
                }


                WriteObject(list);

            }
            else
            {

                var query = (SelectedWeb.Lists.IncludeWithDefaultProperties(l => l.Id, l => l.BaseTemplate, l => l.OnQuickLaunch, l => l.DefaultViewUrl, l => l.Title, l => l.Hidden, l => l.RootFolder.ServerRelativeUrl));

                //var lists = ClientContext.LoadQuery((SelectedWeb.Lists.IncludeWithDefaultProperties(l => l.Id, l => l.BaseTemplate, l => l.OnQuickLaunch, l => l.DefaultViewUrl, l => l.Title, l => l.Hidden, l => l.RootFolder.ServerRelativeUrl));

                if (Includes.Length > 0)
                {
                    foreach (var include in Includes)
                    {
                        switch (include)
                        {
                            case "ContentTypes":
                                query.Include(l => l.ContentTypes);
                                break;
                            case "DataSource":
                                query.Include(l => l.DataSource);
                                break;
                            case "EventReceivers":
                                query.Include(l => l.EventReceivers);

                                break;
                            case "Fields":
                                query.Include(l => l.Fields);
                                break;
                            case "SchemaXml":
                                query.Include(l => l.SchemaXml);

                                break;
                            default: break;
                        }
                    }
                    ClientContext.ExecuteQueryRetry();
                }
                var lists = ClientContext.LoadQuery(query);
                ClientContext.ExecuteQueryRetry();
                WriteObject(lists, true);
            }
        }
    }

}