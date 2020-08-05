#if !SP2013 && !SP2016
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Pages;
using PnP.PowerShell.Commands.ClientSidePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class ClientSideComponentPipeBind
    {
        private readonly ClientSideComponent _component;
        private string _name;
        private Guid _id;

        public ClientSideComponentPipeBind(ClientSideComponent component)
        {
            _component = component;
            _id = Guid.Parse(_component.Id);
            _name = _component.Name;
        }

        public ClientSideComponentPipeBind(string nameOrId)
        {
            _component = null;
            if (!Guid.TryParse(nameOrId, out _id))
            {
                _name = nameOrId;
            }
        }

        public ClientSideComponentPipeBind(Guid id)
        {
            _id = id;
            _name = null;
            _component = null;
        }

        public ClientSideComponent Component => _component;

        public string Name => _component?.Name;

        public string Id => _component == null ? Guid.Empty.ToString() : _component.Id;

        public override string ToString() => Name;

        internal ClientSideComponent GetComponent(ClientSidePage page)
        {
            if (_component != null)
            {
                return _component;
            }
            else if (!string.IsNullOrEmpty(_name))
            {
                ClientSideComponent com = page.AvailableClientSideComponents(_name).FirstOrDefault();
                return com;
            }
            else if (_id != Guid.Empty)
            {
                string idAsString = _id.ToString();
                var comQuery = from c in page.AvailableClientSideComponents(_name)
                               where c.Id == idAsString
                               select c;
                return comQuery.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
    }
}
#endif