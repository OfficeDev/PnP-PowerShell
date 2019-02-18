#if !ONPREMISES
using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Utilities;
using SharePointPnP.PowerShell.Commands.ClientSidePages;
using System;
using System.Linq;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class PagePipeBind
    {
        private const string CAMLQueryByExtensionAndName = @"
                <View Scope='Recursive'>
                  <Query>
                    <Where>
                      <Eq>
                        <FieldRef Name='FileLeafRef'/>
                        <Value Type='text'>{0}</Value>
                      </Eq>
                    </Where>
                  </Query>
                </View>";

        private readonly ListItem pageListItem;
        private readonly string name;

        public PagePipeBind(ListItem pageListItem)
        {
            this.pageListItem = pageListItem;
            if (this.pageListItem != null)
            {
                this.name = this.pageListItem.FieldValues["FileLeafRef"].ToString();
            }
        }

        public PagePipeBind(string name)
        {
            this.name = ClientSidePageUtilities.EnsureCorrectPageName(name);
            this.pageListItem = null;
        }

        public ListItem PageListItem => this.pageListItem;

        public string Name => this.name;

        internal ListItem GetPage(Web web)
        {
            // Get pages library
            web.EnsureProperty(w => w.ServerRelativeUrl);
            var listServerRelativeUrl = UrlUtility.Combine(web.ServerRelativeUrl, "sitepages");
            var sitePagesLibrary = web.GetList(listServerRelativeUrl);

            if (sitePagesLibrary != null)
            {
                CamlQuery query = null;
                if (!string.IsNullOrEmpty(this.name))
                {
                    query = new CamlQuery
                    {
                        ViewXml = string.Format(CAMLQueryByExtensionAndName, this.name)
                    };

                    var page = sitePagesLibrary.GetItems(query);
                    web.Context.Load(page);
                    web.Context.ExecuteQueryRetry();

                    if (page.Count == 1)
                    {
                        return page[0];
                    }
                }
            }

            return null;
        }

    }
}
#endif