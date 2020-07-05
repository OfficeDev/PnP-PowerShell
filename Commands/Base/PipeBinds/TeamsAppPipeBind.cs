using System;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class TeamsAppPipeBind
    {
        private readonly Guid _id;
        private readonly string _stringValue;

        public TeamsAppPipeBind()

        {
            _id = Guid.Empty;
        }

        public TeamsAppPipeBind(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException(nameof(input));
            }
            if (Guid.TryParse(input, out Guid tabId))
            {
                _id = tabId;
            }
            else
            {
                _stringValue = input;
            }
        }

        public Guid Id => _id;

        public string StringValue => _stringValue;
    }
}
