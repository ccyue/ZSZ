using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity;

namespace ZSZ.Service
{
    public class RoleService : IRoleService
    {
        public RoleDTO[] GetAll()
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<RoleEntity> rsRole = new CommonService<RoleEntity>(dbc);
                return rsRole.GetAll().Select(p => ToDTO(p)).ToArray();
            }
        }

        public RoleDTO GetById(long roleId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<RoleEntity> csRole = new CommonService<RoleEntity>(dbc);
                var role = csRole.GetById(roleId);
                return role == null ? null : ToDTO(role);
            }
        }

        public RoleDTO GetByName(string roleName)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<RoleEntity> csRole = new CommonService<RoleEntity>(dbc);
                var role = csRole.GetAll().SingleOrDefault(p => p.Name == roleName);
                return role == null ? null : ToDTO(role);
            }
        }

        public long Add(string roleName)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<RoleEntity> rsRole = new CommonService<RoleEntity>(dbc);
                var exist = rsRole.GetAll().Any(p => p.Name == roleName);
                if(exist)
                {
                    throw new ArgumentException("The role has been already");
                }
                var role = new RoleEntity()
                {
                    Name = roleName,
                    CreateDateTime = DateTime.Now
                };
                dbc.Roles.Add(role);
                dbc.SaveChanges();
                return role.Id;
            }
        }

        public void Update(long roleId, string roleName)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<RoleEntity> csRole = new CommonService<RoleEntity>(dbc);
                var role = csRole.GetById(roleId);
                if (role == null)
                {
                    throw new ArgumentException("The role is not exist");
                }
                role.Name = roleName;
                dbc.SaveChanges();
            }
        }

        public void Delete(long roleId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<RoleEntity> rsRole = new CommonService<RoleEntity>(dbc);
                rsRole.MarkDeleted(roleId);
            }
        }

        public RoleDTO[] GetByAdminUser(long adminUserId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var user = cs.GetById(adminUserId);
                if (user == null)
                {
                    throw new ArgumentException("The user is not exist");
                }
                return user.Roles.Count() > 0 ? user.Roles.Select(p => ToDTO(p)).ToArray() : null;
            }
        }

        public void GrantRoleToAdmin(long adminUserId, long[] roleIds)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AdminUserEntity> csAdmin = new CommonService<AdminUserEntity>(dbc);
                var admin = csAdmin.GetById(adminUserId);
                if(admin==null)
                {
                    throw new ArgumentException("The Admin account is not exist.");
                }
                CommonService<RoleEntity> csRole = new CommonService<RoleEntity>(dbc);
                var roles = csRole.GetAll().Where(p => roleIds.Contains(p.Id)).ToList();
                admin.Roles.Clear();
                admin.Roles = roles;
                dbc.SaveChanges();
            }
        }

        private RoleDTO ToDTO(RoleEntity role)
        {
            return new RoleDTO()
            {
                Id = role.Id,
                Name = role.Name,
                CreateDateTime = role.CreateDateTime
            };
        }
    }
}
