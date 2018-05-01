using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using ZSZ.Common;

namespace ZSZ.Service
{
    class AdminUserService : IAdminUserService
    {
        public long Add(string name, string phoneNum, string password, string email, long? cityId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                bool exist = cs.GetAll().Any(p => p.PhoneNum == phoneNum);
                if (exist)
                {
                    throw new ArgumentException(string.Format("{0} has already exist.", phoneNum));
                }
                string salt = CommonHelper.GenerateCaptchaCode(5);
                var admin = new AdminUserEntity()
                {
                    Name = name,
                    PhoneNum = phoneNum,
                    PasswordSalt = salt,
                    PasswordHash = CommonHelper.CalcMD5(salt + password),                   
                    Email = email,
                    CityId = cityId
                };
                dbc.AdminUsers.Add(admin);
                return admin.Id;
            }
        }

        public bool CheckLogin(string phoneNum, string password)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var admin = cs.GetAll().SingleOrDefault(p=>p.PhoneNum == phoneNum);     
                if(admin == null)
                {
                    return false;
                }
                string userHash = CommonHelper.CalcMD5(admin.PasswordSalt + password);
                return userHash == admin.PasswordHash;
            }
        }

        public void Delete(long id)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                cs.MarkDeleted(id);
            }
        }

        public AdminUserDTO[] GetAll()
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                return cs.GetAll()
                    .Include(p=>p.City).AsNoTracking()
                    .Select(p => ToDTO(p)).ToArray();
            }
        }
        public AdminUserDTO[] GetAll(long? cityId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                return cs.GetAll().Where(p => p.CityId == cityId).Include(p => p.City).AsNoTracking()
                    .Select(p => ToDTO(p)).ToArray();
            }
        }
        public AdminUserDTO GetById(long id)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                AdminUserEntity entity = cs.GetAll().Include(p => p.City).AsNoTracking()
                    .SingleOrDefault(p => p.Id == id);

                return entity == null ? null : ToDTO(entity);
            }
        }
        public AdminUserDTO GetbyPhoneNum(string phoneNum)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var users = cs.GetAll().Include(p => p.City).AsNoTracking()
                    .Where(p => p.PhoneNum == phoneNum);
                int count = users.Count();
                if (count<=0)
                {
                    return null;
                }
                else if(count==1)
                {
                    return ToDTO(users.Single());
                }
                else
                {
                    throw new ArgumentException("one phonenum mapping many admin, system error.");
                }
            }
        }

        public bool HasPermission(long adminUserId, string permissionName)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AdminUserEntity> csAdminUser = new CommonService<AdminUserEntity>(dbc);
                var admin = csAdminUser.GetAll().Include(p => p.Roles).AsNoTracking().SingleOrDefault(p => p.Id == adminUserId);
                if(admin == null)
                {
                    throw new ArgumentException(string.Format("{0} is not exist.", adminUserId));
                }
                bool exist = admin.Roles.SelectMany(p => p.Permissions).Any(p => p.Name == permissionName);
                return exist;
            }
        }

        public void RecordLoginError(long adminUserId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var entity = cs.GetById(adminUserId);
                if(entity == null)
                {
                    throw new ArgumentException(string.Format("{0} is not exist.", adminUserId));
                }
                entity.LoginErrorTimes += 1;
                entity.LastLoginErrorDateTime = DateTime.Now;
                dbc.SaveChanges();
            }
        }

        public void ResetLoginError(long adminUserId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var entity = cs.GetById(adminUserId);
                if (entity == null)
                {
                    throw new ArgumentException(string.Format("{0} is not exist.", adminUserId));
                }
                entity.LoginErrorTimes = 0;
                entity.LastLoginErrorDateTime = null;
                dbc.SaveChanges();
            }
        }

        public void Update(long id, string name, string phoneNum, string password, string email, long? cityId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var entity = cs.GetById(id);
                if (entity == null)
                {
                    throw new ArgumentException(string.Format("{0} is exist.", name));
                }
                entity.Name = name;
                entity.PhoneNum = phoneNum;
                if(string.IsNullOrEmpty(password))
                {
                    entity.PasswordHash = CommonHelper.CalcMD5(entity.PasswordSalt + password);
                }                
                entity.Email = email;
                entity.CityId = cityId;
                dbc.SaveChanges();
            }
        }
        private AdminUserDTO ToDTO(AdminUserEntity user)
        {
            AdminUserDTO dto = new AdminUserDTO();
            dto.CityId = user.CityId;
            if (user.City != null)
            {
                dto.CityName = user.City.Name;//需要Include提升性能
                //如鹏总部（北京）、如鹏网上海分公司、如鹏广州分公司、如鹏北京分公司
            }
            else
            {
                dto.CityName = "总部";
            }

            dto.CreateDateTime = user.CreateDateTime;
            dto.Email = user.Email;
            dto.Id = user.Id;
            dto.LastLoginErrorDateTime = user.LastLoginErrorDateTime;
            dto.LoginErrorTimes = user.LoginErrorTimes;
            dto.Name = user.Name;
            dto.PhoneNum = user.PhoneNum;
            return dto;
        }
    }
}
