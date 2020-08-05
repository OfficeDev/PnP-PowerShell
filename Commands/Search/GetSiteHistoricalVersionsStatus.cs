#if !ONPREMISES
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Administration;
using PnP.PowerShell.CmdletHelpAttributes;

namespace PnP.PowerShell.Commands.Search
{
    [Cmdlet(VerbsCommon.Get, "PnPSiteHistoricalVersionsStatus", DefaultParameterSetName = "Xml")]
    [CmdletHelp("Returns information about the Historical Versions feature for the current site collection from the context. " +
       "The document statistics are only updated periodically, check the next update time property (in UTC) to see when new data will be available.",
        SupportedPlatform = CmdletSupportedPlatform.Online,
        Category = CmdletHelpCategory.Search)]
    [CmdletExample(
        Code = @"PS:> Get-PnPSiteHistoricalVersionsStatus",
        Remarks = "Returns the status of the feature as well as the number of documents processed for the site if the feature is enabled.",
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
