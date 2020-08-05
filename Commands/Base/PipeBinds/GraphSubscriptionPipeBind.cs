using OfficeDevPnP.Core.Framework.Graph.Model;
using System;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public class GraphSubscriptionPipeBind
    {
        private readonly Subscription _subscription;
        private readonly String _subscriptionId;

        public GraphSubscriptionPipeBind()
        {
        }

        public GraphSubscriptionPipeBind(Subscription subscription)
        {
            _subscription = subscription;
        }

        public GraphSubscriptionPipeBind(String input)
        {
            if (Guid.TryParse(input, out Guid idValue))
            {
                _subscriptionId = input;
            }
            else
            {
                throw new ArgumentException("Idenity must be a GUID", nameof(input));
            }
        }

        public Subscription Subscription => (_subscription);

        public String SubscriptionId => _subscriptionId ?? _subscription?.Id;
    }
}
