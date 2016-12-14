using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OfficeDevPnP.Core.Framework.Provisioning.Connectors;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using SharePointPnP.PowerShell.Commands.Enums;
using SharePointPnP.PowerShell.Commands.Provisioning;
using SharePointPnP.PowerShell.Commands.Utilities;

namespace SharePointPnP.PowerShell.Commands.Components
{
    public static class GalleryHelper
    {
        private static string BaseTemplateGalleryUrl = "https://templates-gallery.sharepointpnp.com";

        internal static void SaveTemplate(Guid templateId, string path, Func<string, bool> overWriteFileAction  = null, Action<string> itemSavedAction = null)
        {
            HttpContentHeaders headers = null;
            // Get the template via HTTP REST
            var templateStream =
                HttpHelper.MakeGetRequestForStreamWithResponseHeaders(
                    $"{BaseTemplateGalleryUrl}/api/DownloadTemplate?templateId={templateId}", "application/octet-stream",
                    out headers);

            // If we have any result
            if (templateStream != null)
            {
                XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(new OpenXMLConnector(templateStream));
                var cd = new ContentDisposition(headers.ContentDisposition.ToString());
                var openXMLFileName = cd.FileName;

                if (path != null)
                {
                    if (System.IO.Directory.Exists(path))
                    {
                        var fileName = System.IO.Path.Combine(path, openXMLFileName);
                        bool doSave = false;
                        if (System.IO.File.Exists(fileName))
                        {
                            if (overWriteFileAction != null)
                            {
                                doSave = overWriteFileAction(fileName);
                            }
                            else
                            {
                                doSave = true;
                            }
                        }
                        else
                        {
                            doSave = true;
                        }
                        if (doSave)
                        {
                            // Save the template to the path
                            using (var fileStream = System.IO.File.Create(fileName))
                            {
                                templateStream.Seek(0, SeekOrigin.Begin);
                                templateStream.CopyTo(fileStream);
                            }
                            itemSavedAction?.Invoke(fileName);
                        }
                    }
                }
                else
                {
                    // Determine the name of the XML file inside the PNP Open XML file
                    var xmlTemplateFile = openXMLFileName.ToLower().Replace(".pnp", ".xml");

                    // Get the template
                    var result = provider.GetTemplate(xmlTemplateFile);
                    result.Connector = provider.Connector;
                    templateStream.Close();
                }
            }
        }

        internal static ProvisioningTemplate GetTemplate(Guid templateId)
        {
            HttpContentHeaders headers = null;
            // Get the template via HTTP REST
            var templateStream = HttpHelper.MakeGetRequestForStreamWithResponseHeaders($"{BaseTemplateGalleryUrl}/api/DownloadTemplate?templateId={templateId}", "application/octet-stream", out headers);

            // If we have any result
            if (templateStream != null)
            {
                XMLTemplateProvider provider = new XMLOpenXMLTemplateProvider(new OpenXMLConnector(templateStream));
                var cd = new ContentDisposition(headers.ContentDisposition.ToString());
                var openXMLFileName = cd.FileName;

                // Determine the name of the XML file inside the PNP Open XML file
                var xmlTemplateFile = openXMLFileName.ToLower().Replace(".pnp", ".xml");

                // Get the template
                var result = provider.GetTemplate(xmlTemplateFile);
                result.Connector = provider.Connector;
                templateStream.Close();
                return result;

            }
            return null;
        }

        internal static IEnumerable<ProvisioningTemplateInformation> SearchTemplates(string searchKey, TargetPlatform targetPlatforms, TargetScope targetScopes)
        {
            var targetPlatformsString = targetPlatforms.ToString();
            var targetScopesString = targetScopes.ToString();
            searchKey = searchKey != null ? System.Web.HttpUtility.UrlEncode(searchKey) : "";
            var jsonSearchResult =
                HttpHelper.MakeGetRequestForString(
                    $"{BaseTemplateGalleryUrl}/api/SearchTemplates?searchText={searchKey}&Platform={System.Web.HttpUtility.UrlEncode(targetPlatformsString)}&Scope={System.Web.HttpUtility.UrlEncode(targetScopesString)}");

            if (!string.IsNullOrEmpty(jsonSearchResult))
            {
                // Convert from JSON to a typed array
                var searchResultItems =
                    JsonConvert.DeserializeObject<PnPTemplatesGalleryResultItem[]>(jsonSearchResult);

                var result = (from r in searchResultItems
                              select new ProvisioningTemplateInformation
                              {
                                  Id = r.Id,
                                  DisplayName = r.Title,
                                  Description = r.Abstract,
                                  TemplateFileUri = r.TemplatePnPUrl,
                                  TemplateImageUrl = r.ImageUrl,
                                  Scope = r.Scopes,
                                  Platforms = r.Platforms,
                              }).ToArray();
                return result;
            }
            return null;
        }
    }
}
