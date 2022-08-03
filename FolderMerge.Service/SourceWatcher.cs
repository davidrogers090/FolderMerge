using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderMerge.Service
{
    public sealed class SourceWatcher : ISourceWatcher
    {
        private readonly FileSystemWatcher fileSystemWatcher;
        private readonly ILinker linker;
        private readonly ILogger<SourceWatcher> logger;

        public SourceWatcher(string sourcePath, ILinker linker, ILogger<SourceWatcher> logger)
        {
            this.linker = linker;
            this.fileSystemWatcher = new(sourcePath);
            this.fileSystemWatcher.Created += File_Created;
            this.fileSystemWatcher.Renamed += File_Renamed;
            this.fileSystemWatcher.Deleted += File_Deleted;
            this.fileSystemWatcher.Error += FileSystemWatcher_Error;
            this.logger = logger;
        }

        private void FileSystemWatcher_Error(object sender, ErrorEventArgs e)
        {
            logger.LogError(e.GetException(), "FileSystemWatcher threw an error.");
        }

        private void File_Deleted(object sender, FileSystemEventArgs e)
        {
            linker.Delete(e.FullPath);
        }

        private void File_Renamed(object sender, RenamedEventArgs e)
        {
            logger.LogDebug("File {old} renamed to {new}", e.OldName, e.Name);
            linker.Delete(e.OldFullPath);
            linker.Link(e.FullPath);
        }

        private void File_Created(object sender, FileSystemEventArgs e)
        {
            linker.Link(e.FullPath);
        }

        public void Start()
        {
            logger.LogDebug("Starting FileSystemWatcher on {path}", this.fileSystemWatcher.Path);
            this.fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Dispose()
        {
            fileSystemWatcher.Dispose();
        }
    }
}
