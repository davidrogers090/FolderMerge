using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderMerge.Data;
using FolderMerge.Service.Factories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FolderMerge.Service
{
    public class MergerManager : BackgroundService
    {
        private readonly List<IMerger> mergers = new();
        private readonly IDatabase _database;
        private readonly IMergerFactory _mergerFactory;
        private readonly ILogger<MergerManager> _logger;

        public MergerManager(IDatabase database, IMergerFactory mergerFactory, ILogger<MergerManager> logger)
        {
            _database = database;
            database.OnChange += database_OnChange;
            _mergerFactory = mergerFactory;
            _logger = logger;
        }

        private void database_OnChange(object? sender, EventArgs args)
        {
            Clear();
            Init();
        }

        public void Init()
        {
            foreach (var target in _database.FetchTargets())
            {
                mergers.Add(_mergerFactory.CreateMerger(target));
            }
        }

        public void Clear()
        {
            foreach (var merger in mergers)
            {
                merger.Dispose();
            }
            mergers.Clear();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                // Terminates this process and returns an exit code to the operating system.
                // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
                // performs one of two scenarios:
                // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
                // 2. When set to "StopHost": will cleanly stop the host, and log errors.
                //
                // In order for the Windows Service Management system to leverage configured
                // recovery options, we need to terminate the process with a non-zero exit code.
                Environment.Exit(1);
            }
        }
    }
}
