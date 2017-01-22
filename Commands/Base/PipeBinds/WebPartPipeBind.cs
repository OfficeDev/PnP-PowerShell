using System;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public class WebPartPipeBind
    {
        private readonly Guid _id;
        private readonly string _title;

        public WebPartPipeBind(Guid guid)
        {
            _id = guid;
        }

        public WebPartPipeBind(string id)
        {
            if (!Guid.TryParse(id, out _id))
            {
                _title = id;
            }
        }

        public Guid Id => _id;

        public string Title => _title;

        public WebPartPipeBind()
        {
            _id = Guid.Empty;
            _title = string.Empty;
        }
    }
}
