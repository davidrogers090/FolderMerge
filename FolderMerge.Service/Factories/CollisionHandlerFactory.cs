using FolderMerge.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderMerge.Service.Factories
{
    public class CollisionHandlerFactory : ICollisionHandlerFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public CollisionHandlerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public ICollisionHandler CreateCollisionHandler(Target target)
        {
            return new CollisionHandler(target, _loggerFactory.CreateLogger<CollisionHandler>());
        }
    }
}
