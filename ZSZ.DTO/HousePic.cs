using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    public class HousePic:BaseDTO
    {
        public long HouseId { get; set; }
        public string Url { get; set; }
        public string ThumbUrl { get; set; }
        public int IsDeleted { get; set; }
    }
}
