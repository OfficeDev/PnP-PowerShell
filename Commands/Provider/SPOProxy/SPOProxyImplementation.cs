using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.Serialization;
using Microsoft.PowerShell.Commands;
using Microsoft.SharePoint.Client;
using File = System.IO.File;

namespace PnP.PowerShell.Commands.Provider.SPOProxy
{
    internal class SPOProxyImplementation
    {
        internal static void AddAlias(SessionState sessionState)
        {
            //Set-Alias -Name Copy-Item -Value Copy-PnPItemProxy -Scope 1
            sessionState.InvokeCommand.InvokeScript($@"Set-Alias -Name Copy-Item -Value {SPOProxyCopyItem.CmdletVerb}-{SPOProxyCmdletBase.CmdletNoun} -Scope Global", false, PipelineResultTypes.None, null, null);
            sessionState.InvokeCommand.InvokeScript($@"Set-Alias -Name Move-Item -Value {SPOProxyMoveItem.CmdletVerb}-{SPOProxyCmdletBase.CmdletNoun} -Scope Global", false, PipelineResultTypes.None, null, null);
        }

        internal static void RemoveAlias(SessionState sessionState)
        {
            sessionState.InvokeCommand.InvokeScript(@"Remove-Item -Path alias:\Copy-Item -ErrorAction SilentlyContinue", false, PipelineResultTypes.None, null, null);
            sessionState.InvokeCommand.InvokeScript(@"Remove-Item -Path alias:\Move-Item -ErrorAction SilentlyContinue", false, PipelineResultTypes.None, null, null);
        }

        internal static void CopyMoveImplementation(SPOProxyCmdletBase cmdlet)
        {
            //Flag
            var isProcessed = false;

            //Destination
            ProviderInfo destProvider;
            PSDriveInfo destDrive;
            var destPath = cmdlet.SessionState.Path.GetUnresolvedProviderPathFromPSPath(cmdlet.Destination ?? string.Empty, out destProvider, out destDrive).Replace($"{destDrive.Name}:", string.Empty);
            var destParts = destPath.Split(@"\/".ToCharArray());
            var destFolderPath = (cmdlet.Container || !destParts.Last().Contains(".")) ? destPath : string.Join(@"\", destParts.Take(destParts.Length - 1));
            if (string.IsNullOrEmpty(destFolderPath)) destFolderPath = @"\";
            var destFolderFullPath = $@"{destDrive.Name}:{destFolderPath}";
            var destIsFile = destPath != destFolderPath;

            //Source
            var pathInfos = cmdlet.PsPaths.SelectMany(psPath => cmdlet.SessionState.Path.GetResolvedPSPathFromPSPath(psPath)).ToList();

            //Verify all sources uses same provider and that SPOProvider in use
            if (
                pathInfos.Any() &&
                pathInfos.All(p => p.Provider == pathInfos.First().Provider) &&
                (destProvider.ImplementingType == typeof(SPOProvider) || pathInfos.First().Provider.ImplementingType == typeof(SPOProvider)) &&
                cmdlet.ShouldProcess($"Source: {string.Join(", ", cmdlet.PsPaths)} to Destination: {destFolderFullPath}", cmdlet.CmdletType)
                )
            {
                var srcProvider = pathInfos.First().Provider;

                //Verify destination file compatibility
                if (destIsFile && pathInfos.Count() != 1)
                {
                    cmdlet.ThrowTerminatingError(new ErrorRecord(new ArgumentException("Can only copy one file to another file"), "Can only copy one file to another file", ErrorCategory.InvalidArgument, cmdlet));
                }

                if (destIsFile && cmdlet.InvokeProvider.Item.IsContainer(pathInfos.First().Path))
                {
                    cmdlet.ThrowTerminatingError(new ErrorRecord(new ArgumentException("Can not copy directory to file"), "Can not copy directory to file", ErrorCategory.InvalidArgument, cmdlet));
                }

                //Copy from FileSystem to SharePoint
                if (srcProvider.ImplementingType == typeof(FileSystemProvider) && destProvider.ImplementingType == typeof(SPOProvider))
                {

                    //Create destination folder
                    var isDestFolderCreated = !cmdlet.InvokeProvider.Item.Exists(destFolderFullPath);
                    var folderObj = (destDrive.Root != destFolderFullPath) ? cmdlet.InvokeProvider.Item.New(new[] { destFolderFullPath }, string.Empty, "Folder", null, cmdlet.Force) : cmdlet.InvokeProvider.Item.Get(destFolderFullPath);
                    var destFolder = (Folder)folderObj.First().BaseObject;

                    foreach (var pathInfo in pathInfos)
                    {
                        if (File.Exists(pathInfo.Path))
                        {
                            var fileInfo = new FileInfo(pathInfo.Path);
                            using (var fs = fileInfo.OpenRead())
                            {
                                var result = cmdlet.InvokeProvider.Item.New(new[] { $@"{destFolderFullPath}\{(destIsFile ? destParts.Last() : fileInfo.Name)}" }, string.Empty, "File", fs, cmdlet.Force);
                                HandleResult(result, cmdlet);
                            }
                        }
                        else if (Directory.Exists(pathInfo.Path))
                        {
                            var rootDirInfo = new DirectoryInfo(pathInfo.Path);
                            var rootFolder = isDestFolderCreated ? destFolder : destFolder.CreateFolder(rootDirInfo.Name);
                            var rootFolderFullPath = isDestFolderCreated ? destFolderFullPath : $@"{destFolderFullPath}\{rootFolder.Name}";

                            if (cmdlet.Recurse)
                            {
                                var spoChildItems = cmdlet.InvokeProvider.ChildItem.Get(pathInfo.Path, cmdlet.Recurse);
                                foreach (var spoChildItem in spoChildItems)
                                {
                                    if ((bool)spoChildItem.Properties["PSIsContainer"].Value)
                                    {
                                        var childDir = (DirectoryInfo)spoChildItem.BaseObject;
                                        var childPath = childDir.FullName.Replace(rootDirInfo.FullName, string.Empty);
                                        var result = cmdlet.InvokeProvider.Item.New(new[] { $@"{rootFolderFullPath}\{childPath}" }, string.Empty, "Folder", null, cmdlet.Force);
                                        HandleResult(result, cmdlet);
                                    }
                                    else
                                    {
                                        var childFileInfo = (FileInfo)spoChildItem.BaseObject;
                                        var childPath = childFileInfo.FullName.Replace(rootDirInfo.FullName, string.Empty);
                                        using (var fs = childFileInfo.OpenRead())
                                        {
                                            var result = cmdlet.InvokeProvider.Item.New(new[] { $@"{rootFolderFullPath}\{childPath}" }, string.Empty, "File", fs, cmdlet.Force);
                                            HandleResult(result, cmdlet);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    isProcessed = true;
                }
                //Copy from SharePoint to FileSystem
                else if (srcProvider.ImplementingType == typeof(SPOProvider) && destProvider.ImplementingType == typeof(FileSystemProvider))
                {
                    //Create destination directory
                    var isDestDirCreated = !cmdlet.InvokeProvider.Item.Exists(destFolderFullPath);
                    var dirObject = (destDrive.Root != destFolderFullPath) ? cmdlet.InvokeProvider.Item.New(new[] { destFolderFullPath }, string.Empty, "Directory", null, true) : cmdlet.InvokeProvider.Item.Get(destFolderFullPath);
                    var destDir = (DirectoryInfo)dirObject.First().BaseObject;

                    foreach (var pathInfo in pathInfos)
                    {
                        var spoObjects = cmdlet.InvokeProvider.Item.Get(pathInfo.Path);
                        foreach (var spoObject in spoObjects)
                        {
                            if (spoObject.BaseObject is Microsoft.SharePoint.Client.File)
                            {
                                var file = (Microsoft.SharePoint.Client.File)spoObject.BaseObject;
                                ((ClientContext)file.Context).Web.SaveFileToLocal(file.EnsureProperty(f => f.ServerRelativeUrl), destFolderFullPath, destIsFile ? destParts.Last() : null, s => cmdlet.Force);
                                HandleResult($@"{destFolderFullPath}\{(destIsFile ? destParts.Last() : file.Name)}", cmdlet);
                            }
                            else if (spoObject.BaseObject is Microsoft.SharePoint.Client.Folder)
                            {
                                var rootFolder = (Microsoft.SharePoint.Client.Folder)spoObject.BaseObject;
                                rootFolder.EnsureProperties(p => p.ServerRelativeUrl, p => p.Name);
                                var rootDirInfo = (isDestDirCreated) ? destDir : destDir.CreateSubdirectory(rootFolder.Name);
                                HandleResult(rootDirInfo, cmdlet);

                                if (cmdlet.Recurse)
                                {
                                    var spoChildItems = cmdlet.InvokeProvider.ChildItem.Get(pathInfo.Path, cmdlet.Recurse);
                                    foreach (var spoChildItem in spoChildItems)
                                    {
                                        if ((bool)spoChildItem.Properties["PSIsContainer"].Value)
                                        {
                                            var childFolder = (Microsoft.SharePoint.Client.Folder)spoChildItem.BaseObject;
                                            childFolder.EnsureProperty(p => p.ServerRelativeUrl);
                                            var childPath = childFolder.ServerRelativeUrl.Replace(rootFolder.ServerRelativeUrl, string.Empty);
                                            var result = cmdlet.InvokeProvider.Item.New(new[] { $@"{rootDirInfo.FullName}\{childPath}" }, string.Empty, "Directory", null, cmdlet.Force);
                                            HandleResult(result, cmdlet);
                                        }
                                        else
                                        {
                                            var childFile = (Microsoft.SharePoint.Client.File)spoChildItem.BaseObject;
                                            childFile.EnsureProperty(p => p.ServerRelativeUrl);
                                            var childParts = childFile.ServerRelativeUrl.Replace(rootFolder.ServerRelativeUrl, string.Empty).Split(@"\/".ToCharArray());
                                            var childPath = string.Join(@"\", childParts.Take(childParts.Length - 1));
                                            ((ClientContext)childFile.Context).Web.SaveFileToLocal(childFile.ServerRelativeUrl, $@"{rootDirInfo.FullName}\{childPath}", null, s => cmdlet.Force);
                                            HandleResult($@"{rootDirInfo.FullName}\{childPath}", cmdlet);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    isProcessed = true;
                }
                if (isProcessed && cmdlet.CmdletType == SPOProxyMoveItem.CmdletVerb)
                {
                    cmdlet.InvokeProvider.Item.Remove(pathInfos.Select(p => p.Path).ToArray(), true, true, true);
                }
            }

            //Cmd not processed transfer to standard provider
            if (!isProcessed)
            {
                cmdlet.WriteObject(cmdlet.InvokeCommand.InvokeScript($@"$args=$args[0];Microsoft.PowerShell.Management\{cmdlet.CmdletType}-Item @args", false, PipelineResultTypes.None, null, new Hashtable(cmdlet.MyInvocation.BoundParameters)));
            }
        }

        private static void HandleResult(string fullPath, SPOProxyCmdletBase cmdlet)
        {
            if (cmdlet.PassThru)
            {
                cmdlet.WriteObject(cmdlet.InvokeProvider.Item.Get(fullPath));
            }
        }

        private static void HandleResult(object result, SPOProxyCmdletBase cmdlet)
        {
            if (cmdlet.PassThru)
            {
                cmdlet.WriteObject(result);
            }
        }
    }
}
