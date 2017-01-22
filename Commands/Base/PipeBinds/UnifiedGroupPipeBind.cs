using OfficeDevPnP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public class UnifiedGroupPipeBind
    {
        private readonly UnifiedGroupEntity _group;
        private readonly String _groupId;
        private readonly String _displayName;

        public UnifiedGroupPipeBind()
        {
        }

        public UnifiedGroupPipeBind(UnifiedGroupEntity group)
        {
            _group = group;
        }

        public UnifiedGroupPipeBind(String input)
        {
            Guid idValue;
            if (Guid.TryParse(input, out idValue))
            {
                _groupId = input;
            }
            else
            {
                _displayName = input;
            }
        }

        public UnifiedGroupEntity Group => (_group);

        public String DisplayName => (_displayName);

        public String GroupId => (_groupId);
    }
}
