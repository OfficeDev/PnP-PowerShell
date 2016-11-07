using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;

namespace SharePointPnP.PowerShell.Commands.Fields
{
    [Cmdlet(VerbsCommon.Add, "SPOFieldFromXml")]
    [CmdletHelp("Adds a field to a list or as a site column based upon a CAML/XML field definition",
        Category = CmdletHelpCategory.Fields,
        OutputType = typeof(Field),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx")]
    [CmdletExample(
        Code = @"PS:> $xml = '<Field Type=""Text"" Name=""PSCmdletTest"" DisplayName=""PSCmdletTest"" ID=""{27d81055-f208-41c9-a976-61c5473eed4a}"" Group=""Test"" Required=""FALSE"" StaticName=""PSCmdletTest"" />'
PS:> Add-SPOFieldFromXml -FieldXml $xml",
        Remarks = "Adds a field with the specified field CAML code to the site.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> $xml = '<Field Type=""Text"" Name=""PSCmdletTest"" DisplayName=""PSCmdletTest"" ID=""{27d81055-f208-41c9-a976-61c5473eed4a}"" Group=""Test"" Required=""FALSE"" StaticName=""PSCmdletTest"" />'
PS:> Add-SPOFieldFromXml -List ""Demo List"" -FieldXml $xml",
        Remarks = @"Adds a field with the specified field CAML code to the list ""Demo List"".",
        SortOrder = 2)]
    [CmdletRelatedLink(
        Text = "Field CAML",
        Url = "http://msdn.microsoft.com/en-us/library/office/ms437580(v=office.15).aspx")]
    public class AddFieldFromXml : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, HelpMessage = "The name of the list, its ID or an actual list object where this field needs to be added")]
        public ListPipeBind List;

        [Parameter(Mandatory = true, HelpMessage = "CAML snippet containing the field definition. See http://msdn.microsoft.com/en-us/library/office/ms437580(v=office.15).aspx", Position = 0)]
        public string FieldXml;

        protected override void ExecuteCmdlet()
        {
            Field f;

            if (List != null)
            {
                List list = List.GetList(SelectedWeb);

                f = list.CreateField(FieldXml);
            }
            else
            {
                f = SelectedWeb.CreateField(FieldXml);
            }
            ClientContext.Load(f);
            ClientContext.ExecuteQueryRetry();
            WriteObject(f);
        }

    }

}
