using OfficeDevPnP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public class WebhookSubscriptionPipeBind
    {
        #region Fields
        private WebhookSubscription _subscription;
        private Guid _subscriptionId;
        #endregion

        #region Properties
        public WebhookSubscription Subscription => _subscription;
        public Guid Id => _subscriptionId;
        #endregion

        #region Ctors
        public WebhookSubscriptionPipeBind()
        {
            _subscriptionId = Guid.Empty;
            _subscription = new WebhookSubscription() { Id = _subscriptionId.ToString() };
        }

        public WebhookSubscriptionPipeBind(WebhookSubscription subscription)
        {
            _subscriptionId = Guid.Parse(subscription.Id);
            _subscription = subscription;
        }

        public WebhookSubscriptionPipeBind(Guid subscriptionId)
        {
            _subscriptionId = subscriptionId;
            _subscription = new WebhookSubscription() { Id = subscriptionId.ToString() };
        }

        public WebhookSubscriptionPipeBind(string subscriptionId)
        {
            _subscriptionId = Guid.Parse(subscriptionId);
            _subscription = new WebhookSubscription() { Id = subscriptionId };
        }
        #endregion
    }
}
