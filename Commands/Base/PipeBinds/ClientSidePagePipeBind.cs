#if !ONPREMISES
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Pages;
using SharePointPnP.PowerShell.Commands.ClientSidePages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class ClientSidePagePipeBind
    {
        private readonly ClientSidePage _page;
        private string _name;

        public ClientSidePagePipeBind(ClientSidePage page)
        {
            _page = page;
            if (page.PageListItem != null)
            {
                File file = page.PageListItem.EnsureProperty(li => li.File);
                _name = file.EnsureProperty(f => f.Name);
            }
            else
            {
                _name = page.PageTitle;
            }
        }

        public ClientSidePagePipeBind(string name)
        {
            _page = null;
            _name = name;
        }

        public ClientSidePage Page => _page;

        public string Name => ClientSidePageUtilities.EnsureCorrectPageName(_name);

        public override string ToString() => Name;

        internal ClientSidePage GetPage(ClientContext ctx)
        {
            if (_page != null)
            {
                return _page;
            }
            else if (!string.IsNullOrEmpty(_name))
            {
                try
                {
                    return ClientSidePage.Load(ctx, Name);
                }
                catch (ArgumentException ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
#endif