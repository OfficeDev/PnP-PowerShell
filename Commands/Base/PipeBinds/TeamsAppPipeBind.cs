using PnP.PowerShell.Commands.Model.Teams;
using PnP.PowerShell.Commands.Utilities.REST;
using System;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class TeamsAppPipeBind
    {
        private readonly Guid _id;
        private readonly string _stringValue;

        public TeamsAppPipeBind()

        {
            _id = Guid.Empty;
        }

        public TeamsAppPipeBind(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException(nameof(input));
            }
            if (Guid.TryParse(input, out Guid tabId))
            {
                _id = tabId;
            }
            else
            {
                _stringValue = input;
            }
        }

        public Guid Id => _id;

        public string StringValue => _stringValue;

        public TeamApp GetApp(HttpClient httpClient, string accessToken)
        {
            if (Id != Guid.Empty)
            {
                var collection = GraphHelper.GetAsync<GraphCollection<TeamApp>>(httpClient, $"v1.0/appCatalogs/teamsApps?$filter=id eq '{_id}'", accessToken).GetAwaiter().GetResult();
                if (collection != null && collection.Items.Any())
                {
                    return collection.Items.First();
                }
                else
                {
                    collection = GraphHelper.GetAsync<GraphCollection<TeamApp>>(httpClient, $"v1.0/appCatalogs/teamsApps?$filter=externalId eq '{_id}'", accessToken).GetAwaiter().GetResult();
                    if (collection != null && collection.Items.Any())
                    {
                        return collection.Items.First();
                    }
                }
            }
            else
            {
                var collection = GraphHelper.GetAsync<GraphCollection<TeamApp>>(httpClient, $"v1.0/appCatalogs/teamsApps?$filter=displayName eq '{_stringValue}'", accessToken).GetAwaiter().GetResult();
                if (collection != null && collection.Items.Any())
                {
                    if (collection.Items.Count() == 1)
                    {
                        return collection.Items.First();
                    }
                    else
                    {
                        throw new PSArgumentException("Multiple apps found with the same display name. Specify id instead to reference to the app.");
                    }
                }
            }
            return null;
        }
    }
}
