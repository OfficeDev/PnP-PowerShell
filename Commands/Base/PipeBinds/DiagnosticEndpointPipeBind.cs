using Microsoft.SharePoint.Client;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class DiagnosticEndpointPipeBind
    {
        private string _serverRelativeUrl;
        private Web _web;
        private File _file;
        private Folder _folder;

        public DiagnosticEndpointPipeBind()
        {
            _serverRelativeUrl = null;
            _web = null;
            _file = null;
            _folder = null;
        }

        public DiagnosticEndpointPipeBind(string serverRelativeUrl)
        {
            _serverRelativeUrl = serverRelativeUrl;
        }

        public DiagnosticEndpointPipeBind(Web web)
        {
            _web = web;
        }

        public DiagnosticEndpointPipeBind(File file)
        {
            _file = file;
        }

        public DiagnosticEndpointPipeBind(Folder folder)
        {
            _folder = folder;
        }

        public override string ToString()
        {
            if (_serverRelativeUrl == null)
            {
                if (_web != null)
                {
                    _web.EnsureProperty(w => w.RootFolder);
                    _folder = _web.RootFolder;
                }
                if (_folder != null)
                {
                    _folder.EnsureProperties(f => f.WelcomePage, f => f.ServerRelativeUrl);
                    _serverRelativeUrl = System.IO.Path.Combine(_folder.ServerRelativeUrl, _folder.WelcomePage);
                }
                if (_file != null)
                {
                    _file.EnsureProperty(f => f.ServerRelativeUrl);
                    _serverRelativeUrl = _file.ServerRelativeUrl;
                }
            }
            return _serverRelativeUrl;
        }
    }

}
