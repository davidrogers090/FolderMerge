using FolderMerge.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderMerge.Data
{
    public interface IDatabase : IDisposable
    {
        public event EventHandler? OnChange;
        public List<Target> FetchTargets();
        public void StartMonitoring();

    }
}
