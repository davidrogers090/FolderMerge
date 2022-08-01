using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderMerge.Data;

namespace FolderMerge.Service
{
    public class MergerManager
    {
        private readonly List<Merger> mergers = new();
        private readonly Database database;

        public MergerManager(Database database)
        {
            this.database = database;
            database.OnChange += database_OnChange;
        }

        private void database_OnChange(object? sender, EventArgs args)
        {
            Clear();
            Init();
        }

        public void Init()
        {
            foreach (var target in database.FetchTargets())
            {
                mergers.Add(new Merger(target));
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
    }
}
