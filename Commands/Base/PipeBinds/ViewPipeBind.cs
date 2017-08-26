using Microsoft.SharePoint.Client;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public class ViewPipeBind
    {
        private readonly View _view;
        private readonly Guid _id;
        private readonly string _name;
        private readonly Guid _parentListId;

        public ViewPipeBind()
        {
            _view = null;
            _id = Guid.Empty;
            _name = string.Empty;
            _parentListId = Guid.Empty;
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

        public ViewPipeBind(PSObject psObject)
        {
            if (psObject.Properties["Id"] != null && Guid.TryParse(psObject.Properties["Id"].Value.ToString(), out Guid viewId))
            {
                _id = viewId;
            }
            if (psObject.Properties["List"] != null && Guid.TryParse(psObject.Properties["List"].Value.ToString(), out Guid listId))
            {
                _parentListId = listId;
            }
        }

        public Guid Id => _id;

        public View View => _view;

        public string Title => _name;

        public Guid ParentListId => _parentListId;
    }
}
