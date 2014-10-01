using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lombiq.IsolatedOrchardDevelopmentToolkit.FileSystemTree
{
    internal class Node
    {
        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }

        private readonly NodeType _nodeType;
        public NodeType NodeType
        {
            get { return _nodeType; }
        }

        private Dictionary<string, Node> _children;
        public IEnumerable<Node> Children
        {
            get { return _children.Values; }
        }


        public Node(string name, NodeType nodeType)
        {
            _name = name;
            _nodeType = nodeType;
            _children = new Dictionary<string, Node>();
        }


        public Node GetChild(string name)
        {
            Node node;
            if (_children.TryGetValue(name.ToLowerInvariant(), out node)) return node;
            return null;
        }

        public void AddChild(Node node)
        {
            // The virtual file system, that is mimicked here, is case-insensitive, so normalizing the name.
            _children[node.Name.ToLowerInvariant()] = node;
        }
    }
}
