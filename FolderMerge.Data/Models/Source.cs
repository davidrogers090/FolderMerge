using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace FolderMerge.Data.Models
{
    public class Source
    {
        public int SourceId { get; set; }
        public string Path { get; set; }
        public int Priority { get; set; }

        public int TargetId { get; set; }
        public Target Target { get; set; }
    }
}
