using FolderMerge.Data.Models;
using FolderMerge.Service;
using FolderMerge.Test.Stubs;
using Microsoft.Extensions.Logging.Abstractions;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace FolderMerge.Test
{
    [TestClass]
    public class SourceWatcherTests
    {
        private Target target = new();
        private Source source = new();
        private Source source2 = new();

        [TestInitialize]
        public void Init()
        {
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
        }

        [TestMethod]
        public void File_Created()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);

            var linked = new HashSet<string>();
            var deleted = new HashSet<string>();
            var linker = new LinkerStub(linked, deleted);
            var watcher = new FileSystemWatcherStub(source.Path);
            var sourceWatcher = new SourceWatcher(linker, watcher, new NullLogger<SourceWatcher>());

            watcher.TriggerCreated(fileName);

            Assert.IsTrue(linked.Contains(sourcePath));
        }

        [TestMethod]
        public void File_Renamed()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            string oldFileName = @"file.txt";
            string oldFullPath = fileSystem.Path.Combine(source.Path, oldFileName);
            string fileName = @"file2.txt";
            string fullPath = fileSystem.Path.Combine(source.Path, fileName);

            var linked = new HashSet<string>();
            var deleted = new HashSet<string>();
            var linker = new LinkerStub(linked, deleted);
            var watcher = new FileSystemWatcherStub(source.Path);
            var sourceWatcher = new SourceWatcher(linker, watcher, new NullLogger<SourceWatcher>());

            watcher.TriggerRenamed(oldFileName, fileName);

            Assert.IsTrue(linked.Contains(fullPath));
            Assert.IsTrue(deleted.Contains(oldFullPath));
        }

        [TestMethod]
        public void File_Deleted()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);

            var linked = new HashSet<string>();
            var deleted = new HashSet<string>();
            var linker = new LinkerStub(linked, deleted);
            var watcher = new FileSystemWatcherStub(source.Path);
            var sourceWatcher = new SourceWatcher(linker, watcher, new NullLogger<SourceWatcher>());

            watcher.TriggerDeleted(fileName);

            Assert.IsTrue(deleted.Contains(sourcePath));
        }

        [TestMethod]
        public void Dispose()
        {
            var watcher = new FileSystemWatcherStub(source.Path);
            var sourceWatcher = new SourceWatcher(null!, watcher, new NullLogger<SourceWatcher>());

            sourceWatcher.Dispose();

            Assert.IsTrue(watcher.IsDisposed);
        }

        [TestMethod]
        public void Start()
        {
            var watcher = new FileSystemWatcherStub(source.Path);
            var sourceWatcher = new SourceWatcher(null!, watcher, new NullLogger<SourceWatcher>());

            sourceWatcher.Start();

            Assert.IsTrue(watcher.EnableRaisingEvents);
        }
    }
}