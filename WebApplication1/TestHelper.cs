using ITestService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1
{
    public class TestHelper
    {
        public static bool Test()
        {
            IUserService user = DependencyResolver.Current.GetService<IUserService>();
            return user.CheckUserExist("ttt");
        }
    }
}