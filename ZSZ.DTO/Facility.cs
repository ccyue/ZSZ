using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    public class Facility:BaseDTO
    {
        public string Name { get; set; }
        public string IconName { get; set; }
        public int IsDeleted { get; set; }
    }
}
