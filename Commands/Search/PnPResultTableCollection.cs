using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Office.Server.Search.WebControls;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Query;

namespace PnP.PowerShell.Commands.Search
{

    public class PnPResultTableCollection : ClientValueObjectCollection<PnPResultTable>
    {
        public int ElapsedTime;
        public IDictionary<string, object> Properties;
        public IDictionary<string, ControlMessage> QueryErrors;
        public string QueryId;
        public string SpellingSuggestion;
        public IList<Guid> TriggeredRules;

        public static explicit operator PnPResultTableCollection(ResultTableCollection collection)
        {
            if (collection == null)
                throw new NoNullAllowedException();
            var pnpTableCollection = new PnPResultTableCollection
            {
                ElapsedTime = collection.ElapsedTime,
                QueryErrors = collection.QueryErrors,
                QueryId = collection.QueryId,
                SpellingSuggestion = collection.SpellingSuggestion,
                TriggeredRules = collection.TriggeredRules
            };

            foreach (ResultTable resultTable in collection)
            {
                pnpTableCollection.Add((PnPResultTable)resultTable);
            }
            pnpTableCollection.Properties = collection.Properties;
            return pnpTableCollection;
        }


        public override string TypeId => "{11f20d08-7f42-49c1-8c0c-8ee4c32b203e}";
    }
}
