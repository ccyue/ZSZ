using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IPermissionService:IServiceSupport
    {
        PermissionDTO[] GetAll();
        PermissionDTO GetById(long id);
        PermissionDTO GetByName(string name);
        PermissionDTO[] GetByRoleId(long roleId);
        void UpdatePermForRole(long roleId, long[] permIds);
        void UpdatePermission(long id, string permName, string description);
        void Deleted(long id);
        long Add(string permName, string description);
    }
}
