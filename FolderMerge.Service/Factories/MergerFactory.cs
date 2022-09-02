using FolderMerge.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderMerge.Service.Factories
{
    public class MergerFactory : IMergerFactory
    {
        private readonly ISourceWatcherFactory _watcherFactory;
        private readonly ILinkerFactory _linkerFactory;
        private readonly ICollisionHandlerFactory _collisionHandlerFactory;
        private readonly IFileSystem _fileSystem;
        private readonly ILoggerFactory _loggerFactory;

        public MergerFactory(ISourceWatcherFactory watcherFactory, ILinkerFactory linkerFactory, ICollisionHandlerFactory collisionHandlerFactory, IFileSystem fileSystem, ILoggerFactory loggerFactory)
        {
            _watcherFactory = watcherFactory;
            _linkerFactory = linkerFactory;
            _collisionHandlerFactory = collisionHandlerFactory;
            _fileSystem = fileSystem;
            _loggerFactory = loggerFactory;
        }
        public IMerger CreateMerger(Target target)
        {
            return new Merger(target, _watcherFactory, _linkerFactory, _collisionHandlerFactory, _fileSystem, _loggerFactory.CreateLogger<Merger>());
        }
    }
}
