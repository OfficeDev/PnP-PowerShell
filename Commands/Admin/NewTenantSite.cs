using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
#if ONPREMISES
using OfficeDevPnP.Core.Entities;
#endif
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base;
using System;
using System.Management.Automation;
using Resources = PnP.PowerShell.Commands.Properties.Resources;

namespace PnP.PowerShell.Commands
{
    [Cmdlet(VerbsCommon.New, "PnPTenantSite")]
    [CmdletHelp("Creates a new (classic) site collection for the current tenant",
        @"The New-PnPTenantSite cmdlet creates a new site collection for the current company. However, creating a new SharePoint
Online site collection fails if a deleted site with the same URL exists in the Recycle Bin. If you want to use this command for an on-premises farm, please refer to http://blogs.msdn.com/b/vesku/archive/2014/06/09/provisioning-site-collections-using-sp-app-model-in-on-premises-with-just-csom.aspx ",
        Category = CmdletHelpCategory.TenantAdmin)]
    [CmdletExample(
        Code = @"PS:> New-PnPTenantSite -Title Contoso -Url https://tenant.sharepoint.com/sites/contoso -Owner user@example.org -TimeZone 4 -Template STS#0",
        Remarks = @"This will add a site collection with the title 'Contoso', the url 'https://tenant.sharepoint.com/sites/contoso', the timezone 'UTC+01:00',the owner 'user@example.org' and the template used will be STS#0, a TeamSite",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"PS:> New-PnPTenantSite -Title Contoso -Url /sites/contososite -Owner user@example.org -TimeZone 4 -Template STS#0",
        Remarks = @"This will add a site collection with the title 'Contoso', the url 'https://tenant.sharepoint.com/sites/contososite' of which the base part will be picked up from your current connection, the timezone 'UTC+01:00', the owner 'user@example.org' and the template used will be STS#0, a TeamSite",
        SortOrder = 2)]
    [CmdletRelatedLink(
        Text = "Locale IDs",
        Url = "https://github.com/pnp/PnP-PowerShell/wiki/Supported-LCIDs-by-SharePoint")]
    [CmdletRelatedLink(
        Text = "Resource Usage Limits on Sandboxed Solutions in SharePoint 2010",
        Url = "https://docs.microsoft.com/previous-versions/office/developer/sharepoint-2010/gg615462(v=office.14)")]
    [CmdletRelatedLink(
        Text = "Creating on-premises site collections using CSOM",
        Url = "http://blogs.msdn.com/b/vesku/archive/2014/06/09/provisioning-site-collections-using-sp-app-model-in-on-premises-with-just-csom.aspx")]
    public class NewTenantSite : PnPAdminCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"Specifies the title of the new site collection")]
        public string Title;

        [Parameter(Mandatory = true, HelpMessage = @"Specifies the full URL of the new site collection. It must be in a valid managed path in the company's site. For example, for company contoso, valid managed paths are https://contoso.sharepoint.com/sites and https://contoso.sharepoint.com/teams.")]
        public string Url;

        [Obsolete("This parameter is currently ignored due to server side API issues when setting this value.")]
        [Parameter(Mandatory = false, HelpMessage = @"Specifies the description of the new site collection. Setting a value for this parameter will override the Wait parameter as we have to set the description after the site has been created.")]
        public string Description = string.Empty;

        [Parameter(Mandatory = true, HelpMessage = @"Specifies the user name of the site collection's primary owner. The owner must be a user instead of a security group or an email-enabled security group.")]
        public string Owner = string.Empty;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the language of this site collection. For more information, see Locale IDs supported by SharePoint at https://github.com/pnp/PnP-PowerShell/wiki/Supported-LCIDs-by-SharePoint. To get the list of supported languages on a SharePoint environment use: Get-PnPAvailableLanguage.")]
        public uint Lcid = 1033;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the site collection template type. Use the Get-PnPWebTemplates cmdlet to get the list of valid templates. If no template is specified, one can be added later. The Template and LocaleId parameters must be a valid combination as returned from the Get-PnPWebTemplates cmdlet.")]
        public string Template = "STS#0";

#if ONPREMISES
        [Parameter(Mandatory = false, HelpMessage = "Use Get-PnPTimeZoneId to retrieve possible timezone values")]
#else
        [Parameter(Mandatory = true, HelpMessage = "Use Get-PnPTimeZoneId to retrieve possible timezone values")]
#endif
        public int TimeZone;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the quota for this site collection in Sandboxed Solutions units. This value must not exceed the company's aggregate available Sandboxed Solutions quota. The default value is 0. For more information, see Resource Usage Limits on Sandboxed Solutions in SharePoint 2010 : http://msdn.microsoft.com/en-us/library/gg615462.aspx.")]
        public double ResourceQuota = 0;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the warning level for the resource quota. This value must not exceed the value set for the ResourceQuota parameter")]
        public double ResourceQuotaWarningLevel = 0;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the storage quota for this site collection in megabytes. This value must not exceed the company's available quota.")]
        public long StorageQuota = 100;

        [Parameter(Mandatory = false, HelpMessage = @"Specifies the warning level for the storage quota in megabytes. This value must not exceed the values set for the StorageQuota parameter")]
        public long StorageQuotaWarningLevel = 100;

#if !ONPREMISES
        [Parameter(Mandatory = false, HelpMessage = "Specifies if any existing site with the same URL should be removed from the recycle bin")]
        public SwitchParameter RemoveDeletedSite;
#endif
        [Parameter(Mandatory = false)]
        public SwitchParameter Wait;

        [Parameter(Mandatory = false, HelpMessage = "Do not ask for confirmation.")]
        public SwitchParameter Force;

        protected override void ExecuteCmdlet()
        {
            bool shouldContinue = true;
            if (!Url.ToLower().StartsWith("https://") && !Url.ToLower().StartsWith("http://"))
            {
                Uri uri = BaseUri;
                Url = $"{uri.ToString().TrimEnd('/')}/{Url.TrimStart('/')}";
                if (!Force)
                {
                    shouldContinue = ShouldContinue(string.Format(Resources.CreateSiteWithUrl0, Url), Resources.Confirm);
                }
            }
            if (shouldContinue)
            {
#if ONPREMISES
                var entity = new SiteEntity();
                entity.Url = Url;
                entity.Title = Title;
                entity.SiteOwnerLogin = Owner;
                entity.Template = Template;
                entity.StorageMaximumLevel = StorageQuota;
                entity.StorageWarningLevel = StorageQuotaWarningLevel;
                entity.TimeZoneId = TimeZone;
                entity.UserCodeMaximumLevel = ResourceQuota;
                entity.UserCodeWarningLevel = ResourceQuotaWarningLevel;
                entity.Lcid = Lcid;
                Tenant.CreateSiteCollection(entity);
#else
                Func<TenantOperationMessage, bool> timeoutFunction = TimeoutFunction;

#pragma warning disable CS0618 // Type or member is obsolete
                if (ParameterSpecified(nameof(Description)))
#pragma warning restore CS0618 // Type or member is obsolete
                {
                    // We have to fall back to synchronous behaviour as we have to wait for the site to be present in order to set the description.
                    Wait = true;
                }

                Tenant.CreateSiteCollection(Url, Title, Owner, Template, (int)StorageQuota,
                    (int)StorageQuotaWarningLevel, TimeZone, (int)ResourceQuota, (int)ResourceQuotaWarningLevel, Lcid,
                    RemoveDeletedSite, Wait, Wait == true ? timeoutFunction : null);
#endif
            }
        }

#if !ONPREMISES
        private bool TimeoutFunction(TenantOperationMessage message)
        {
            if (message == TenantOperationMessage.CreatingSiteCollection)
            {
                Host.UI.Write(".");
            }
            return Stopping;
        }
#endif
    }
}
