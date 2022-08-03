using FolderMerge.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderMerge.Service.Factories
{
    public interface ILinkerFactory
    {
        public ILinker CreateLinker(Source source, ICollisionHandler collisionHandler);
    }
}
