using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    public class RoleAddDTO: BaseDTO
    {
        [Required]
        [MaxLength(50)]
        public String Name { get; set; }
        public long[] PermissionIds { get; set; }
    }
}
