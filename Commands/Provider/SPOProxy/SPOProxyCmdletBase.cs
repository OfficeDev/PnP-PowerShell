using System.Management.Automation;

namespace PnP.PowerShell.Commands.Provider.SPOProxy
{
    public abstract class SPOProxyCmdletBase : PSCmdlet
    {
        internal string[] PsPaths { get; private set; }

        internal bool ShouldExpandWildcards { get; private set; }

        [Parameter(Mandatory = true, ParameterSetName = "Path", Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public virtual string[] Path
        {
            get { return PsPaths; }
            set
            {
                ShouldExpandWildcards = true;
                PsPaths = value;
            }
        }

        [Parameter(Mandatory = true, ParameterSetName = "LiteralPath", Position = 0, ValueFromPipeline = false, ValueFromPipelineByPropertyName = true)]
        [Alias("PSPath")]
        public virtual string[] LiteralPath
        {
            get { return PsPaths; }
            set { PsPaths = value; }
        }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true)]
        public virtual string Destination { get; set; }

        [Parameter]
        public virtual SwitchParameter Container { get; set; }

        [Parameter]
        public virtual SwitchParameter Force { get; set; }

        [Parameter]
        public virtual string Filter { get; set; }

        [Parameter]
        public virtual string[] Include { get; set; }

        [Parameter]
        public virtual string[] Exclude { get; set; }

        //[Parameter]
        public virtual SwitchParameter Recurse { get; set; }

        [Parameter]
        public virtual SwitchParameter PassThru { get; set; }

        [Credential]
        [Parameter(ValueFromPipelineByPropertyName = true)]
        public virtual PSCredential Credential { get; set; }

        internal virtual string CmdletType { get; }

        public const string CmdletNoun = "PnPItemProxy";
    }
}