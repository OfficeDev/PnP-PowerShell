using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PnP.PowerShell.Commands.Model.Teams
{
    public class GraphException : Exception
    {
        public GraphError Error { get; set; }
    }

    public class GraphError
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public GraphError InnerError { get; set; }

        public Dictionary<string,object> AdditionalData { get; set; }

        public string ThrowSite { get; set; }
    }
}
