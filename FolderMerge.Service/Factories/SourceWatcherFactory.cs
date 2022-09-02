﻿using FolderMerge.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderMerge.Service.Factories
{
    public class SourceWatcherFactory : ISourceWatcherFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public SourceWatcherFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public ISourceWatcher CreateSourceWatcher(string sourcePath, ILinker linker)
        {
            return new SourceWatcher(linker, new FileSystemWatcherWrapper(sourcePath), _loggerFactory.CreateLogger<SourceWatcher>());
        }
    }
}
