using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class AdminUserAddModel
    {
        [Required]
        [StringLength(50)]
        public String Name { get; set; }
        [Required]
        [Phone]
        public String PhoneNum { get; set; }
        [Required]
        [StringLength(30)]
        [EmailAddress]
        public String Email { get; set; }
        public long? CityId { get; set; }
        [Required]
        [StringLength(30)]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string Password2 { get; set; }
        public long[] RoleIds { get; set; }
    }
}