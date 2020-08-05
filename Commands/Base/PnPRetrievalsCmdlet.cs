using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Threading;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using Resources = PnP.PowerShell.Commands.Properties.Resources;


namespace PnP.PowerShell.Commands
{
    [CmdletAdditionalParameter(ParameterType = typeof(string[]), ParameterName = "Includes", HelpMessage = "Specify properties to include when retrieving objects from the server.")]
    public abstract class PnPRetrievalsCmdlet<TType> : PnPSharePointCmdlet, IDynamicParameters where TType : ClientObject
    {
        public object GetDynamicParameters()
        {
            var classAttribute = this.GetType().GetCustomAttributes(false).FirstOrDefault(a => a is PropertyLoadingAttribute);
            var maxDepth = ((PropertyLoadingAttribute)classAttribute)?.Depth ?? 2;
            const string parameterName = "Includes";
            var parameterDictionary = new RuntimeDefinedParameterDictionary();
            var attributeCollection = new System.Collections.ObjectModel.Collection<Attribute>();

            var parameterAttribute = new ParameterAttribute
            {
                ValueFromPipeline = false,
                ValueFromPipelineByPropertyName = false,
                Mandatory = false
            };

            attributeCollection.Add(parameterAttribute);

            var attributes = GetProperties(typeof(TType), typeof(TType), maxDepth);

            var validateSetAttribute = new ValidateSetAttribute(attributes.ToArray());
            attributeCollection.Add(validateSetAttribute);

            var runtimeParameter = new RuntimeDefinedParameter(parameterName, typeof(string[]), attributeCollection);

            parameterDictionary.Add(parameterName, runtimeParameter);

            return parameterDictionary;
        }
        protected IEnumerable<string> Includes
        {
            get
            {
                if (ParameterSpecified(nameof(Includes)) && MyInvocation.BoundParameters["Includes"] != null)
                {
                    return MyInvocation.BoundParameters["Includes"] as string[];
                }
                else
                {
                    return null;
                }
            }
        }

        protected Expression<Func<TType, object>>[] RetrievalExpressions => GetPropertyExpressions();

        protected Expression<Func<TType, object>>[] DefaultRetrievalExpressions { get; set; }

        protected Expression<Func<TType, object>>[] GetPropertyExpressions()
        {
            var fieldsToLoad = new List<string>();

            if (ParameterSpecified(nameof(Includes)))
            {
                var values = MyInvocation.BoundParameters["Includes"] as string[];

                if (values != null)
                {
                    fieldsToLoad.AddRange(values);
                }
            }

            var type = typeof(TType);
            var expressions = new List<Expression<Func<TType, object>>>();

            if (DefaultRetrievalExpressions != null)
            {
                expressions.AddRange(DefaultRetrievalExpressions);
            }

            foreach (var include in fieldsToLoad)
            {
                var exp = (Expression<Func<TType, object>>)Utilities.DynamicExpression.ParseLambda(type, typeof(object), include, null);

                expressions.Add(exp);

            }
            return expressions.ToArray();
        }

        private static List<string> GetProperties(Type baseType, Type type, int maxDepth, int level = 0, string objectPath = "")
        {
            objectPath = !string.IsNullOrEmpty(objectPath) ? $"{objectPath}." : objectPath;
            level++;
            List<string> propsList = new List<string>();

            // CSOM PROPS
            var properties = type.GetProperties().Where(p => p.PropertyType.Assembly == baseType.Assembly);
            foreach (var prop in properties)
            {
                propsList.Add($"{objectPath}{prop.Name}");
                if (level < maxDepth)
                {
                    propsList.AddRange(GetProperties(baseType, prop.PropertyType, maxDepth, level, $"{objectPath}{prop.Name}"));
                }
            }

            // Other props
            var otherProps = type.GetProperties().Where(p => p.PropertyType.Assembly != baseType.Assembly).Select(p => $"{objectPath}{p.Name}");
            propsList.AddRange(otherProps);
            return propsList;
        }
    }
}