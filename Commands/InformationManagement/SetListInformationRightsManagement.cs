using System;
using System.Management.Automation;
using Microsoft.SharePoint.Client;
using PnP.PowerShell.CmdletHelpAttributes;
using PnP.PowerShell.Commands.Base.PipeBinds;

namespace PnP.PowerShell.Commands.InformationManagement
{

    [Cmdlet(VerbsCommon.Set, "PnPListInformationRightsManagement")]
    [CmdletHelp("Get the site closure status of the site which has a site policy applied", Category = CmdletHelpCategory.InformationManagement)]
    [CmdletExample(
      Code = @"PS:> Set-PnPListInformationRightsManagement -List ""Documents"" -Enabled $true",
      Remarks = @"Enables Information Rights Management (IRM) on the list.", SortOrder = 1)]
    public class SetListInformationRightsManagement : PnPWebCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The list to set Information Rights Management (IRM) settings for.")]
        public ListPipeBind List;

        [Parameter(Mandatory = false, HelpMessage = "Specifies whether Information Rights Management (IRM) is enabled for the list.")]
        public bool? Enable;

        [Parameter(Mandatory = false, HelpMessage = "Specifies whether Information Rights Management (IRM) expiration is enabled for the list.")]
        public bool? EnableExpiration;

        [Parameter(Mandatory = false, HelpMessage = "Specifies whether Information Rights Management (IRM) rejection is enabled for the list.")]
        public bool? EnableRejection;

        [Parameter(Mandatory = false, HelpMessage = "Sets a value indicating whether the viewer can print the downloaded document.")]
        public bool? AllowPrint;

        [Parameter(Mandatory = false, HelpMessage = "Sets a value indicating whether the viewer can run a script on the downloaded document.")]
        public bool? AllowScript;

        [Parameter(Mandatory = false, HelpMessage = "Sets a value indicating whether the viewer can write on a copy of the downloaded document.")]
        public bool? AllowWriteCopy;

        [Parameter(Mandatory = false, HelpMessage = "Sets a value indicating whether to block Office Web Application Companion applications (WACs) from showing this document.")]
        public bool? DisableDocumentBrowserView;

        [Parameter(Mandatory = false, HelpMessage = "Sets the number of days after which the downloaded document will expire.")]
        public int? DocumentAccessExpireDays;

        [Parameter(Mandatory = false, HelpMessage = "Sets the date after which the Information Rights Management (IRM) protection of this document library will stop.")]
        public DateTime? DocumentLibraryProtectionExpireDate;

        [Parameter(Mandatory = false, HelpMessage = "Sets a value indicating whether the downloaded document will expire.")]
        public bool? EnableDocumentAccessExpire;

        [Parameter(Mandatory = false, HelpMessage = "Sets a value indicating whether to enable Office Web Application Companion applications (WACs) to publishing view.")]
        public bool? EnableDocumentBrowserPublishingView;

        [Parameter(Mandatory = false, HelpMessage = "Sets a value indicating whether the permission of the downloaded document is applicable to a group.")]
        public bool? EnableGroupProtection;

        [Parameter(Mandatory = false, HelpMessage = "Sets whether a user must verify their credentials after some interval.")]
        public bool? EnableLicenseCacheExpire;

        [Parameter(Mandatory = false, HelpMessage = "Sets the number of days that the application that opens the document caches the IRM license. When these elapse, the application will connect to the IRM server to validate the license.")]
        public int? LicenseCacheExpireDays;

        [Parameter(Mandatory = false, HelpMessage = "Sets the group name (email address) that the permission is also applicable to.")]
        public string GroupName;

        [Parameter(Mandatory = false, HelpMessage = "Sets the permission policy description.")]
        public string PolicyDescription;

        [Parameter(Mandatory = false, HelpMessage = "Sets the permission policy title.")]
        public string PolicyTitle;

        [Parameter(Mandatory = false)]
        public string TemplateId;

        protected override void ExecuteCmdlet()
        {
            var list = List.GetList(SelectedWeb, l => l.InformationRightsManagementSettings, l => l.IrmEnabled, l => l.IrmExpire, l => l.IrmReject);
            if (list == null)
                throw new PSArgumentException($"No list found with id, title or url '{List}'", "List");

            if (list.IrmEnabled == false && !Enable.HasValue)
            {
                WriteWarning("Information Rights Management is currently disabled for this list. Enable with Set-PnPListInformationRightsManagement -Enable $true");
            }
            else
            {
                var isDirty = false;
                     
                if (Enable.HasValue)
                {
                    list.IrmEnabled = Enable.Value;
                    isDirty = true;
                }
                if(EnableExpiration.HasValue)
                {
                    list.IrmExpire = EnableExpiration.Value;
                    isDirty = true;
                }
                if(EnableRejection.HasValue)
                {
                    list.IrmReject = EnableRejection.Value;
                    isDirty = true;
                }
                if(isDirty)
                {
                    list.Update();
                    ClientContext.Load(list, l => l.InformationRightsManagementSettings, l => l.IrmEnabled, l => l.IrmExpire, l => l.IrmReject);
                    ClientContext.ExecuteQueryRetry();
                }

                if (list.IrmEnabled)
                {
                    // Enablers
                    isDirty = false;
                    if (EnableDocumentAccessExpire.HasValue)
                    {
                        list.InformationRightsManagementSettings.EnableDocumentAccessExpire = EnableDocumentAccessExpire.Value;
                        isDirty = true;
                    }

                    if (EnableDocumentBrowserPublishingView.HasValue)
                    {
                        list.InformationRightsManagementSettings.EnableDocumentBrowserPublishingView = EnableDocumentBrowserPublishingView.Value;
                        isDirty = true;
                    }

                    if (EnableGroupProtection.HasValue)
                    {
                        list.InformationRightsManagementSettings.EnableGroupProtection = EnableGroupProtection.Value;
                        isDirty = true;
                    }

                    if (EnableLicenseCacheExpire.HasValue)
                    {
                        list.InformationRightsManagementSettings.EnableLicenseCacheExpire = EnableLicenseCacheExpire.Value;
                        isDirty = true;
                    }

                    if (DisableDocumentBrowserView.HasValue)
                    {
                        list.InformationRightsManagementSettings.DisableDocumentBrowserView = DisableDocumentBrowserView.Value;
                        isDirty = true;
                    }

                    if (isDirty)
                    {
                        list.Update();
                        ClientContext.ExecuteQueryRetry();
                    }

                    // Properties
                    isDirty = false;
                    if (AllowPrint.HasValue)
                    {
                        list.InformationRightsManagementSettings.AllowPrint = AllowPrint.Value;
                        isDirty = true;
                    }

                    if (AllowScript.HasValue)
                    {
                        list.InformationRightsManagementSettings.AllowScript = AllowScript.Value;
                        isDirty = true;
                    }

                    if (AllowWriteCopy.HasValue)
                    {
                        list.InformationRightsManagementSettings.AllowWriteCopy = AllowWriteCopy.Value;
                        isDirty = true;
                    }

                    if (DocumentAccessExpireDays.HasValue)
                    {
                        if (list.InformationRightsManagementSettings.EnableDocumentAccessExpire)
                        {
                            list.InformationRightsManagementSettings.DocumentAccessExpireDays = DocumentAccessExpireDays.Value;
                            isDirty = true;
                        }
                        else
                        {
                            WriteWarning("Document Access expiration is not enabled. Enable with -EnableDocumentAccessExpire $true");
                        }
                    }

                    if(LicenseCacheExpireDays.HasValue)
                    {
                        if(list.InformationRightsManagementSettings.EnableLicenseCacheExpire)
                        {
                            list.InformationRightsManagementSettings.LicenseCacheExpireDays = LicenseCacheExpireDays.Value;
                            isDirty = true;
                        } else {
                            WriteWarning("License Cache expiration is not enabled. Enable with -EnableLicenseCacheExpire $true");
                        }
                    }

                    if (DocumentLibraryProtectionExpireDate.HasValue)
                    {
                        if (list.IrmExpire)
                        {
                            list.InformationRightsManagementSettings.DocumentLibraryProtectionExpireDate = DocumentLibraryProtectionExpireDate.Value;
                            isDirty = true;
                        } else
                        {
                            WriteWarning("Information Rights Management (IRM) expiration is not enabled. Enable with -EnableExpiration");
                        }
                    }

                    if(GroupName != null)
                    {
                        list.InformationRightsManagementSettings.GroupName = GroupName;
                        isDirty = true;
                    }

                    if (PolicyDescription != null)
                    {
                        list.InformationRightsManagementSettings.PolicyDescription = PolicyDescription;
                        isDirty = true;
                    }

                    if (PolicyTitle != null)
                    {
                        list.InformationRightsManagementSettings.PolicyTitle = PolicyTitle;
                        isDirty = true;
                    }

#if !ONPREMISES
                    if (TemplateId != null)
                    {
                        list.InformationRightsManagementSettings.TemplateId = TemplateId;
                        isDirty = true;
                    }
#endif

                    if (isDirty)
                    {
                        //list.InformationRightsManagementSettings.Update();
                        list.Update();
                        ClientContext.Load(list.InformationRightsManagementSettings);
                        ClientContext.ExecuteQueryRetry();
                    }

                    var irm = new
                    {
                        InformationRightsManagementEnabled = list.IrmEnabled,
                        InformationRightsManagementExpire = list.IrmExpire,
                        InformationRightsManagementReject = list.IrmReject,
                        list.InformationRightsManagementSettings.AllowPrint,
                        list.InformationRightsManagementSettings.AllowScript,
                        list.InformationRightsManagementSettings.AllowWriteCopy,
                        list.InformationRightsManagementSettings.DisableDocumentBrowserView,
                        list.InformationRightsManagementSettings.DocumentAccessExpireDays,
                        list.InformationRightsManagementSettings.DocumentLibraryProtectionExpireDate,
                        list.InformationRightsManagementSettings.EnableDocumentAccessExpire,
                        list.InformationRightsManagementSettings.EnableDocumentBrowserPublishingView,
                        list.InformationRightsManagementSettings.EnableGroupProtection,
                        list.InformationRightsManagementSettings.EnableLicenseCacheExpire,
                        list.InformationRightsManagementSettings.GroupName,
                        list.InformationRightsManagementSettings.LicenseCacheExpireDays,
                        list.InformationRightsManagementSettings.PolicyDescription,
                        list.InformationRightsManagementSettings.PolicyTitle,
#if !ONPREMISES
                        list.InformationRightsManagementSettings.TemplateId
#endif
                    };
                    WriteObject(irm);
                }
            }
        }
    }
}
