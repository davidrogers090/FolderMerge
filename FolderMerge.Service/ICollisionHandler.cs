using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderMerge.Data.Models;

namespace FolderMerge.Service
{
    public interface ICollisionHandler
    {

        public bool HasPriority(Source source, string targetFile);

        public bool Owns(Source source, string targetFile);
    }
}
