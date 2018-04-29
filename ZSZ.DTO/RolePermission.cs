using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    public class RolePermission : BaseDTO
    {
        public long RoleId { get; set; }
        public long PermissionId { get; set; }
    }
}
