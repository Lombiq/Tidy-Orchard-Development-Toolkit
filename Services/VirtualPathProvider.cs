﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Orchard.FileSystems.VirtualPath;

namespace Lombiq.IsolatedDevelopmentToolkit.Services
{
    // No need to override DynamicModuleVirtualPathProvider as the VPP implementations are executed in a cascading
    // manner. Might need to add 
    public class VirtualPathProvider : System.Web.Hosting.VirtualPathProvider, IVirtualPathDispatcher, ICustomVirtualPathProvider
    {
        public System.Web.Hosting.VirtualPathProvider Instance
        {
            get { return this; }
        }


        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            this.AlterPathIfOrchard(ref virtualPath);

            var alteredDependencies = new List<string>();
            foreach (string dependency in virtualPathDependencies)
            {
                alteredDependencies.Add(AlterPathIfOrchard(dependency));
            }

            return base.GetCacheDependency(virtualPath, alteredDependencies, utcStart);
        }

        public override bool FileExists(string virtualPath)
        {
            this.AlterPathIfOrchard(ref virtualPath);
            return base.FileExists(virtualPath);
        }

        public override System.Web.Hosting.VirtualFile GetFile(string virtualPath)
        {
            var newPath = AlterPathIfOrchard(virtualPath);

            if (newPath != virtualPath)
            {
                // RemappedVirtualFile fakes that the file comes from under the requested virtual path.
                // If the path of the returned file would be different from the requested path we'd get an
                // exception from view engines.
                return new RemappedVirtualFile(base.GetFile(newPath), virtualPath);
            }

            return base.GetFile(virtualPath);
        }

        public string AlterPathIfOrchard(string virtualPath)
        {
            var orchardPath = virtualPath;

            if (!orchardPath.StartsWith("~"))
            {
                orchardPath = VirtualPathUtility.ToAppRelative(orchardPath);
            }

            if (orchardPath.StartsWith("~/Orchard/src/Orchard.Web/")) return virtualPath;

            orchardPath = orchardPath.Replace("~/", "~/Orchard/src/Orchard.Web/");

            if (base.FileExists(orchardPath) || base.DirectoryExists(orchardPath))
            {
                return orchardPath;
            }

            return virtualPath;
        }


        private class RemappedVirtualFile : System.Web.Hosting.VirtualFile
        {
            private readonly System.Web.Hosting.VirtualFile _originalFile;


            public RemappedVirtualFile(System.Web.Hosting.VirtualFile originalFile, string originalVirtualPath)
                : base(originalVirtualPath)
            {
                _originalFile = originalFile;
            }
        
        
            public override System.IO.Stream Open()
            {
                return _originalFile.Open();
            }
        }
    }
}
