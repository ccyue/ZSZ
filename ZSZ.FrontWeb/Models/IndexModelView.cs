using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.FrontWeb.Models
{
    public class IndexModelView
    {
        public long CityId { get; set; }
        public CityDTO[] Cities { get; set; }
        public Dictionary<string,long> Types { get; set; }
    }
}