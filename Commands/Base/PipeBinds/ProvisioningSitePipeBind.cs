#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using System;
using System.Linq;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class ProvisioningSitePipeBind
    {
        private readonly SiteCollection _site;

        public ProvisioningSitePipeBind(TeamSiteCollection site)
        {
            _site = site;
        }

        public ProvisioningSitePipeBind(TeamNoGroupSiteCollection site)
        {
            _site = site;
        }

        public ProvisioningSitePipeBind(CommunicationSiteCollection site)
        {
            _site = site;
        }

        public SiteCollection Site => _site;

        public SiteCollection GetSiteFromSequence(ProvisioningSequence sequence)
        {
            return sequence.SiteCollections.FirstOrDefault(s => s.Id == _site.Id);
        }
    }
}
#endif