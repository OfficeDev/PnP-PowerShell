#if !SP2013
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.Commands.Extensions;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Collections.Generic;

namespace SharePointPnP.PowerShell.Commands.Diagnostic
{
    [Cmdlet(VerbsDiagnostic.Measure, "PnPList")]
    [CmdletHelp("Returns statistics on the list object. This may fail on lists larger than the list view threshold",
        Category = CmdletHelpCategory.Diagnostic,
        SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2016 | CmdletSupportedPlatform.SP2019)]
    [CmdletExample(
        Code = @"PS:> Measure-PnPList ""Documents""",
        Remarks = @"Gets statistics on Documents document library",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Measure-PnPList ""Documents"" -BrokenPermissions -ItemLevel",
        Remarks = @"Displays items and folders with broken permissions inside Documents library",
        SortOrder = 2)]
    
    public class MeasurePnPList : PnPWebRetrievalsCmdlet<List>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
        public ListPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Show item level statistics")]
        public SwitchParameter ItemLevel;

        [Parameter(Mandatory = false, HelpMessage = "Show items with broken permissions")]
        public SwitchParameter BrokenPermissions;

        protected override void ExecuteCmdlet()
        {
            var list = Identity.GetList(SelectedWeb);
            if (list == null)
                throw new PSArgumentException($"No list found with id, title or url '{Identity}'", "Identity");
            var retrievalExpressions = new Expression<Func<List, object>>[] {
                l => l.ItemCount,
                l => l.HasUniqueRoleAssignments,
                l => l.RootFolder.Folders.Include(f => f.Name)
            };
            list.EnsureProperties(retrievalExpressions);

            if (BrokenPermissions)
            {
                List<string> permissions = GetBrokenPermissions(list.RootFolder, list);
                if(list.HasUniqueRoleAssignments)
                {
                    list.RootFolder.EnsureProperty(f => f.ServerRelativeUrl);
                    permissions.Insert(0, list.RootFolder.ServerRelativeUrl);
                }
                WriteObject(permissions, true);
            }
            else
            {
                var statistics = new ListStatistics
                {
                    ItemCount = list.ItemCount,
                    FolderCount = 1,
                    ItemVersionCount = 0,
                    TotalFileSize = 0
                };
                statistics += GetFolderStatistics(list.RootFolder, list);
                WriteObject(statistics);
            }
        }

        private List<string> GetBrokenPermissions(Folder folder, List list)
        {
            List<string> res = new List<string>();
            folder.EnsureProperties(
                f => f.ListItemAllFields,
                f => f.ServerRelativeUrl,
                f => f.ItemCount,
                f => f.Folders.Include(fl => fl.Name));

            if (!folder.ListItemAllFields.ServerObjectIsNull())
            {
                folder.ListItemAllFields.EnsureProperty(i => i.HasUniqueRoleAssignments);
                if(folder.ListItemAllFields.HasUniqueRoleAssignments)
                {
                    res.Add(folder.ServerRelativeUrl);
                }
            }

            if (ItemLevel && (folder.ItemCount > 0))
            {
                var items = GetItemsInFolder(folder, list);
                var brokenItems = items?.Where(i => i.HasUniqueRoleAssignments).Select(i => $"{folder.ServerRelativeUrl}:{i.Id}");
                if(brokenItems != null && brokenItems.Count() > 0)
                {
                    res.AddRange(brokenItems);
                }
            }

            foreach (var subfolder in folder.Folders)
            {
                res.AddRange(GetBrokenPermissions(subfolder, list));
            }

            return res;
        }

        private FolderStatistics GetFolderStatistics(Folder folder, List list)
        {
            folder.EnsureProperties(
                f => f.ServerRelativeUrl,
                f => f.ListItemAllFields,
                f => f.ItemCount,
                f => f.Files.Include(fl => fl.Length),
                f => f.Files.Include(fl => fl.Versions),
                f => f.Folders.Include(fl => fl.Name));

            var stat = new FolderStatistics
            {
                ItemCount = folder.ItemCount,
                TotalFileSize = folder.Files.Sum(fl => fl.Length + fl.Versions.Sum(v => v.Length)),
                FolderCount = folder.Folders.Count,
                ItemVersionCount = folder.Files.Sum(fl => fl.Versions.Count),
            };

            if(!folder.ListItemAllFields.ServerObjectIsNull())
            {
                folder.ListItemAllFields.EnsureProperty(i => i.HasUniqueRoleAssignments);
                if (folder.ListItemAllFields.HasUniqueRoleAssignments)
                {
                    stat.BrokenPermissionCount++;
                    WriteVerbose($"Folder ${folder.ServerRelativeUrl} has unique permissions");
                }
            }

            if (ItemLevel && (stat.ItemCount > 0))
            {
                var items = GetItemsInFolder(folder, list);
                stat.BrokenPermissionCount += items.Count(i => i.HasUniqueRoleAssignments);
            }

            foreach (var subfolder in folder.Folders)
            {
                stat += GetFolderStatistics(subfolder, list);
            }
            return stat;
        }

        private IEnumerable<ListItem> GetItemsInFolder(Folder folder, List list)
        {
            folder.EnsureProperties(f => f.ServerRelativeUrl);
            var query = CamlQuery.CreateAllItemsQuery();
            query.FolderServerRelativeUrl = folder.ServerRelativeUrl;

            query.ViewXml = "<View><Query><Where><Eq><FieldRef Name='FSObjType' /><Value Type='int'>0</Value></Eq></Where></Query></View>";
            var items = list.GetItems(query);
            ClientContext.Load(items, 
                it => it.Include(
                    i => i.Id, 
                    i => i.File.Length,
                    i => i.File.Versions,
                    i => i.File,
                    i => i.HasUniqueRoleAssignments));
            ClientContext.ExecuteQueryRetry();
            return items;
        }

        private void WriteProgress(ProgressRecord record, string message, int step, int count)
        {
            var percentage = Convert.ToInt32((100 / Convert.ToDouble(count)) * Convert.ToDouble(step));
            record.StatusDescription = message;
            record.PercentComplete = percentage;
            record.RecordType = ProgressRecordType.Processing;
            WriteProgress(record);
        }
    }
}
#endif
