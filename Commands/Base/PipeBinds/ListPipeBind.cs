using Microsoft.SharePoint.Client;
using System;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class ListPipeBind
    {
        private readonly List _list;
        private readonly Guid _id;
        private readonly string _name;

        public ListPipeBind()
        {
            _list = null;
            _id = Guid.Empty;
            _name = string.Empty;
        }

        public ListPipeBind(PSObject psObject)
        {
            if (psObject.Properties["Id"] != null && Guid.TryParse(psObject.Properties["Id"].Value.ToString(), out Guid result))
            {
                _id = result;
            }
        }

        public ListPipeBind(List list)
        {
            _list = list;
        }

        public ListPipeBind(Guid guid)
        {
            _id = guid;
        }

        public ListPipeBind(string id)
        {
            if (!Guid.TryParse(id, out _id))
            {
                _name = id;
            }
        }

        public Guid Id => _id;

        public List List => _list;

        public string Title => _name;

        public override string ToString()
        {
            return Title ?? Id.ToString();
        }

        internal List GetList(Web web)
        {
            List list = null;
            if (List != null)
            {
                list = List;
            }
            else if (Id != Guid.Empty)
            {
                list = web.Lists.GetById(Id);
            }
            else if (!string.IsNullOrEmpty(Title))
            {
                list = web.GetListByTitle(Title);
                if (list == null)
                {
                    list = web.GetListByUrl(Title);
                }
            }
            if (list != null)
            {
                web.Context.Load(list, l => l.Id, l => l.BaseTemplate, l => l.OnQuickLaunch, l => l.DefaultViewUrl, l => l.Title, l => l.Hidden, l => l.ContentTypesEnabled, l => l.RootFolder.ServerRelativeUrl);
                web.Context.ExecuteQueryRetry();
            }
            return list;
        }
    }
}