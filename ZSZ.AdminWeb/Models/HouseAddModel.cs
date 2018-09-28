using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZSZ.AdminWeb.Models
{
    public class HouseAddModel
    {
        public string OwnerPhoneNum { get; set; }
        public string OwnerName { get; set; }
        public DateTime CheckInDateTime { get; set; }
        public DateTime LookableDateTime { get; set; }
        public string Direction { get; set; }
        public long TypeId { get; set; }
        public int FloorIndex { get; set; }
        public int TotalFloorCount { get; set; }
        public long DecorateStatusId { get; set; }
        public decimal Area { get; set; }
        public long StatusId { get; set; }
        public int MonthRent { get; set; }
        public string Address { get; set; }
        public long RoomTypeId { get; set; }
        public long CommunityId { get; set; }
        public string Description { get; set; }
        public long[] AttachmentIds { get; set; }
    }
}