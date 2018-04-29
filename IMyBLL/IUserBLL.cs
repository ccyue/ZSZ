using System;

namespace IMyBLL
{
    public interface IUserBLL
    {
        bool Check(string username, string pwd);
        void Add(string username, string pwd);
    }
}
