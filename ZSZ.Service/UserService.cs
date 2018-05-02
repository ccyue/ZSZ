using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class UserService : IUserService
    {
        public long Add(string phoneNum, string password)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<UserEntity> csUser = new CommonService<UserEntity>(dbc);
                bool exist = csUser.GetAll().Any(p => p.PhoneNum == phoneNum);
                if(exist)
                {
                    throw new ArgumentException("PhoneNum has already exist.");
                }
                var user = new UserEntity();
                user.PhoneNum = phoneNum;
                string salt = CommonHelper.GenerateCaptchaCode(5);
                user.PasswordSalt = salt;
                user.PasswordHash = CommonHelper.CalcMD5(salt + password);
                user.CreateDateTime = DateTime.Now;
                dbc.Users.Add(user);
                dbc.SaveChanges();
                return user.Id;
            }
        }

        public bool CheckLogin(string phoneNum, string password)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<UserEntity> csUser = new CommonService<UserEntity>(dbc);
                var user = csUser.GetAll().SingleOrDefault(p => p.PhoneNum == phoneNum);
                if (user == null)
                {
                    return false;
                }
                string loginPassword = CommonHelper.CalcMD5(user.PasswordSalt + password);
                return loginPassword == user.PasswordHash;                
            }
        }

        public UserDTO GetById(long id)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<UserEntity> csUser = new CommonService<UserEntity>(dbc);
                var user = csUser.GetById(id);
                if(user==null)
                {
                    throw new ArgumentException("User is not exist.");
                }
                return ToDTO(user);
            }
        }

        public UserDTO GetByPhoneNum(string phoneNum)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<UserEntity> csUser = new CommonService<UserEntity>(dbc);
                var user = csUser.GetAll().SingleOrDefault(p => p.PhoneNum == phoneNum);
                if (user == null)
                {
                    throw new ArgumentException("User is not exist.");
                }
                return ToDTO(user);
            }
        }

        public void SetCityForUser(long userId, long cityId)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<UserEntity> csUser = new CommonService<UserEntity>(dbc);
                var user = csUser.GetById(userId);
                if (user == null)
                {
                    throw new ArgumentException("User is not exist.");
                }
                CommonService<CityEntity> csCity = new CommonService<CityEntity>(dbc);
                var city = csCity.GetById(cityId);
                if (city == null)
                {
                    throw new ArgumentException("City is not exist.");
                }
                user.CityId = cityId;
                dbc.SaveChanges();
            }
        }

        public bool UpdatePwd(long userId, string newPassword)
        {
            using (ZSZDbContext dbc = new ZSZDbContext())
            {
                CommonService<UserEntity> csUser = new CommonService<UserEntity>(dbc);
                var user = csUser.GetById(userId);
                if (user == null)
                {
                    throw new ArgumentException("User is not exist.");
                }
                user.PasswordHash = CommonHelper.CalcMD5(user.PasswordSalt + newPassword);
                return dbc.SaveChanges()>0;
            }
        }

        public UserDTO ToDTO(UserEntity user)
        {
            return new UserDTO()
            {
                Id = user.Id,
                CityId = user.CityId,
                PhoneNum = user.PhoneNum,
                CreateDateTime = user.CreateDateTime,
                LastLoginErrorDateTime = user.LastLoginErrorDateTime,
                LoginErrorTimes = user.LoginErrorTimes
            };
        }
    }
}
