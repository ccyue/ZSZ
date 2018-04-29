using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    public class SysDictionary : BaseDTO
    {
        public long ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
    }
}
