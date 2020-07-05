using System;
using System.Collections.Generic;
using System.Text;

namespace SharePointPnP.PowerShell.Core.Attributes
{
    public enum TokenType : short
    {
        All = 0,
        Application = 1,
        Delegate = 2
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class CmdletTokenTypeAttribute : Attribute
    {
        public TokenType TokenType { get; set; }

        public CmdletTokenTypeAttribute(TokenType tokenType = TokenType.All)
        {
            TokenType = tokenType;
        }
    }
}
