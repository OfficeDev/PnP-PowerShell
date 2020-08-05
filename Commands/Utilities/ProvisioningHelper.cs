using OfficeDevPnP.Core.Framework.Provisioning.Providers;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnP.PowerShell.Commands.Utilities
{
    public static class ProvisioningHelper
    {
        public static ITemplateFormatter GetFormatter(XMLPnPSchemaVersion schema)
        {
            ITemplateFormatter formatter = null;
            switch (schema)
            {
                case XMLPnPSchemaVersion.LATEST:
                    {
                        formatter = XMLPnPSchemaFormatter.LatestFormatter;
                        break;
                    }
                case XMLPnPSchemaVersion.V201503:
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2015_03);
#pragma warning restore CS0618 // Type or member is obsolete
                        break;
                    }
                case XMLPnPSchemaVersion.V201505:
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2015_05);
#pragma warning restore CS0618 // Type or member is obsolete
                        break;
                    }
                case XMLPnPSchemaVersion.V201508:
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2015_08);
#pragma warning restore CS0618 // Type or member is obsolete
                        break;
                    }
                case XMLPnPSchemaVersion.V201512:
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2015_12);
#pragma warning restore CS0618 // Type or member is obsolete
                        break;
                    }
                case XMLPnPSchemaVersion.V201605:
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2016_05);
#pragma warning restore CS0618 // Type or member is obsolete
                        break;
                    }
                case XMLPnPSchemaVersion.V201705:
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2017_05);
#pragma warning restore CS0618 // Type or member is obsolete

                        break;
                    }
                case XMLPnPSchemaVersion.V201801:
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2018_01);
#pragma warning restore CS0618 // Type or member is obsolete
                        break;
                    }
                case XMLPnPSchemaVersion.V201805:
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2018_05);
#pragma warning restore CS0618 // Type or member is obsolete
                        break;
                    }
                case XMLPnPSchemaVersion.V201807:
                    {
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2018_07);
                        break;
                    }
                case XMLPnPSchemaVersion.V201903:
                    {
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2019_03);
                        break;
                    }
                case XMLPnPSchemaVersion.V201909:
                    {
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2019_09);
                        break;
                    }
                case XMLPnPSchemaVersion.V202002:
                    {
                        formatter = XMLPnPSchemaFormatter.GetSpecificFormatter(XMLConstants.PROVISIONING_SCHEMA_NAMESPACE_2020_02);
                        break;
                    }
            }
            return formatter;
        }
    }
}
