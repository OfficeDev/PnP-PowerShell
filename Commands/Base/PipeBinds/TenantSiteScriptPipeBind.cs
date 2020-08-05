#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using System;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class TenantSiteScriptPipeBind
    {
        private readonly Guid _id;
        private readonly TenantSiteScript _siteScript;

        public TenantSiteScriptPipeBind(Guid guid)
        {
            _id = guid;
        }

        public TenantSiteScriptPipeBind(string id)
        {
            _id = new Guid(id);
        }

        public TenantSiteScriptPipeBind(TenantSiteScript siteScript)
        {
            _siteScript = siteScript;
        }

        public Guid Id
        {
            get
            {
                if(_siteScript != null)
                {
                    return _siteScript.Id;
                } else
                {
                    return _id;
                }
            }
        }

        public TenantSiteScriptPipeBind()
        {
            _id = Guid.Empty;
            _siteScript = null;
        }

    }
}
#endif