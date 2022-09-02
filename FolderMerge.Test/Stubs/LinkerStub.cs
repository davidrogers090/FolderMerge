using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderMerge.Data.Models;
using FolderMerge.Service;
using Microsoft.Extensions.Logging;

namespace FolderMerge.Test.Stubs
{
    public class LinkerStub : ILinker
    {
        private readonly HashSet<string> linked;
        private readonly HashSet<string> deleted;

        public LinkerStub(HashSet<string> linked, HashSet<string> deleted)
        {
            this.linked = linked;
            this.deleted = deleted;
        }

        public void Delete(string sourcePath)
        {
            deleted.Add(sourcePath);
        }

        public void Link(string sourcePath)
        {
            linked.Add(sourcePath);
        }
    }
}
