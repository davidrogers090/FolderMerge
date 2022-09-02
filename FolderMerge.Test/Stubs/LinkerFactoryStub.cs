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
    public class LinkerFactoryStub : ILinkerFactory
    {
        public HashSet<string> Linked { get; } = new();
        public HashSet<string> Deleted { get; } = new();


        public ILinker CreateLinker(Source source, ICollisionHandler collisionHandler)
        {
            return new LinkerStub(Linked, Deleted);
        }
    }
}
