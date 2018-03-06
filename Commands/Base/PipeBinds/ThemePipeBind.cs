#if !ONPREMISES
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.Commands.Model;
using System;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class ThemePipeBind
    {
        private readonly string _name;

        public ThemePipeBind()
        {
            _name = string.Empty;
        }

        public ThemePipeBind(string name)
        {
            _name = name;
        }

        public ThemePipeBind(SPOTheme theme)
        {
            _name = theme.Name;
        }

        public string Name => _name;
    }
}
#endif