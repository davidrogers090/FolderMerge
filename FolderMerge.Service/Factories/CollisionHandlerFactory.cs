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
    public class CollisionHandlerFactory : ICollisionHandlerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IFileSystem _fileSystem;

        public CollisionHandlerFactory(IFileSystem fileSystem, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _fileSystem = fileSystem;
        }

        public ICollisionHandler CreateCollisionHandler(Target target)
        {
            return new CollisionHandler(target, _fileSystem, _loggerFactory.CreateLogger<CollisionHandler>());
        }
    }
}
