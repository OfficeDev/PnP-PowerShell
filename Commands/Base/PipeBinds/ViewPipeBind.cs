using Microsoft.SharePoint.Client;
using System;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public class ViewPipeBind
    {
        private readonly View _view;
        private readonly Guid _id;
        private readonly string _name;

        public ViewPipeBind()
        {
            _view = null;
            _id = Guid.Empty;
            _name = string.Empty;
        }

        public ViewPipeBind(View view)
        {
            _view = view;
        }

        public ViewPipeBind(Guid guid)
        {
            _id = guid;
        }

        public ViewPipeBind(string id)
        {
            if (!Guid.TryParse(id, out _id))
            {
                _name = id;
            }
        }

        public Guid Id => _id;

        public View View => _view;

        public string Title => _name;
    }
}
