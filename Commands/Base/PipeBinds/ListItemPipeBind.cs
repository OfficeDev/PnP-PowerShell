using Microsoft.SharePoint.Client;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class ListItemPipeBind
    {
        private readonly ListItem _item;
        private readonly uint _id;

        public ListItemPipeBind()
        {
            _item = null;
            _id = uint.MinValue;
        }

        public ListItemPipeBind(ListItem item)
        {
            _item = item;
        }

        public ListItemPipeBind(string id)
        {
            uint uintId;

            if (uint.TryParse(id, out uintId))
            {
                _id = uintId;
            }
            else
            {
                _id = uint.MinValue;
            }
        }

        public ListItem Item => _item;

        public uint Id => _id;

        internal ListItem GetListItem(List list)
        {
            ListItem item = null;
            if (_id != uint.MinValue)
            {
                item = list.GetItemById((int)_id);
            }
            if (item == null && _item != null)
            {
                item = _item;
            }

            if (item != null)
            {
                list.Context.Load(item);
                list.Context.ExecuteQueryRetry();
            }
            return item;
        }
    }
}
