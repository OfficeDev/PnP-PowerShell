using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests.Search
{
    [TestClass]
    public class SubmitSearchQueryTests
    {
        #region Test Setup/CleanUp
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            // This runs on class level once before all tests run
            //using (var ctx = TestCommon.CreateClientContext())
            //{
            //}
        }

        [ClassCleanup]
        public static void Cleanup(TestContext testContext)
        {
            // This runs on class level once
            //using (var ctx = TestCommon.CreateClientContext())
            //{
            //}
        }

        [TestInitialize]
        public void Initialize()
        {
            using (var scope = new PSTestScope())
            {
                // Example
                // scope.ExecuteCommand("cmdlet", new CommandParameter("param1", prop));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var scope = new PSTestScope())
            {
                try
                {
                    // Do Test Setup - Note, this runs PER test
                }
                catch (Exception)
                {
                    // Describe Exception
                }
            }
        }
        #endregion

        #region Scaffolded Cmdlet Tests
        //TODO: This is a scaffold of the cmdlet - complete the unit test
        //[TestMethod]
        public void SubmitPnPSearchQueryTest()
        {
            using (var scope = new PSTestScope(true))
            {
                // Complete writing cmd parameters

				// This is a mandatory parameter
				// From Cmdlet Help: Search query in Keyword Query Language (KQL).
				var query = "";
				// From Cmdlet Help: Search result item to start returning the results from. Useful for paging. Leave at 0 to return all results.
				var startRow = "";
				// From Cmdlet Help: Maximum amount of search results to return. Default and max per page is 500 search results.
				var maxResults = "";
				// From Cmdlet Help: Automatically page results until the end to get more than 500. Use with caution!
				var all = "";
				// From Cmdlet Help: Specifies whether near duplicate items should be removed from the search results.
				var trimDuplicates = "";
				// From Cmdlet Help: Extra query properties. Can for example be used for Office Graph queries.
				var properties = "";
				// From Cmdlet Help: The list of refiners to be returned in a search result.
				var refiners = "";
				// From Cmdlet Help: The locale for the query.
				var culture = "";
				// From Cmdlet Help: Specifies the query template that is used at run time to transform the query based on user input.
				var queryTemplate = "";
				// From Cmdlet Help: The list of properties to return in the search results.
				var selectProperties = "";
				// From Cmdlet Help: The set of refinement filters used.
				var refinementFilters = "";
				// From Cmdlet Help: The list of properties by which the search results are ordered.
				var sortList = "";
				// From Cmdlet Help: The identifier (ID) of the ranking model to use for the query.
				var rankingModelId = "";
				// From Cmdlet Help: Specifies the name of the client which issued the query.
				var clientType = "";
				// From Cmdlet Help: Limit the number of items per the collapse specification. See https://docs.microsoft.com/en-us/sharepoint/dev/general-development/customizing-search-results-in-sharepoint#collapse-similar-search-results-using-the-collapsespecification-property for more information.
				var collapseSpecification = "";
				// From Cmdlet Help: The keyword query’s hidden constraints.
				var hiddenConstraints = "";
				// From Cmdlet Help: The identifier for the search query time zone.
				var timeZoneId = "";
				// From Cmdlet Help: Specifies whether the phonetic forms of the query terms are used to find matches.
				var enablePhonetic = "";
				// From Cmdlet Help: Specifies whether stemming is enabled.
				var enableStemming = "";
				// From Cmdlet Help: Specifies whether Query Rules are enabled for this query.
				var enableQueryRules = "";
				// From Cmdlet Help: Specifies the identifier (ID or name) of the result source to be used to run the query.
				var sourceId = "";
				// From Cmdlet Help: Determines whether Best Bets are enabled.
				var processBestBets = "";
				// From Cmdlet Help: Determines whether personal favorites data is processed or not.
				var processPersonalFavorites = "";
				// From Cmdlet Help: Specifies whether only relevant results are returned
				var relevantResults = "";

                var results = scope.ExecuteCommand("Submit-PnPSearchQuery",
					new CommandParameter("Query", query),
					new CommandParameter("StartRow", startRow),
					new CommandParameter("MaxResults", maxResults),
					new CommandParameter("All", all),
					new CommandParameter("TrimDuplicates", trimDuplicates),
					new CommandParameter("Properties", properties),
					new CommandParameter("Refiners", refiners),
					new CommandParameter("Culture", culture),
					new CommandParameter("QueryTemplate", queryTemplate),
					new CommandParameter("SelectProperties", selectProperties),
					new CommandParameter("RefinementFilters", refinementFilters),
					new CommandParameter("SortList", sortList),
					new CommandParameter("RankingModelId", rankingModelId),
					new CommandParameter("ClientType", clientType),
					new CommandParameter("CollapseSpecification", collapseSpecification),
					new CommandParameter("HiddenConstraints", hiddenConstraints),
					new CommandParameter("TimeZoneId", timeZoneId),
					new CommandParameter("EnablePhonetic", enablePhonetic),
					new CommandParameter("EnableStemming", enableStemming),
					new CommandParameter("EnableQueryRules", enableQueryRules),
					new CommandParameter("SourceId", sourceId),
					new CommandParameter("ProcessBestBets", processBestBets),
					new CommandParameter("ProcessPersonalFavorites", processPersonalFavorites),
					new CommandParameter("RelevantResults", relevantResults));
                
                Assert.IsNotNull(results);
            }
        }
        #endregion
    }
}
            