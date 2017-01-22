using System.Globalization;
using System.IO;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using SharePointPnP.PowerShell.CmdletHelpAttributes;

namespace SharePointPnP.PowerShell.Commands.Apps
{
    [Cmdlet(VerbsData.Import, "PnPAppPackage")]
    [CmdletAlias("Import-SPOAppPackage")]
    [CmdletHelp("Adds a SharePoint Addin to a site",
        DetailedDescription = "This commands requires that you have an addin package to deploy",
        Category = CmdletHelpCategory.Apps,
         OutputType = typeof(AppInstance),
        OutputTypeLink = "https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.appinstance.aspx")]
    [CmdletExample(
        Code = @"PS:> Import-PnPAppPackage -Path c:\files\demo.app -LoadOnly",
        Remarks = @"This will load the addin in the demo.app package, but will not install it to the site.
 ", SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> Import-PnPAppPackage -Path c:\files\demo.app -Force",
        Remarks = @"This load first activate the addin sideloading feature, upload and install the addin, and deactivate the addin sideloading feature.
    ", SortOrder = 2)]
    public class ImportAppPackage : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Path pointing to the .app file")]
        public string Path = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = "Will forcibly install the app by activating the addin sideloading feature, installing the addin, and deactivating the sideloading feature")]
        public SwitchParameter Force;

        [Parameter(Mandatory = false, HelpMessage = "Will only upload the addin, but not install it")]
        public SwitchParameter LoadOnly = false;

        [Parameter(Mandatory = false, HelpMessage = "Will install the addin for the specified locale")]
        public int Locale = -1;

        protected override void ExecuteCmdlet()
        {
            if (!System.IO.Path.IsPathRooted(Path))
            {
                Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
            }

            if (System.IO.File.Exists(Path))
            {
                if (Force)
                {
                    ClientContext.Site.ActivateFeature(Constants.FeatureId_Site_AppSideLoading);
                }
                AppInstance instance;


                var appPackageStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
                if (Locale == -1)
                {
                    if (LoadOnly)
                    {
                        instance = SelectedWeb.LoadApp(appPackageStream, CultureInfo.CurrentCulture.LCID);
                    }
                    else
                    {
                        instance = SelectedWeb.LoadAndInstallApp(appPackageStream);
                    }
                }
                else
                {
                    if (LoadOnly)
                    {
                        instance = SelectedWeb.LoadApp(appPackageStream, Locale);
                    }
                    else
                    {
                        instance = SelectedWeb.LoadAndInstallAppInSpecifiedLocale(appPackageStream, Locale);
                    }
                }
                ClientContext.Load(instance);
                ClientContext.ExecuteQueryRetry();


                if (Force)
                {
                    ClientContext.Site.DeactivateFeature(Constants.FeatureId_Site_AppSideLoading);
                }
                WriteObject(instance);
            }
            else
            {
                ThrowTerminatingError(new ErrorRecord(new IOException(Properties.Resources.FileDoesNotExist), "1", ErrorCategory.InvalidArgument, null));
            }
        }
    }
}
