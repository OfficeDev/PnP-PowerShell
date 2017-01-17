using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Text.RegularExpressions;
using Microsoft.SharePoint.Client;
using SharePointPnP.PowerShell.Commands.Base;
using File = Microsoft.SharePoint.Client.File;
using PnPResources = SharePointPnP.PowerShell.Commands.Properties.Resources;

namespace SharePointPnP.PowerShell.Commands.Provider
{
    [CmdletProvider("SPO", ProviderCapabilities.ShouldProcess)]
    public class SPOProvider : NavigationCmdletProvider, IContentCmdletProvider
    {
        //Private properties
        private const string Pattern = @"^[\\w\\d\\.\\s]*$";
        private const string PathSeparator = "/";
        private const int DefaultItemCacheTimeout = 1000; //1 second default caching of file/folder object
        private const int DefaultWebCacheTimeout = 1000 * 60 * 10; //10 minutes default caching of web object

        //Init
        protected override ProviderInfo Start(ProviderInfo providerInfo)
        {
#if DEBUG
            SessionState.PSVariable.Set("WebRequestCounter", 0);
#endif
            return base.Start(providerInfo);
        }

        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            WriteVerbose(string.Format("SPOProvider::NewDrive (Drive.Name = ’{0}’, Drive.Root = ’{1}’)", drive.Name, drive.Root));

            var spoParametes = DynamicParameters as SPODriveParameters;
            Web web = null;

            if (spoParametes != null && spoParametes.Context != null)
            {
                var webUrl = spoParametes.Url ?? spoParametes.Context.Url;
                web = spoParametes.Context.Clone(webUrl).Web;
            }
            else if (spoParametes != null && spoParametes.Web != null)
            {
                var webUrl = spoParametes.Url ?? spoParametes.Web.EnsureProperty(w => w.Url);
                web = spoParametes.Web.Context.Clone(webUrl).Web;
            }
            else if (SPOnlineConnection.CurrentConnection != null)
            {
                var webUrl = (spoParametes != null && spoParametes.Url != null) ? spoParametes.Url : SPOnlineConnection.CurrentConnection.Context.Web.EnsureProperty(w => w.Url);
                web = SPOnlineConnection.CurrentConnection.Context.Clone(webUrl).Web;
            }
            else
            {
                WriteErrorInternal(PnPResources.NoConnection, drive.Root, ErrorCategory.ConnectionError);
            }

            if (web != null)
            {
#if DEBUG
                SetCounter(web.Context);
#endif
                var itemTimeout = (spoParametes != null && spoParametes.ItemCacheTimeout != default(int)) ? spoParametes.ItemCacheTimeout : DefaultItemCacheTimeout;
                var webTimeout = (spoParametes != null && spoParametes.WebCacheTimeout != default(int)) ? spoParametes.WebCacheTimeout : DefaultWebCacheTimeout;
                var normalizePath = NormalizePath(IsPropertyAvailable(web, "ServerRelativeUrl") ? web.ServerRelativeUrl : web.EnsureProperty(w => w.ServerRelativeUrl));

                //Set root to host root and current location to normalized path
                var normalizedDrive = new SPODriveInfo(new PSDriveInfo(drive.Name, drive.Provider, "/", drive.Description, drive.Credential, drive.DisplayRoot))
                {
                    Web = web,
                    ItemTimeout = itemTimeout,
                    WebTimeout = webTimeout,
                    CurrentLocation = normalizePath
                };

                //Add web to cache
                normalizedDrive.CachedWebs.Add(new SPODriveCacheWeb
                {
                    Web = normalizedDrive.Web,
                    LastRefresh = DateTime.Now,
                    Path = normalizePath

                });

                return normalizedDrive;
            }
            return null;
        }

        protected override object NewDriveDynamicParameters()
        {
            return new SPODriveParameters();
        }

        protected override PSDriveInfo RemoveDrive(PSDriveInfo drive)
        {
            WriteVerbose(string.Format("SPOProvider::RemoveDrive (Drive.Name = ’{0}’)", drive.Name));

            var spoDrive = drive as SPODriveInfo;
            if (spoDrive == null) return null;

            spoDrive.Web.Context.Dispose();
            return spoDrive;
        }

        //Validate
        protected override bool IsValidPath(string path)
        {
            WriteVerbose(string.Format("SPOProvider::IsValidPath (Path = ’{0}’)", path));
            return Regex.IsMatch(path, Pattern);
        }

        protected override bool IsItemContainer(string path)
        {
            WriteVerbose(string.Format("SPOProvider::IsItemContainer (Path = ’{0}’)", path));

            try
            {
                if (IsPathDrive(path)) return true;
                return GetFileOrFolder(path, false) is Folder;
            }
            catch (Exception e)
            {
                WriteErrorInternal(e.Message, path, exception: e);
                return false;
            }
        }

        protected override bool ItemExists(string path)
        {
            WriteVerbose(string.Format("SPOProvider::ItemExists (Path = ’{0}’)", path));

            if (IsPathDrive(path)) return true;
            try
            {
                if (path.EndsWith("*")) return false;
                return GetFileOrFolder(path, false) != null;
            }
            catch (Exception e)
            {
                WriteErrorInternal(e.Message, path, exception: e);
                return false;
            }
        }

        protected override bool HasChildItems(string path)
        {
            WriteVerbose(string.Format("SPOProvider::HasChildItems (Path = ’{0}’)", path));

            try
            {
                var folder = GetFileOrFolder(path) as Folder;
                if (folder != null)
                {
                    var folderAndFiles = GetFolderItems(folder);
                    return folderAndFiles.Any();
                }
                return false;
            }
            catch (Exception e)
            {
                WriteErrorInternal(e.Message, path, exception: e);
                return false;
            }
        }

        //Path
        protected override string MakePath(string parent, string child)
        {
            var result = base.MakePath(parent, child);
            WriteVerbose(string.Format("SPOProvider::MakePath (parent = ’{0}’, child = ’{1}’) = {2}", parent, child, result));
            return result;
        }

        protected override string GetParentPath(string path, string root)
        {
            var result = base.GetParentPath(path, root);
            WriteVerbose(string.Format("SPOProvider::GetParentPath (path = ’{0}’, root = ’{1}’) = {2}", path, root, result));
            return result;
        }

        protected override string GetChildName(string path)
        {
            var result = base.GetChildName(path);
            WriteVerbose(string.Format("SPOProvider::GetChildName (path = ’{0}’) = {1}", path, result));
            return result;
        }

        protected override string NormalizeRelativePath(string path, string basePath)
        {
            var result = base.NormalizeRelativePath(path, basePath);
            WriteVerbose(string.Format("SPOProvider::NormalizeRelativePath (path = ’{0}’, basePath = ’{1}’) = {2}", path, basePath, result));
            return result;
        }

        //Get
        protected override void GetItem(string path)
        {
            WriteVerbose(string.Format("SPOProvider::GetItem (Path = ’{0}’)", path));

            var obj = GetFileOrFolder(path);
            if (obj != null)
            {
                WriteItemObject(obj, GetServerRelativePath(path), (obj is Folder));
            }
        }

        protected override void GetChildItems(string path, bool recurse)
        {
            WriteVerbose(string.Format("SPOProvider::GetChildItems (Path = ’{0}’)", path));

            var folder = GetFileOrFolder(path) as Folder;
            if (folder != null)
            {

                var serverRelativePath = GetServerRelativePath(path);
                var folderAndFiles = GetFolderItems(folder).ToArray();

                folderAndFiles.OfType<Folder>().ToList().ForEach(subFolder => WriteItemObject(subFolder, subFolder.ServerRelativeUrl, true));
                folderAndFiles.OfType<File>().ToList().ForEach(file => WriteItemObject(file, file.ServerRelativeUrl, false));
                if (recurse)
                {
                    folderAndFiles.OfType<Folder>().ToList().ForEach(subFolder => GetChildItems(subFolder.ServerRelativeUrl, true));
                }
            }
            else
            {
                WriteErrorInternal("No folder at end of path", path, ErrorCategory.InvalidOperation);
            }
        }

        protected override void GetChildNames(string path, ReturnContainers returnContainers)
        {
            WriteVerbose(string.Format("SPOProvider::GetChildNames (Path = ’{0}’)", path));

            var folder = GetFileOrFolder(path) as Folder;
            if (folder != null)
            {
                var folderAndFiles = GetFolderItems(folder).ToArray();
                var serverRelaitivePath = GetServerRelativePath(path);
                foreach (var subFolder in folderAndFiles.OfType<Folder>().ToList())
                {
                    string name;
                    if (string.IsNullOrEmpty(subFolder.Name))
                    {
                        var serverRelativeUrl = IsPropertyAvailable(subFolder, "ServerRelativeUrl") ? subFolder.ServerRelativeUrl : subFolder.EnsureProperty(s => s.ServerRelativeUrl);
                        name = serverRelativeUrl.Split(PathSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Last();
                    }
                    else
                    {
                        name = subFolder.Name;
                    }
                    WriteItemObject(name, serverRelaitivePath, true);
                }
                foreach (var file in folderAndFiles.OfType<File>().ToList())
                {
                    WriteItemObject(file.Name, serverRelaitivePath, false);
                }
            }
            else
            {
                WriteErrorInternal("No folder at end of path", path, ErrorCategory.InvalidOperation);
            }
        }

        //Set
        protected override void CopyItem(string path, string copyPath, bool recurse)
        {
            WriteVerbose(string.Format("SPOProvider::CopyItem (Path = ’{0}’, copyPath = ’{1}’)", path, copyPath));

            CopyMoveImplementation(path, copyPath, recurse);
        }

        protected override void MoveItem(string path, string destination)
        {
            WriteVerbose(string.Format("SPOProvider::MoveItem (Path = ’{0}’, destination = ’{1}’)", path, destination));

            CopyMoveImplementation(path, destination, true, false);
        }

        protected override void RenameItem(string path, string newName)
        {
            WriteVerbose(string.Format("SPOProvider::RenameItem (Path = ’{0}’)", path));

            if (newName.Contains(PathSeparator))
            {
                var msg = "Name can not contain path separator";
                var err = new ArgumentException(msg);
                WriteErrorInternal(msg, path, ErrorCategory.InvalidArgument, true, err);
                return;
            }
            CopyMoveImplementation(path, GetParentServerRelativePath(path) + PathSeparator + newName, true, false);
        }

        protected override void RemoveItem(string path, bool recurse)
        {
            WriteVerbose(string.Format("SPOProvider::RemoveItem (Path = ’{0}’)", path));

            var obj = GetFileOrFolder(path) as ClientObject;

            if (obj != null && ShouldProcess(GetServerRelativePath(path), string.Format("{0} {1}", (Force) ? "Delete" : "Recycle", obj.GetType().Name)))
            {
                if (obj is File)
                {
                    var file = obj as File;
                    if (Force)
                    {
                        file.DeleteObject();
                    }
                    else
                    {
                        file.Recycle();
                    }
                    file.Context.ExecuteQueryRetry();
                }
                else if (obj is Folder)
                {
                    var folder = obj as Folder;
                    if (Force)
                    {
                        folder.DeleteObject();
                    }
                    else
                    {
                        folder.Recycle();
                    }
                    folder.Context.ExecuteQueryRetry();
                }
            }
        }

        protected override void NewItem(string path, string itemTypeName, object newItemValue)
        {
            WriteVerbose(string.Format("SPOProvider::NewItem (Path = ’{0}’, itemTypeName = ’{1}’)", path, itemTypeName));

            var itemUrl = GetServerRelativePath(path);
            var parentUrl = GetParentServerRelativePath(itemUrl);
            if (string.IsNullOrEmpty(itemTypeName)) itemTypeName = "File";

            var parentFolder = GetFileOrFolder(parentUrl) as Folder;
            if (parentFolder != null)
            {
                if (itemTypeName.Equals("file", StringComparison.InvariantCultureIgnoreCase))
                {

                    var fileCreationInfo = new FileCreationInformation
                    {
                        Url = itemUrl,
                        Overwrite = Force
                    };

                    if (newItemValue is string)
                    {
                        fileCreationInfo.Content = System.Text.Encoding.UTF8.GetBytes((string)newItemValue);
                    }
                    else if (newItemValue is byte[])
                    {
                        fileCreationInfo.Content = (byte[])newItemValue;
                    }
                    else if (newItemValue is Stream)
                    {
                        fileCreationInfo.ContentStream = (Stream)newItemValue;
                    }
                    else
                    {
                        fileCreationInfo.Content = new byte[0];
                    }

                    parentFolder.Files.Add(fileCreationInfo);

                }
                else if (itemTypeName.Equals("folder", StringComparison.InvariantCultureIgnoreCase) || itemTypeName.Equals("directory", StringComparison.InvariantCultureIgnoreCase))
                {
                    var folderName = itemUrl.Split(PathSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Last();
                    if (!parentFolder.FolderExists(folderName))
                    {
                        parentFolder.Folders.Add(itemUrl);
                    }
                }
                else
                {
                    WriteErrorInternal("Only File or Folder (Directory) supported for Type", path, ErrorCategory.InvalidArgument);
                }
                parentFolder.Context.ExecuteQueryRetry();
            }
        }

        //Content
        public IContentReader GetContentReader(string path)
        {
            WriteVerbose(string.Format("SPOProvider::GetContentReader (path = ’{0}’)", path));

            var obj = GetFileOrFolder(path);
            if (obj is Folder)
            {
                WriteErrorInternal("Directories have no content", path, ErrorCategory.InvalidOperation);
            }
            if (obj is File)
            {
                var file = obj as File;
                var isString = false;
                var contentParameters = DynamicParameters as SPOContentParameters;
                if (contentParameters != null)
                {
                    isString = contentParameters.IsBinary;
                }
                return new SPOContentReaderWriter(file, isString);
            }

            return null;
        }

        public object GetContentReaderDynamicParameters(string path)
        {
            return new SPOContentParameters();
        }

        public IContentWriter GetContentWriter(string path)
        {
            WriteVerbose(string.Format("SPOProvider::GetContentWriter (path = ’{0}’)", path));

            var obj = GetFileOrFolder(path, false);
            if (obj is Folder)
            {
                WriteErrorInternal("Directories have no content", path, ErrorCategory.InvalidOperation);
            }
            if (obj == null)
            {
                NewItem(path, "File", null);
                obj = GetFileOrFolder(path, useChache: false);
            }
            if (obj is File)
            {
                var file = obj as File;
                var isBinary = false;
                var contentParameters = DynamicParameters as SPOContentParameters;
                if (contentParameters != null)
                {
                    isBinary = contentParameters.IsBinary;
                }

                if (ShouldProcess(string.Format("Set content in {0}", GetServerRelativePath(path))))
                {
                    return new SPOContentReaderWriter(file, isBinary);
                }
            }

            return null;
        }

        public object GetContentWriterDynamicParameters(string path)
        {
            return new SPOContentParameters();
        }

        public void ClearContent(string path)
        {
            WriteVerbose(string.Format("SPOProvider::ClearContent (path = ’{0}’)", path));

            var obj = GetFileOrFolder(path);
            if (obj is Folder)
            {
                WriteErrorInternal("Directories have no content", path, ErrorCategory.InvalidOperation);
            }
            if (obj is File)
            {
                if (ShouldProcess(string.Format("Clear content from {0}", GetServerRelativePath(path))))
                {
                    var file = obj as File;
                    File.SaveBinaryDirect(file.Context as ClientContext, file.ServerRelativeUrl, Stream.Null, true);
                }
            }
        }

        public object ClearContentDynamicParameters(string path)
        {
            return null;
        }

        #region Helpers

        //Get helpers
        private object GetFileOrFolder(string path, bool throwError = true, bool useChache = true)
        {
            var spoDrive = GetCurrentDrive(path);
            if (spoDrive == null) return null;

            var serverRelativePath = GetServerRelativePath(path);
            if (string.IsNullOrEmpty(serverRelativePath)) return null;

            //Try get cached item
            if (useChache)
            {
                var fileOrFolder = GetCachedItem(serverRelativePath);
                if (fileOrFolder != null) return fileOrFolder.Item;
            }

            //Find web closes to object
            var web = FindWebInPath(serverRelativePath);
            spoDrive.Web = web;
            var webUrl = IsPropertyAvailable(web, "ServerRelativeUrl") ? web.ServerRelativeUrl : web.EnsureProperty(w => w.ServerRelativeUrl);

            //If path is current web root return root folder
            if (serverRelativePath.Equals(webUrl, StringComparison.InvariantCultureIgnoreCase))
            {
                if (!IsPropertyAvailable(web, "RootFolder"))
                {
                    web.EnsureProperty(w => w.RootFolder);
                }
                SetCachedItem(serverRelativePath, web.RootFolder);
                return web.RootFolder;
            }

            //Search object
            var result = ExecuteObjectSearch(serverRelativePath, web);
            if (result is File || result is Folder)
            {
                //Cache item
                SetCachedItem(serverRelativePath, result);
                return result;
            }

            //Cache not found item
            SetCachedItem(serverRelativePath, null);

            //Error
            if (throwError && result is Exception)
            {
                var ex = result as Exception;
                WriteErrorInternal(ex.Message, path, ErrorCategory.ObjectNotFound, exception: ex);
            }

            return null;
        }

        private object ExecuteObjectSearch(string serverRelativePath, Web web)
        {
            var ctx = web.Context;
            File file;
            Folder folder;
            var scope = new ExceptionHandlingScope(ctx);
            var tryFileFirst = serverRelativePath.Split(PathSeparator.ToCharArray()).Last().Contains(".");

            if (tryFileFirst)
            {
                using (scope.StartScope())
                {
                    using (scope.StartTry())
                    {
                        file = web.GetFileByServerRelativeUrl(serverRelativePath);
                        ctx.Load(file);
                    }
                    using (scope.StartCatch())
                    {
                        folder = web.GetFolderByServerRelativeUrl(serverRelativePath);
                        ctx.Load(folder);
                    }
                }
            }
            else
            {
                using (scope.StartScope())
                {
                    using (scope.StartTry())
                    {
                        folder = web.GetFolderByServerRelativeUrl(serverRelativePath);
                        ctx.Load(folder);
                    }
                    using (scope.StartCatch())
                    {
                        file = web.GetFileByServerRelativeUrl(serverRelativePath);
                        ctx.Load(file);
                    }
                }
            }

            try
            {
                ctx.ExecuteQueryRetry();
            }
            catch (Exception e)
            {
                return e;
            }

            //Check if we got data
            if (IsPropertyAvailable(file, "Name"))
            {
                return file;
            }
            else if (IsPropertyAvailable(folder, "Name"))
            {
                return folder;
            }

            return null;
        }

        private IEnumerable<object> GetFolderItems(Folder folder, bool throwError = false)
        {
            var folderAndFiles = new List<object>();
            try
            {
                if (folder != null)
                {
                    var serverRelativePath = GetServerRelativePath(IsPropertyAvailable(folder, "ServerRelativeUrl") ? folder.ServerRelativeUrl : folder.EnsureProperty(f => f.ServerRelativeUrl));

                    //Get cached child items
                    var cachedFolderAndFiles = GetCachedChildItems(serverRelativePath);
                    if (cachedFolderAndFiles != null) return cachedFolderAndFiles;

                    var ctx = folder.Context as ClientContext;

                    if (ctx != null)
                    {
                        var webUrl = GetServerRelativePath(IsPropertyAvailable(ctx.Web, "ServerRelativeUrl") ? ctx.Web.ServerRelativeUrl : ctx.Web.EnsureProperty(w => w.ServerRelativeUrl));

                        //If root of web get subweb
                        if (serverRelativePath.Equals(webUrl, StringComparison.InvariantCultureIgnoreCase))
                        {
                            var subWebs = ctx.Web.EnsureProperty(w => w.Webs.Include(sw => sw.RootFolder));
                            folderAndFiles.AddRange(subWebs.Select(subWeb => subWeb.RootFolder));
                        }
                    }

                    //Get files and folders
                    var files = folder.Context.LoadQuery(folder.Files).OrderBy(f => f.Name);
                    var folders = folder.Context.LoadQuery(folder.Folders).OrderBy(f => f.Name);
                    folder.Context.ExecuteQueryRetry();

                    //Merge
                    folderAndFiles.AddRange(folders);
                    folderAndFiles.AddRange(files);

                    //Cache the result
                    SetCachedChildItems(serverRelativePath, folderAndFiles);
                }
            }
            catch (Exception e)
            {
                if (throwError)
                {
                    WriteErrorInternal(e.Message, "GetFolderItems", exception: e);
                }
            }
            return folderAndFiles;
        }

        private Web FindWebInPath(string serverRelativePath)
        {
            var spoDrive = GetCurrentDrive(serverRelativePath);
            if (spoDrive == null) return null;

            var pathParts = serverRelativePath.Split(PathSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var webUrl = IsPropertyAvailable(spoDrive.Web, "Url") ? spoDrive.Web.Url : spoDrive.Web.EnsureProperty(w => w.Url);
            var webUri = new Uri(webUrl);
            var hostUrl = string.Format("{0}://{1}/", webUri.Scheme, webUri.Host);

            for (var i = pathParts.Length; i >= 0; i--)
            {
                var path = string.Join(PathSeparator, pathParts.Take(i));
                var fullUrl = hostUrl + path;
                var partsServerRelativePath = PathSeparator + path;

                try
                {
                    var cachedWeb = GetCachedWeb(partsServerRelativePath);
                    if (cachedWeb == null)
                    {
                        var ctx = spoDrive.Web.Context.Clone(fullUrl);
                        ctx.ExecuteQueryRetry();
#if DEBUG
                        SetCounter(ctx);
#endif
                        SetCachedWeb(partsServerRelativePath, ctx.Web);
                        return ctx.Web;
                    }
                    else if (cachedWeb.Web != null)
                    {
                        return cachedWeb.Web;
                    }
                }
                catch
                {
                    SetCachedWeb(partsServerRelativePath, null);
                }
            }
            return null;
        }

        private void SetCounter(ClientRuntimeContext ctx)
        {
            ctx.ExecutingWebRequest += (sender, args) =>
            {
                var counter = 0;
                try
                {
                    counter = (int) SessionState.PSVariable.GetValue("WebRequestCounter");
                }
                catch
                {
                }
                counter++;
                SessionState.PSVariable.Set("WebRequestCounter", counter);
            };
        }

        //Copymove helper
        private void CopyMoveImplementation(string sourcePath, string targetPath, bool recurse = false, bool isCopyOperation = true, bool reCreateSourceFolder = true)
        {
            var source = GetFileOrFolder(sourcePath) as ClientObject;
            if (source == null) return;

            var targetUrl = GetServerRelativePath(targetPath);
            var targetIsFile = targetUrl.Split(PathSeparator.ToCharArray()).Last().Contains(".");
            var targetFolderPath = (targetIsFile) ? GetParentServerRelativePath(targetUrl) : targetUrl;
            var sourceWeb = ((ClientContext)source.Context).Web;
            var targetWeb = FindWebInPath(targetFolderPath);

            if (targetWeb == null)
            {
                var msg = "Target web not found";
                var err = new FileNotFoundException(msg);
                WriteErrorInternal(msg, sourcePath, ErrorCategory.ObjectNotFound, true, err);
                return;
            }

            var isSameWeb = IsSameWeb(sourceWeb, targetWeb);
            var targetFolder = GetFileOrFolder(targetFolderPath, false) as Folder;
            var endOfPathFolderCreated = false;
            if (targetFolder == null)
            {
                //Create target folder
                endOfPathFolderCreated = true;
                targetFolder = targetWeb.EnsureFolderPath(PathSeparator + GetWebRelativePath(targetFolderPath));
            }

            if (ShouldProcess(string.Format("{0} to {1}", GetServerRelativePath(sourcePath), targetUrl)))
            {
                if (source is File)
                {
                    var sourceFile = source as File;
                    var targetFilePath = (targetIsFile) ? targetUrl : targetFolderPath + PathSeparator + sourceFile.Name;

                    if (isSameWeb)
                    {
                        if (isCopyOperation)
                        {
                            sourceFile.CopyTo(targetFilePath, Force);
                        }
                        else
                        {
                            var moveToOperations = (Force) ? MoveOperations.Overwrite : MoveOperations.None;
                            sourceFile.MoveTo(targetFilePath, moveToOperations);
                        }
                        sourceFile.Context.ExecuteQueryRetry();
                    }
                    else
                    {
                        var sourceStream = sourceFile.OpenBinaryStream();
                        sourceFile.Context.ExecuteQueryRetry();

                        targetFolder.UploadFile(targetFilePath.Split(PathSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Last(), sourceStream.Value, Force);

                        if (!isCopyOperation)
                        {
                            sourceFile.DeleteObject();
                            sourceFile.Context.ExecuteQueryRetry();
                        }
                    }
                }
                else if (source is Folder && !targetIsFile)
                {
                    var sourceFolder = source as Folder;

                    var rootFolder = (endOfPathFolderCreated || !reCreateSourceFolder) ? targetFolder : targetFolder.CreateFolder(sourceFolder.Name);
                    var folderAndFiles = GetFolderItems(sourceFolder);

                    foreach (var folder in folderAndFiles.OfType<Folder>())
                    {
                        var subFolder = rootFolder.CreateFolder(folder.Name);
                        if (recurse)
                        {
                            CopyMoveImplementation(folder.ServerRelativeUrl, subFolder.ServerRelativeUrl, true, isCopyOperation, false);
                        }
                    }
                    foreach (var file in folderAndFiles.OfType<File>())
                    {
                        CopyMoveImplementation(file.ServerRelativeUrl, rootFolder.ServerRelativeUrl, recurse, isCopyOperation);
                    }

                    if (!isCopyOperation)
                    {
                        sourceFolder.DeleteObject();
                        sourceFolder.Context.ExecuteQueryRetry();
                    }
                }
                else
                {
                    WriteErrorInternal("Operation not supported", sourcePath, ErrorCategory.InvalidOperation);
                }
            }
        }

        //Drive helpers
        private SPODriveInfo GetCurrentDrive(string path)
        {
            return PSDriveInfo as SPODriveInfo ?? GetDrive(path);
        }

        private SPODriveInfo GetDrive(string path)
        {
            if (!path.Contains(":")) return null;
            return ProviderInfo.Drives.FirstOrDefault(d => path.StartsWith(d.Name, StringComparison.InvariantCultureIgnoreCase)) as SPODriveInfo;
        }

        private string RemoveDriveFromPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            var spoDrive = GetCurrentDrive(path);
            if (spoDrive == null) return path;

            var result = Regex.Replace(path, string.Format(@"^{0}:", spoDrive.Name), string.Empty);

            return result;
        }

        private bool IsPathDrive(string path)
        {
            var spoDrive = GetCurrentDrive(path);
            if (spoDrive == null) return false;

            var normalizedPath = GetServerRelativePath(path);
            return normalizedPath.Equals(spoDrive.Root, StringComparison.InvariantCultureIgnoreCase);
        }

        //Validate helpers
        private bool IsPropertyAvailable(ClientObject clientObject, string propertyName)
        {
            return clientObject.IsObjectPropertyInstantiated(propertyName) || clientObject.IsPropertyAvailable(propertyName);
        }

        private bool IsInSameSiteCollection(Web web1, Web web2)
        {
            var ctx1 = web1.Context as ClientContext;
            var ctx2 = web2.Context as ClientContext;

            if (ctx1 == null || ctx2 == null) return false;

            ctx1.Load(ctx1.Site, s => s.Url);
            ctx2.Load(ctx2.Site, s => s.Url);
            ctx1.ExecuteQueryRetry();
            ctx2.ExecuteQueryRetry();

            return ctx1.Site.Url.Equals(ctx2.Site.Url);
        }

        private bool IsSameWeb(Web web1, Web web2)
        {
            var url1 = IsPropertyAvailable(web1, "Id") ? web1.Url : web1.EnsureProperty(w => w.Url);
            var url2 = IsPropertyAvailable(web2, "Id") ? web2.Url : web2.EnsureProperty(w => w.Url);

            return url1.Equals(url2);
        }

        //Error helper
        internal void WriteErrorInternal(string message, object path, ErrorCategory errorCategory = ErrorCategory.NotSpecified, bool terminate = false, Exception exception = null)
        {
            exception = exception ?? new Exception(message);
            var error = new ErrorRecord(exception, message, errorCategory, path);

            if (terminate)
            {
                ThrowTerminatingError(error);
            }
            else
            {
                WriteError(error);
            }
        }

        //Path helpers
        private string GetServerRelativePath(string path)
        {
            var spoDrive = GetCurrentDrive(path);
            if (spoDrive == null) return null;

            var normalizePath = NormalizePath(path);
            normalizePath = RemoveDriveFromPath(normalizePath);
            if (!normalizePath.StartsWith(PathSeparator))
            {
                normalizePath = NormalizePath(spoDrive.Root.TrimEnd(PathSeparator.ToCharArray()) + PathSeparator + normalizePath);
            }
            return normalizePath;
        }

        private string GetParentServerRelativePath(string path)
        {
            var serverRelativePath = GetServerRelativePath(path);
            if (string.IsNullOrEmpty(serverRelativePath)) return serverRelativePath;

            var index = serverRelativePath.LastIndexOf(PathSeparator);
            if (index > -1)
            {
                return serverRelativePath.Substring(0, index);
            }

            return serverRelativePath;
        }

        private string GetWebRelativePath(string serverRelativePath)
        {
            if (string.IsNullOrEmpty(serverRelativePath)) return serverRelativePath;

            var spoDrive = GetCurrentDrive(serverRelativePath);
            if (spoDrive == null) return null;

            var webPath = IsPropertyAvailable(spoDrive.Web, "ServerRelativeUrl") ? spoDrive.Web.ServerRelativeUrl : spoDrive.Web.EnsureProperty(w => w.ServerRelativeUrl);
            var result = Regex.Replace(serverRelativePath, string.Format(@"^{0}", webPath), string.Empty);
            if (string.IsNullOrEmpty(result)) result = PathSeparator;
            return result;
        }

        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            var result = Regex.Replace(path, @"\\", PathSeparator);
            //result = result.Replace("*", string.Empty);
            result = Regex.Replace(result, @"/{2,}", PathSeparator);
            result = result.TrimEnd(PathSeparator.ToCharArray());
            result = string.IsNullOrEmpty(result) ? PathSeparator : result;

            return result;
        }

        //Cache helpers
        private SPODriveCacheItem GetCachedItem(string serverRelativePath)
        {
            var spoDrive = GetCurrentDrive(serverRelativePath);
            if (spoDrive == null) return null;

            var cachedItem = spoDrive.CachedItems.FirstOrDefault(c => c.Path == serverRelativePath && (new TimeSpan(DateTime.Now.Ticks - c.LastRefresh.Ticks)).TotalMilliseconds < spoDrive.ItemTimeout);
            return cachedItem;
        }

        private void SetCachedItem(string serverRelativePath, object obj)
        {
            var spoDrive = GetCurrentDrive(serverRelativePath);
            if (spoDrive == null) return;

            spoDrive.CachedItems.RemoveAll(item => item.Path == serverRelativePath);
            spoDrive.CachedItems.Add(new SPODriveCacheItem
            {
                Path = serverRelativePath,
                LastRefresh = DateTime.Now,
                Item = obj
            });
        }

        private IEnumerable<object> GetCachedChildItems(string serverRelativePath)
        {
            var spoDrive = GetCurrentDrive(serverRelativePath);
            if (spoDrive == null) return null;

            var childItems = spoDrive.CachedItems.Where(c => GetParentServerRelativePath(c.Path) == serverRelativePath && (new TimeSpan(DateTime.Now.Ticks - c.LastRefresh.Ticks)).TotalMilliseconds < spoDrive.ItemTimeout);
            var spoDriveCacheItems = childItems as SPODriveCacheItem[] ?? childItems.ToArray();
            return spoDriveCacheItems.Any() ? spoDriveCacheItems.Select(c => c.Item) : null;
        }

        private void SetCachedChildItems(string parentServerRelativePath, IEnumerable<object> childItems)
        {
            var spoDrive = GetCurrentDrive(parentServerRelativePath);
            if (spoDrive == null) return;

            foreach (var item in childItems)
            {
                var name = string.Empty;

                if (item is Folder)
                {
                    var folder = item as Folder;
                    name = string.IsNullOrEmpty(folder.Name) ? folder.ServerRelativeUrl.Split(PathSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Last() : folder.Name;
                }
                else if (item is File)
                {
                    var file = item as File;
                    name = file.Name;
                }
                SetCachedItem(parentServerRelativePath.TrimEnd(PathSeparator.ToCharArray()) + PathSeparator + name, item);
            }
        }

        private SPODriveCacheWeb GetCachedWeb(string serverRelativePath)
        {
            var spoDrive = GetCurrentDrive(serverRelativePath);
            if (spoDrive == null) return null;

            var result = spoDrive.CachedWebs.FirstOrDefault(c => c.Path == serverRelativePath && (new TimeSpan(DateTime.Now.Ticks - c.LastRefresh.Ticks)).TotalMilliseconds < spoDrive.ItemTimeout * 100);
            return result;
        }

        private void SetCachedWeb(string serverRelativePath, Web web)
        {
            var spoDrive = GetCurrentDrive(serverRelativePath);
            if (spoDrive == null) return;

            spoDrive.CachedWebs.RemoveAll(c => c.Path.Equals(serverRelativePath));
            spoDrive.CachedWebs.Add(new SPODriveCacheWeb
            {
                Web = web,
                Path = serverRelativePath,
                LastRefresh = DateTime.Now
            });
        }

        #endregion
    }
}
