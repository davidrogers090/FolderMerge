using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderMerge.Service
{
    public class SourceWatcherStub : ISourceWatcher
    {
        public bool IsStarted { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            IsStarted = true;
        }
    }
}
