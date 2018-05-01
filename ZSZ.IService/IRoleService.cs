using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IRoleService:IServiceSupport
    {
        long Add(string roleName);
        void Update(long roleId, string roleName);
        void Delete(long roleId);
        RoleDTO GetById(long roleId);
        RoleDTO GetByName(string roleName);
        RoleDTO[] GetAll();
        void GrantRoleToAdmin(long adminUserId, long[] roleIds);
        RoleDTO[] GetByAdminUser(long adminUserId);
    }
}
