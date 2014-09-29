using System.IO;

namespace FileScannerLibrary.Common
{
    public struct MapNode
    {
        public readonly DirectoryInfo Node;
        public readonly long          NodeDepth;

        public MapNode(DirectoryInfo Node, long NodeDepth)
        {
            this.Node      = Node;
            this.NodeDepth = NodeDepth;
        }
    }
}
