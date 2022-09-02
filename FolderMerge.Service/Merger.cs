using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderMerge.Data.Models;
using FolderMerge.Service.Factories;
using Microsoft.Extensions.Logging;

namespace FolderMerge.Service
{
    public sealed class Merger : IMerger
    {
        private readonly Target target;
        private readonly List<ISourceWatcher> watchers = new();
        private readonly ISourceWatcherFactory watcherFactory;
        private readonly ILinkerFactory linkerFactory;
        private readonly ICollisionHandlerFactory collisionHandlerFactory;
        private readonly IFileSystem fileSystem;
        private readonly ILogger<Merger> logger;

        public Merger(Target target, ISourceWatcherFactory watcherFactory, ILinkerFactory linkerFactory, ICollisionHandlerFactory collisionHandlerFactory, IFileSystem fileSysteme, ILogger<Merger> logger)
        {
            this.target = target;
            this.watcherFactory = watcherFactory;
            this.linkerFactory = linkerFactory;
            this.collisionHandlerFactory = collisionHandlerFactory;
            this.fileSystem = fileSysteme;
            this.logger = logger;
        }

        public void Dispose()
        {
            foreach (var watcher in watchers)
            {
                watcher.Dispose();
            }
        }

        public void Init()
        {
            logger.LogDebug("Initializing Merger on {targetPath}", target.TargetPath);
            ICollisionHandler collisionHandler = collisionHandlerFactory.CreateCollisionHandler(target);
            foreach (var source in target.Sources)
            {
                string[] files = fileSystem.Directory.GetFiles(source.Path);
                ILinker linker = linkerFactory.CreateLinker(source, collisionHandler);
                foreach (var file in files)
                {
                    linker.Link(file);
                }
                watchers.Add(watcherFactory.CreateSourceWatcher(source.Path, linker));
            }
        }

        public void Start()
        {
            foreach (var watcher in watchers)
            {
                watcher.Start();
            }
        }
    }
}
