using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IUserService:IServiceSupport
    {
        long Add(string phoneNum, string password);
        UserDTO GetById(long id);
        UserDTO GetByPhoneNum(string phoneNum);
        bool CheckLogin(string phoneNum, string password);
        bool UpdatePwd(long userId, string newPassword);
        void SetCityForUser(long userId, long cityId);
        bool IsLocked(long id);
        /// <summary>
        /// 记录一次登录失败
        /// </summary>
        /// <param name="id"></param>
        void IncrLoginError(long id);

        /// <summary>
        /// 充值登录失败信息
        /// </summary>
        /// <param name="id"></param>
        void ResetLoginError(long id);
    }
}
