using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orchard.FileSystems.VirtualPath;

namespace Lombiq.IsolatedDevelopmentToolkit.Services
{
    public class VirtualPathProviderDecorator : IVirtualPathProvider
    {
        private readonly IVirtualPathProvider _virtualPathProvider;
        private readonly IVirtualPathDispatcher _virtualPathDispatcher;


        public VirtualPathProviderDecorator(IVirtualPathProvider virtualPathProvider, IVirtualPathDispatcher virtualPathDispatcher)
        {
            _virtualPathProvider = virtualPathProvider;
            _virtualPathDispatcher = virtualPathDispatcher;
        }


        public string Combine(params string[] paths)
        {
            return _virtualPathProvider.Combine(paths);
        }

        public string ToAppRelative(string virtualPath)
        {
            return _virtualPathProvider.ToAppRelative(virtualPath);
        }

        public string MapPath(string virtualPath)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            return _virtualPathProvider.MapPath(virtualPath);
        }

        public bool FileExists(string virtualPath)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            return _virtualPathProvider.FileExists(virtualPath);
        }

        public bool TryFileExists(string virtualPath)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            return _virtualPathProvider.FileExists(virtualPath);
        }

        public Stream OpenFile(string virtualPath)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            return _virtualPathProvider.OpenFile(virtualPath);
        }

        public StreamWriter CreateText(string virtualPath)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            return _virtualPathProvider.CreateText(virtualPath);
        }

        public Stream CreateFile(string virtualPath)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            return _virtualPathProvider.CreateFile(virtualPath);
        }

        public DateTime GetFileLastWriteTimeUtc(string virtualPath)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            return _virtualPathProvider.GetFileLastWriteTimeUtc(virtualPath);
        }

        public string GetFileHash(string virtualPath)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            return _virtualPathProvider.GetFileHash(virtualPath);
        }

        public string GetFileHash(string virtualPath, IEnumerable<string> dependencies)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            return _virtualPathProvider.GetFileHash(virtualPath);
        }

        public void DeleteFile(string virtualPath)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            _virtualPathProvider.DeleteFile(virtualPath);
        }

        public bool DirectoryExists(string virtualPath)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            return _virtualPathProvider.DirectoryExists(virtualPath);
        }

        public void CreateDirectory(string virtualPath)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            _virtualPathProvider.CreateDirectory(virtualPath);
        }

        public string GetDirectoryName(string virtualPath)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            return _virtualPathProvider.GetDirectoryName(virtualPath);
        }

        public void DeleteDirectory(string virtualPath)
        {
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            _virtualPathProvider.DeleteDirectory(virtualPath);
        }

        public IEnumerable<string> ListFiles(string virtualPath)
        {
            var files = _virtualPathProvider.ListFiles(virtualPath);
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            return _virtualPathProvider.ListFiles(virtualPath).Select(_virtualPathDispatcher.ConvertBackIfOrchard).Union(files);
        }

        public IEnumerable<string> ListDirectories(string virtualPath)
        {
            var directories = _virtualPathProvider.ListDirectories(virtualPath);
            _virtualPathDispatcher.AlterPathIfOrchard(ref virtualPath);
            return _virtualPathProvider.ListDirectories(virtualPath).Select(_virtualPathDispatcher.ConvertBackIfOrchard).Union(directories);
        }
    }
}
