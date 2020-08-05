using System;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class WorkflowSubscriptionPipeBind
    {
        private readonly WorkflowSubscription _sub;
        private readonly Guid _id;
        private readonly string _name = string.Empty;

        public WorkflowSubscriptionPipeBind(WorkflowSubscription sub)
        {
            _sub = sub;
        }

        public WorkflowSubscriptionPipeBind(Guid guid)
        {
            _id = guid;
        }

        public WorkflowSubscriptionPipeBind(string id)
        {
            if (!Guid.TryParse(id, out _id))
            {
                _name = id;
            }
        }

        public Guid Id => _id;

        public WorkflowSubscription Subscription => _sub;

        public string Name => _name;

        internal WorkflowSubscription GetWorkflowSubscription(Web web)
        {
            if (_sub != null)
                return _sub;

            if (_id != default(Guid))
                return new WorkflowServicesManager(web.Context, web)
                    .GetWorkflowSubscriptionService()
                    .GetSubscription(_id);

            return web.GetWorkflowSubscription(_name);
        }

        public override string ToString()
        {
            if (_sub?.IsPropertyAvailable(nameof(_sub.Name)) == true)
                return _sub.Name;

            if (!string.IsNullOrEmpty(_name))
                return _name;

            if (_sub?.IsPropertyAvailable(nameof(_sub.Id)) == true)
                return _sub.Id.ToString();

            return _id.ToString();
        }
    }
}
