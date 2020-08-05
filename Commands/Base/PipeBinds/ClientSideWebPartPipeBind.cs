#if !SP2013 && !SP2016
using OfficeDevPnP.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public class ClientSideWebPartPipeBind
    {
        private readonly Guid _instanceId;
        private readonly string _title;

        public ClientSideWebPartPipeBind(Guid guid)
        {
            _instanceId = guid;
        }

        public ClientSideWebPartPipeBind(string instanceId)
        {
            if (!Guid.TryParse(instanceId, out _instanceId))
            {
                _title = instanceId;
            }
        }

        public Guid InstanceId => _instanceId;

        public string Title => _title;

        public ClientSideWebPartPipeBind()
        {
            _instanceId = Guid.Empty;
            _title = string.Empty;
        }

        public List<ClientSideWebPart> GetWebPart(ClientSidePage page)
        {
            if (page == null)
            {
                throw new ArgumentException(nameof(page));
            }
            if (!string.IsNullOrEmpty(_title))
            {
                return page.Controls.Where(c => c.GetType() == typeof(ClientSideWebPart) && ((ClientSideWebPart)c).Title.Equals(_title, StringComparison.InvariantCultureIgnoreCase)).Cast<ClientSideWebPart>().ToList();
            }
            return page.Controls.Where(c => c.GetType() == typeof(ClientSideWebPart) && c.InstanceId == _instanceId).Cast<ClientSideWebPart>().ToList();
        }
    }
}
#endif