using IMyBLL;
using System;

namespace MyBLL
{
    public class UserBLL : IUserBLL
    {
        public void Add(string username, string pwd)
        {
            Console.WriteLine("create user successfully:{0}", username);
        }

        public bool Check(string username, string pwd)
        {
            Console.WriteLine("username or password is not correct");
            return false;
        }
    }
}
