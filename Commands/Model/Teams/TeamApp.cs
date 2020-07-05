using System;
using System.Collections.Generic;
using System.Text;

namespace SharePointPnP.PowerShell.Commands.Model.Teams
{
   public partial class TeamApp
    {
        public string DisplayName { get; set; }
        public string DistributionMethod { get; set; }
        public string ExternalId { get; set; }
        public string Id { get; set; }
    }
}
