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
    public class LinkerFactory : ILinkerFactory
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILoggerFactory _loggerFactory;

        public LinkerFactory(IFileSystem fileSystem, ILoggerFactory loggerFactory)
        {
            _fileSystem = fileSystem;
            _loggerFactory = loggerFactory;
        }
        public ILinker CreateLinker(Source source, ICollisionHandler collisionHandler)
        {
            return new Linker(source, collisionHandler, _fileSystem, _loggerFactory.CreateLogger<Linker>());
        }
    }
}
