#if !SP2013 && !SP2016
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System;
using System.Linq;
using System.Management.Automation;

namespace PnP.PowerShell.Commands.ClientSidePages
{
    [Cmdlet(VerbsCommon.Move, "PnPClientSideComponent")]
    [CmdletHelp("Moves a Client-Side Component to a different section/column",
        SupportedPlatform = CmdletSupportedPlatform.Online | CmdletSupportedPlatform.SP2019,
        DetailedDescription = "Moves a Client-Side Component to a different location on the page. Notice that the sections and or columns need to be present before moving the component.",
        Category = CmdletHelpCategory.ClientSidePages)]
    [CmdletExample(
        Code = @"PS:> Move-PnPClientSideComponent -Page Home -InstanceId a2875399-d6ff-43a0-96da-be6ae5875f82 -Section 1",
        Remarks = @"Moves the specified component to the first section of the page.", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Move-PnPClientSideComponent -Page Home -InstanceId a2875399-d6ff-43a0-96da-be6ae5875f82 -Column 2",
        Remarks = @"Moves the specified component to the second column of the current section.", SortOrder = 2)]
    [CmdletExample(
        Code = @"PS:> Move-PnPClientSideComponent -Page Home -InstanceId a2875399-d6ff-43a0-96da-be6ae5875f82 -Section 1 -Column 2",
        Remarks = @"Moves the specified component to the first section of the page into the second column.", SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Move-PnPClientSideComponent -Page Home -InstanceId a2875399-d6ff-43a0-96da-be6ae5875f82 -Section 1 -Column 2 -Position 2",
        Remarks = @"Moves the specified component to the first section of the page into the second column and sets the column to position 2 in the list of webparts.", SortOrder = 4)]
    public class MoveClientSideWebPart : PnPWebCmdlet
    {
        const string ParameterSet_SECTION = "Move to other section";
        const string ParameterSet_COLUMN = "Move to other column";
        const string ParameterSet_SECTIONCOLUMN = "Move to other section and column";
        const string ParameterSet_POSITION = "Move within a column";

        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0, HelpMessage = "The name of the page")]
        public ClientSidePagePipeBind Page;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The instance id of the control. Use Get-PnPClientSideControl retrieve the instance ids.")]
        public GuidPipeBind InstanceId;

        [Parameter(Mandatory = true, ValueFromPipeline = false, ParameterSetName = ParameterSet_SECTION, HelpMessage = "The section to move the web part to")]
        [Parameter(Mandatory = true, ValueFromPipeline = false, ParameterSetName = ParameterSet_SECTIONCOLUMN, HelpMessage = "The section to move the web part to")]
        public int Section;

        [Parameter(Mandatory = true, ValueFromPipeline = false, ParameterSetName = ParameterSet_COLUMN, HelpMessage = "The column to move the web part to")]
        [Parameter(Mandatory = true, ValueFromPipeline = false, ParameterSetName = ParameterSet_SECTIONCOLUMN, HelpMessage = "The column to move the web part to")]
        public int Column;

        [Parameter(Mandatory = false, ValueFromPipeline = false, ParameterSetName = ParameterSet_COLUMN, HelpMessage = "Change to order of the web part in the column")]
        [Parameter(Mandatory = false, ValueFromPipeline = false, ParameterSetName = ParameterSet_SECTION, HelpMessage = "Change to order of the web part in the column")]
        [Parameter(Mandatory = false, ValueFromPipeline = false, ParameterSetName = ParameterSet_SECTIONCOLUMN, HelpMessage = "Change to order of the web part in the column")]
        [Parameter(Mandatory = true, ValueFromPipeline = false, ParameterSetName = ParameterSet_POSITION, HelpMessage = "Change to order of the web part in the column")]
        public int Position;

        protected override void ExecuteCmdlet()
        {
            var clientSidePage = Page.GetPage(ClientContext);

            if (clientSidePage == null)
                throw new Exception($"Page '{Page?.Name}' does not exist");

            var control = clientSidePage.Controls.FirstOrDefault(c => c.InstanceId == InstanceId.Id);
            if (control != null)
            {
                bool updated = false;

                switch (ParameterSetName)
                {
                    case ParameterSet_COLUMN:
                        {
                            var column = control.Section.Columns[Column - 1];
                            if (column != control.Column)
                            {
                                if (ParameterSpecified(nameof(Position)))
                                {
                                    control.MovePosition(column, Position);
                                }
                                else
                                {
                                    control.Move(column);
                                }
                                updated = true;
                            }
                            break;
                        }
                    case ParameterSet_SECTION:
                        {
                            var section = clientSidePage.Sections[Section - 1];
                            if (section != control.Section)
                            {
                                if (ParameterSpecified(nameof(Position)))
                                {
                                    control.MovePosition(section, Position);
                                }
                                else
                                {
                                    control.Move(section);
                                }
                                updated = true;
                            }
                            break;
                        }
                    case ParameterSet_SECTIONCOLUMN:
                        {
                            var section = clientSidePage.Sections[Section - 1];
                            if (section != control.Section)
                            {
                                control.Move(section);
                                updated = true;
                            }
                            var column = section.Columns[Column - 1];
                            if (column != control.Column)
                            {
                                if (ParameterSpecified(nameof(Position)))
                                {
                                    control.MovePosition(column, Position);
                                }
                                else
                                {
                                    control.Move(column);
                                }
                                updated = true;
                            }
                            break;
                        }
                    case ParameterSet_POSITION:
                        {
                            control.MovePosition(control.Column, Position);
                            updated = true;
                            break;
                        }
                }


                if (updated)
                {
                    clientSidePage.Save();
                }
            }
            else
            {
                throw new Exception($"Webpart does not exist");
            }
        }
    }
}
#endif