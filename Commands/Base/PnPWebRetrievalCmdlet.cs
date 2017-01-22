using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharePointPnP.PowerShell.Commands.Base;
using SharePointPnP.PowerShell.Commands.Base.PipeBinds;
using System.Management.Automation;
using System.Reflection;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Extensions;

namespace SharePointPnP.PowerShell.Commands
{
    [CmdletAdditionalParameter(ParameterType = typeof(string[]), ParameterName = "Includes", HelpMessage = "Specify properties to include when retrieving objects from the server.")]
    public abstract class PnPWebRetrievalCmdlet<TType> : SPOCmdlet, IDynamicParameters
    {
        private Web _selectedWeb;


        [Parameter(Mandatory = false, HelpMessage = "The web to apply the command to. Omit this parameter to use the current web.")]
        public WebPipeBind Web = new WebPipeBind();

        protected Web SelectedWeb
        {
            get
            {
                if (_selectedWeb == null)
                {
                    _selectedWeb = GetWeb();
                }
                return _selectedWeb;
            }
        }

        private Web GetWeb()
        {
            Web web = ClientContext.Web;

            if (Web.Id != Guid.Empty)
            {
                web = web.GetWebById(Web.Id);
                SPOnlineConnection.CurrentConnection.CloneContext(web.Url);

                web = SPOnlineConnection.CurrentConnection.Context.Web;
            }
            else if (!string.IsNullOrEmpty(Web.Url))
            {
                web = web.GetWebByUrl(Web.Url);
                SPOnlineConnection.CurrentConnection.CloneContext(web.Url);
                web = SPOnlineConnection.CurrentConnection.Context.Web;
            }
            else if (Web.Web != null)
            {
                web = Web.Web;

                web.EnsureProperty(w => w.Url);

                SPOnlineConnection.CurrentConnection.CloneContext(web.Url);
                web = SPOnlineConnection.CurrentConnection.Context.Web;
            }
            else
            {
                if (SPOnlineConnection.CurrentConnection.Context.Url != SPOnlineConnection.CurrentConnection.Url)
                {
                    SPOnlineConnection.CurrentConnection.RestoreCachedContext(SPOnlineConnection.CurrentConnection.Url);
                }
                web = ClientContext.Web;
            }

            SPOnlineConnection.CurrentConnection.Context.ExecuteQueryRetry();

            return web;
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            if (SPOnlineConnection.CurrentConnection.Context.Url != SPOnlineConnection.CurrentConnection.Url)
            {
                SPOnlineConnection.CurrentConnection.RestoreCachedContext(SPOnlineConnection.CurrentConnection.Url);
            }
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            SPOnlineConnection.CurrentConnection.CacheContext();
        }

        protected IEnumerable<string> Includes
        {
            get
            {
                if (MyInvocation.BoundParameters.ContainsKey("Includes") && MyInvocation.BoundParameters["Includes"] != null)
                {
                    return MyInvocation.BoundParameters["Includes"] as string[];
                }
                else
                {
                    return null;
                }
            }
        }

        protected Expression<Func<TType, object>>[] Expressions
        {
            get { return GetPropertyExpressions(); }
        }

        protected string[] AlwaysLoadProperties { get; set; }

        protected Expression<Func<TType, object>>[] GetPropertyExpressions()
        {
            var fieldsToLoad = new List<string>();
            if (AlwaysLoadProperties != null)
            {
                fieldsToLoad.AddRange(AlwaysLoadProperties);
            }
            if (MyInvocation.BoundParameters.ContainsKey("Includes"))
            {
                var values = MyInvocation.BoundParameters["Includes"] as string[];

                if (values != null)
                {
                    fieldsToLoad.AddRange(values);
                }
            }

            var type = typeof(TType);
            var expressions = new List<Expression<Func<TType, object>>>();

            foreach (var include in fieldsToLoad)
            {
                Expression<Func<TType, object>> exp = null;
                var paramExpression = Expression.Parameter(type, "i");
                var cast = Expression.Convert(paramExpression, type);

                if (include.Contains("."))
                {
                    var incProps = include.Split('.');
                    MemberExpression curBody = null;
                    var curType = typeof(TType);
                    PropertyInfo curProp = null;
                    foreach (var incProp in incProps)
                    {
                        if (curProp == null) {
                            curProp = curType.GetProperty(incProp);
                            curType = curProp.PropertyType;
                            curBody = Expression.Property(paramExpression, curProp);
                        }
                        else
                        {
                            curProp = curType.GetProperty(incProp);
                            curType = curProp.PropertyType;
                            curBody = Expression.Property(curBody, curProp);
                        }                  
                    }
                    Expression<Func<List,object>> test = l => l.RootFolder.ServerRelativeUrl;
                    exp = Expression.Lambda<Func<TType, object>>(Expression.Convert(curBody, typeof(object)), paramExpression);
                    expressions.Add(exp);
                }
                else
                {
                    var memberExpression = Expression.Property(paramExpression, include);
                    var memberName = memberExpression.Member.Name;
                    var body = Expression.Property(cast, memberName);
                    exp = Expression.Lambda<Func<TType, object>>(Expression.Convert(body, typeof(object)), paramExpression);
                }
                expressions.Add(exp);

            }
            return expressions.ToArray();
        }

        public object GetDynamicParameters()
        {
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

            var attributes = typeof(TType).GetProperties().Select(p => p.Name).ToArray();
           
            var validateSetAttribute = new ValidateSetAttribute(attributes);
            attributeCollection.Add(validateSetAttribute);

            var runtimeParameter = new RuntimeDefinedParameter(parameterName, typeof(string[]), attributeCollection);

            parameterDictionary.Add(parameterName, runtimeParameter);

            return parameterDictionary;
        }

    }
}