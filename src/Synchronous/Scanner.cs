using FileScannerLibrary.Common;
using FileScannerLibrary.Common.Interface;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileScannerLibrary.Synchronous
{
    public sealed class Scanner : IFileScanner, IDisposable
    {
        public event System.EventHandler<System.IO.FileInfo> FileFoundEvent;

        private static TypeAccessException ActiveScanException        = new TypeAccessException(ActiveScanExceptionMessage);
        private static string              ActiveScanExceptionMessage = @"Fields Or Properties Cannot Be Modified Whilst A Scan Is Occuring";

        private string   directory;
        private string[] searchFor;
        private bool     recursive;
        private long     recursiveDepth;

        public Scanner()
        {
            recursiveDepth = 0;
        }

        public bool Active { private set; get; }

        public void AddSearchTerm(string SearchTerm)
        {
            if (Active)
            {
                throw ActiveScanException;
            }

            if (SearchFor == null)
            {
                SearchFor = new string[1] { SearchTerm };
            }
            else
            {
                remodel(resizeMode.grow, SearchTerm);
            }
        }

        public string Directory 
        {
            set
            {
                if (Active)
                {
                    throw ActiveScanException;
                }

                directory = value;
            }

            get
            {
                return directory;
            }
        }

        public void Dispose()
        {
            if (SubDirectoryMap != null)
            {
                SubDirectoryMap.Dispose();
            }
        }

        public FileSystemMap SubDirectoryMap { private set; get; }

        public bool Recursive
        {
            set
            {
                if (Active)
                {
                    throw ActiveScanException;
                }

                recursive = value;
            }

            get
            {
                return recursive;
            }
        }

        public long RecursiveDepth
        {
            set
            {
                if (Active)
                {
                    throw ActiveScanException;
                }

                recursiveDepth = value;
            }
            get
            {
                return recursiveDepth;
            }
        }

        private void recursiveScan()
        {
            if (!Active)
                return;

            shallowScan();

            SubDirectoryMap = new FileSystemMap(new DirectoryInfo(Directory));

            SubDirectoryMap.MaxDepth        = RecursiveDepth;
            SubDirectoryMap.NodeAddedEvent +=
                (sender, e) =>
                {
                    shallowScan(e.Node);
                };

            SubDirectoryMap.GenerateMap();
        }

        private void remodel(resizeMode resizeMode, string searchTerm)
        {
            long     arrayLength = SearchFor.Length;
            string[] tempArray   = null;

            switch (resizeMode)
            {
                case resizeMode.grow:
                    tempArray = new string[arrayLength + 1];

                    Array.Copy(SearchFor, tempArray, arrayLength);
                    SearchFor[arrayLength] = searchTerm;
                    break;

                case resizeMode.shrink:
                    tempArray = new string[arrayLength - 1];
                    
                    long offset = 0;

                    for (int i = 0; i < arrayLength; i++)
                    {
                        if (searchTerm.Equals(SearchFor[i]))
                        {
                            offset--;
                            continue;
                        }

                        tempArray[i + offset] = SearchFor[i];
                    }

                    break;
            }

            SearchFor = tempArray;
            tempArray = null;
        }

        public void RemoveSearchTerm(string SearchTerm)
        {
            remodel(resizeMode.shrink, SearchTerm);
        }

        private enum resizeMode
        {
            grow,
            shrink
        }

        public void Scan()
        {
            if (String.IsNullOrEmpty(Directory))
            {
                throw new ArgumentNullException("The Directory Property Is Null Or Empty");
            }

            if (!System.IO.Directory.Exists(Directory))
            {
                throw new DirectoryNotFoundException("Directory Does Not Exist");
            }

            if (SearchFor == null)
            {
                return;
            }

            if (SearchFor.Length < 1)
            {
                return;
            }

            Active = true;

            if (Recursive)
            {
                recursiveScan();
            }
            else
            {
                shallowScan();
            }

            Active = false;
        }

        public ScanMode ScanMode { set; get; }

        public string[] SearchFor
        {
            set
            {
                if (Active)
                {
                    throw ActiveScanException;
                }
                else
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException();
                    }

                    searchFor = value;
                }
            }

            get 
            {
                return searchFor;
            }
        }

        private void shallowScan(DirectoryInfo DirectoryInfo)
        {
            if (!Active)
                return;

            FileInfo[] FileInfo = DirectoryInfo.GetFiles();

            for(int i = 0; i < FileInfo.Length; i++)
            {
                string matchString = null;

                switch (ScanMode)
                {
                    case ScanMode.MatchExetension:
                        matchString = FileInfo[i].Extension;
                        break;

                    case ScanMode.MatchName:
                        matchString = FileInfo[i].Name;
                        break;
                }

                for (int x = 0; x < SearchFor.Length; x++)
                {
                    if (matchString.Equals(SearchFor[x]))
                    {
                        if (FileFoundEvent != null)
                        {
                            FileFoundEvent(this, FileInfo[i]);
                        }
                    }
                }
            }
        }

        private void shallowScan()
        {
            shallowScan(new DirectoryInfo(Directory));
        }

        public void Stop()
        {
            Active = false;
        }
    }
}
