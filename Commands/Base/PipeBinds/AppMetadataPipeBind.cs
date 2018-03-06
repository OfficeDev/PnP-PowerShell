#if !ONPREMISES
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.ALM;
using System;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class AppMetadataPipeBind
    {
        private readonly Guid _id;
        private readonly string _title;

        public AppMetadataPipeBind(AppMetadata metadata)
        {
            //_appMetadata = metadata;
            _id = metadata.Id;
        }


        public AppMetadataPipeBind(Guid guid)
        {
            _id = guid;
        }

        public AppMetadataPipeBind(string id)
        {
            if(!Guid.TryParse(id, out _id))
            {
                _title = id;
            }
        }

        public Guid Id => _id;

        public string Title => _title;
    }

}
#endif