using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.Fields
{
    [Cmdlet(VerbsCommon.Add, "PnPFieldFromXml")]
    [CmdletHelp("Adds a field to a list or as a site column based upon a CAML/XML field definition",
        Category = CmdletHelpCategory.Fields,
        OutputType = typeof(Field),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.field.aspx")]
    [CmdletExample(
        Code = @"PS:> $xml = '<Field Type=""Text"" Name=""PSCmdletTest"" DisplayName=""PSCmdletTest"" ID=""{27d81055-f208-41c9-a976-61c5473eed4a}"" Group=""Test"" Required=""FALSE"" StaticName=""PSCmdletTest"" />'
PS:> Add-PnPFieldFromXml -FieldXml $xml",
        Remarks = "Adds a field with the specified field CAML code to the site.",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> $xml = '<Field Type=""Text"" Name=""PSCmdletTest"" DisplayName=""PSCmdletTest"" ID=""{27d81055-f208-41c9-a976-61c5473eed4a}"" Group=""Test"" Required=""FALSE"" StaticName=""PSCmdletTest"" />'
PS:> Add-PnPFieldFromXml -List ""Demo List"" -FieldXml $xml",
        Remarks = @"Adds a field with the specified field CAML code to the list ""Demo List"".",
        SortOrder = 2)]
    [CmdletRelatedLink(
        Text = "Field CAML",
        Url = "http://msdn.microsoft.com/en-us/library/office/ms437580(v=office.15).aspx")]
    public class AddFieldFromXml : PnPWebCmdlet
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
            switch (f.FieldTypeKind)
            {
                case FieldType.DateTime:
                    {
                        WriteObject(ClientContext.CastTo<FieldDateTime>(f));
                        break;
                    }
                case FieldType.Choice:
                    {
                        WriteObject(ClientContext.CastTo<FieldChoice>(f));
                        break;
                    }
                case FieldType.Calculated:
                    {
                        WriteObject(ClientContext.CastTo<FieldCalculated>(f));
                        break;
                    }
                case FieldType.Computed:
                    {
                        WriteObject(ClientContext.CastTo<FieldComputed>(f));
                        break;
                    }
                case FieldType.Geolocation:
                    {
                        WriteObject(ClientContext.CastTo<FieldGeolocation>(f));
                        break;

                    }
                case FieldType.User:
                    {
                        WriteObject(ClientContext.CastTo<FieldUser>(f));
                        break;
                    }
                case FieldType.Currency:
                    {
                        WriteObject(ClientContext.CastTo<FieldCurrency>(f));
                        break;
                    }
                case FieldType.Guid:
                    {
                        WriteObject(ClientContext.CastTo<FieldGuid>(f));
                        break;
                    }
                case FieldType.URL:
                    {
                        WriteObject(ClientContext.CastTo<FieldUrl>(f));
                        break;
                    }
                case FieldType.Lookup:
                    {
                        WriteObject(ClientContext.CastTo<FieldLookup>(f));
                        break;
                    }
                case FieldType.MultiChoice:
                    {
                        WriteObject(ClientContext.CastTo<FieldMultiChoice>(f));
                        break;
                    }
                case FieldType.Number:
                    {
                        WriteObject(ClientContext.CastTo<FieldNumber>(f));
                        break;
                    }
                default:
                    {
                        WriteObject(f);
                        break;
                    }
            }
        }

    }

}
