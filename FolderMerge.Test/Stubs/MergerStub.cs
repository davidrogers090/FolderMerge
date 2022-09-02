using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderMerge.Data.Models;
using FolderMerge.Service.Factories;
using Microsoft.Extensions.Logging;

namespace FolderMerge.Service
{
    public sealed class MergerStub : IMerger
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}
