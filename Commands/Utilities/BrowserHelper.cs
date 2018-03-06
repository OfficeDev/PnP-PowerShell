#if !NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharePointPnP.PowerShell.Commands.Utilities
{
    internal static class BrowserHelper
    {
        public static void OpenBrowser(string url, Action<bool> success, System.Drawing.Icon icon = null)
        {
            var thread = new Thread(() =>
            {
                var form = new System.Windows.Forms.Form();
                if (icon != null)
                {
                    form.Icon = icon;
                }
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
                form.FormClosed += (sender, args) =>
                {
                    success(false);
                };
                browser.Navigated += (sender, args) =>
                {
                    if(browser.DocumentText.Contains("You have signed in to the PnP Office 365 Management Shell application on your device. You may now close this window."))
                    {
                        form.Close();
                        success(true);
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
    }
}
#endif