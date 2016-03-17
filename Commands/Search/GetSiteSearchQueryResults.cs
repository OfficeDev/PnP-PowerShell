using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Query;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using System.Collections.Generic;

namespace OfficeDevPnP.PowerShell.Commands.Search
{
    [Cmdlet(VerbsCommon.Get, "SPOSiteSearchQueryResults")]
    [CmdletHelp("Executes a search query to retrieve indexed site collections",
        Category = CmdletHelpCategory.Search)]
    [CmdletExample(
        Code = @"PS:> Get-SPOSiteSearchQueryResults",
        Remarks = "Returns all site collections indexed by SharePoint Search",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-SPOSiteSearchQueryResults -Query ""WebTemplate:STS""",
        Remarks = "Returns all site collections indexed by SharePoint Search which have are based on the STS (Team Site) template",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-SPOSiteSearchQueryResults -Query ""WebTemplate:SPSPERS""",
        Remarks = "Returns all site collections indexed by SharePoint Search which have are based on the SPSPERS (MySite) template",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-SPOSiteSearchQueryResults -Query ""Title:Intranet*""",
        Remarks = "Returns all site collections indexed by SharePoint Search of which the title starts with the word Intranet",
        SortOrder = 4)]
    public class GetSiteSearchQueryResults : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "Search query in Keyword Query Language (KQL) to execute to refine the returned sites. If omited, all indexed sites will be returned.")]
        public string Query = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Search result item to start returning the results from. Useful for paging. Leave at 0 to return all results.")]
        public int StartRow = 0;

        [Parameter(Mandatory = false, HelpMessage = "Maximum amount of search results to return. Default is 500 search results.")]
        public int MaxResults = 500;

        protected override void ExecuteCmdlet()
        {
            var keywordQuery = new KeywordQuery(ClientContext);

            // Construct query to execute
            var query = "contentclass:STS_Site";
            if(!string.IsNullOrEmpty(Query))
            {
                query = query + " AND " + Query;
            }

            keywordQuery.QueryText = query;
            keywordQuery.RowLimit = MaxResults;
            keywordQuery.StartRow = StartRow;
            keywordQuery.SelectProperties.Add("Title");
            keywordQuery.SelectProperties.Add("SPSiteUrl");
            keywordQuery.SelectProperties.Add("Description");
            keywordQuery.SelectProperties.Add("WebTemplate");
            keywordQuery.SortList.Add("SPSiteUrl", SortDirection.Ascending);
            SearchExecutor searchExec = new SearchExecutor(ClientContext);

            // Important to avoid trimming "similar" site collections
            keywordQuery.TrimDuplicates = false;

            ClientResult<ResultTableCollection> results = searchExec.ExecuteQuery(keywordQuery);
            ClientContext.ExecuteQueryRetry();

            var dynamicList = new List<dynamic>();

            if (results != null)
            {
                if (results.Value[0].RowCount > 0)
                {
                    foreach (var row in results.Value[0].ResultRows)
                    {
                        dynamicList.Add(
                            new {
                                Title = row["Title"] != null ? row["Title"].ToString() : "",
                                Url = row["SPSiteUrl"] != null ? row["SPSiteUrl"].ToString() : "",
                                Description = row["Description"] != null ? row["Description"].ToString() : "",
                                WebTemplate = row["WebTemplate"] != null ? row["WebTemplate"].ToString() : ""
                            });
                    }
                }
            }

            WriteObject(dynamicList, true);
        }
    }
}
