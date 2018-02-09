using System.Text.RegularExpressions;

namespace SharePointPnP.PowerShell.Commands.Utilities
{
    public static class StringExtensions
    {
        public static string ReplaceCaseInsensitive(this string input, string search, string replacement)
        {
            return Regex.Replace(
                input,
                Regex.Escape(search),
                replacement.Replace("$", "$$"),
                RegexOptions.IgnoreCase
            );
        }
    }
}