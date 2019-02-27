using System;
using System.Linq;

namespace SharePointPnP.PowerShell.Commands.Utilities
{
    public static class UrlUtilities
    {
        public static string GetTenantAdministrationUrl(Uri uri)
        {
            var uriParts = uri.Host.Split('.');
            if (uriParts[0].EndsWith("-admin")) return uri.OriginalString;
            if (!uriParts[0].EndsWith("-admin"))
                return $"https://{uriParts[0]}-admin.{string.Join(".", uriParts.Skip(1))}";
            return null;
        }

        public static string GetTenantAdministrationUrl(string url)
        {
            return GetTenantAdministrationUrl(new Uri(url));
        }

        public static bool IsTenantAdministrationUrl(Uri uri)
        {
            var uriParts = uri.Host.Split('.');
            return uriParts[0].EndsWith("-admin");
        }

        public static bool IsTenantAdministrationUrl(string url)
        {
            return IsTenantAdministrationUrl(new Uri(url));
        }
    }
}