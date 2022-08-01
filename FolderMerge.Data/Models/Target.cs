using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace FolderMerge.Data.Models
{
    public class Target
    {
        public int TargetId { get; set; }
        public string TargetPath { get; set; }
        public List<Source> Sources { get; set; } = new();
    }
}
