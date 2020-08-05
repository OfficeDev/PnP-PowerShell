using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using PnP.PowerShell.Commands.Enums;
using PnP.PowerShell.Commands.Utilities;

// IMPORTANT: If you make changes to this cmdlet, also make the similar/same changes to the Set-PnPListItem Cmdlet

namespace PnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Add, "PnPListItem")]
    [CmdletHelp("Adds an item to a list",
        Description = "Adds an item to the list and sets the creation time to the current date and time. The author is set to the current authenticated user executing the cmdlet. In order to set the author to a different user, please refer to Set-PnPListItem.",
        Category = CmdletHelpCategory.Lists,
        OutputType = typeof(ListItem),
        OutputTypeLink = "https://docs.microsoft.com/en-us/previous-versions/office/sharepoint-server/ee539951(v=office.15)")]
    [CmdletExample(
        Code = @"Add-PnPListItem -List ""Demo List"" -Values @{""Title"" = ""Test Title""; ""Category""=""Test Category""}",
        Remarks = @"Adds a new list item to the ""Demo List"", and sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"Add-PnPListItem -List ""Demo List"" -ContentType ""Company"" -Values @{""Title"" = ""Test Title""; ""Category""=""Test Category""}",
        Remarks = @"Adds a new list item to the ""Demo List"", sets the content type to ""Company"" and sets both the Title and Category fields with the specified values. Notice, use the internal names of fields.",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"Add-PnPListItem -List ""Demo List"" -Values @{""MultiUserField""=""user1@domain.com"",""user2@domain.com""}",
        Remarks = @"Adds a new list item to the ""Demo List"" and sets the user field called MultiUserField to 2 users. Separate multiple users with a comma.",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"Add-PnPListItem -List ""Demo List"" -Values @{""Title""=""Sales Report""} -Folder ""projects/europe""",
        Remarks = @"Adds a new list item to the ""Demo List"". It will add the list item to the europe folder which is located in the projects folder. Folders will be created if needed.",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"Add-PnPListItem -List ""Demo List"" -Values @{""Title""=""Sales Report""} -Label ""Public""",
        Remarks = @"Adds a new list item to the ""Demo List"". Sets the retention label to ""Public"" if it exists on the site.",
        SortOrder = 4)]
    public class AddListItem : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, Title or Url of the list.")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "Specify either the name, ID or an actual content type.")]
        public ContentTypePipeBind ContentType;

        [Parameter(Mandatory = false, HelpMessage = "Use the internal names of the fields when specifying field names." +
                                                    "\n\nSingle line of text: -Values @{\"Title\" = \"Title New\"}" +
                                                    "\n\nMultiple lines of text: -Values @{\"MultiText\" = \"New text\\n\\nMore text\"}" +
                                                    "\n\nRich text: -Values @{\"MultiText\" = \"<strong>New</strong> text\"}" +
            "\n\nChoice: -Values @{\"Choice\" = \"Value 1\"}" +
            "\n\nNumber: -Values @{\"Number\" = \"10\"}" +
            "\n\nCurrency: -Values @{\"Number\" = \"10\"}" +
            "\n\nCurrency: -Values @{\"Currency\" = \"10\"}" +
            "\n\nDate and Time: -Values @{\"DateAndTime\" = \"03/13/2015 14:16\"}" +
            "\n\nLookup (id of lookup value): -Values @{\"Lookup\" = \"2\"}" +
            "\n\nMulti value lookup (id of lookup values as array 1): -Values @{\"MultiLookupField\" = \"1\",\"2\"}" +
            "\n\nMulti value lookup (id of lookup values as array 2): -Values @{\"MultiLookupField\" = 1,2}" +
            "\n\nMulti value lookup (id of lookup values as string): -Values @{\"MultiLookupField\" = \"1,2\"}" +
            "\n\nYes/No: -Values @{\"YesNo\" = $false}" +
            "\n\nPerson/Group (id of user/group in Site User Info List or email of the user, separate multiple values with a comma): -Values @{\"Person\" = \"user1@domain.com\",\"21\"}" +
            "\n\nManaged Metadata (single value with path to term): -Values @{\"MetadataField\" = \"CORPORATE|DEPARTMENTS|FINANCE\"}" +
            "\n\nManaged Metadata (single value with id of term): -Values @{\"MetadataField\" = \"fe40a95b-2144-4fa2-b82a-0b3d0299d818\"} with Id of term" +
            "\n\nManaged Metadata (multiple values with paths to terms): -Values @{\"MetadataField\" = \"CORPORATE|DEPARTMENTS|FINANCE\",\"CORPORATE|DEPARTMENTS|HR\"}" +
            "\n\nManaged Metadata (multiple values with ids of terms): -Values @{\"MetadataField\" = \"fe40a95b-2144-4fa2-b82a-0b3d0299d818\",\"52d88107-c2a8-4bf0-adfa-04bc2305b593\"}" +
            "\n\nHyperlink or Picture: -Values @{\"Hyperlink\" = \"https://github.com/OfficeDev/, OfficePnp\"}")]
        public Hashtable Values;

        [Parameter(Mandatory = false, HelpMessage = @"The list relative URL of a folder. E.g. ""MyFolder"" for a folder located in the root of the list, or ""MyFolder/SubFolder"" for a folder located in the MyFolder folder which is located in the root of the list.")]
        public string Folder;

#if !ONPREMISES
        [Parameter(Mandatory = false, HelpMessage = "The name of the retention label.")]
        public String Label;
#endif

        protected override void ExecuteCmdlet()
        {
            List list = null;
            if (List != null)
            {
                list = List.GetList(SelectedWeb);
            }
            if (list != null)
            {
                ListItemCreationInformation liCI = new ListItemCreationInformation();
                if (Folder != null)
                {
                    // Create the folder if it doesn't exist
                    var rootFolder = list.EnsureProperty(l => l.RootFolder);
                    var targetFolder =
                        SelectedWeb.EnsureFolder(rootFolder, Folder);

                    liCI.FolderUrl = targetFolder.ServerRelativeUrl;
                }
                var item = list.AddItem(liCI);

                if (ContentType != null)
                {
                    ContentType ct = null;
                    if (ContentType.ContentType == null)
                    {
                        if (ContentType.Id != null)
                        {
                            ct = SelectedWeb.GetContentTypeById(ContentType.Id, true);
                        }
                        else if (ContentType.Name != null)
                        {
                            ct = SelectedWeb.GetContentTypeByName(ContentType.Name, true);
                        }
                    }
                    else
                    {
                        ct = ContentType.ContentType;
                    }
                    if (ct != null)
                    {
                        ct.EnsureProperty(w => w.StringId);

                        item["ContentTypeId"] = ct.StringId;
                        item.Update();
                        ClientContext.ExecuteQueryRetry();
                    }
                }

                if (Values != null)
                {
                    item = ListItemHelper.UpdateListItem(item, Values, ListItemUpdateType.Update,
                        (warning) =>
                        {
                            WriteWarning(warning);
                        },
                        (terminatingErrorMessage, terminatingErrorCode) =>
                        {
                            ThrowTerminatingError(new ErrorRecord(new Exception(terminatingErrorMessage), terminatingErrorCode, ErrorCategory.InvalidData, this));
                        });
                }

#if !ONPREMISES
                if (!String.IsNullOrEmpty(Label))
                {
                    IList<Microsoft.SharePoint.Client.CompliancePolicy.ComplianceTag> tags = Microsoft.SharePoint.Client.CompliancePolicy.SPPolicyStoreProxy.GetAvailableTagsForSite(ClientContext, ClientContext.Url);
                    ClientContext.ExecuteQueryRetry();

                    var tag = tags.Where(t => t.TagName == Label).FirstOrDefault();

                    if (tag != null)
                    {
                        item.SetComplianceTag(tag.TagName, tag.BlockDelete, tag.BlockEdit, tag.IsEventTag, tag.SuperLock);
                    }
                    else
                    {
                        WriteWarning("Can not find compliance tag with value: " + Label);
                    }
                }
#endif

                item.Update();
                ClientContext.Load(item);
                ClientContext.ExecuteQueryRetry();
                WriteObject(item);
            }
        }
    }
}
