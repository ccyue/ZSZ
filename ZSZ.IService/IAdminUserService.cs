using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IAdminUserService:IServiceSupport
    {
        /// <summary>
        /// Add new AdminUser
        /// </summary>
        long Add(string name, string phoneNum, string password, string email, long? cityId);
        /// <summary>
        /// Update AdminUser information
        /// </summary>
        void Update(long id, string name, string phoneNum, string password, string email, long? cityId);
        /// <summary>
        /// Delete AdminUser
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(long id);
        /// <summary>
        /// Get All AdminUser
        /// </summary>
        /// <returns></returns>
        AdminUserDTO[] GetAll();
        /// <summary>
        /// Get All AdminUserbyCityId
        /// </summary>
        /// <returns></returns>
        AdminUserDTO[] GetAll(long? cityId);
        /// <summary>
        /// Get AdminUser information by Id
        /// </summary>
        /// <returns></returns>
        AdminUserDTO GetById(long id);
        /// <summary>
        /// Get AdminUser information by phoneNum
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        AdminUserDTO GetbyPhoneNum(string phoneNum);
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool CheckLogin(string phoneNum, string password);
        /// <summary>
        /// Check Permission
        /// </summary>
        /// <param name="adminUserId"></param>
        /// <param name="permissionName"></param>
        /// <returns></returns>
        bool HasPermission(long adminUserId, string permissionName);
        /// <summary>
        /// Record Login Error time
        /// </summary>
        /// <param name="id"></param>
        void RecordLoginError(long adminUserId);
        /// <summary>
        /// Reset Login Error time
        /// </summary>
        /// <param name="id"></param>
        void ResetLoginError(long adminUserId);
    }
}
