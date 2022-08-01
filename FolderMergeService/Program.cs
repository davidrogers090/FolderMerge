using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using FolderMerge.Data;

namespace FolderMerge.Service
{
    public class Program
    {
        

        public static void Main(string[] args)
        {
            using FolderContext context = new();
            context.Database.EnsureCreated();
            var connection = context.Database.GetDbConnection();
            var database = new Database(connection);
            var manager = new MergerManager(database);

            while (true) ;
        }

    }
}