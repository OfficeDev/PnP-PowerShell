using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.SharePoint.Client.Search.Query;

namespace SharePointPnP.PowerShell.Commands.Search
{
    public class PnPResultTable
    {
        public string GroupTemplateId { get; set; }

        public string ItemTemplateId { get; set; }

        public IDictionary<string, object> Properties { get; set; }


        public string QueryId { get; set; }


        public string QueryRuleId { get; set; }

        public List<IDictionary<string, object>> ResultRows { get; set; }


        public string ResultTitle { get; set; }


        public string ResultTitleUrl { get; set; }

        public int RowCount
        {
            get
            {
                if (ResultRows != null && ResultRows.Count > 0)
                    return ResultRows.Count;
                return 0;
            }
        }

        public string TableType { get; set; }

        public int TotalRows { get; set; }

        public int TotalRowsIncludingDuplicates { get; set; }

        public string TypeId { get; set; }

        public static explicit operator PnPResultTable(ResultTable table)
        {
            if (table == null)
                throw new NoNullAllowedException();
            var pnpTable = new PnPResultTable
            {
                GroupTemplateId = table.GroupTemplateId,
                ItemTemplateId = table.ItemTemplateId,
                QueryId = table.QueryId,
                QueryRuleId = table.QueryRuleId,
                ResultTitle = table.ResultTitle,
                ResultTitleUrl = table.ResultTitleUrl,
                TableType = table.TableType,
                TotalRows = table.TotalRows,
                TotalRowsIncludingDuplicates = table.TotalRowsIncludingDuplicates,
                TypeId = table.TypeId,
                Properties = table.Properties,
                ResultRows = table.ResultRows.ToList()
            };

            return pnpTable;
        }
    }
}