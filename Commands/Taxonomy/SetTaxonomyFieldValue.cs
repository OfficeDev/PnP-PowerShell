using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;
using System.Collections;
using System.Collections.Generic;

namespace PnP.PowerShell.Commands.Taxonomy
{
    [Cmdlet(VerbsCommon.Set, "PnPTaxonomyFieldValue", DefaultParameterSetName = "ITEM")]
    [CmdletHelp("Sets a taxonomy term value in a listitem field",
        Category = CmdletHelpCategory.Taxonomy)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTaxonomyFieldValue -ListItem $item -InternalFieldName 'Department' -TermId 863b832b-6818-4e6a-966d-2d3ee057931c",
        Remarks = @"Sets the field called 'Department' to the value of the term with the ID specified",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Set-PnPTaxonomyFieldValue -ListItem $item -InternalFieldName 'Department' -TermPath 'CORPORATE|DEPARTMENTS|HR'",
        Remarks = @"Sets the field called 'Department' to the term called HR which is located in the DEPARTMENTS termset, which in turn is located in the CORPORATE termgroup.",
        SortOrder = 2)]
    [CmdletExample(
     Code = @"PS:> Set-PnPTaxonomyFieldValue -ListItem $item -InternalFieldName 'Department' -Terms @{""TermId1""=""Label1"";""TermId2""=""Label2""}",
     Remarks = @"Sets the field called 'Department' with multiple terms by ID and label. You can refer to those terms with the {ID:label} token.",
        SortOrder = 3)]
    public class SetTaxonomyFieldValue : PnPSharePointCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "The list item to set the field value to")]
        public ListItem ListItem;

        [Parameter(Mandatory = true, ParameterSetName = ParameterAttribute.AllParameterSets, HelpMessage = "The internal name of the field")]
        public string InternalFieldName;

        [Parameter(Mandatory = true, ParameterSetName = "ITEM", HelpMessage = "The Id of the Term")]
        public GuidPipeBind TermId;

        [Parameter(Mandatory = false, ParameterSetName = "ITEM", HelpMessage = "The Label value of the term")]
        public string Label;

        [Parameter(Mandatory = true, ParameterSetName = "PATH", HelpMessage = "A path in the form of GROUPLABEL|TERMSETLABEL|TERMLABEL")]
        public string TermPath;

        [Parameter(Mandatory = false, ParameterSetName = "ITEMS", HelpMessage = "Allows you to specify terms with key value pairs that can be referred to in the template by means of the {id:label} token. See examples on how to use this parameter.")]
        public Hashtable Terms;

        protected override void ExecuteCmdlet()
        {
            Field field = ListItem.ParentList.Fields.GetByInternalNameOrTitle(InternalFieldName);
            ListItem.Context.Load(field);
            ListItem.Context.ExecuteQueryRetry();

            switch (ParameterSetName)
            {
                case "ITEM":
                    {
                        ListItem.SetTaxonomyFieldValue(field.Id, Label, TermId.Id);
                        break;
                    }
                case "PATH":
                    {
                        ListItem.SetTaxonomyFieldValueByTermPath(TermPath, field.Id);
                        break;
                    }
                case "ITEMS":
                    {
                        var terms = new List<KeyValuePair<Guid, string>>();
                        foreach (string key in Terms.Keys)
                        {
                            var termId = Guid.Empty;
                            Guid.TryParse(key, out termId);

                            string termValue = Terms[key] as string;

                            if (termId != Guid.Empty && !string.IsNullOrEmpty(termValue))
                            {
                                terms.Add(new KeyValuePair<Guid, string>(termId, termValue));
                            }
                        }

                        ListItem.SetTaxonomyFieldValues(field.Id, terms);
                        break;
                    }
            }
        }
    }

}
