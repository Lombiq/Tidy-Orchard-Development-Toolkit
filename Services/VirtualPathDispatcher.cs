using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Lombiq.IsolatedOrchardDevelopmentToolkit.FileSystemTree;
using Orchard.Validation;

namespace Lombiq.IsolatedOrchardDevelopmentToolkit.Services
{
    public class VirtualPathDispatcher : IVirtualPathDispatcher
    {
        private readonly Lazy<Tree> _treeLazy;
        private Tree Tree { get { return _treeLazy.Value; } }


        public VirtualPathDispatcher()
        {
            _treeLazy = new Lazy<Tree>(BuildTree);
        }


        public string ReMapPathIfOrchard(string virtualPath)
        {
            Argument.ThrowIfNullOrEmpty(virtualPath, "virtualPath");

            var orchardPath = ToAppRelativeIfNecessary(virtualPath);

            if (orchardPath.StartsWith("~/Orchard/src/Orchard.Web/")) return virtualPath;

            orchardPath = orchardPath.Replace("~/", "~/Orchard/src/Orchard.Web/");

            // This is needed so paths like 
            // "~/Orchard/src/Orchard.Web/Modules/Piedone.HelpfulLibraries/../../../../lib/autofac/Autofac.dll"
            // are normalized to "~/Orchard/lib/autofac/Autofac.dll".
            orchardPath = VirtualPathUtility.ToAppRelative(orchardPath);

            if (Tree.FileExists(orchardPath) || Tree.DirectoryExists(orchardPath))
            {
                return orchardPath;
            }

            return virtualPath;
        }

        public string ConvertBackIfOrchard(string virtualPath)
        {
            var orchardPath = ToAppRelativeIfNecessary(virtualPath);

            if (!orchardPath.StartsWith("~/Orchard/src/Orchard.Web/")) return virtualPath;

            return orchardPath.Replace("~/Orchard/src/Orchard.Web/", "~/");
        }


        private Tree BuildTree()
        {
            var rootDirectory = HostingEnvironment.VirtualPathProvider.GetDirectory("~/Orchard");
            var rootNode = new Node("ROOT", NodeType.Directory);
            rootNode.AddChild(ExpandVirtualNode(rootDirectory));
            return new Tree(rootNode);
        }

        private Node ExpandVirtualNode(VirtualFileBase virtualNode)
        {
            if (!virtualNode.IsDirectory) return new Node(virtualNode.Name, NodeType.File);

            var node = new Node(virtualNode.Name, NodeType.Directory);

            var virtualDirectory = (VirtualDirectory)virtualNode;
            foreach (var child in virtualDirectory.Directories.Cast<VirtualFileBase>().Union(virtualDirectory.Files.Cast<VirtualFileBase>()))
            {
                // Excluding common folders that are unnecessary to process, the easy way. May need to be configurable.
                if (child.Name == ".hg" || child.Name == ".git") continue;

                node.AddChild(ExpandVirtualNode(child));
            }

            return node;
        }


        private static string ToAppRelativeIfNecessary(string virtualPath)
        {
            // Otherwise exceptions like "Failed to map the path 
            // '/HostingSuite/Orchard/src/Orchard.Web/Modules/Lombiq.Hosting.Azure.Indexing/$(ModulesRoot)../Lucene/Lucene.csproj'.
            // would be thrown.
            virtualPath = virtualPath.Replace("$(ModulesRoot)", string.Empty);

            if (!virtualPath.StartsWith("~"))
            {
                return VirtualPathUtility.ToAppRelative(virtualPath);
            }

            return virtualPath;
        }
    }
}
