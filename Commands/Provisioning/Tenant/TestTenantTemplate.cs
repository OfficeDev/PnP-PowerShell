#if !ONPREMISES
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace SharePointPnP.PowerShell.Commands.Provisioning.Tenant
{
    [Cmdlet(VerbsDiagnostic.Test, "PnPTenantTemplate", SupportsShouldProcess = true)]
    [Alias("Test-PnPProvisioningHierarchy")]
    [CmdletHelp("Tests a tenant template for invalid references",
        Category = CmdletHelpCategory.Provisioning)]
    [CmdletExample(
       Code = @"PS:> Test-PnPTenantTemplate -Template $myTemplate",
       Remarks = "Checks for valid template references",
       SortOrder = 1)]
    public class TestTenantTemplate : PnPCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "The in-memory template to test", ParameterSetName = ParameterAttribute.AllParameterSets)]
        [Alias("Hierarchy")]
        public ProvisioningHierarchy Template;

        protected override void ExecuteCmdlet()
        {
            if (MyInvocation.InvocationName.ToLower() == "test-pnpprovisioninghierarchy")
            {
                WriteWarning("Test-PnPProvisioningHierarchy has been deprecated. Use Test-PnPTenantTemplate instead.");
            }
            List<string> issues = new List<string>();
            foreach(var sequence in Template.Sequences)
            {
                foreach (var site in sequence.SiteCollections)
                {
                    foreach (var template in site.Templates)
                    {
                        if(Template.Templates.FirstOrDefault(t => t.Id == template) == null)
                        {
                            issues.Add($"Template {template} referenced in site {site.Id} is not present in tenant template.");
                        }
                    }
                    foreach(var subsite in site.Sites.Cast<TeamNoGroupSubSite>())
                    {
                        foreach (var template in subsite.Templates)
                        {
                            if (Template.Templates.FirstOrDefault(t => t.Id == template) == null)
                            {
                                issues.Add($"Template {template} referenced in subsite with url {subsite.Url} in site {site.Id} is not present in tenant template.");
                            }
                        }
                    }
                }
            }
            foreach(var template in Template.Templates)
            {
                var used = false;
                foreach(var sequence in Template.Sequences)
                {
                    foreach(var site in sequence.SiteCollections)
                    {
                        if(site.Templates.Contains(template.Id))
                        {
                            used = true;
                            break;
                        }

                        foreach(var subsite in site.Sites)
                        {
                            if(subsite.Templates.Contains(template.Id))
                            {
                                used = true;
                                break;
                            }
                        }
                        if (used)
                        {
                            break;
                        }
                    }
                    if(used)
                    {
                        break;
                    }
                }
                if(!used)
                {
                    issues.Add($"Template {template.Id} is not used by any site in the tenant template sequence.");
                }
            }
            if(issues.Any())
            {
                WriteObject(issues, true);
            }

        }
    }
}
#endif