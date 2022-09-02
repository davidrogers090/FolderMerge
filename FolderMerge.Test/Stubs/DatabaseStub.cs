using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderMerge.Data;
using FolderMerge.Data.Models;
using FolderMerge.Service.Factories;
using Microsoft.Extensions.Logging;

namespace FolderMerge.Service
{
    public sealed class DatabaseStub : IDatabase
    {
        private List<Target> targets = new();

        public DatabaseStub()
        {

            Target target = new();
            Source source = new();
            Source source2 = new();

            target.TargetPath = @"C:\Test\Target\Path";
            target.TargetId = 0;
            target.Sources.Add(source);
            target.Sources.Add(source2);

            source.Path = @"C:\Test\Source\Path";
            source.Priority = 1;
            source.SourceId = 0;
            source.Target = target;
            source.TargetId = target.TargetId;

            source2.Path = @"C:\Test\Source2\Path";
            source2.Priority = 2;
            source2.SourceId = 1;
            source2.Target = target;
            source2.TargetId = target.TargetId;

            targets.Add(target);

        }

        public event EventHandler? OnChange;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<Target> FetchTargets()
        {
            return targets;
        }

        public void StartMonitoring()
        {
            
        }

        public void Add(Target target)
        {
            targets.Add(target);
            OnChange?.Invoke(this, EventArgs.Empty);
        }

        public void Remove(int index)
        {
            targets.RemoveAt(index);
            OnChange?.Invoke(this, EventArgs.Empty);
        }

        public void Change(int index, Target target)
        {
            targets[index] = target;
            OnChange?.Invoke(this, EventArgs.Empty);
        }
    }
}
