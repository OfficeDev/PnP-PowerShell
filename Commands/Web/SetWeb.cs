using System.Management.Automation;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.Set, "SPOWeb")]
    [CmdletHelp("Sets properties on a web",
        Category = CmdletHelpCategory.Webs)]
    public class SetWeb : SPOWebCmdlet
    {
        [Parameter(Mandatory = false)]
        public string SiteLogoUrl;

        [Parameter(Mandatory = false)]
        public string AlternateCssUrl;

        [Parameter(Mandatory = false)]
        public string Title;

        [Parameter(Mandatory = false)]
        public string Description;

        [Parameter(Mandatory = false)]
        public string MasterUrl;

        [Parameter(Mandatory = false)]
        public string CustomMasterUrl;

        [Parameter(Mandatory = false)]
        public string RequestAccessEmail;

        protected override void ExecuteCmdlet()
        {
            if (SiteLogoUrl != null)
            {
                SelectedWeb.SiteLogoUrl = SiteLogoUrl;
                SelectedWeb.Update();
            }
            if (!string.IsNullOrEmpty(AlternateCssUrl))
            {
                SelectedWeb.AlternateCssUrl = AlternateCssUrl;
                SelectedWeb.Update();
            }
            if(!string.IsNullOrEmpty(Title))
            {
                SelectedWeb.Title = Title;
                SelectedWeb.Update();
            }

            if (!string.IsNullOrEmpty(Description))
            {
                SelectedWeb.Description = Description;
                SelectedWeb.Update();
            }

            if (!string.IsNullOrEmpty(MasterUrl))
            {
                SelectedWeb.MasterUrl = MasterUrl;
                SelectedWeb.Update();
            }

            if (!string.IsNullOrEmpty(CustomMasterUrl))
            {
                SelectedWeb.CustomMasterUrl = CustomMasterUrl;
                SelectedWeb.Update();
            }

            if (!string.IsNullOrEmpty(RequestAccessEmail))
            {
                SelectedWeb.RequestAccessEmail = RequestAccessEmail;
                SelectedWeb.Update();
            }

            ClientContext.ExecuteQueryRetry();
        }
    }

}
