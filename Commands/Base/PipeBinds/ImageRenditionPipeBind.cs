using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class ImageRenditionPipeBind
    {
        private ImageRendition _item;
        private readonly int? _id;
        private readonly string _name;
        public ImageRenditionPipeBind()
        {
            _item = null;
            _id = null;
        }

        public ImageRenditionPipeBind(ImageRendition item)
        {
            _item = item;
        }

        public ImageRenditionPipeBind(string idOrName)
        {
            int id;

            if (int.TryParse(idOrName, out id))
            {
                _id = id;
                _name = null;
            }
            else
            {
                _id = null;
                _name = idOrName;
            }
        }

        public ImageRendition Item => _item;

        public int? Id => _id;

        internal ImageRendition GetImageRendition(Web web)
        {
            if (Item != null) return Item;
            var items = web.GetPublishingImageRenditions();
            if (_id.HasValue)
            {
                return items.FirstOrDefault(i => i.Id == _id);
            }

            return items.FirstOrDefault(i => i.Name == _name);
        }
    }
}
