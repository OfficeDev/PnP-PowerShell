#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using System;
using System.Linq;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class ProvisioningSequencePipeBind
    {
        private readonly ProvisioningSequence _sequence;
        private readonly string _identity;

        public ProvisioningSequencePipeBind(ProvisioningSequence sequence)
        {
            _sequence = sequence;
        }

        public ProvisioningSequencePipeBind(string identity)
        {
            _identity = identity;
        }

        public ProvisioningSequence GetSequenceFromHierarchy(ProvisioningHierarchy hierarchy)
        {
            var id = string.Empty;
            if(_sequence == null)
            {
                id = _identity;
            } else
            {
                id = _sequence.ID;
            }
            return hierarchy.Sequences.FirstOrDefault(s => s.ID == id);
        }

    }
}
#endif