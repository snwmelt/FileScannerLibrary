using System;
using System.IO;

namespace FileScannerLibrary.Common.Interface
{
    public interface IFileScanner
    {
        event EventHandler<FileInfo> FileFoundEvent;

        bool          Active          { get; }
        string        Directory       { get; }
        FileSystemMap SubDirectoryMap { get; }
        string[]      SearchFor       { get; }
        bool          Recursive       { get; }
        long          RecursiveDepth  { get; }
        ScanMode      ScanMode        { get; }

        void AddSearchTerm(string SearchTerm);
        void RemoveSearchTerm(string SearchTerm);
        void Scan();
        void Stop();
    }
}
