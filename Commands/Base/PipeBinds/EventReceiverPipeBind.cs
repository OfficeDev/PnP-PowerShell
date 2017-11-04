using Microsoft.SharePoint.Client;
using System;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class EventReceiverPipeBind
    {
        private readonly EventReceiverDefinition _eventReceiverDefinition;
        private readonly Guid _id;
        private readonly string _name;

        public EventReceiverPipeBind()
        {
            _eventReceiverDefinition = null;
            _id = Guid.Empty;
            _name = string.Empty;
        }

        public EventReceiverPipeBind(EventReceiverDefinition eventReceiverDefinition)
        {
            _eventReceiverDefinition = eventReceiverDefinition;
        }

        public EventReceiverPipeBind(Guid guid)
        {
            _id = guid;
        }

        public EventReceiverPipeBind(string id)
        {
            if (!Guid.TryParse(id, out _id))
            {
                _name = id;
            }
        }

        public Guid Id => _id;

        public EventReceiverDefinition EventReceiver => _eventReceiverDefinition;

        public string Name => _name;

        public override string ToString()
        {
            return Name ?? Id.ToString();
        }

        internal EventReceiverDefinition GetEventReceiverOnWeb(Web web)
        {
            if (_eventReceiverDefinition != null)
            {
                return _eventReceiverDefinition;
            }

            if(_id != Guid.Empty)
            {
                return web.GetEventReceiverById(_id);
            }
            else if (!string.IsNullOrEmpty(Name))
            {
                return web.GetEventReceiverByName(Name);
            }
            return null;
        }

        internal EventReceiverDefinition GetEventReceiverOnList(List list)
        {
            if (_eventReceiverDefinition != null)
            {
                return _eventReceiverDefinition;
            }

            if (_id != Guid.Empty)
            {
                return list.GetEventReceiverById(_id);
            }
            else if (!string.IsNullOrEmpty(Name))
            {
                return list.GetEventReceiverByName(Name);
            }
            return null;
        }
    }
}