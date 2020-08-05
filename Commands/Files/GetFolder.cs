using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Files
{
    [Cmdlet(VerbsCommon.Get, "PnPFolder")]
    [CmdletHelp("Return a folder object", Category = CmdletHelpCategory.Files,
        DetailedDescription = "Retrieves a folder if it exists or all folders inside a provided list or library. Use Resolve-PnPFolder to create the folder if it does not exist.",
        OutputType = typeof(Folder),
        OutputTypeLink = "https://docs.microsoft.com/previous-versions/office/sharepoint-server/ee538057(v=office.15)")]
    [CmdletExample(
        Code = @"PS:> Get-PnPFolder -Url ""Shared Documents""",
        Remarks = "Returns the folder called 'Shared Documents' which is located in the root of the current web",
        SortOrder = 1
        )]
    [CmdletExample(
        Code = @"PS:> Get-PnPFolder -Url ""/sites/demo/Shared Documents""",
        Remarks = "Returns the folder called 'Shared Documents' which is located in the root of the current web",
        SortOrder = 2
        )]
    [CmdletExample(
        Code = @"PS:> Get-PnPFolder -List ""Shared Documents""",
        Remarks = "Returns the folder(s) residing inside a folder called 'Shared Documents'",
        SortOrder = 3
        )]
    [CmdletRelatedLink(
        Text = "Resolve-PnPFolder",
        Url = "https://github.com/MicrosoftDocs/office-docs-powershell/blob/master/sharepoint/sharepoint-ps/sharepoint-pnp/Resolve-PnPFolder.md")]
    public class GetFolder : PnPWebRetrievalsCmdlet<Folder>
    {
        private const string ParameterSet_FOLDERSINLIST = "Folders In List";
        private const string ParameterSet_FOLDERBYURL = "Folder By Url";

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "Site or server relative URL of the folder to retrieve. In the case of a server relative url, make sure that the url starts with the managed path as the current web.", ParameterSetName = ParameterSet_FOLDERBYURL)]
        [Alias("RelativeUrl")]
        public string Url;

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Name, ID or instance of a list or document library to retrieve the folders residing in it for.", ParameterSetName = ParameterSet_FOLDERSINLIST)]
        public ListPipeBind List;

        protected override void ExecuteCmdlet()
        {
#if !SP2013
            DefaultRetrievalExpressions = new Expression<Func<Folder, object>>[] { f => f.ServerRelativeUrl, f => f.Name, f => f.TimeLastModified, f => f.ItemCount };
#else
            DefaultRetrievalExpressions = new Expression<Func<Folder, object>>[] { f => f.ServerRelativeUrl, f => f.Name, f => f.ItemCount };
#endif
            if (List != null)
            {
                // Gets the provided list
                var list = List.GetList(SelectedWeb);

                // Query for all folders in the list
                CamlQuery query = CamlQuery.CreateAllFoldersQuery();
                do
                {
                    // Execute the query. It will retrieve all properties of the folders. Refraining to using the RetrievalExpressions would cause a tremendous increased load on SharePoint as it would have to execute a query per list item which would be less efficient, especially on lists with many folders, than just getting all properties directly
                    ListItemCollection listItems = list.GetItems(query);
                    ClientContext.Load(listItems, item => item.Include(t => t.Folder), item => item.ListItemCollectionPosition);
                    ClientContext.ExecuteQueryRetry();

                    // Take all the folders from the resulting list items and put them in a list to return
                    var folders = new List<Folder>(listItems.Count);
                    foreach(ListItem listItem in listItems)
                    {
                        var folder = listItem.Folder;
                        folders.Add(folder);
                    }

                    WriteObject(folders, true);

                    query.ListItemCollectionPosition = listItems.ListItemCollectionPosition;
                } while (query.ListItemCollectionPosition != null);                
            }
            else
            {
                var webServerRelativeUrl = SelectedWeb.EnsureProperty(w => w.ServerRelativeUrl);
                if (!Url.StartsWith(webServerRelativeUrl, StringComparison.OrdinalIgnoreCase))
                {
                    Url = UrlUtility.Combine(webServerRelativeUrl, Url);
                }
#if ONPREMISES
                var folder = SelectedWeb.GetFolderByServerRelativeUrl(Url);
#else
                var folder = SelectedWeb.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(Url));
#endif
                folder.EnsureProperties(RetrievalExpressions);

                WriteObject(folder);
            }
        }
    }
}
