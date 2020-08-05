using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using OfficeDevPnP.Core.AppModelExtensions;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace PnP.PowerShell.Commands.Provisioning.Site
{
    [Cmdlet(VerbsCommon.Add, "PnPListFoldersToProvisioningTemplate")]
    [CmdletHelp("Adds folders to a list in a PnP Provisioning Template",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Add-PnPListFoldersToProvisioningTemplate -Path template.pnp -List 'PnPTestList'",
       Remarks = "Adds top level folders from a list to an existing template and returns an in-memory PnP Site Template",
       SortOrder = 1)]
    [CmdletExample(
       Code = @"PS:> Add-PnPListFoldersToProvisioningTemplate -Path template.pnp -List 'PnPTestList' -Recursive",
       Remarks = "Adds all folders from a list to an existing template and returns an in-memory PnP Site Template",
       SortOrder = 2)]
    [CmdletExample(
       Code = @"PS:> Add-PnPListFoldersToProvisioningTemplate -Path template.pnp -List 'PnPTestList' -Recursive -IncludeSecurity",
       Remarks = "Adds all folders from a list with unique permissions to an in-memory PnP Site Template",
       SortOrder = 3)]

    public class AddListFoldersToProvisioningTemplate : PnPWebCmdlet
    {

        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Filename of the .PNP Open XML site template to read from, optionally including full path.")]
        public string Path;

        [Parameter(Mandatory = true, HelpMessage = "The list to query", Position = 2)]
        public ListPipeBind List;

        [Parameter(Mandatory = false, Position = 4, HelpMessage = "A switch parameter to include all folders in the list, or just top level folders.")]
        public SwitchParameter Recursive;

        [Parameter(Mandatory = false, Position = 5, HelpMessage = "A switch to include ObjectSecurity information.")]
        public SwitchParameter IncludeSecurity;

        [Parameter(Mandatory = false, Position = 6, HelpMessage = "Allows you to specify ITemplateProviderExtension to execute while loading the template.")]
        public ITemplateProviderExtension[] TemplateProviderExtensions;


        protected override void ExecuteCmdlet()
        {

            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }
            // Load the template
            var template = ReadProvisioningTemplate
                .LoadProvisioningTemplateFromFile(Path,
                TemplateProviderExtensions, (e) =>
                {
                    WriteError(new ErrorRecord(e, "TEMPLATENOTVALID", ErrorCategory.SyntaxError, null));
                });

            if (template == null)
            {
                throw new ApplicationException("Invalid template file!");
            }


            List spList = List.GetList(SelectedWeb);
            ClientContext.Load(spList, l => l.RootFolder, l => l.HasUniqueRoleAssignments);
            ClientContext.ExecuteQueryRetry();

            //We will remove a list if it's found so we can get the list
            ListInstance listInstance = template.Lists.Find(l => l.Title == spList.Title);
            if (listInstance == null)
            {
                throw new ApplicationException("List does not exist in the template file!");
            }


            Microsoft.SharePoint.Client.Folder listFolder = spList.RootFolder;
            ClientContext.Load(listFolder);
            ClientContext.ExecuteQueryRetry();

            IList<OfficeDevPnP.Core.Framework.Provisioning.Model.Folder> folders = GetChildFolders(listFolder);

            template.Lists.Remove(listInstance);
            listInstance.Folders.AddRange(folders);
            template.Lists.Add(listInstance);

            // Determine the output file name and path
            var outFileName = System.IO.Path.GetFileName(Path);
            var outPath = new FileInfo(Path).DirectoryName;

            var fileSystemConnector = new FileSystemConnector(outPath, "");
            var formatter = XMLPnPSchemaFormatter.LatestFormatter;
            var extension = new FileInfo(Path).Extension.ToLowerInvariant();
            if (extension == ".pnp")
            {
                XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(new OpenXMLConnector(outPath, fileSystemConnector));
                var templateFileName = outFileName.Substring(0, outFileName.LastIndexOf(".", StringComparison.Ordinal)) + ".xml";

                provider.SaveAs(template, templateFileName, formatter, TemplateProviderExtensions);
            }
            else
            {
                XMLTemplateProvider provider = new XMLFileSystemTemplateProvider(Path, "");
                provider.SaveAs(template, Path, formatter, TemplateProviderExtensions);
            }
        }

        private IList<OfficeDevPnP.Core.Framework.Provisioning.Model.Folder> GetChildFolders(Microsoft.SharePoint.Client.Folder listFolder)
        {
            List<OfficeDevPnP.Core.Framework.Provisioning.Model.Folder> retFolders = new List<OfficeDevPnP.Core.Framework.Provisioning.Model.Folder>();
            ClientContext.Load(listFolder, l => l.Name, l => l.Folders);
            ClientContext.ExecuteQueryRetry();
            var folders = listFolder.Folders;
            ClientContext.Load(folders, fl => fl.Include(f => f.Name, f => f.ServerRelativeUrl, f => f.ListItemAllFields));
            ClientContext.ExecuteQueryRetry();
            foreach (var folder in folders)
            {
                if (folder.ListItemAllFields.ServerObjectIsNull != null && !folder.ListItemAllFields.ServerObjectIsNull.Value)
                {
                    var retFolder = GetFolder(folder);
                    retFolders.Add(retFolder);
                }
            }
            return retFolders;
        }

        private OfficeDevPnP.Core.Framework.Provisioning.Model.Folder GetFolder(Microsoft.SharePoint.Client.Folder listFolder)
        {
            ListItem folderItem = listFolder.ListItemAllFields;
            ClientContext.Load(folderItem, fI => fI.HasUniqueRoleAssignments);
            ClientContext.Load(listFolder, l => l.Name, l => l.Folders);
            ClientContext.ExecuteQueryRetry();

            OfficeDevPnP.Core.Framework.Provisioning.Model.Folder retFolder = new OfficeDevPnP.Core.Framework.Provisioning.Model.Folder();
            retFolder.Name = listFolder.Name;

            if (Recursive)
            {
                foreach (var folder in listFolder.Folders)
                {
                    var childFolder = GetFolder(folder);
                    retFolder.Folders.Add(childFolder);
                }
            }
            if (IncludeSecurity && folderItem.HasUniqueRoleAssignments)
            {
                var RoleAssignments = folderItem.RoleAssignments;
                ClientContext.Load(RoleAssignments);
                ClientContext.ExecuteQueryRetry();

                retFolder.Security.ClearSubscopes = true;
                retFolder.Security.CopyRoleAssignments = false;

                ClientContext.Load(RoleAssignments, r => r.Include(a => a.Member.LoginName, a => a.Member, a => a.RoleDefinitionBindings));
                ClientContext.ExecuteQueryRetry();

                foreach (var roleAssignment in RoleAssignments)
                {
                    var principalName = roleAssignment.Member.LoginName;
                    var roleBindings = roleAssignment.RoleDefinitionBindings;
                    foreach (var roleBinding in roleBindings)
                    {
                        retFolder.Security.RoleAssignments.Add(new OfficeDevPnP.Core.Framework.Provisioning.Model.RoleAssignment() { Principal = principalName, RoleDefinition = roleBinding.Name });
                    }
                }
            }

            return retFolder;
        }



    }
}
