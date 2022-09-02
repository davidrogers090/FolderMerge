using FolderMerge.Data.Models;
using FolderMerge.Service;
using FolderMerge.Test.Stubs;
using Microsoft.Extensions.Logging.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace FolderMerge.Test
{
    [TestClass]
    public class MergerTests
    {
        private Target? target;
        private Source? source;
        private Source? source2;
        private MockFileSystem? mockFileSystem;
        private string? file1;
        private string? file2;
        private string? file3;
        private string? sfile1;
        private string? sfile2;
        private string? sfile3;

        private readonly CollisionHandlerFactoryStub collisionHandlerFactory = new();
        private readonly NullLogger<Merger> logger = new();

        [TestInitialize]
        public void Init()
        {
            target = new();
            target.TargetPath = @"C:\Test\Target\Path";
            target.TargetId = 0;

            source = new();
            source.Path = @"C:\Test\Source\Path";
            source.Priority = 1;
            source.SourceId = 0;
            source.Target = target;
            source.TargetId = target.TargetId;

            source2 = new();
            source2.Path = @"C:\Test\Source2\Path";
            source2.Priority = 2;
            source2.SourceId = 1;
            source2.Target = target;
            source2.TargetId = target.TargetId;

            target.Sources.Add(source);
            target.Sources.Add(source2);

            mockFileSystem = new();
            file1 = mockFileSystem.Path.Combine(source.Path, @"file1.txt");
            file2 = mockFileSystem.Path.Combine(source.Path, @"file2.txt");
            file3 = mockFileSystem.Path.Combine(source.Path, @"file3.txt");

            mockFileSystem.AddDirectory(source.Path);
            mockFileSystem.AddFile(file1, new MockFileData("Test1"));
            mockFileSystem.AddFile(file2, new MockFileData("Test2"));
            mockFileSystem.AddFile(file3, new MockFileData("Test3"));

            sfile1 = mockFileSystem.Path.Combine(source2.Path, @"sfile1.txt");
            sfile2 = mockFileSystem.Path.Combine(source2.Path, @"sfile2.txt");
            sfile3 = mockFileSystem.Path.Combine(source2.Path, @"sfile3.txt");

            mockFileSystem.AddDirectory(source2.Path);
            mockFileSystem.AddFile(sfile1, new MockFileData("sTest1"));
            mockFileSystem.AddFile(sfile2, new MockFileData("sTest2"));
            mockFileSystem.AddFile(sfile3, new MockFileData("sTest3"));
        }

        [TestMethod]
        public void Init_ShouldLinkAllAndCreateSourceWatcher()
        {
            SourceWatcherFactoryStub sourceWatcherFactory = new();
            LinkerFactoryStub linkerFactory = new();

            IMerger merger = new Merger(target!, sourceWatcherFactory, linkerFactory, collisionHandlerFactory, mockFileSystem!, logger);
            merger.Init();

            Assert.IsTrue(linkerFactory.Linked.Contains(file1!));
            Assert.IsTrue(linkerFactory.Linked.Contains(file2!));
            Assert.IsTrue(linkerFactory.Linked.Contains(file3!));
            Assert.IsTrue(sourceWatcherFactory.SourcePaths.Contains(source!.Path));

            Assert.IsTrue(linkerFactory.Linked.Contains(sfile1!));
            Assert.IsTrue(linkerFactory.Linked.Contains(sfile2!));
            Assert.IsTrue(linkerFactory.Linked.Contains(sfile3!));
            Assert.IsTrue(sourceWatcherFactory.SourcePaths.Contains(source2!.Path));
        }

        [TestMethod]
        public void Start_ShouldStartAllWatchers()
        {
            SourceWatcherFactoryStub sourceWatcherFactory = new();
            LinkerFactoryStub linkerFactory = new();

            IMerger merger = new Merger(target!, sourceWatcherFactory, linkerFactory, collisionHandlerFactory, mockFileSystem!, logger);
            merger.Init();
            merger.Start();


            Assert.AreEqual(target!.Sources.Count, sourceWatcherFactory.SourceWatchers.Count);

            foreach (var sourceWatcher in sourceWatcherFactory.SourceWatchers)
            {
                Assert.IsTrue(sourceWatcher.IsStarted);
            }
        }
    }
}