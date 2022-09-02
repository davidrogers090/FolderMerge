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
        public IMerger CreateMerger(Target target)
        {
            return new MergerStub();
        }
    }
}
