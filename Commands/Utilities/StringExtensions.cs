using System.Text.RegularExpressions;

namespace PnP.PowerShell.Commands.Utilities
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