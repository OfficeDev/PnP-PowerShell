using System;
using Microsoft.SharePoint.Client;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class FeaturePipeBind
    {
        readonly Guid _id;
        readonly string _name;
        readonly Feature _feature;

        public FeaturePipeBind(Guid id)
        {
            _id = id;
        }

        public FeaturePipeBind(string str)
        {
            if (!Guid.TryParse(str, out _id))
            {
                _name = str;
            }
        }

        public FeaturePipeBind(Feature feature)
        {
            _feature = feature;
        }

        internal Guid Id
        {
            get
            {
                if (_feature != null)
                {
                    return _feature.DefinitionId;
                }
                else
                {
                    return _id;
                }
            }
        }

        internal string Name => _name;

        internal Feature Feature => _feature;
    }
}
