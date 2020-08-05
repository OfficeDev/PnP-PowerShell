using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Collections;

namespace PnP.PowerShell.Commands.Fields
{
    [Cmdlet(VerbsCommon.Set, "PnPView")]
    [CmdletHelp("Change view properties",
        DetailedDescription = "Sets one or more properties of an existing view, see here https://docs.microsoft.com/previous-versions/office/sharepoint-server/ee543328(v=office.15) for the list of view properties.",
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
    [CmdletExample(
        Code = @"PS:> Set-PnPView -List ""Documents"" -Identity ""Corporate Documents"" -Fields ""Title"",""Created""",
        Remarks = @"Updates the Corporate Documents view on the Documents library to have two fields",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"PS:> Set-PnPView -List ""Documents"" -Identity ""Corporate Documents"" -Fields ""Title"",""Created"" -Aggregations ""<FieldRef Name='Title' Type='COUNT'/>""",
        Remarks = @"Updates the Corporate Documents view on the Documents library and sets the totals (aggregations) to Count on the Title field",
        SortOrder = 4)]
    public class SetView : PnPWebCmdlet
    {
        [Parameter(Mandatory = false, Position = 0, HelpMessage = "The Id, Title or Url of the list")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, ValueFromPipeline = true, HelpMessage = "The Id, Title or instance of the view")]
        public ViewPipeBind Identity;

        [Parameter(Mandatory = false, HelpMessage = "Hashtable of properties to update on the view. Use the syntax @{property1=\"value\";property2=\"value\"}.")]
        public Hashtable Values;

        [Parameter(Mandatory = false, HelpMessage = "An array of fields to use in the view. Notice that specifying this value will remove the existing fields")]
        public string[] Fields;

        [Parameter(Mandatory = false, HelpMessage = "A valid XML fragment containing one or more Aggregations")]
        public string Aggregations;

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
            else if (Identity.View != null)
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

            if (ParameterSpecified(nameof(Values)))
            {
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
                        catch (Exception e)
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
            if(ParameterSpecified(nameof(Fields)))
            {
                view.ViewFields.RemoveAll();
                foreach(var viewField in Fields)
                {
                    view.ViewFields.Add(viewField);
                }
                view.Update();
                ClientContext.ExecuteQueryRetry();
            }
            if(ParameterSpecified(nameof(Aggregations)))
            {
                view.Aggregations = Aggregations;
                view.Update();
                ClientContext.Load(view);
                ClientContext.ExecuteQueryRetry();
            }
            WriteObject(view);
        }
    }
}
