using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderMerge.Service
{
    public sealed class SourceWatcher : IDisposable
    {
        private readonly FileSystemWatcher fileSystemWatcher;
        private readonly Linker linker;

        public SourceWatcher(string sourcePath, Linker linker)
        {
            this.linker = linker;
            this.fileSystemWatcher = new(sourcePath);
            this.fileSystemWatcher.Created += File_Created;
            this.fileSystemWatcher.Renamed += File_Renamed;
            this.fileSystemWatcher.Deleted += File_Deleted;
            this.fileSystemWatcher.Error += FileSystemWatcher_Error;
        }

        private void FileSystemWatcher_Error(object sender, ErrorEventArgs e)
        {
            throw e.GetException();
        }

        private void File_Deleted(object sender, FileSystemEventArgs e)
        {
            linker.Delete(e.FullPath);
        }

        private void File_Renamed(object sender, RenamedEventArgs e)
        {

            linker.Delete(e.OldFullPath);
            linker.Link(e.FullPath);
        }

        private void File_Created(object sender, FileSystemEventArgs e)
        {
            linker.Link(e.FullPath);
        }

        public void Start()
        {
            this.fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Dispose()
        {
            fileSystemWatcher.Dispose();
        }
    }
}
