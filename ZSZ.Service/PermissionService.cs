using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class PermissionService : IPermissionService
    {
        public PermissionDTO[] GetAll()
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<PermissionEntity> cs = new CommonService<PermissionEntity>(dbc);
                return cs.GetAll().Select(p => ToDTO(p)).ToArray();
            }
        }

        public PermissionDTO GetById(long id)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<PermissionEntity> cs = new CommonService<PermissionEntity>(dbc);
                var permission = cs.GetById(id);
                return permission == null ? null : ToDTO(permission);
            }
        }

        public PermissionDTO GetByName(string name)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<PermissionEntity> cs = new CommonService<PermissionEntity>(dbc);
                var permission = cs.GetAll().Where(p => p.Name == name).SingleOrDefault();
                return permission == null ? null : ToDTO(permission);
            }
        }
        public PermissionDTO[] GetByRoleId(long roleId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<RoleEntity> cs = new CommonService<RoleEntity>(dbc);
                return cs.GetAll().Where(p => p.Id == roleId).Include(p => p.Permissions)
                    .SelectMany(p => p.Permissions).Select(p => ToDTO(p))
                    .ToArray();
            }
        }

        public void UpdatePermForRole(long roleId, long[] permIds)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<RoleEntity> csRole = new CommonService<RoleEntity>(dbc);
                var role = csRole.GetById(roleId);
                if(role==null)
                {
                    throw new ArgumentException("The role is not exist.");
                }
                CommonService<PermissionEntity> csPermission = new CommonService<PermissionEntity>(dbc);
                var newPermission = csPermission.GetAll().Where(p => permIds.Contains(p.Id)).ToList();
                role.Permissions.Clear();
                role.Permissions = newPermission;
                dbc.SaveChanges();
            }
        }
        public void UpdatePermission(long id, string permName, string description)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<PermissionEntity> cs = new CommonService<PermissionEntity>(dbc);
                var entity = cs.GetById(id);
                if(entity ==null)
                {
                    throw new ArgumentException("The Permission is not exist");
                }
                entity.Name = permName;
                entity.Description = description;
                dbc.SaveChanges();
            }
        }
        public void Deleted(long id)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<PermissionEntity> cs = new CommonService<PermissionEntity>(dbc);
                cs.MarkDeleted(id);
            }
        }
        public PermissionDTO ToDTO(PermissionEntity entity)
        {
            return new PermissionDTO()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CreateDateTime = entity.CreateDateTime
            };
        }
    }
}
