using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.FrontWeb.Models
{
    public class UserRegModel
    {
        [Required]
        [Phone]
        public string PhonenNum { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public string SMSCode { get; set; }
    }
}