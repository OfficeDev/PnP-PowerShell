using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Graph;
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

        public UnifiedGroupEntity GetGroup(string accessToken)
        {
            UnifiedGroupEntity group = null;
            if (Group != null)
            {
                group = UnifiedGroupsUtility.GetUnifiedGroup(Group.GroupId, accessToken);
            }
            else if (!String.IsNullOrEmpty(GroupId))
            {
                group = UnifiedGroupsUtility.GetUnifiedGroup(GroupId, accessToken);
            }
            else if (!string.IsNullOrEmpty(DisplayName))
            {
                var groups = UnifiedGroupsUtility.ListUnifiedGroups(accessToken, DisplayName, includeSite: true);
                if (groups == null || groups.Count == 0)
                {
                    groups = UnifiedGroupsUtility.ListUnifiedGroups(accessToken, mailNickname: DisplayName, includeSite: true);
                }
                if (groups != null && groups.Any())
                {
                    group = groups.FirstOrDefault();
                }
            }
            return group;
        }
    }
}
