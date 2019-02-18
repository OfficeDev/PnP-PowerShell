#if !SP2013
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.Commands.Extensions;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Diagnostic
{
    [Cmdlet(VerbsDiagnostic.Measure, "PnPWeb")]
    [CmdletHelp("Returns statistics on the web object",
        SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2016 | CmdletSupportedPlatform.SP2019,
        Category = CmdletHelpCategory.Diagnostic)]
    [CmdletExample(
        Code = @"PS:> Measure-PnPWeb",
        Remarks = @"Gets statistics on the current web",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Measure-PnPWeb $web -Recursive",
        Remarks = @"Gets statistics on the provided web including all its subwebs",
        SortOrder = 2)]

    public class MeasurePnPWeb : PnPCmdlet
    {
        private Expression<Func<Web, object>>[] _retrievalExpressions;

        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0)]
        public WebPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Iterate all sub webs recursively")]
        public SwitchParameter Recursive;

        [Parameter(Mandatory = false, HelpMessage = "Include hidden lists in statistics calculation")]
        public SwitchParameter IncludeHiddenList;

        public MeasurePnPWeb()
        {
            _retrievalExpressions = new Expression<Func<Web, object>>[] {
                w => w.Url,
                w => w.Webs.Include(sw => sw.HasUniqueRoleAssignments),
                w => w.Lists.Include(l => l.Title, l => l.ItemCount, l => l.HasUniqueRoleAssignments, l => l.Hidden),
                w => w.SiteUsers.Include(u => u.Id),
                w => w.SiteGroups.Include(g => g.Id),
                w => w.Folders
            };
        }

        protected override void ExecuteCmdlet()
        {
            var web = GetWebByIdentity(_retrievalExpressions);
            var statistics = GetStatistics(web);
            WriteObject(statistics);
        }

        private void WriteProgress(ProgressRecord record, string message, int step, int count)
        {
            var percentage = Convert.ToInt32((100 / Convert.ToDouble(count)) * Convert.ToDouble(step));
            record.StatusDescription = message;
            record.PercentComplete = percentage;
            record.RecordType = ProgressRecordType.Processing;
            WriteProgress(record);
        }

        private FolderStatistics GetFolderStatistics(Folder folder)
        {
            FolderStatistics stat = null;
            try
            {
                folder.EnsureProperties(
                    f => f.Name,
                    f => f.Files.Include(fl => fl.Length, fl => fl.Versions.Include(v => v.Length)),
                    f => f.Folders.Include(fl => fl.Name));
                stat = new FolderStatistics
                {
                    ItemCount = folder.Files.Count,
                    TotalFileSize = folder.Files.Sum(fl => fl.Length + ((fl.Versions != null) ? fl.Versions.Sum(v => v.Length) : 0))
                };
                foreach (var subfolder in folder.Folders)
                {
                    stat += GetFolderStatistics(subfolder);
                }
            }
            catch (Exception e)
            {
                WriteWarning($"Cannot inspect folder: {e.Message}");
            }

            return stat;
        }

        private WebStatistics GetStatistics(Web web)
        {
            var progress = new ProgressRecord(0, $"Getting statistics for {web.Url}", "Retrieving web data");

            WriteProgress(progress, $"Retrieving web data", 0, web.Folders.Count);

            var lists = IncludeHiddenList ? web.Lists : web.Lists.Where(l => !l.Hidden);
            var uniqueLists = lists.Where(l => l.HasUniqueRoleAssignments);

            var stat = new WebStatistics
            {
                WebCount = web.Webs.Count,
                ListCount = lists.Count(),
                ItemCount = lists.Sum(l => l.ItemCount),
                SiteUserCount = web.SiteUsers.Count,
                SiteGroupCount = web.SiteGroups.Count,
                BrokenPermissionCount = uniqueLists.Count() + web.Webs.Count(w => w.HasUniqueRoleAssignments),
            };

            var i = 0;
            foreach (var folder in web.Folders)
            {
                stat += GetFolderStatistics(folder);
                WriteProgress(progress, $"Retrieving folder data {folder.Name}", i++, web.Folders.Count);
            }

            foreach (var list in uniqueLists)
            {
                WriteVerbose($"List {list.Title} has unique permissions");
            }

            if (Recursive)
            {
                foreach (var subweb in web.Webs)
                {
                    subweb.EnsureProperties(_retrievalExpressions);
                    stat += GetStatistics(subweb);
                }
            }

            progress.RecordType = ProgressRecordType.Completed;
            WriteProgress(progress);

            return stat;
        }

        private Web GetWebByIdentity(Expression<Func<Web, object>>[] expressions)
        {
            Web web = null;
            if (Identity == null)
            {
                ClientContext.Web.EnsureProperties(expressions);
                web = ClientContext.Web;
            }
            else if (Identity.Id != Guid.Empty)
            {
                web = ClientContext.Web.GetWebById(Identity.Id, expressions);
            }
            else if (Identity.Web != null)
            {
                web = ClientContext.Web.GetWebById(Identity.Web.Id, expressions);
            }
            else if (Identity.Url != null)
            {
                web = ClientContext.Web.GetWebByUrl(Identity.Url, expressions);
            }
            return web;
        }
    }
}
#endif