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
        PermissionDTO GetById();
        PermissionDTO GetByName(string name);
        PermissionDTO[] GetByRoleId { get; set; }
        void AddPermForRole(long roleId, long[] permIds);
        void UpdatePermForRole(long roleId, long[] permIds);
    }
}
