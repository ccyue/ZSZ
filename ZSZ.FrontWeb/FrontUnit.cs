using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZSZ.FrontWeb
{
    public class FrontUnit
    {
        public long? GetUserID(HttpContextBase httpContext)
        {
            return (long?)httpContext.Session["UserId"];
        }
        public long? GetCityID(HttpContextBase httpContext)
        {
            return (long?)httpContext.Session["CityId"];
        }
    }
}