using FolderMerge.Data.Models;
using FolderMerge.Service;
using FolderMerge.Test.Stubs;
using Microsoft.Extensions.Logging.Abstractions;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace FolderMerge.Test
{
    [TestClass]
    public class CollisionHandlerTests
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
        public void HasPriority_ShouldReturnTrueWhenTargetFileNotExists()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);
            fileSystem.AddDirectory(target.TargetPath);
            fileSystem.AddDirectory(source.Path);
            fileSystem.AddFile(sourcePath, new MockFileData("Test1"));

            ICollisionHandler collisionHandler = new CollisionHandler(target, fileSystem, new NullLogger<CollisionHandler>());

            bool value = collisionHandler.HasPriority(source, @"C:\not\real.txt");
            Assert.IsTrue(value);
        }

        [TestMethod]
        public void HasPriority_ShouldReturnFalseIfNotLink()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);
            fileSystem.AddDirectory(target.TargetPath);
            fileSystem.AddDirectory(source.Path);
            fileSystem.AddFile(sourcePath, new MockFileData("Test1"));

            ICollisionHandler collisionHandler = new CollisionHandler(target, fileSystem, new NullLogger<CollisionHandler>());

            bool value = collisionHandler.HasPriority(source, sourcePath);
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void HasPriority_ShouldReturnFalseIfLowerPriority()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);
            string targetFile = fileSystem.Path.Combine(target.TargetPath, fileName);
            fileSystem.AddDirectory(target.TargetPath);
            fileSystem.AddDirectory(source.Path);
            fileSystem.AddDirectory(source2.Path);
            fileSystem.AddFile(sourcePath, new MockFileData("Test1"));
            fileSystem.File.CreateSymbolicLink(targetFile, sourcePath);

            ICollisionHandler collisionHandler = new CollisionHandler(target, fileSystem, new NullLogger<CollisionHandler>());

            bool value = collisionHandler.HasPriority(source2, targetFile);
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void HasPriority_ShouldReturnFalseIfEqualPriority()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);
            string targetFile = fileSystem.Path.Combine(target.TargetPath, fileName);
            fileSystem.AddDirectory(target.TargetPath);
            fileSystem.AddDirectory(source.Path);
            fileSystem.AddDirectory(source2.Path);
            fileSystem.AddFile(sourcePath, new MockFileData("Test1"));
            fileSystem.File.CreateSymbolicLink(targetFile, sourcePath);

            ICollisionHandler collisionHandler = new CollisionHandler(target, fileSystem, new NullLogger<CollisionHandler>());

            bool value = collisionHandler.HasPriority(source, targetFile);
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void HasPriority_ShouldReturnTrueIfHigherPriority()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source2.Path, fileName);
            string targetFile = fileSystem.Path.Combine(target.TargetPath, fileName);
            fileSystem.AddDirectory(target.TargetPath);
            fileSystem.AddDirectory(source.Path);
            fileSystem.AddDirectory(source2.Path);
            fileSystem.AddFile(sourcePath, new MockFileData("Test1"));
            fileSystem.File.CreateSymbolicLink(targetFile, sourcePath);

            ICollisionHandler collisionHandler = new CollisionHandler(target, fileSystem, new NullLogger<CollisionHandler>());

            bool value = collisionHandler.HasPriority(source, targetFile);
            Assert.IsTrue(value);
        }

        [TestMethod]
        public void Owns_ReturnsFalseIfFileNotExist()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);
            string targetFile = fileSystem.Path.Combine(target.TargetPath, fileName);
            fileSystem.AddDirectory(target.TargetPath);
            fileSystem.AddDirectory(source.Path);
            fileSystem.AddDirectory(source2.Path);
            fileSystem.AddFile(sourcePath, new MockFileData("Test1"));
            fileSystem.File.CreateSymbolicLink(targetFile, sourcePath);

            ICollisionHandler collisionHandler = new CollisionHandler(target, fileSystem, new NullLogger<CollisionHandler>());

            bool value = collisionHandler.Owns(source, @"C:\Not\an\existing\path.txt");
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void Owns_ReturnsFalseIfFileNotLink()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);
            string targetFile = fileSystem.Path.Combine(target.TargetPath, fileName);
            fileSystem.AddDirectory(target.TargetPath);
            fileSystem.AddDirectory(source.Path);
            fileSystem.AddDirectory(source2.Path);
            fileSystem.AddFile(sourcePath, new MockFileData("Test1"));
            fileSystem.File.CreateSymbolicLink(targetFile, sourcePath);

            ICollisionHandler collisionHandler = new CollisionHandler(target, fileSystem, new NullLogger<CollisionHandler>());

            bool value = collisionHandler.Owns(source, sourcePath);
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void Owns_ReturnsFalseIfFromDifferentSource()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);
            string targetFile = fileSystem.Path.Combine(target.TargetPath, fileName);
            fileSystem.AddDirectory(target.TargetPath);
            fileSystem.AddDirectory(source.Path);
            fileSystem.AddDirectory(source2.Path);
            fileSystem.AddFile(sourcePath, new MockFileData("Test1"));
            fileSystem.File.CreateSymbolicLink(targetFile, sourcePath);

            ICollisionHandler collisionHandler = new CollisionHandler(target, fileSystem, new NullLogger<CollisionHandler>());

            bool value = collisionHandler.Owns(source2, targetFile);
            Assert.IsFalse(value);
        }

        [TestMethod]
        public void Owns_ReturnsTrueIfFromThisSource()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            string fileName = @"file.txt";
            string sourcePath = fileSystem.Path.Combine(source.Path, fileName);
            string targetFile = fileSystem.Path.Combine(target.TargetPath, fileName);
            fileSystem.AddDirectory(target.TargetPath);
            fileSystem.AddDirectory(source.Path);
            fileSystem.AddDirectory(source2.Path);
            fileSystem.AddFile(sourcePath, new MockFileData("Test1"));
            fileSystem.File.CreateSymbolicLink(targetFile, sourcePath);

            ICollisionHandler collisionHandler = new CollisionHandler(target, fileSystem, new NullLogger<CollisionHandler>());

            bool value = collisionHandler.Owns(source, targetFile);
            Assert.IsTrue(value);
        }
    }
}