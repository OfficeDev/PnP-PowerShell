using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace PnP.PowerShell.Commands.Model
{
    internal class AzureApp
    {
        public string Id { get; set; }
        public string AppId { get; set; }
        public string DisplayName { get; set; }
        public string SignInAudience { get; set; }
    }

    public class PermissionScope
    {
        [JsonIgnore]
        public string resourceAppId { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; } = "Role";
        [JsonIgnore]
        public string Identifier { get; set; }
    }

    public class PermissionScopes
    {
        private List<PermissionScope> scopes = new List<PermissionScope>();
        public PermissionScopes()
        {
            #region GRAPH
            // Graph
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "bf7b1a76-6e77-406b-b258-bf5c7720e98f",
                Identifier = "MSGraph.Group.Create"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "5b567255-7703-4780-807c-7be8301ae99b",
                Identifier = "MSGraph.Group.Read.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "62a82d76-70ea-41e2-9197-370581804d09",
                Identifier = "MSGraph.Group.ReadWrite.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "5ef47bde-23a3-4cfb-be03-6ab63044aec6",
                Identifier = "MSGraph.Group.Select"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "98830695-27a2-44f7-8c18-0c3ebc9698f6",
                Identifier = "MSGraph.GroupMember.Read.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "dbaae8cf-10b5-4b86-a4a1-f871c94c6695",
                Identifier = "MSGraph.GroupMember.ReadWrite.All"
            });

            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "01d4889c-1287-42c6-ac1f-5d1e02578ef6",
                Identifier = "MSGraph.Files.Read.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "75359482-378d-4052-8f01-80520e7db3cd",
                Identifier = "MSGraph.Files.ReadWrite.All"
            });

            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "b0afded3-3588-46d8-8b3d-9842eff778da",
                Identifier = "MSGraph.AuditLog.Read.All"
            });

            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "798ee544-9d2d-430c-a058-570e29e34338",
                Identifier = "MSGraph.Calendars.Read"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "ef54d2bf-783f-4e0f-bca1-3210c0444d99",
                Identifier = "MSGraph.Calendars.ReadWrite"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "df021288-bdef-4463-88db-98f22de89214",
                Identifier = "MSGraph.User.Read.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "741f803b-c850-494e-b5df-cde7c675a1ca",
                Identifier = "MSGraph.User.ReadWrite.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "88e58d74-d3df-44f3-ad47-e89edf4472e4",
                Identifier = "MSGraph.AppCatalog.Read.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "1ca167d5-1655-44a1-8adf-1414072e1ef9",
                Identifier = "MSGraph.AppCatalog.ReadWrite.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0000-c000-000000000000",
                Id = "3db89e36-7fa6-4012-b281-85f3d9d9fd2e",
                Identifier = "MSGraph.AppCatalog.Submit"
            });
            #endregion
            #region SPO
            // SPO
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0ff1-ce00-000000000000",
                Id = "678536fe-1083-478a-9c59-b99265e6b0d3",
                Identifier = "SPO.Sites.FullControl.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0ff1-ce00-000000000000",
                Id = "fbcd29d2-fcca-4405-aded-518d457caae4",
                Identifier = "SPO.Sites.ReadWrite.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0ff1-ce00-000000000000",
                Id = "9bff6588-13f2-4c48-bbf2-ddab62256b36",
                Identifier = "SPO.Sites.Manage.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0ff1-ce00-000000000000",
                Id = "d13f72ca-a275-4b96-b789-48ebcc4da984",
                Identifier = "SPO.Sites.Read.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0ff1-ce00-000000000000",
                Id = "2a8d57a5-4090-4a41-bf1c-3c621d2ccad3",
                Identifier = "SPO.TermStore.Read.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0ff1-ce00-000000000000",
                Id = "c8e3537c-ec53-43b9-bed3-b2bd3617ae97",
                Identifier = "SPO.TermStore.ReadWrite.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0ff1-ce00-000000000000",
                Id = "df021288-bdef-4463-88db-98f22de89214",
                Identifier = "SPO.User.Read.All"
            });
            scopes.Add(new PermissionScope()
            {
                resourceAppId = "00000003-0000-0ff1-ce00-000000000000",
                Id = "741f803b-c850-494e-b5df-cde7c675a1ca",
                Identifier = "SPO.User.ReadWrite.All"
            });
            #endregion
        }

        public string[] GetIdentifiers()
        {
            return this.scopes.Select(s => s.Identifier).ToArray();
        }

        public PermissionScope GetScope(string identifier)
        {
            return this.scopes.FirstOrDefault(s => s.Identifier == identifier);
        }
    }

    public class AppResource
    {
        [JsonPropertyName("resourceAppId")]
        public string Id { get; set; }
        [JsonPropertyName("resourceAccess")]
        public List<PermissionScope> ResourceAccess { get; set; } = new List<PermissionScope>();
    }
}