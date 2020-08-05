using System.Management.Automation;

namespace PnP.PowerShell.Commands.Provider.Parameters
{
    public class SPOChildItemsParameters
    {
        [Parameter(HelpMessage = "Limit items collected when using list query")]
        public Limits Limit { get; set; }

        public enum Limits
        {
            Default,
            All = -1
        }
    }
}