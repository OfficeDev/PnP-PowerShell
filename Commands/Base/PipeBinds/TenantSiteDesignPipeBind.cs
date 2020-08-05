#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using System;
using System.Linq;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class TenantSiteDesignPipeBind
    {
        private readonly Guid _id;
        private readonly string _title;
        private readonly TenantSiteDesign _siteDesign;

        public TenantSiteDesignPipeBind(Guid guid)
        {
            _id = guid;
        }

        public TenantSiteDesignPipeBind(string id)
        {
            if (!Guid.TryParse(id, out _id))
            {
                _title = id;
            }
        }

        public TenantSiteDesignPipeBind(TenantSiteDesign siteDesign)
        {
            _siteDesign = siteDesign;
        }

        public Guid Id
        {
            get
            {
                if (_siteDesign != null)
                {
                    return _siteDesign.Id;
                }
                else
                {
                    return _id;
                }
            }
        }

        public TenantSiteDesign GetTenantSiteDesign(Tenant tenant)
        {
            if (_siteDesign != null)
            {
                return _siteDesign;
            }
            if (!string.IsNullOrEmpty(_title))
            {
                var designs = tenant.GetSiteDesigns();
                var result = tenant.Context.LoadQuery(designs.Where(d => d.Title == _title));
                (tenant.Context as ClientContext).ExecuteQueryRetry();
                return result.FirstOrDefault();
            }
            else if (_id != Guid.Empty)
            {
                var design = Tenant.GetSiteDesign(tenant.Context, Id);
                tenant.Context.Load(design);
                (tenant.Context as ClientContext).ExecuteQueryRetry();
                return design;
            }
            return null;
        }

        public TenantSiteDesignPipeBind()
        {
            _id = Guid.Empty;
            _siteDesign = null;
        }

    }
}
#endif