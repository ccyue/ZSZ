using System;

namespace ITestService
{
    public interface IUserService
    {
        void CheckLogin(string UserName, string password);
        bool CheckUserExist(string UserNam);
    }
}
