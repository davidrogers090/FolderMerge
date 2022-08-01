using FolderMerge.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace FolderMerge.Data
{
    public sealed class FolderContext : DbContext
    {
        public DbSet<Target> Targets { get; set; }

        private static readonly string db_file = @"database.db";

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={db_file}");
    }
}
