using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net.Http.Headers;
using System.Net.Mime;
using Newtonsoft.Json;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.Commands.Components;
using SharePointPnP.PowerShell.Commands.Enums;
using SharePointPnP.PowerShell.Commands.Utilities;
using Resources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.Provisioning
{
    [Cmdlet(VerbsCommon.Get, "PnPProvisioningTemplateFromGallery", DefaultParameterSetName = "Search")]
    [CmdletAlias("Get-SPOProvisioningTemplateFromGallery")]
    [CmdletHelp("Retrieves or searches provisioning templates from the PnP Template Gallery", Category = CmdletHelpCategory.Lists)]
    [CmdletExample(
        Code = @"Get-PnPProvisioningTemplateFromGallery",
        Remarks = @"Retrieves all templates from the gallery",
        SortOrder = 1)]
    [CmdletExample(
        Code = @"Get-PnPProvisioningTemplateFromGallery -Search ""Data""",
        Remarks = @"Searches for a templates containing the word 'Data' in the Display Name",
        SortOrder = 2)]
    [CmdletExample(
        Code = @"Get-PnPProvisioningTemplateFromGallery -Identity ae925674-8aa6-438b-acd0-d2699a022edd",
        Remarks = @"Retrieves a template with the specified ID",
        SortOrder = 3)]
    [CmdletExample(
        Code = @"$template = Get-PnPProvisioningTemplateFromGallery -Identity ae925674-8aa6-438b-acd0-d2699a022edd
Apply-PnPProvisioningTemplate -InputInstance $template",
        Remarks = @"Retrieves a template with the specified ID and applies it to the site.",
        SortOrder = 4)]
    [CmdletExample(
        Code = @"$template = Get-PnPProvisioningTemplateFromGallery -Identity ae925674-8aa6-438b-acd0-d2699a022edd -Path c:\temp",
        Remarks = @"Retrieves a template with the specified ID and saves the template to the specified path",
        SortOrder = 4)]
    public class GetProvisioningTemplateFromGallery : PSCmdlet
    {
        [Parameter(Mandatory = false, ParameterSetName = "Identity")]
        public Guid Identity;

        [Parameter(Mandatory = false, ParameterSetName = "Search")]
        public string Search;

        [Parameter(Mandatory = false, ParameterSetName = "Search")]
        public TargetPlatform TargetPlatform = TargetPlatform.All;

        [Parameter(Mandatory = false, ParameterSetName = "Search")]
        public TargetScope TargetScope = TargetScope.Site;

        [Parameter(Mandatory = false, ParameterSetName = "Identity")]
        public string Path;

        [Parameter(Mandatory = false, ParameterSetName = "Identity")]
        public SwitchParameter Force;

        protected override void BeginProcessing()
        {
            if (ParameterSetName == "Identity")
            {
                if (MyInvocation.BoundParameters.ContainsKey("Path"))
                {
                    if (!System.IO.Path.IsPathRooted(Path))
                    {
                        Path = System.IO.Path.Combine(SessionState.Path.CurrentFileSystemLocation.Path, Path);
                    }
                    GalleryHelper.SaveTemplate(Identity, Path, s =>
                    {
                        if (Force || ShouldContinue(string.Format(Resources.File0ExistsOverwrite, s), Resources.Confirm))
                        {
                            return true;
                        }
                        return false;
                    }, f =>
                    {
                        WriteObject($"Template saved: {f}");
                    });
                }
                else
                {
                    var template = GalleryHelper.GetTemplate(Identity);
                    if (template != null)
                    {
                        WriteObject(template);
                    }
                }
            }
            else
            {
                WriteObject(GalleryHelper.SearchTemplates(Search, TargetPlatform, TargetScope).ToArray(), true);
            }

        }


    }
}
