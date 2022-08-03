using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderMerge.Data.Models;

namespace FolderMerge.Service
{
    public interface IMerger : IDisposable
    {

        public void Init();

        public void Start();
    }
}
