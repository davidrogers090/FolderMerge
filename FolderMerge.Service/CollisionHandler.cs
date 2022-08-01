using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderMerge.Data.Models;

namespace FolderMerge.Service
{
    public class CollisionHandler
    {
        private readonly Dictionary<string, Source> _sources;

        public CollisionHandler(Target target)
        {
            _sources = new();
            foreach (var source in target.Sources)
            {
                _sources.Add(source.Path, source);
            }
        }

        public bool HasPriority(Source source, string targetFile)
        {

            var sourceFile = File.ResolveLinkTarget(targetFile, false);
            if (sourceFile == null) return true;

            var sourcePath = Path.GetDirectoryName(sourceFile.FullName);
            if (sourcePath == null) return true;

            Source existingSource = _sources[sourcePath];

            return source.Priority > existingSource.Priority;
        }

        public bool Owns(Source source, string targetFile)
        {
            var sourceFile = File.ResolveLinkTarget(targetFile, false);
            if (sourceFile == null) return false;

            var sourcePath = Path.GetDirectoryName(sourceFile.FullName);
            if (sourcePath == null) return true;

            var left = Path.TrimEndingDirectorySeparator(source.Path);
            var right = Path.TrimEndingDirectorySeparator(sourcePath);

            return left.Equals(right);
        }
    }
}
