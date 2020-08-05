#if !ONPREMISES
using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace PnP.PowerShell.Tests
{
    [TestClass]
    public class FilesTests
    {
        private static string _site1Id;
        private static string _site2Id;
        private static string _site1Url;
        private static string _site2Url;
        private static string _site1RelativeUrl;
        private static string _site2RelativeUrl;


        private static string Site1RelativeFolderUrl
        {
            get
            {
#if ONPREMISES
                return $"{_site1RelativeUrl}/Shared%20Documents";
#else
                return $"{_site1RelativeUrl}/Shared Documents";
#endif
            }
        }

        private string Site2RelativeFolderUrl
        {
            get
            {
#if ONPREMISES
                return $"{_site2RelativeUrl}/Shared%20Documents";
#else
                return $"{_site2RelativeUrl}/Shared Documents";
#endif
            }
        }

        private const string SourceFolderWithFolders = "SourceFolderWithFoldersAndFilesAndEmptyFolder";
        private const string SourceFolderName = "SourceFolder";
        private const string TargetFileName = "Testfile.txt";
        private const string TargetFileContents = "Some random file contents";
        private const string TargetCopyFolderName = "CopyDestination";
        private const string EmptyFolderName = "EmptyFolder";
        private const string TargetFileNameWithAmpersand = "Test & file.txt";
        private const string TargetFileNameWithHashtag = "Test & file.txt";
        
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            using (var ctx = TestCommon.CreateClientContext())
            {
                
                _site1Id = Guid.NewGuid().ToString();
                _site2Id = Guid.NewGuid().ToString();

                _site1RelativeUrl = $"/sites/PNPPS_Test_{_site1Id}";
                _site2RelativeUrl = $"/sites/PNPPS_Test_{_site2Id}";

                _site1Url = $"{TestCommon.GetTenantRootUrl(ctx)}{_site1RelativeUrl}";
                _site2Url = $"{TestCommon.GetTenantRootUrl(ctx)}{_site2RelativeUrl}";


                using (var site1Ctx = OfficeDevPnP.Core.Sites.SiteCollection.CreateAsync(ctx, new OfficeDevPnP.Core.Sites.CommunicationSiteCollectionCreationInformation()
                {
                    Url = _site1Url,
                    Lcid = 1033,
                    Title = "PnP PowerShell File Copy Test Site 1"
                }).GetAwaiter().GetResult())
                {
                    Folder folder = site1Ctx.Web.GetFolderByServerRelativeUrl(Site1RelativeFolderUrl);

                    FileCreationInformation fci = new FileCreationInformation
                    {
                        Content = System.Text.Encoding.ASCII.GetBytes(TargetFileContents),
                        Url = TargetFileName,
                        Overwrite = true
                    };
                    File fileToUpload = folder.Files.Add(fci);
                    site1Ctx.Load(fileToUpload);

                    fci.Url = TargetFileNameWithAmpersand;
                    fci.Overwrite = true;
                    fileToUpload = folder.Files.Add(fci);
                    site1Ctx.Load(fileToUpload);

                    fci.Url = TargetFileNameWithHashtag;
                    fci.Overwrite = true;
                    fileToUpload = folder.Files.Add(fci);
                    site1Ctx.Load(fileToUpload);

                    folder.EnsureFolder(TargetCopyFolderName);

                    // Prereq for CopyFile_EmptyFolderBetweenSiteCollections_Test
                    folder.EnsureFolder(EmptyFolderName);

                    // Prereq for CopyFile_FolderWithSkipSourceFolderNameBetweenSiteCollections_Test
                    var sourceFolder = folder.EnsureFolder(SourceFolderName);
                    fileToUpload = sourceFolder.Files.Add(fci);
                    site1Ctx.Load(fileToUpload);


                    // Prereq for CopyFile_FolderWithFoldersAndEmptyFolderBetweenSiteCollections_Test
                    var folderHirachyFolder0 = folder.EnsureFolder(SourceFolderWithFolders);
                    var folderHirachyFolder1 = folderHirachyFolder0.EnsureFolder(TargetCopyFolderName);

                    var folderHirachyFile0 = folderHirachyFolder0.Files.Add(fci);
                    var folderHirachyFile1 = folderHirachyFolder1.Files.Add(fci);
                    site1Ctx.Load(folderHirachyFile0);
                    site1Ctx.Load(folderHirachyFile1);

                    folderHirachyFolder1.EnsureFolder(EmptyFolderName);

                    site1Ctx.ExecuteQueryRetry();
                }
                OfficeDevPnP.Core.Sites.SiteCollection.CreateAsync(ctx, new OfficeDevPnP.Core.Sites.CommunicationSiteCollectionCreationInformation()
                {
                    Url = _site2Url,
                    Lcid = 1033,
                    Title = "PnP PowerShell File Copy Test Site 2"
                }).GetAwaiter().GetResult();
            }
        }


        [ClassCleanup]
        public static void Cleanup()
        {

            using (var ctx = TestCommon.CreateTenantClientContext())
            {
                Tenant tenant = new Tenant(ctx);
                // For any accidents lets allow for recoverability
                tenant.DeleteSiteCollection(_site1Url, true);
                tenant.DeleteSiteCollection(_site2Url, true);
            }
        }

        [TestMethod]
        public void GetFile_AsListItem_Test()
        {
            using (var scope = new PSTestScope(_site1Url, true))
            {
                var siteRelativeFileUrl = $"{Site1RelativeFolderUrl}/{TargetFileName}";

                var values = new Hashtable();
                values.Add("Title", "Test");
                //Get-PnPFile -Url /sites/project/_catalogs/themes/15/company.spcolor -AsFile",
                var results = scope.ExecuteCommand("Get-PnPFile",
                    new CommandParameter("Url", siteRelativeFileUrl),
                    new CommandParameter("AsListItem"));

                Assert.IsTrue(results.Any());

                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.ListItem));
            }
        }

        [TestMethod]
        public void GetFile_AsFile_Test()
        {
            using (var scope = new PSTestScope(_site1Url, true))
            {
                var siteRelativeFileUrl = $"{Site1RelativeFolderUrl}/{TargetFileName}";

                var results = scope.ExecuteCommand("Get-PnPFile",
                    new CommandParameter("Url", siteRelativeFileUrl),
                    new CommandParameter("AsFile"));

                Assert.IsFalse(results.Any());
            }
        }

        [TestMethod]
        public void GetFile_AsObject_Test()
        {
            using (var scope = new PSTestScope(_site1Url, true))
            {
                var siteRelativeFileUrl = $"{Site1RelativeFolderUrl}/{TargetFileName}";

                var results = scope.ExecuteCommand("Get-PnPFile",
                    new CommandParameter("Url", siteRelativeFileUrl));

                Assert.IsTrue(results.Any());
                Assert.IsTrue(results[0].BaseObject.GetType() == typeof(Microsoft.SharePoint.Client.File));
            }
        }

        [TestMethod]
        public void GetFile_AsString_Test()
        {
            using (var scope = new PSTestScope(_site1Url, true))
            {
                var siteRelateFileUrl = $"{Site1RelativeFolderUrl}/{TargetFileName}";

                var results = scope.ExecuteCommand("Get-PnPFile",
                    new CommandParameter("Url", siteRelateFileUrl),
                    new CommandParameter("AsString"));

                Assert.IsTrue(results.Any());
                Assert.IsTrue(results[0].ToString() == "Some random file contents");
            }
        }

        [TestMethod]
        public void CopyFileTest()
        {
            using (var scope = new PSTestScope(_site1Url, true))
            {
                var sourceUrl = $"{Site1RelativeFolderUrl}/{TargetFileName}";
                var destinationUrl = $"{Site1RelativeFolderUrl}/{TargetCopyFolderName}";
                var destinationFileUrl = $"{destinationUrl}/{TargetFileName}";

                var results = scope.ExecuteCommand("Copy-PnPFile",
                    new CommandParameter("SourceUrl", sourceUrl),
                    new CommandParameter("TargetUrl", destinationUrl),
                    new CommandParameter("Force"));


                using (var ctx = TestCommon.CreateClientContext(_site1Url))
                {
                    File initialFile = ctx.Web.GetFileByServerRelativeUrl(destinationFileUrl);
                    ctx.Load(initialFile);
                    ctx.ExecuteQueryRetry();
                    if (!initialFile.Exists)
                    {
                        Assert.Fail("Copied file cannot be found");
                    }
                }
            }
        }

        [TestMethod]
        public void CopyFile_WithAmpersand_Test()
        {
            using (var scope = new PSTestScope(_site1Url, true))
            {
                var sourceUrl = $"{Site1RelativeFolderUrl}/{TargetFileNameWithAmpersand}";
                var destinationUrl = $"{Site1RelativeFolderUrl}/{TargetCopyFolderName}";
                var destinationFileUrl = $"{destinationUrl}/{TargetFileNameWithAmpersand}";

                var results = scope.ExecuteCommand("Copy-PnPFile",
                    new CommandParameter("SourceUrl", sourceUrl),
                    new CommandParameter("TargetUrl", destinationUrl),
                    new CommandParameter("Force"));

                using (var ctx = TestCommon.CreateClientContext(_site1Url))
                {
                    File initialFile = ctx.Web.GetFileByServerRelativeUrl(destinationFileUrl);
                    ctx.Load(initialFile);
                    ctx.ExecuteQueryRetry();
                    if (!initialFile.Exists)
                    {
                        Assert.Fail("Copied file cannot be found");
                    }
                }
            }
        }

        [TestMethod]
        public void CopyFile_WithHashtag_Test()
        {
            using (var scope = new PSTestScope(_site1Url, true))
            {
                var sourceUrl = $"{Site1RelativeFolderUrl}/{TargetFileNameWithHashtag}";
                var destinationUrl = $"{Site1RelativeFolderUrl}/{TargetCopyFolderName}";
                var destinationFileUrl = $"{destinationUrl}/{TargetFileNameWithHashtag}";

                var results = scope.ExecuteCommand("Copy-PnPFile",
                    new CommandParameter("SourceUrl", sourceUrl),
                    new CommandParameter("TargetUrl", destinationUrl),
                    new CommandParameter("Force"));

                using (var ctx = TestCommon.CreateClientContext(_site1Url))
                {
                    File initialFile = ctx.Web.GetFileByServerRelativeUrl(destinationFileUrl);
                    ctx.Load(initialFile);
                    ctx.ExecuteQueryRetry();
                    if (!initialFile.Exists)
                    {
                        Assert.Fail("Copied file cannot be found");
                    }
                }
            }
        }

        [TestMethod]
        public void CopyFile_BetweenSiteCollections_Test()
        {
            using (var scope = new PSTestScope(_site1Url, true))
            {
                var sourceUrl = $"{Site1RelativeFolderUrl}/{TargetFileName}";
                var destinationFolderUrl = $"{Site2RelativeFolderUrl}";
                string destinationFileUrl = $"{destinationFolderUrl}/{TargetFileName}";

                var results = scope.ExecuteCommand("Copy-PnPFile",
                    new CommandParameter("SourceUrl", sourceUrl),
                    new CommandParameter("TargetUrl", destinationFolderUrl),
                    new CommandParameter("Force"));

                using (var ctx = TestCommon.CreateClientContext(_site2Url))
                {
                    File initialFile = ctx.Web.GetFileByServerRelativeUrl(destinationFileUrl);
                    ctx.Load(initialFile);
                    ctx.ExecuteQuery();
                    if (!initialFile.Exists)
                    {
                        Assert.Fail("Copied file cannot be found");
                    }
                }
            }
        }

        [TestMethod]
        public void CopyFile_BetweenSiteCollectionsWithAmpersand_Test()
        {
            using (var scope = new PSTestScope(_site1Url, true))
            {
                var sourceUrl = $"{Site1RelativeFolderUrl}/{TargetFileNameWithAmpersand}";
                var destinationFolderUrl = $"{Site2RelativeFolderUrl}";
                string destinationFileUrl = $"{destinationFolderUrl}/{TargetFileNameWithAmpersand}";

                var results = scope.ExecuteCommand("Copy-PnPFile",
                    new CommandParameter("SourceUrl", sourceUrl),
                    new CommandParameter("TargetUrl", destinationFolderUrl),
                    new CommandParameter("Force"));

                using (var ctx = TestCommon.CreateClientContext(_site2Url))
                {
                    File initialFile = ctx.Web.GetFileByServerRelativeUrl(destinationFileUrl);
                    ctx.Load(initialFile);
                    ctx.ExecuteQuery();
                    if (!initialFile.Exists)
                    {
                        Assert.Fail("Copied file cannot be found");
                    }
                }
            }
        }

        [TestMethod]
        public void CopyFile_BetweenSiteCollectionsWithHashtag_Test()
        {
            using (var scope = new PSTestScope(_site1Url, true))
            {
                var sourceUrl = $"{Site1RelativeFolderUrl}/{TargetFileNameWithHashtag}";
                var destinationFolderUrl = $"{Site2RelativeFolderUrl}";
                string destinationFileUrl = $"{destinationFolderUrl}/{TargetFileNameWithHashtag}";

                var results = scope.ExecuteCommand("Copy-PnPFile",
                    new CommandParameter("SourceUrl", sourceUrl),
                    new CommandParameter("TargetUrl", destinationFolderUrl),
                    new CommandParameter("Force"));

                using (var ctx = TestCommon.CreateClientContext(_site2Url))
                {
                    File initialFile = ctx.Web.GetFileByServerRelativeUrl(destinationFileUrl);
                    ctx.Load(initialFile);
                    ctx.ExecuteQuery();
                    if (!initialFile.Exists)
                    {
                        Assert.Fail("Copied file cannot be found");
                    }
                }
            }
        }

        [TestMethod]
        public void CopyFile_EmptyFolder_Test()
        {
            using (var scope = new PSTestScope(_site1Url, true))
            {
                var sourceUrl = $"{Site1RelativeFolderUrl}/{EmptyFolderName}";
                var destinationUrl = $"{Site1RelativeFolderUrl}/{TargetCopyFolderName}/{EmptyFolderName}";

                var results = scope.ExecuteCommand("Copy-PnPFile",
                    new CommandParameter("SourceUrl", sourceUrl),
                    new CommandParameter("TargetUrl", destinationUrl),
                    new CommandParameter("Force"));

                using (var ctx = TestCommon.CreateClientContext(_site1Url))
                {
                    Folder initialFolder = ctx.Web.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(destinationUrl));
                    initialFolder.EnsureProperties(f => f.Name, f => f.Exists);
                    ctx.Load(initialFolder);
                    ctx.ExecuteQuery();
                    if (!initialFolder.Exists)
                    {
                        Assert.Fail("Copied folder cannot be found");
                    }
                }
            }
        }

        [TestMethod]
        public void CopyFile_EmptyFolderBetweenSiteCollections_Test()
        {
            using (var scope = new PSTestScope(_site1Url, true))
            {
                var sourceUrl = $"{Site1RelativeFolderUrl}/{EmptyFolderName}";
                var destinationUrl = $"{Site2RelativeFolderUrl}/{EmptyFolderName}";

                var results = scope.ExecuteCommand("Copy-PnPFile",
                    new CommandParameter("SourceUrl", sourceUrl),
                    new CommandParameter("TargetUrl", destinationUrl),
                    new CommandParameter("Force"));

                using (var ctx = TestCommon.CreateClientContext(_site2Url))
                {
                    Folder initialFolder = ctx.Web.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(destinationUrl));
                    initialFolder.EnsureProperties(f => f.Name, f => f.Exists);
                    ctx.Load(initialFolder);
                    ctx.ExecuteQuery();
                    if (!initialFolder.Exists)
                    {
                        Assert.Fail("Copied folder cannot be found");
                    }
                }
            }
        }

        [TestMethod]
        public void CopyFile_FolderWithSkipSourceFolderNameBetweenSiteCollections_Test()
        {
            using (var scope = new PSTestScope(_site1Url, true))
            {
                var sourceUrl = $"{Site1RelativeFolderUrl}/{SourceFolderName}";
                var destinationUrl = $"{Site2RelativeFolderUrl}";

                var results = scope.ExecuteCommand("Copy-PnPFile",
                    new CommandParameter("SourceUrl", sourceUrl),
                    new CommandParameter("TargetUrl", destinationUrl),
                    new CommandParameter(name: "SkipSourceFolderName"),
                    new CommandParameter("Force"));

                using (var ctx = TestCommon.CreateClientContext(_site2Url))
                {
                    Folder initialFolder = ctx.Web.GetFolderByServerRelativePath(ResourcePath.FromDecodedUrl(destinationUrl));
                    initialFolder.EnsureProperties(f => f.Name, f => f.Exists, f => f.Files);
                    ctx.Load(initialFolder);
                    ctx.ExecuteQuery();
                    Assert.AreEqual(1, initialFolder.Files.Count);
                }
            }
        }

        [TestMethod]
        public void CopyFile_FolderWithFoldersAndEmptyFolderBetweenSiteCollections_Test()
        {
            using (var scope = new PSTestScope(_site1Url, true))
            {
                var sourceUrl = $"{Site1RelativeFolderUrl}/{SourceFolderWithFolders}";
                var destinationUrl = $"{Site2RelativeFolderUrl}";

                var results = scope.ExecuteCommand("Copy-PnPFile",
                    new CommandParameter("SourceUrl", sourceUrl),
                    new CommandParameter("TargetUrl", destinationUrl),
                    new CommandParameter("Force"));

                using (var ctx = TestCommon.CreateClientContext(_site2Url))
                {
                    List list = ctx.Web.GetListUsingPath(ResourcePath.FromDecodedUrl(destinationUrl));
                    ctx.Load(list);
                    ctx.ExecuteQuery();
                    Assert.AreEqual(5, list.ItemCount);
                }
            }
        }
    }
}
#endif