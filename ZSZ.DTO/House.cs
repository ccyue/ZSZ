using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    public class House : BaseDTO
    {
        public long CommunityId { get; set; }
        public long RoomTypeId { get; set; }
        public string Address { get; set; }
        public int MonthRent { get; set; }
        public long StatusId { get; set; }
        public double Area { get; set; }
        public long DecorateStatusId{ get; set; }
        public int TotalFloorCount { get; set; }
        public int Floor { get; set; }
        public long TypeId { get; set; }
        public string Direction { get; set; }
        public DateTime LookableDateTime { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhoneNum { get; set; }
        public string Description { get; set; }
        public int IsDeleted { get; set; }
    }
}
