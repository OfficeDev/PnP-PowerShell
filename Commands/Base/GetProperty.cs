using Microsoft.SharePoint.Client;
using OfficeDevPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OfficeDevPnP.PowerShell.Commands.Base
{
    [Cmdlet(VerbsCommon.Get, "SPOProperty")]
    [CmdletHelp("Will populate properties of an object and optionally, if needed, load the value from the server. If one property is specified its value will be returned to the output.",
       Category = CmdletHelpCategory.Base)]
    [CmdletExample(Code = @"
PS:> $web = Get-SPOWeb
PS:> Get-SPOProperty -ClientObject $web -Property Id, Lists
PS:> $web.Lists",
        Remarks = "Will load both the Id and Lists properties of the specified Web object.",
        SortOrder = 1)]
    [CmdletExample(Code = @"
PS:> $list = Get-SPOList -Identity 'Site Assets'
PS:> Get-SPOProperty -ClientObject $list -Property Views",
        Remarks = "Will load the views object of the specified list object and return its value to the output.",
        SortOrder = 2)]
    public class EnsureProperty : SPOCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public ClientObject ClientObject;

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "The properties to load. If one property is specified its value will be returned to the output.")]
        public string[] Property;

        protected override void ExecuteCmdlet()
        {

            var loadRequired = false;
            foreach (var property in Property)
            {
                var expression = GetClientObjectExpression(ClientObject, property);

                if (!ClientObject.IsPropertyAvailable(expression))
                {
                    ClientObject.Context.Load(ClientObject, expression);
                    loadRequired = true;
                }
            }
            if (loadRequired)
            {
                ClientObject.Context.ExecuteQueryRetry();
            }
            if (Property.Length == 1)
            {
                WriteObject((GetClientObjectExpression(ClientObject, Property[0]).Compile())(ClientObject));
            }

        }

        private Expression<Func<ClientObject, Object>> GetClientObjectExpression(ClientObject clientObject, string property)
        {
            var memberExpression = Expression.PropertyOrField(Expression.Constant(clientObject), property);
            var memberName = memberExpression.Member.Name;

            var parameter = Expression.Parameter(typeof(ClientObject), "i");
            var cast = Expression.Convert(parameter, memberExpression.Member.ReflectedType);
            var body = Expression.Property(cast, memberName);
            var exp = Expression.Lambda<Func<ClientObject, Object>>(Expression.Convert(body, typeof(object)), parameter);

            return exp;

        }

    }
}
