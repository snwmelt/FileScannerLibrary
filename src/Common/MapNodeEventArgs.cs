using System;

namespace FileScannerLibrary.Common
{
    public sealed class MapNodeEventArgs : EventArgs
    {
        private MapNode MapNode;

        public MapNodeEventArgs(ref MapNode MapNode)
        {
            this.MapNode = MapNode;
        }

        public System.IO.DirectoryInfo Node
        {
            get
            {
                return MapNode.Node;
            }
        }

        public long NodeDepth
        {
            get
            {
                return MapNode.NodeDepth;
            }
        }
    }
}
