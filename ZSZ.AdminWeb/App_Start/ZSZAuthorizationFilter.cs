using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.AdminWeb.App_Start
{
    public class ZSZAuthorizationFilter : IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            CheckPermissionAttribute[] permAttrs = 
            (CheckPermissionAttribute[])filterContext.ActionDescriptor.GetCustomAttributes(typeof(CheckPermissionAttribute), false);
            if(permAttrs.Length==0)
            {
                return;
            }
            //获取当前登录用户的id
            long? userId = (long?)filterContext.HttpContext.Session["LoginUserId"];
            if(userId==null)
            {
                if(filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    AjaxResult result = new AjaxResult();
                    result.Status = "redirect";
                    result.Data = "/Main/Login";
                    result.ErrorMsg = "未登录";
                    filterContext.Result = new JsonResult { Data = result };
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Main/Login");   
                }
                return;
            }
            IAdminUserService AdminUserService = DependencyResolver.Current.GetService<IAdminUserService>();
            foreach(var perm in permAttrs)
            {
                if (!AdminUserService.HasPermission(userId.Value, perm.Permision))
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        AjaxResult result = new AjaxResult();
                        result.Status = "error";
                        result.ErrorMsg = string.Format("没有{0}权限", perm.Permision);
                        filterContext.Result = new JsonResult { Data = result };
                    }

                    filterContext.Result = new ContentResult() { Content = string.Format("没有{0}权限", perm.Permision) };
                    return;
                }
            }
        }
    }
}