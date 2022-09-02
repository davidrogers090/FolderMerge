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
    public class CollisionHandler : ICollisionHandler
    {
        private readonly Dictionary<string, Source> _sources;
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<CollisionHandler> _logger;

        public CollisionHandler(Target target, IFileSystem fileSystem, ILogger<CollisionHandler> logger)
        {
            _sources = new();
            foreach (var source in target.Sources)
            {
                _sources.Add(source.Path, source);
            }
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public bool HasPriority(Source source, string targetFileName)
        {
            var targetFile = _fileSystem.FileInfo.FromFileName(targetFileName);
            if (targetFile == null || !targetFile.Exists) return true; // Has priority if the target doesn't exist

            if (targetFile.LinkTarget == null) return false; // originals always have priority

            var sourcePath = _fileSystem.Path.GetDirectoryName(targetFile.LinkTarget);

            Source existingSource = _sources[sourcePath];
            bool hasPriority = source.Priority < existingSource.Priority;

            _logger.LogDebug("Priority: {thisSource} > {existingSource} = {hasPriority}", source.Path, existingSource.Path, hasPriority);

            return hasPriority;
        }

        public bool Owns(Source source, string targetFileName)
        {
            var targetFile = _fileSystem.FileInfo.FromFileName(targetFileName);
            if (targetFile == null || !targetFile.Exists) return false;

            if (targetFile.LinkTarget == null) return false; // Can't own an original

            var sourcePath = _fileSystem.Path.GetDirectoryName(targetFile.LinkTarget);

            var left = _fileSystem.Path.TrimEndingDirectorySeparator(source.Path);
            var right = _fileSystem.Path.TrimEndingDirectorySeparator(sourcePath);

            var owns = left.Equals(right);

            _logger.LogDebug("Source {sourcePath} owns {targetFile}: {owns}", left, targetFile, owns);

            return owns;
        }
    }
}
