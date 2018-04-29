using ITestService;
using System;

namespace TestService
{
    public class UserService : IUserService
    {
        public void CheckLogin(string UserName, string password)
        {
            
        }

        public bool CheckUserExist(string UserNam)
        {
            return true;
        }
    }
}
