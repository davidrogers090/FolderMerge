using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderMerge.Data.Models;

namespace FolderMerge.Service
{
    public class Linker
    {
        private readonly Source source;
        private CollisionHandler collisionHandler;

        public Linker(Source source, CollisionHandler collisionHandler)
        {
            this.source = source;
            this.collisionHandler = collisionHandler;
        }

        public void Delete(string sourcePath)
        {
            var targetFilename = GetTarget(sourcePath);
            if (collisionHandler.Owns(source, targetFilename))
            {
                File.Delete(targetFilename);
            }
        }

        public void Link(string sourcePath)
        {
            var targetPath = GetTarget(sourcePath);
            if (!File.Exists(targetPath))
            {
                File.CreateSymbolicLink(sourcePath, targetPath);
            }
            else if (collisionHandler.HasPriority(source, targetPath))
            {
                File.Delete(targetPath);
                File.CreateSymbolicLink(sourcePath, targetPath);
            }
        }

        private string GetTarget(string sourceFile)
        {
            var filename = Path.GetFileName(sourceFile);
            var targetFilename = Path.Combine(source.Target.TargetPath, filename);
            return targetFilename;
        }
    }
}
