using System;
using Microsoft.SharePoint.Client;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class RecycleBinItemPipeBind
    {
        private RecycleBinItem _item;
        private readonly Guid? _id;

        public RecycleBinItemPipeBind()
        {
            _item = null;
            _id = null;
        }

        public RecycleBinItemPipeBind(RecycleBinItem item)
        {
            _item = item;
        }

        public RecycleBinItemPipeBind(string id)
        {
            Guid guid;

            if (Guid.TryParse(id, out guid))
            {
                _id = guid;
            }
            else
            {
                _id = null;
            }
        }

        public RecycleBinItem Item => _item;

        public Guid? Id => _id;

        internal RecycleBinItem GetRecycleBinItem(Microsoft.SharePoint.Client.Site site)
        {
            if (Item != null) return Item;
            if (!_id.HasValue) return null;

            _item = site.RecycleBin.GetById(_id.Value);
            site.Context.Load(_item);
            site.Context.ExecuteQueryRetry();
            return Item;
        }
    }
}
