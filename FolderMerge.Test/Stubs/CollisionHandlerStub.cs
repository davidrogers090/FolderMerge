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
    public class CollisionHandlerStub : ICollisionHandler
    {

        private readonly bool priority, owns;

        public CollisionHandlerStub(bool priority, bool owns)
        {
            this.priority = priority;
            this.owns = owns;
        }

        public bool HasPriority(Source source, string targetFile)
        {
            return priority;
        }

        public bool Owns(Source source, string targetFile)
        {
            return owns;
        }
    }
}
