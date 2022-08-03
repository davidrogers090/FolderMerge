using System;
using System.Collections.Generic;
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
        private readonly ILogger<Linker> _logger;

        public Linker(Source source, ICollisionHandler collisionHandler, ILogger<Linker> logger)
        {
            _source = source;
            _collisionHandler = collisionHandler;
            _logger = logger;
        }

        public void Delete(string sourcePath)
        {
            var targetPath = GetTarget(sourcePath);
            if (_collisionHandler.Owns(_source, targetPath))
            {
                _logger.LogTrace("Deleting {target}", targetPath);
                File.Delete(targetPath);
            }
        }

        public void Link(string sourcePath)
        {
            var targetPath = GetTarget(sourcePath);
            if (!File.Exists(targetPath))
            {
                _logger.LogTrace("Linking {source} to {target}", sourcePath, targetPath);
                File.CreateSymbolicLink(sourcePath, targetPath);
            }
            else if (_collisionHandler.HasPriority(_source, targetPath))
            {
                _logger.LogTrace("Overwriting link from {source} to {target}", sourcePath, targetPath);
                File.Delete(targetPath);
                File.CreateSymbolicLink(sourcePath, targetPath);
            }
            else
            {
                _logger.LogTrace("Not Linking {source} to {target}: Has lower priority", sourcePath, targetPath);
            }
        }

        private string GetTarget(string sourceFile)
        {
            var filename = Path.GetFileName(sourceFile);
            var targetFilename = Path.Combine(_source.Target.TargetPath, filename);
            return targetFilename;
        }
    }
}
