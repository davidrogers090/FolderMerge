using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderMerge.Service
{
    public class FileSystemWatcherStub : IFileSystemWatcher
    {
        public bool IsDisposed { get; set; }
        public bool IncludeSubdirectories { get; set; }
        public bool EnableRaisingEvents { get; set; }
        public string Filter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Collection<string> Filters => throw new NotImplementedException();

        public int InternalBufferSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public NotifyFilters NotifyFilter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Path { get; set; }
        public ISite Site { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISynchronizeInvoke SynchronizingObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event FileSystemEventHandler Changed = delegate { };
        public event FileSystemEventHandler Created = delegate { };
        public event FileSystemEventHandler Deleted = delegate { };
        public event ErrorEventHandler Error = delegate { };
        public event RenamedEventHandler Renamed = delegate { };

        public FileSystemWatcherStub(string path)
        {
            Path = path;
        }

        public void BeginInit()
        {
            
        }

        public void Dispose()
        {
            IsDisposed = true;
        }

        public void EndInit()
        {
        }

        public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
        {
            throw new NotImplementedException();
        }

        public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
        {
            throw new NotImplementedException();
        }

        public void TriggerCreated(string fileName)
        {
            Created.Invoke(this, new FileSystemEventArgs(WatcherChangeTypes.Created, Path, fileName));
        }

        public void TriggerDeleted(string fileName)
        {
            Deleted.Invoke(this, new FileSystemEventArgs(WatcherChangeTypes.Deleted, Path, fileName));
        }

        public void TriggerError()
        {
            Error.Invoke(this, new ErrorEventArgs(new Exception("TestMessage")));
        }

        public void TriggerRenamed(string oldFileName, string newFileName)
        {
            Renamed.Invoke(this, new(WatcherChangeTypes.Renamed, Path, newFileName, oldFileName));
        }
    }
}
