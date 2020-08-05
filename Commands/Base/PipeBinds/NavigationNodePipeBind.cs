using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public class NavigationNodePipeBind
    {
        private int _id;

        public NavigationNodePipeBind(int id)
        {
            _id = id;
        }

        public NavigationNodePipeBind(NavigationNode node)
        {
            _id = node.Id;
        }

        public int Id => _id;
    }
}
