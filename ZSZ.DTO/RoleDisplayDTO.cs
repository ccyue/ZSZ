using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    public class RoleDisplayDTO
    {
        public RoleDTO Role { get; set; }
        public long[] RolePermissionIds { get; set; }
        public PermissionDTO[] AllPermission { get; set; }
    }
}
