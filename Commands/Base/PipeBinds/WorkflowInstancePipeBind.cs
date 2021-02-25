using System;

using Microsoft.SharePoint.Client.WorkflowServices;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class WorkflowInstancePipeBind
    {
        public WorkflowInstancePipeBind()
        {
            Instance = null;
            Id = Guid.Empty;
        }

        public WorkflowInstancePipeBind(WorkflowInstance instance)
        {
            Instance = instance;
        }

        public WorkflowInstancePipeBind(Guid guid)
        {
            Id = guid;
        }

        public WorkflowInstancePipeBind(string id)
        {
            Id = Guid.Parse(id);
        }

        public Guid Id { get; }

        public WorkflowInstance Instance { get; }
    }
}
