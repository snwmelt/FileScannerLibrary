using FileScannerLibrary.Asynchronous;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileScannerLibrary.Common
{
    public class FileSystemMap : IDisposable
    {
        public event EventHandler<MapNodeEventArgs> NodeAddedEvent;

        private List<MapNode> directoryMap;
        private long          maxDepth;
        private DirectoryInfo rootDirectory;

        public FileSystemMap(DirectoryInfo RootDirectory)
        {
            rootDirectory = RootDirectory;
            DirectoryMap  = new List<MapNode>();

            addNode(new MapNode(RootDirectory, -1));
        }

        public FileSystemMap(string RootDirectory)
        {
            this.RootDirectory = RootDirectory;
        }

        public FileSystemMap()
        {

        }

        private void addNode(MapNode mapNode)
        {
            DirectoryMap.Add(mapNode);

            if (NodeAddedEvent != null)
            {
                NodeAddedEvent(this, new MapNodeEventArgs(ref mapNode));
            }
        }

        public List<MapNode> DirectoryMap
        {
            private set
            {
                directoryMap = value;
            }

            get
            {
                return directoryMap;
            }
        }

        public void Dispose()
        {
            if (DirectoryMap != null)
            {
                DirectoryMap.Clear();
                DirectoryMap.TrimExcess();
                DirectoryMap = null;
            }
        }

        public void GenerateMap()
        {
            genSubDirMap(rootDirectory, 0);
        }

        public long MaxDepth 
        {
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Max Depth Cannot Be A Negetive Value");
                }

                maxDepth = value;
            }
            get
            {
                return maxDepth;
            }
        }

        public string RootDirectory
        {
            set
            {
                if (!Directory.Exists(value))
                {
                    throw new DirectoryNotFoundException(value + " Does Not Exsist");
                }

                rootDirectory = new DirectoryInfo(value);

                if (DirectoryMap == null)
                {
                    DirectoryMap = new List<MapNode>();
                    DirectoryMap.Add(new MapNode(rootDirectory, -1));
                }
                else
                {
                    DirectoryMap[0] = new MapNode(rootDirectory, -1);
                }
            }

            get
            {
                return rootDirectory.FullName;
            }
        }

        private void genSubDirMap(DirectoryInfo directoryInfo, long depth)
        {
            if (depth > MaxDepth)
            {
                return;
            }

            DirectoryInfo[] Directories = directoryInfo.GetDirectories();

            long nodeDepth = (depth + 1);

            for (int i = 0; i < Directories.Length; i++)
            {
                addNode(new MapNode(Directories[i], nodeDepth));
                genSubDirMap(Directories[i], nodeDepth);
            }
        }
    }
}
