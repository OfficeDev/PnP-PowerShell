using Microsoft.SharePoint.Client;
using System;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class FolderPipeBind
    {
        private readonly Folder _folder;
        private readonly Guid _id;
        private readonly string _name;

        public FolderPipeBind()
        {
            _folder = null;
            _id = Guid.Empty;
            _name = string.Empty;
        }

        public FolderPipeBind(Folder folder)
        {
            _folder = folder;
        }

#if !SP2013
        public FolderPipeBind(Guid guid)
        {
            _id = guid;
        }
#endif

        public FolderPipeBind(string id)
        {
            if (!Guid.TryParse(id, out _id))
            {
                _name = id;
            }
        }

#if !SP2013
        public Guid Id => _id;
#endif

        public Folder Folder => _folder;

        public string ServerRelativeUrl => _name;

        internal Folder GetFolder(Web web)
        {
            Folder folder = null;
            if (Folder != null)
            {
                folder = Folder;
            }
#if !SP2013
            else if (Id != Guid.Empty)
            {
                folder = web.GetFolderById(Id);
            }
#endif
            else if (!string.IsNullOrEmpty(ServerRelativeUrl))
            {
                folder = web.GetFolderByServerRelativeUrl(ServerRelativeUrl);
            }
            if (folder != null)
            {
                web.Context.Load(folder);
                web.Context.ExecuteQueryRetry();
            }
            return folder;
        }
    }
}
