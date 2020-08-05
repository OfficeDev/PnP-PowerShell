using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if !PNPPSCORE
using System.Windows.Forms;
#endif

namespace PnP.PowerShell.Commands.Utilities
{
    internal static class BrowserHelper
    {
#if !PNPPSCORE
        public static void LaunchBrowser(string url)
        {
            var thread = new Thread(() =>
            {
                var form = new System.Windows.Forms.Form();
                var browser = new System.Windows.Forms.WebBrowser
                {
                    ScriptErrorsSuppressed = true,
                    Dock = DockStyle.Fill
                };

                form.SuspendLayout();
                form.Width = 568;
                form.Height = 1012;
                form.Text = $"Authenticate";
                form.Controls.Add(browser);
                form.ResumeLayout(false);
                browser.Navigated += (sender, args) =>
                {
                    if (browser.Url.AbsoluteUri.Equals("https://login.microsoftonline.com/common/login", StringComparison.InvariantCultureIgnoreCase) || browser.Url.AbsoluteUri.StartsWith("https://login.microsoftonline.com/common/Consent/Set",StringComparison.InvariantCultureIgnoreCase))
                    //    ||browser.Url.AbsoluteUri.StartsWith("https://login.microsoftonline.com/common/reprocess", StringComparison.InvariantCultureIgnoreCase))
                    {
                        form.Close();
                    }
                };
                browser.Navigate(url);

                form.Focus();
                form.ShowDialog();
                browser.Dispose();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
#else
        public static void LaunchBrowser(string url)
        {
            if (OperatingSystem.IsWindows())
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); // Works ok on windows
            }
            else if (OperatingSystem.IsLinux())
            {
                Process.Start("xdg-open", url);  // Works ok on linux
            }
            else if (OperatingSystem.IsMacOS())
            {
                Process.Start("open", url); // Not tested
            }
        }
#endif
    }
}
