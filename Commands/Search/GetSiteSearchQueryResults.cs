using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Query;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System.Collections.Generic;
using System.Linq;

namespace SharePointPnP.PowerShell.Commands.Search
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteSearchQueryResults", DefaultParameterSetName = "Limit")]
    [CmdletAlias("Get-SPOSiteSearchQueryResults")]
    [CmdletHelp("Executes a search query to retrieve indexed site collections",
        Category = CmdletHelpCategory.Search,
        OutputType = typeof(List<dynamic>))]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteSearchQueryResults",
        Remarks = "Returns the top 500 site collections indexed by SharePoint Search",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteSearchQueryResults -Query ""WebTemplate:STS""",
        Remarks = "Returns the top 500 site collections indexed by SharePoint Search which have are based on the STS (Team Site) template",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteSearchQueryResults -Query ""WebTemplate:SPSPERS""",
        Remarks = "Returns the top 500 site collections indexed by SharePoint Search which have are based on the SPSPERS (MySite) template, up to the MaxResult limit",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteSearchQueryResults -Query ""Title:Intranet*""",
        Remarks = "Returns the top 500 site collections indexed by SharePoint Search of which the title starts with the word Intranet",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteSearchQueryResults -MaxResults 10",
        Remarks = "Returns the top 10 site collections indexed by SharePoint Search",
        SortOrder = 5)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteSearchQueryResults -All",
        Remarks = "Returns absolutely all site collections indexed by SharePoint Search",
        SortOrder = 6)]
    public class GetSiteSearchQueryResults : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "Search query in Keyword Query Language (KQL) to execute to refine the returned sites. If omitted, all indexed sites will be returned.", ParameterSetName = ParameterAttribute.AllParameterSets)]
        public string Query = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Search result item to start returning the results from. Useful for paging. Leave at 0 to return all results.", ParameterSetName = "Limit")]
        public int StartRow = 0;

        [Parameter(Mandatory = false, HelpMessage = "Maximum amount of search results to return. Default and max is 500 search results.", ParameterSetName = "Limit")]
        [ValidateRange(0, 500)]
        public int MaxResults = 500;

        [Parameter(Mandatory = false, HelpMessage = "Automatically page results until the end to get more than 500 sites. Use with caution!", ParameterSetName = "All")]
        public SwitchParameter All;

        protected override void ExecuteCmdlet()
        {
            int startRow = StartRow;
            int rowLimit = MaxResults;
            if (All.IsPresent)
            {
                startRow = 0;
                rowLimit = 500;
            }
            int currentCount = 0;
            var dynamicList = new List<dynamic>();
            do
            {
                var keywordQuery = GetKeywordQuery();
                keywordQuery.StartRow = startRow;
                keywordQuery.RowLimit = rowLimit;

                var searchExec = new SearchExecutor(ClientContext);
                var results = searchExec.ExecuteQuery(keywordQuery);
                ClientContext.ExecuteQueryRetry();

                if (results?.Value[0].RowCount > 0)
                {
                    var result = results.Value[0];
                    currentCount = result.ResultRows.Count();

                    foreach (var row in result.ResultRows)
                    {
                        dynamicList.Add(
                            new
                            {
                                Title = row["Title"]?.ToString() ?? "",
                                Url = row["SPSiteUrl"]?.ToString() ?? "",
                                Description = row["Description"]?.ToString() ?? "",
                                WebTemplate = row["WebTemplate"]?.ToString() ?? ""
                            });
                    }
                }
                startRow += rowLimit;

            } while (currentCount == rowLimit && All.IsPresent);
            WriteObject(dynamicList, true);
        }

        private KeywordQuery GetKeywordQuery()
        {
            var keywordQuery = new KeywordQuery(ClientContext);

            // Construct query to execute
            var query = "contentclass:STS_Site";
            if (!string.IsNullOrEmpty(Query))
            {
                query = query + " AND " + Query;
            }

            keywordQuery.QueryText = query;
            keywordQuery.SelectProperties.Add("Title");
            keywordQuery.SelectProperties.Add("SPSiteUrl");
            keywordQuery.SelectProperties.Add("Description");
            keywordQuery.SelectProperties.Add("WebTemplate");
            keywordQuery.SortList.Add("SPSiteUrl", SortDirection.Ascending);
            // Important to avoid trimming "similar" site collections
            keywordQuery.TrimDuplicates = false;
            return keywordQuery;
        }
    }
}
