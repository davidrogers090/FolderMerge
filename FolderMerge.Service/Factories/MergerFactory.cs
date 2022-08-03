using FolderMerge.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        private readonly ILoggerFactory _loggerFactory;

        public MergerFactory(ISourceWatcherFactory watcherFactory, ILinkerFactory linkerFactory, ICollisionHandlerFactory collisionHandlerFactory, ILoggerFactory loggerFactory)
        {
            _watcherFactory = watcherFactory;
            _linkerFactory = linkerFactory;
            _collisionHandlerFactory = collisionHandlerFactory;
            _loggerFactory = loggerFactory;
        }
        public IMerger CreateMerger(Target target)
        {
            return new Merger(target, _watcherFactory, _linkerFactory, _collisionHandlerFactory, _loggerFactory.CreateLogger<Merger>());
        }
    }
}
