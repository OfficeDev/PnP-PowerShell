#if !SP2013 && !SP2016
using Microsoft.SharePoint.Client;
using System;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class AlertPipeBind
    {
        private readonly Guid _id;

        public AlertPipeBind(Guid guid)
        {
            _id = guid;
        }

        public AlertPipeBind(string id)
        {
            _id = new Guid(id);
        }

        public AlertPipeBind(Alert alert)
        {
            _id = alert.ID;
        }

        public Guid Id => _id;

        public AlertPipeBind()
        {
            _id = Guid.Empty;
        }
    }
}
#endif