using FolderMerge.Data.Models;
using FolderMerge.Service;
using FolderMerge.Service.Factories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderMerge.Test.Stubs
{
    public class SourceWatcherFactoryStub : ISourceWatcherFactory
    {
        public List<SourceWatcherStub> SourceWatchers { get; } = new();

        public HashSet<string> SourcePaths { get; } = new();

        public ISourceWatcher CreateSourceWatcher(string sourcePath, ILinker linker)
        {
            SourcePaths.Add(sourcePath);
            SourceWatcherStub watcher = new();
            SourceWatchers.Add(watcher);
            return watcher;
        }
    }
}
