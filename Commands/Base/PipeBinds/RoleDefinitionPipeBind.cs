using Microsoft.SharePoint.Client;
using System;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class RoleDefinitionPipeBind
    {
        private readonly RoleDefinition _roleDefinition;
        private readonly string _name;

        public RoleDefinitionPipeBind(RoleDefinition definition)
        {
            _roleDefinition = definition;
        }

        public RoleDefinitionPipeBind(string name)
        {
            _name = name;
        }
        
        public RoleDefinition GetRoleDefinition(Microsoft.SharePoint.Client.Site site)
        {
            if(_roleDefinition != null)
            {
                return _roleDefinition;
            }
            var roleDefinition = site.RootWeb.RoleDefinitions.GetByName(_name);
            site.Context.Load(roleDefinition);
            site.Context.ExecuteQueryRetry();
            return roleDefinition;
        }

    }
}
