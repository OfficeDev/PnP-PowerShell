using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Collections;

namespace SharePointPnP.PowerShell.Commands.Fields
{
    [Cmdlet(VerbsCommon.Set, "PnPView")]
    [CmdletHelp("Changes one or more properties of a specific view",
        Category = CmdletHelpCategory.Fields,
        OutputType = typeof(Field),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.view.aspx")]
    [CmdletExample(
        Code = @"PS:> Set-PnPView -List ""Tasks"" -Identity ""All Tasks"" -Values @{JSLink=""hierarchytaskslist.js|customrendering.js"";Title=""My view""}",
        Remarks = @"Updates the ""All Tasks"" view on list ""Tasks"" to use hierarchytaskslist.js and customrendering.js for the JSLink and changes the title of the view to ""My view""",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Get-PnPList -Identity ""Tasks"" | Get-PnPView | Set-PnPView -Values @{JSLink=""hierarchytaskslist.js|customrendering.js""}",
        Remarks = @"Updates all views on list ""Tasks"" to use hierarchytaskslist.js and customrendering.js for the JSLink",
        SortOrder = 2)]
    public class SetView : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, HelpMessage = "The Id, Title or Url of the list")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The Id, Title or instance of the view")]
        public ViewPipeBind Identity;

        [Parameter(Mandatory = true, HelpMessage = "Hashtable of properties to update on the view. Use the syntax @{property1=\"value\";property2=\"value\"}.")]
        public Hashtable Values;

        protected override void ExecuteCmdlet()
        {
            List list;
            View view = null;
            if (List != null)
            {
                list = List.GetList(SelectedWeb);
                if (list == null)
                {
                    throw new PSArgumentException("List provided in the List argument could not be found", "List");
                }
                
                if (Identity.Id != Guid.Empty)
                {
                    WriteVerbose($"Retrieving view by Id '{Identity.Id}'");
                    view = list.GetViewById(Identity.Id);
                }
                else if (!string.IsNullOrEmpty(Identity.Title))
                {
                    WriteVerbose($"Retrieving view by Title '{Identity.Title}'");
                    view = list.GetViewByName(Identity.Title);
                }
            }
            else if(Identity.View != null)
            {
                WriteVerbose("Using view passed through the pipeline");
                view = Identity.View;
            }
            else
            {
                throw new PSArgumentException("List must be provided through the List argument if not passing in a view instance", "List");
            }

            if (view == null)
            {
                throw new PSArgumentException("View provided in the Identity argument could not be found", "Identity");
            }

            bool atLeastOnePropertyChanged = false;
            foreach (string key in Values.Keys)
            {
                var value = Values[key];

                var property = view.GetType().GetProperty(key);
                if (property == null)
                {
                    WriteWarning($"No property '{key}' found on this view. Value will be ignored.");
                }
                else
                {
                    try
                    {
                        property.SetValue(view, value);
                        atLeastOnePropertyChanged = true;
                    }
                    catch(Exception e)
                    {
                        WriteWarning($"Setting property '{key}' to '{value}' failed with exception '{e.Message}'. Value will be ignored.");
                    }
                }
            }

            if (atLeastOnePropertyChanged)
            {
                view.Update();
                ClientContext.ExecuteQueryRetry();
            }
        }
    }
}