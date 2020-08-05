using System;
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Model.Configuration;

namespace PnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class ApplyConfigurationPipeBind
    {
        readonly ApplyConfiguration objectValue;
        readonly string value;

        public ApplyConfigurationPipeBind(string str)
        {
            value = str;
        }

        public ApplyConfigurationPipeBind(ApplyConfiguration configuration)
        {
            objectValue = configuration;
        }

        internal ApplyConfiguration GetConfiguration(string currentFileSystemLocation)
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
                        return ApplyConfiguration.FromString(System.IO.File.ReadAllText(path));
                    }
                    else
                    {
                        return ApplyConfiguration.FromString(value);
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
