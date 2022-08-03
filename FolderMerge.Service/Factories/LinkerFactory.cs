using FolderMerge.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderMerge.Service.Factories
{
    public class LinkerFactory : ILinkerFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public LinkerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }
        public ILinker CreateLinker(Source source, ICollisionHandler collisionHandler)
        {
            return new Linker(source, collisionHandler, _loggerFactory.CreateLogger<Linker>());
        }
    }
}
