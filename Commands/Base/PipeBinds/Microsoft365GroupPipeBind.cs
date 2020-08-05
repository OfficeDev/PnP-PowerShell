using OfficeDevPnP.Core.Entities;
using OfficeDevPnP.Core.Framework.Graph;
using System;
using System.Linq;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public class Microsoft365GroupPipeBind
    {
        private readonly UnifiedGroupEntity _group;
        private readonly String _groupId;
        private readonly String _displayName;

        public Microsoft365GroupPipeBind()
        {
        }

        public Microsoft365GroupPipeBind(UnifiedGroupEntity group)
        {
            _group = group;
        }

        public Microsoft365GroupPipeBind(String input)
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

        public UnifiedGroupEntity GetGroup(string accessToken, bool includeClassification = false)
        {
            UnifiedGroupEntity group = null;
            if (Group != null)
            {
                group = UnifiedGroupsUtility.GetUnifiedGroup(Group.GroupId, accessToken, includeClassification:includeClassification);
            }
            else if (!String.IsNullOrEmpty(GroupId))
            {
                group = UnifiedGroupsUtility.GetUnifiedGroup(GroupId, accessToken, includeClassification:includeClassification);
            }
            else if (!string.IsNullOrEmpty(DisplayName))
            {
                var groups = UnifiedGroupsUtility.GetUnifiedGroups(accessToken, DisplayName, includeSite: true, includeClassification:includeClassification);
                if (groups == null || groups.Count == 0)
                {
                    groups = UnifiedGroupsUtility.GetUnifiedGroups(accessToken, mailNickname: DisplayName, includeSite: true, includeClassification:includeClassification);
                }
                if (groups != null && groups.Any())
                {
                    group = groups.FirstOrDefault();
                }
            }
            return group;
        }

        public UnifiedGroupEntity GetDeletedGroup(string accessToken)
        {
            UnifiedGroupEntity group = null;

            if (Group != null)
            {
                group = UnifiedGroupsUtility.GetDeletedUnifiedGroup(Group.GroupId, accessToken);
            }
            else if (!string.IsNullOrEmpty(GroupId))
            {
                group = UnifiedGroupsUtility.GetDeletedUnifiedGroup(GroupId, accessToken);
            }

            return group;
        }
    }
}
