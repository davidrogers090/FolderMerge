using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using FolderMerge.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FolderMerge.Service.Factories;

namespace FolderMerge.Service
{
    public class Program
    {

        private static readonly string db_file = @"database.db";

        public static void Main(string[] args)
        {
            using IHost host = CreateHost(args);
            FolderContext? context = host.Services.GetService<FolderContext>();
            if (context != null)
            {
                context.Database.EnsureCreated();
            }
            host.Run();
        }

        public static IHost CreateHost(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
            .UseWindowsService(options =>
            {
                options.ServiceName = "FolderMerge Service";
            })
            .ConfigureServices((_, services) =>
                services.AddHostedService<MergerManager>()
                    .AddDbContext<FolderContext>((options) => options.UseSqlite($"Data Source={db_file}"))
                    .AddSingleton<ICollisionHandlerFactory, CollisionHandlerFactory>()
                    .AddSingleton<ILinkerFactory, LinkerFactory>()
                    .AddSingleton<IMergerFactory, MergerFactory>()
                    .AddSingleton<ISourceWatcherFactory, SourceWatcherFactory>()
                    .AddSingleton<IDatabase, Database>()
                    .AddLogging()
                    )
            .Build();
        }

    }
}