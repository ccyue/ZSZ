using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    public class HouseAppointment:BaseDTO
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string PhoneNum { get; set; }
        public DateTime VisitDate { get; set; }
        public long HouseId { get; set; }
        public long StatusId { get; set; }
        public long FollowAdminUserId { get; set; }
        public DateTime FollowDatetime { get; set; }
        public int IsDeleted { get; set; }
    }
}
