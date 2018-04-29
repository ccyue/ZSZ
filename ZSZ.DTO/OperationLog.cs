using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    public class OperationLog
    {
        public DateTime CreateDateTime { get; set; }
        public long UserId { get; set; }
        public string Message { get; set; }
    }
}
