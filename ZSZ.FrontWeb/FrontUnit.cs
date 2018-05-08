using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.FrontWeb
{
    public class FrontUnit
    {
        public static long? GetUserID(HttpContextBase httpContext)
        {
            return (long?)httpContext.Session["UserId"];
        }
        public static long GetCityID(HttpContextBase httpContext)
        {
            long? userId = GetUserID(httpContext);
            if(userId==null)
            {
                long? cityId = (long?)httpContext.Session["CityId"];
                if(cityId!=null)
                {
                    return cityId.Value;
                }
                else
                {
                    var citySvc = DependencyResolver.Current.GetService<ICityService>();
                    return citySvc.GetAll().FirstOrDefault().Id;
                }

            }
            else
            {
                var userSvc = DependencyResolver.Current.GetService<IUserService>();
                long? cityId = userSvc.GetById(userId.Value).CityId;
                if(cityId!=null)
                {
                    return cityId.Value;
                }
                else
                {
                    var citySvc = DependencyResolver.Current.GetService<ICityService>();
                    return citySvc.GetAll().FirstOrDefault().Id;
                }
            }
        }
    }
}