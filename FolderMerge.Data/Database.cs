using FolderMerge.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace FolderMerge.Data
{
    public sealed class Database : IDisposable
    {
        public event EventHandler? OnChange;

        private readonly Timer _timer;
        private readonly DbConnection _connection;

        private int lastDataVersion = 0;

        public Database(DbConnection connection)
        {
            _connection = connection;
            _connection.Open();
            _timer = new Timer(CheckChanges, null, Timeout.Infinite, Timeout.Infinite);
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
                int dataVersion = ((int?)command.ExecuteScalar()).GetValueOrDefault(0);
                if (dataVersion > lastDataVersion)
                {
                    lastDataVersion = dataVersion;
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
