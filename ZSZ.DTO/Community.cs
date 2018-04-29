using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    public class Community : BaseDTO
    {
        public string Name { get; set; }
        public long RegionId { get; set; }
        public int IsDeleted { get; set; }
        public string Traffic { get; set; }
        public int BuiltYear { get; set; }
    }
}
