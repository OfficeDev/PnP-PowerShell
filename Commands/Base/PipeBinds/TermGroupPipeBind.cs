using System;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class TermGroupPipeBind
    {
        private readonly Guid _id = Guid.Empty;
        private readonly string _name = string.Empty;
        public TermGroupPipeBind(Guid guid)
        {
            _id = guid;
        }

        public TermGroupPipeBind()
        {
        }

        public TermGroupPipeBind(string id)
        {
            if (!Guid.TryParse(id, out _id))
            {
                _name = id;
            }
        }

        public TermGroupPipeBind(TermGroup termGroup)
        {
            if (!termGroup.IsPropertyAvailable("Id"))
            {
                termGroup.EnsureProperty(t => t.Id);
            }
            _id = termGroup.Id;
        }

        public Guid Id => _id;

        public string Name => _name;
    }
}
