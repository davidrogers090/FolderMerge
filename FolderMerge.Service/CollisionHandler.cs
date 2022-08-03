using System;
using System.Collections.Generic;
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
        private readonly ILogger<CollisionHandler> _logger;

        public CollisionHandler(Target target, ILogger<CollisionHandler> logger)
        {
            _sources = new();
            foreach (var source in target.Sources)
            {
                _sources.Add(source.Path, source);
            }
            _logger = logger;
        }

        public bool HasPriority(Source source, string targetFile)
        {

            var sourceFile = File.ResolveLinkTarget(targetFile, false);
            if (sourceFile == null) return true;

            var sourcePath = Path.GetDirectoryName(sourceFile.FullName);
            if (sourcePath == null) return true;

            Source existingSource = _sources[sourcePath];
            bool hasPriority = source.Priority > existingSource.Priority;

            _logger.LogDebug("Priority: {thisSource} > {existingSource} = {hasPriority}", source.Path, existingSource.Path, hasPriority);

            return hasPriority;
        }

        public bool Owns(Source source, string targetFile)
        {
            var sourceFile = File.ResolveLinkTarget(targetFile, false);
            if (sourceFile == null) return false;

            var sourcePath = Path.GetDirectoryName(sourceFile.FullName);
            if (sourcePath == null) return true;

            var left = Path.TrimEndingDirectorySeparator(source.Path);
            var right = Path.TrimEndingDirectorySeparator(sourcePath);

            var owns = left.Equals(right);

            _logger.LogDebug("Source {sourcePath} owns {targetFile}: {owns}", left, targetFile, owns);

            return owns;
        }
    }
}
