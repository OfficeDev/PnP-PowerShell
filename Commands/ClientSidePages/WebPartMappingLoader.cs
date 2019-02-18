using System.IO;

namespace SharePointPnP.PowerShell.Commands.Utilities
{
    public static class WebPartMappingLoader
    {
        public static string LoadFile(string fileName)
        {
            var fileContent = "";
            using (Stream stream = typeof(WebPartMappingLoader).Assembly.GetManifestResourceStream(fileName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    fileContent = reader.ReadToEnd();
                }
            }

            return fileContent;
        }
    }
}
