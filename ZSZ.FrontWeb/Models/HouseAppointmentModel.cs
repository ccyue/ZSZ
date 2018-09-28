using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.FrontWeb.Models
{
    public class HouseAppointmentModel
    {
        [Required]
        public long HouseId { get; set; }
        [Required]
        public string Name { get; set; } 
        [Phone]
        [Required]
        public string PhoneNum { get; set; }
        public DateTime VisitDate { get; set; }
    }
}