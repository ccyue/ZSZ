using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb.Models
{
    public class AdminUserEditModel
    {
        [Required]
        public long Id { get; set; }
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
        [StringLength(30)]
        public string Password { get; set; }
        [StringLength(30)]
        [Compare(nameof(Password))]
        public string Password2 { get; set; }
        public long[] RoleIds { get; set; } = new long[] { };
    }
}