using FolderMerge.Data.Models;
using FolderMerge.Service;
using FolderMerge.Test.Stubs;
using Microsoft.Extensions.Logging.Abstractions;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace FolderMerge.Test
{
    [TestClass]
    public class MergerManagerTests
    {

        [TestMethod]
        public void Init()
        {
            var databaseStub = new DatabaseStub();
            var mergerFactoryStub = new MergerFactoryStub();

            var mergerManager = new MergerManager(databaseStub, mergerFactoryStub, new NullLogger<MergerManager>());
            mergerManager.Init();

            foreach (Target target in databaseStub.FetchTargets())
            {
                Assert.IsTrue(mergerFactoryStub.Targets.Contains(target));
            }
        }

        [TestMethod]
        public void Clear()
        {
            var databaseStub = new DatabaseStub();
            var mergerFactoryStub = new MergerFactoryStub();

            var mergerManager = new MergerManager(databaseStub, mergerFactoryStub, new NullLogger<MergerManager>());
            mergerManager.Clear();

            foreach (MergerStub mergerStub in mergerFactoryStub.MergerStubs)
            {
                Assert.IsTrue(mergerStub.IsDisposed);
            }
        }
    }
}