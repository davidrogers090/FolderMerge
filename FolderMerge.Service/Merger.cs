using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderMerge.Data.Models;

namespace FolderMerge.Service
{
    public sealed class Merger : IDisposable
    {
        private readonly Target target;
        private readonly List<SourceWatcher> watchers = new();

        public Merger(Target target)
        {
            this.target = target;
        }

        public void Dispose()
        {
            foreach (var watcher in watchers)
            {
                watcher.Dispose();
            }
        }

        public void Init()
        {
            CollisionHandler collisionHandler = new(target);
            foreach (var source in target.Sources)
            {
                string[] files = Directory.GetFiles(source.Path);
                Linker linker = new Linker(source, collisionHandler);
                foreach (var file in files)
                {
                    linker.Link(file);
                }

                watchers.Add(new SourceWatcher(source.Path, linker));
            }
        }

        public void Start()
        {
            foreach (var watcher in watchers)
            {
                watcher.Start();
            }
        }
    }
}
