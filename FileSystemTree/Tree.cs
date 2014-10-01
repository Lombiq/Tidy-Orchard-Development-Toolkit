using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.IsolatedOrchardDevelopmentToolkit.FileSystemTree
{
    /// <summary>
    /// An in-memory representation of a file system (sub)tree, i.e. the tree of directories and files.
    /// </summary>
    /// <remarks>
    /// Using this for file and directory existence checks is much faster than using the actual file system. It's a static snapshot but this
    /// doesn't matter since it's only used to check whether a file or folder exists under the Orchard directory; and the Orchard directory
    /// in an Isolated Development setup should never change during the execution of the application.
    /// </remarks>
    internal class Tree
    {
        private readonly Node _root;


        public Tree(Node root)
        {
            _root = root;
        }


        public bool FileExists(string normalizedVirtualPath)
        {
            var node = FindNode(normalizedVirtualPath);
            return node != null && node.NodeType == NodeType.File;
        }


        public bool DirectoryExists(string normalizedVirtualPath)
        {
            var node = FindNode(normalizedVirtualPath);
            return node != null && node.NodeType == NodeType.Directory;
        }

        private Node FindNode(string normalizedVirtualPath)
        {
            normalizedVirtualPath = normalizedVirtualPath.Substring(2); // Cutting off "~/".

            var segments = normalizedVirtualPath.Split('/');
            var i = 0;
            var currentNode = _root;
            Node currentChild = null;
            while (i < segments.Length && (currentChild = currentNode.GetChild(segments[i])) != null)
            {
                currentNode = currentChild;
                i++;
            }
            return i == segments.Length ? currentNode : null;
        }
    }
}
