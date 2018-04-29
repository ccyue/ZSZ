using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    [Serializable]
    public abstract class BaseDTO
    {
        public long Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public long CreateBy { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public long ModifyBy { get; set; }
    }
}
