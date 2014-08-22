using System.IO;

namespace FileScannerLibrary.Common
{
    public sealed class MapNode
    {
        private DirectoryInfo node;
        private long          nodeDepth;

        public MapNode(DirectoryInfo Node, long NodeDepth)
        {
            node      = Node;
            nodeDepth = NodeDepth;
        }

        public DirectoryInfo Node
        {
            get
            {
                return node;
            }
        }

        public long NodeDepth
        {
            get
            {
                return nodeDepth;
            }
        }
    }
}
