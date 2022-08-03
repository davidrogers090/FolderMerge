using FolderMerge.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data;
using Microsoft.Extensions.Logging;

namespace FolderMerge.Data
{
    public sealed class Database : IDatabase
    {
        public event EventHandler? OnChange;

        private readonly Timer _timer;
        private readonly IDbConnection _connection;
        private readonly ILogger<Database> _database;

        private int lastDataVersion = 0;

        public Database(FolderContext context, ILogger<Database> database)
        {
            _connection = context.Database.GetDbConnection();
            _connection.Open();
            _timer = new Timer(CheckChanges, null, Timeout.Infinite, Timeout.Infinite);
            _database = database;
        }

        public void StartMonitoring()
        {
            _timer.Change(5000, 5000);
        }

        private void CheckChanges(object? state)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "PRAGMA data_version";
                int? dataVersion = (int?)command.ExecuteScalar();
                if (dataVersion.HasValue && dataVersion.Value > lastDataVersion)
                {
                    lastDataVersion = dataVersion.Value;
                    OnChange?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Retrieves the latest <see cref="Target"/>s from the database.
        /// </summary>
        /// <returns>A list of Targets.</returns>
        public List<Target> FetchTargets()
        {
            using (FolderContext context = new FolderContext())
            {
                return context.Targets.ToList();
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
            _connection.Dispose();
        }
    }
}
