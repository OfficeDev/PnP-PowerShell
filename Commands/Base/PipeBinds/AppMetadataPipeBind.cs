#if !SP2013 && !SP2016
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.ALM;
using OfficeDevPnP.Core.Enums;
using System;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class AppMetadataPipeBind
    {
        private readonly Guid _id;
        private readonly string _title;
        private readonly AppMetadata _metadata;

        public AppMetadataPipeBind(AppMetadata metadata)
        {
            _metadata = metadata;
        }


        public AppMetadataPipeBind(Guid guid)
        {
            _id = guid;
        }

        public AppMetadataPipeBind(string id)
        {
            if (!Guid.TryParse(id, out _id))
            {
                _title = id;
            }
        }

        public Guid Id => _id;

        public string Title => _title;

        public AppMetadata GetAppMetadata(ClientContext context, AppCatalogScope scope)
        {
            var appmanager = new AppManager(context);
            if (_id != Guid.Empty)
            {
                return appmanager.GetAvailable(_id, scope);
            }
            if (!string.IsNullOrEmpty(_title))
            {
                return appmanager.GetAvailable(_title, scope);
            }
            return _metadata;
        }
    }

}
#endif