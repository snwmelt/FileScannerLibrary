using FileScannerLibrary.Common;
using FileScannerLibrary.Common.Interface;
using System;

namespace FileScannerLibrary.Asynchronous
{
    class AsyncScanner : IFileScanner, IDisposable
    {
        public event EventHandler<System.IO.FileInfo> FileFoundEvent;
        
        public AsyncScanner()
        {
            throw new System.NotImplementedException();
        }

        public bool Active
        {
            get { throw new NotImplementedException(); }
        }

        public void AddSearchTerm(string SearchTerm)
        {
            throw new NotImplementedException();
        }

        public string Directory
        {
            get { throw new NotImplementedException(); }
        }

        public FileSystemMap SubDirectoryMap { private set; get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool Recursive
        {
            get { throw new NotImplementedException(); }
        }

        public long RecursiveDepth
        {
            get { throw new NotImplementedException(); }
        }

        public void RemoveSearchTerm(string SearchTerm)
        {
            throw new NotImplementedException();
        }

        public void Scan()
        {
            throw new NotImplementedException();
        }

        public ScanMode ScanMode
        {
            get { throw new NotImplementedException(); }
        }

        public string[] SearchFor
        {
            get { throw new NotImplementedException(); }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
