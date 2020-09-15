#if !ONPREMISES
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Administration;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Search
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteHistoricalVersionsStatus")]
    [CmdletHelp("Returns summary crawl info about the Historical Versions feature for the current site collection from the context. " +
        "This is a feature that makes past versions of documents searchable for eDiscovery when enabled.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.Search)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteHistoricalVersionsStatus",
        Remarks = "Returns the count of documents with historical versions processed and the count of total documents with versions enabled on the site, as well as when these counts will be next updated (all times in UTC).",
        SortOrder = 1)]
    public class GetSiteHistoricalVersionsStatus : PnPWebCmdlet
    {

        protected override void ExecuteCmdlet()
        {
            var siteLog = new SiteCrawlLog(ClientContext, ClientContext.Site);
            ClientContext.Load(siteLog);

            var resultTable = siteLog.GetHistoricalVersionsStatus();
            ClientContext.ExecuteQueryRetry();

            if (resultTable.Value == null || resultTable.Value.Rows.Count == 0)
            {
                WriteWarning("No information was obtained for the current site");
            }
            else
            {
                // The API should only return 1 row
                WriteObject(ConvertToPSObject(resultTable.Value.Rows[0]));
            }

        }

        private object ConvertToPSObject(IDictionary<string, object> r)
        {
            PSObject res = new PSObject();
            if (r != null)
            {
                foreach (var kvp in r)
                {
                    res.Properties.Add(new PSNoteProperty(kvp.Key, kvp.Value));
                }
            }
            return res;
        }
    }
}
#endif
