using System;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Model.Configuration;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class ExtractConfigurationPipeBind
    {
        readonly ExtractConfiguration objectValue;
        readonly string value;

        public ExtractConfigurationPipeBind(string str)
        {
            value = str;
        }

        public ExtractConfigurationPipeBind(ExtractConfiguration configuration)
        {
            objectValue = configuration;
        }

        internal ExtractConfiguration GetConfiguration(string currentFileSystemLocation)
        {
            if (objectValue != null)
            {
                return objectValue;
            }
            if (!string.IsNullOrEmpty(value))
            {
                // is it a path?
                try
                {
                    string path = value;
                    if (!System.IO.Path.IsPathRooted(value))
                    {
                        path = System.IO.Path.Combine(currentFileSystemLocation, path);
                    }
                    if (System.IO.File.Exists(path))
                    {
                        return ExtractConfiguration.FromString(System.IO.File.ReadAllText(path));
                    }
                    else
                    {
                        return ExtractConfiguration.FromString(value);
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}
