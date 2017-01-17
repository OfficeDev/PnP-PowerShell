using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Linq.Expressions;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SharePointPnP.PowerShell.Commands.Lists
{
    [Cmdlet(VerbsCommon.Get, "PnPList")]
    [CmdletAlias("Get-SPOList")]
    [CmdletHelp("Returns a List object",
        Category = CmdletHelpCategory.Lists,
        OutputType = typeof(List),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.list.aspx")]
    [CmdletExample(
        Code = "PS:> Get-PnPList",
        Remarks = "Returns all lists in the current web",
        SortOrder = 1)]
    [CmdletExample(
        Code = "PS:> Get-PnPList -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe",
        Remarks = "Returns a list with the given id.",
        SortOrder = 2)]
    [CmdletExample(
        Code = "PS:> Get-PnPList -Identity Lists/Announcements",
        Remarks = "Returns a list with the given url.",
        SortOrder = 3)]
    public class GetList : SPOWebCmdlet
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0, HelpMessage = "The ID, name or Url (Lists/MyList) of the list.")]
        public ListPipeBind Identity;

        [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 1, HelpMessage = "Additional Properties to retrieve for the List item(s) to be returned")]  
        [ValidateSet("AllowDeletion","BrowserFileHandling","ContentTypes","DataSource","DefaultDisplayFormUrl","DefaultEditFormUrl","DefaultNewFormUrl","DefaultViewPath","DocumentTemplateUrl","EffectiveBasePermissions","EffectiveBasePermissionsForUI","EnableAssignToEmail","EventReceivers","ExcludeFromOfflineClient","Fields","Forms","HasUniqueRoleAssignments","IsSiteAssetsLibrary","IsSystemList","ReadSecurity","RoleAssignments","SchemaXml","Tag","UserCustomActions","ValidationFormula","ValidationMessage","Views","WorkflowAssociations","WriteSecurity")]
        public string[] Includes;

        protected override void ExecuteCmdlet()
        {
            if (Identity != null)
            {
                var list = Identity.GetList(SelectedWeb);
                if (Includes != null && Includes.Length > 0)
                {
                    List<string> fields = new List<string>();
                    fields.AddRange(Includes);
                    List<Expression<Func<List, object>>> expressions = GetPropertyExpression<List>(fields);
                    
                    
                    ClientContext.Load(list, expressions.ToArray());
                    ClientContext.ExecuteQueryRetry();
                }


                WriteObject(list);

            }
            else
            {
                string [] defaultFields = { "Id", "BaseTemplate", "OnQuickLaunch", "DefaultViewUrl", "Title", "Hidden" };

                List<string> fields = new List<string>();
                fields.AddRange(defaultFields);

                if (Includes != null && Includes.Length > 0)
                {
                    fields.AddRange(Includes);
                  //  fields.Add("RootFolder.ServerRelativeUrl");
                }
                
                List<Expression<Func<List, object>>> expressions = GetPropertyExpression<List>(fields);
                
                //Now handle that special case of  RootFolder.ServerRelativeUrl -- Should really do a check for "sub members and implement this recursively...
                Expression<Func<List, object>> expressionRelativeUrl = l => l.RootFolder.ServerRelativeUrl;
                expressions.Add(expressionRelativeUrl);

                var query = (SelectedWeb.Lists.IncludeWithDefaultProperties(expressions.ToArray()));
               
                var lists = ClientContext.LoadQuery(query);
                ClientContext.ExecuteQueryRetry();
                WriteObject(lists, true);
            }
        }

        private static List<Expression<Func<T, object>>> GetPropertyExpression<T>(List<string> Includes)
        {
            var type = typeof(T);
            var expressions = new List<Expression<Func<T, object>>>();

            var properties = type.GetProperties();

            foreach(var Include in Includes)
            {
                //Add . checking for nested includes.. then it would call this T function recursively suing the type of the property
                // example RootFolder.ServerRelativeUrl GetPropertyExpression<Folder>(new string [] { "ServerRelativeUrl" }
                var sanityCheck = (from property in properties where property.Name == Include select property).FirstOrDefault();
                if(sanityCheck != null)
                {
                    //the property exists in the parent type.

                    ParameterExpression paramExpression = Expression.Parameter(type, "l");
                    MemberExpression propertyExpression = Expression.Property(paramExpression, Include);
                    var bodyExpression = Expression.Convert(propertyExpression, typeof(Object));
                    //MemberExpression propertyExpression = Expression.Property(instance,  Include);
                    var expression = System.Linq.Expressions.ParameterExpression.Lambda<Func<T, object>>(bodyExpression, new[] { paramExpression });
                    expressions.Add(expression);
                }
            }

            return expressions;
        }


    }

}