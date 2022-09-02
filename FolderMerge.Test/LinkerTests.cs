using FolderMerge.Data.Models;
using FolderMerge.Service;
using FolderMerge.Test.Stubs;
using Microsoft.Extensions.Logging.Abstractions;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace FolderMerge.Test
{
    [TestClass]
    public class LinkerTests
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
        public void Link_ShouldSucceedWhenFileDoesNotExist()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);
            fileSystem.AddDirectory(target.TargetPath);
            fileSystem.AddDirectory(source.Path);
            fileSystem.AddFile(sourcePath, new MockFileData("Test1"));

            ILinker linker = new Linker(source, new CollisionHandlerStub(true, true), fileSystem, new NullLogger<Linker>());
            linker.Link(sourcePath);

            string targetPath = fileSystem.Path.Combine(target.TargetPath, fileName);
            Assert.IsTrue(fileSystem.File.Exists(targetPath));
            Assert.IsTrue(fileSystem.FileInfo.FromFileName(targetPath).LinkTarget.Equals(sourcePath));
        }

        [TestMethod]
        public void Link_ShouldSucceedWhenHasPriority()
        {

            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);
            string sourcePath2 = fileSystem.Path.Combine(source2.Path, fileName);
            fileSystem.AddDirectory(target.TargetPath);
            fileSystem.AddDirectory(source.Path);
            fileSystem.AddDirectory(source2.Path);
            fileSystem.AddFile(sourcePath, new MockFileData("Test1"));
            fileSystem.AddFile(sourcePath2, new MockFileData("Test2"));

            ILinker linker = new Linker(source, new CollisionHandlerStub(true, false), fileSystem, new NullLogger<Linker>());
            linker.Link(sourcePath);
            linker.Link(sourcePath2);

            string targetPath = fileSystem.Path.Combine(target.TargetPath, fileName);
            Assert.IsTrue(fileSystem.File.Exists(targetPath));
            Assert.IsTrue(fileSystem.FileInfo.FromFileName(targetPath).LinkTarget.Equals(sourcePath2));
        }

        [TestMethod]
        public void Link_ShouldFailWhenNotHasPriority()
        {

            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);
            string sourcePath2 = fileSystem.Path.Combine(source2.Path, fileName);
            fileSystem.AddDirectory(target.TargetPath);
            fileSystem.AddDirectory(source.Path);
            fileSystem.AddDirectory(source2.Path);
            fileSystem.AddFile(sourcePath, new MockFileData("Test1"));
            fileSystem.AddFile(sourcePath2, new MockFileData("Test2"));

            ILinker linker = new Linker(source, new CollisionHandlerStub(false, false), fileSystem, new NullLogger<Linker>());
            linker.Link(sourcePath);
            linker.Link(sourcePath2);

            string targetPath = fileSystem.Path.Combine(target.TargetPath, fileName);
            Assert.IsTrue(fileSystem.File.Exists(targetPath));
            Assert.IsTrue(fileSystem.FileInfo.FromFileName(targetPath).LinkTarget.Equals(sourcePath));
        }

        [TestMethod]
        public void Delete_ShouldSucceedWhenOwns()
        {

            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);
            fileSystem.AddDirectory(target.TargetPath);
            fileSystem.AddDirectory(source.Path);
            fileSystem.AddDirectory(source2.Path);
            fileSystem.AddFile(sourcePath, new MockFileData("Test1"));

            ILinker linker = new Linker(source, new CollisionHandlerStub(false, true), fileSystem, new NullLogger<Linker>());
            linker.Link(sourcePath);
            linker.Delete(sourcePath);

            string targetPath = fileSystem.Path.Combine(target.TargetPath, fileName);
            Assert.IsFalse(fileSystem.File.Exists(targetPath));
        }

        [TestMethod]
        public void Delete_ShouldFailWhenNotOwns()
        {

            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);
            fileSystem.AddDirectory(target.TargetPath);
            fileSystem.AddDirectory(source.Path);
            fileSystem.AddDirectory(source2.Path);
            fileSystem.AddFile(sourcePath, new MockFileData("Test1"));

            ILinker linker = new Linker(source, new CollisionHandlerStub(false, false), fileSystem, new NullLogger<Linker>());
            linker.Link(sourcePath);
            linker.Delete(sourcePath);

            string targetPath = fileSystem.Path.Combine(target.TargetPath, fileName);
            Assert.IsTrue(fileSystem.File.Exists(targetPath));
        }
    }
}