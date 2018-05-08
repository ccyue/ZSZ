using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.DTO;
using ZSZ.FrontWeb.Models;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
    public class MainController : Controller
    {
        public ISettingService SettingService { get; set; }
        public IUserService UserService { get; set; }
        public ICityService CityService { get; set; }
        public ActionResult Index(long? cityId)
        {
            if(cityId==null)
            {
                cityId = FrontUnit.GetCityID(HttpContext);
            }
            ViewBag.CurrentCityId = cityId;
            ViewBag.Cities = CityService.GetAll();
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View(new UserLoginModel());
        }
        [HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = CommonMVC.MVCHelper.GetValidMsg(ModelState) });
            }
            var user = UserService.GetByPhoneNum(model.PhoneNum);
            if (user != null)
            {
                if(UserService.IsLocked(user.Id))
                {
                    TimeSpan? leftTimeSpan = TimeSpan.FromMinutes(30) - (DateTime.Now - user.LastLoginErrorDateTime);
                    return Json(new AjaxResult() {
                        Status = "error",
                        ErrorMsg = "账号已被锁定，请" + (int)leftTimeSpan.Value.TotalMinutes + "分钟再试"
                    });
                }
            }
            var checkLogin = UserService.CheckLogin(model.PhoneNum, model.Password);
            if(checkLogin)
            {
                UserService.ResetLoginError(user.Id);
                Session["UserId"] = user.Id;
                Session["CityId"] = user.CityId;
                return Json(new AjaxResult() { Status = "ok" });
            }
            else
            {
                UserService.IncrLoginError(user.Id);
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "用户名或密码错误!" });
            }
        }

        [HttpGet]
        public ActionResult GenerateCaptchaPic()
        {
            string captchaCode = CommonHelper.GenerateCaptchaCode(4);
            //Session["captchaCode"] = captchaCode;
            TempData["captchaCode"] = captchaCode;
            MemoryStream ms = ImageFactory.GenerateImage(captchaCode, 41, 100, 15, 6);
            return File(ms, "image/jpeg");
        }
        [HttpPost]
        public ActionResult SendCaptchaCode(string phoneNum, string captchaCode)
        {
            //check phoneNum
            var user = UserService.GetByPhoneNum(phoneNum);
            if(user!=null)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "手机号已经被注册" });
            }
            string serverCaptchaCode = TempData["captchaCode"].ToString();
            if (captchaCode != serverCaptchaCode)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "验证码错误" });
            }
            //get seeting from database
            string appKey = SettingService.GetValue("SMS_AppKey");
            string userName = SettingService.GetValue("SMS_UserName");
            string templateId = SettingService.GetValue("SMS_TemplateId");
            //generate SMS code
            string smsCode = new Random().Next(1000, 9999).ToString();
            TempData["SMSCode"] = smsCode;
            //send smscode
            SMSSender sender = new SMSSender() { AppKey = appKey, UserName = userName};
            var smsResult = sender.SendSMS(templateId, smsCode, phoneNum);
            if (smsResult.code == 0)
            {
                TempData["RegPhoneNum"] = phoneNum;
                return Json(new AjaxResult() { Status = "ok" });
            }
            else
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = smsResult.msg });
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserRegModel model)
        {
            if(!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = CommonMVC.MVCHelper.GetValidMsg(ModelState) });
            }
            if (model.PhonenNum != TempData["RegPhoneNum"].ToString() || model.SMSCode != TempData["SMSCode"].ToString())
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "验证码不正确" });

            }
            UserService.Add(model.PhonenNum, model.Password);
            return Json(new AjaxResult() { Status = "ok", ErrorMsg = "注册成功" });
        }
    }
}