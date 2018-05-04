using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.Models;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class MainController : Controller
    {
        public ICityService cityService { get; set; }
        public IAdminUserService AdminUserService { get; set; }
        public ActionResult Index()
        {
            if (Session["ss"] != null)
            {
                return Content((string)Session["ss"]);
            }
            Session["ss"] = "test session";
            //cityService.Add("北京");
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = MVCHelper.GetValidMsg(ModelState) });
            }
            //TempData读取一次，数据就销毁
            if ((string)TempData["captchaCode"] != model.CaptchaCode)
            {
                TempData["captchaCode"] = string.Empty;
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "验证码错误" });
            }            
            bool result = AdminUserService.CheckLogin(model.PhoneNum, model.Password);
            if(result)
            {
                var user = AdminUserService.GetbyPhoneNum(model.PhoneNum);
                Session["LoginUserId"] = user.Id;
                Session["CityId"] = user.CityId;
                return Json(new AjaxResult() { Status = "ok", ErrorMsg = "登陆成功" });
            }
            else
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "用户名或密码错误" });
            }
        }

        [HttpGet]
        public ActionResult GenerateCaptchaPic()
        {
            string captchaCode = CommonHelper.GenerateCaptchaCode(4);
            //Session["captchaCode"] = captchaCode;
            TempData["captchaCode"] = captchaCode;
            MemoryStream ms = ImageFactory.GenerateImage(captchaCode, 41, 100, 18, 1);
            return File(ms, "image/jpeg");
        }
    }
}