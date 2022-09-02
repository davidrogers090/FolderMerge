using FolderMerge.Data.Models;
using FolderMerge.Service;
using FolderMerge.Service.Factories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderMerge.Test.Stubs
{
    public class MergerFactoryStub : IMergerFactory
    {
        public List<Target> Targets { get; } = new();
        public List<MergerStub> MergerStubs { get; } = new();

        public IMerger CreateMerger(Target target)
        {
            Targets.Add(target);
            var mergerStub = new MergerStub();
            return mergerStub;
        }
    }
}
