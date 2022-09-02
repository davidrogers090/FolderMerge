using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderMerge.Data.Models;
using Microsoft.Extensions.Logging;

namespace FolderMerge.Service
{
    public class Linker : ILinker
    {
        private readonly Source _source;
        private readonly ICollisionHandler _collisionHandler;
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<Linker> _logger;

        public Linker(Source source, ICollisionHandler collisionHandler, IFileSystem fileSystem, ILogger<Linker> logger)
        {
            _source = source;
            _collisionHandler = collisionHandler;
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public void Delete(string sourcePath)
        {
            var targetPath = GetTarget(sourcePath);
            if (_collisionHandler.Owns(_source, targetPath))
            {
                _logger.LogTrace("Deleting {target}", targetPath);
                _fileSystem.File.Delete(targetPath);
            }
        }

        public void Link(string sourcePath)
        {
            var targetPath = GetTarget(sourcePath);
            if (!_fileSystem.File.Exists(targetPath))
            {
                _logger.LogTrace("Linking {source} to {target}", sourcePath, targetPath);
                _fileSystem.File.CreateSymbolicLink(targetPath, sourcePath);
            }
            else if (_collisionHandler.HasPriority(_source, targetPath))
            {
                _logger.LogTrace("Overwriting link from {source} to {target}", sourcePath, targetPath);
                _fileSystem.File.Delete(targetPath);
                _fileSystem.File.CreateSymbolicLink(targetPath, sourcePath);
            }
            else
            {
                _logger.LogTrace("Not Linking {source} to {target}: Has lower priority", sourcePath, targetPath);
            }
        }

        private string GetTarget(string sourceFile)
        {
            var filename = _fileSystem.Path.GetFileName(sourceFile);
            var targetFilename = _fileSystem.Path.Combine(_source.Target.TargetPath, filename);
            return targetFilename;
        }
    }
}
