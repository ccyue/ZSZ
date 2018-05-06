using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb
{
    public class AdminHelper
    {
        public static long? GetLoginUserId(HttpContextBase context)
        {
            return (long?)context.Session["LoginUserId"];
        }
    }
}