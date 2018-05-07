using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.FrontWeb.Models
{
    public class UserLoginModel
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [Phone]
        public string PhoneNum { get; set; }
    }
}