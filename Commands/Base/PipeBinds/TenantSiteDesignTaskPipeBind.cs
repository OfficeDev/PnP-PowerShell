#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using System;
using System.Linq;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class TenantSiteDesignTaskPipeBind
    {
        private readonly Guid _id;
        private readonly string _title;
        private readonly TenantSiteDesignTask _siteDesignTask;

        public TenantSiteDesignTaskPipeBind(Guid guid)
        {
            _id = guid;
        }

        public TenantSiteDesignTaskPipeBind(string id)
        {
            if (!Guid.TryParse(id, out _id))
            {
                _title = id;
            }
        }

        public TenantSiteDesignTaskPipeBind(TenantSiteDesignTask siteDesignTask)
        {
            _siteDesignTask = siteDesignTask;
        }

        public Guid Id
        {
            get
            {
                if (_siteDesignTask != null)
                {
                    return _siteDesignTask.Id;
                }
                else
                {
                    return _id;
                }
            }
        }

        public TenantSiteDesignTaskPipeBind()
        {
            _id = Guid.Empty;
            _siteDesignTask = null;
        }

    }
}
#endif