using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.CommonMVC;
using ZSZ.DTO;
using ZSZ.FrontWeb.Models;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
    public class UserController : Controller
    {
        public IUserService UserService { get; set; }
        public ISettingService SettingService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        #region Forget Password
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string phoneNum, string captchaCode)
        {
            string serverCaptchaCode = TempData["captchaCode"].ToString();
            if (captchaCode != serverCaptchaCode)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "验证码错误" });
            }
            //check phoneNum
            var user = UserService.GetByPhoneNum(phoneNum);
            if (user == null)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "手机号不存在" });
            }

            //get seeting from database
            string appKey = SettingService.GetValue("SMS_AppKey");
            string userName = SettingService.GetValue("SMS_UserName");
            string templateId = SettingService.GetValue("SMS_TemplateId");
            //generate SMS code
            string smsCode = new Random().Next(1000, 9999).ToString();
            //send smscode
            SMSSender sender = new SMSSender() { AppKey = appKey, UserName = userName };
            var smsResult = sender.SendSMS(templateId, smsCode, phoneNum);
            if (smsResult.code == 0)
            {
                TempData["ForgotPasswordPhoneNum"] = phoneNum;
                TempData["SMSCode"] = smsCode;
                return Json(new AjaxResult() { Status = "ok" });
            }
            else
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = smsResult.msg });
            }
        }
        [HttpGet]
        public ActionResult ForgotPassword2()
        {
            var phoneNum = (string)TempData["ForgotPasswordPhoneNum"];
            if (phoneNum == null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误" });
            }
            TempData["ForgotPasswordPhoneNum"] = phoneNum;
            return View(phoneNum);
        }
        [HttpPost]
        public ActionResult ForgotPassword2(string phoneNum, string smsCode)
        {
            string serverSMSCode = (string)TempData["SMSCode"];
            string serverPhoneNum = (string)TempData["ForgotPasswordPhoneNum"];
            if (serverSMSCode == smsCode && serverPhoneNum == phoneNum)
            {
                TempData["ForgetPassword2_OK"] = true;
                TempData["ForgotPasswordPhoneNum"] = serverPhoneNum;
                return Json(new AjaxResult { Status = "ok" });
            }
            else
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误" });
            }
        }
        [HttpGet]
        public ActionResult ForgotPassword3()
        {
            return View(new UserUpdatePassword());
        }
        [HttpPost]
        public ActionResult ForgotPassword3(UserUpdatePassword model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = CommonMVC.MVCHelper.GetValidMsg(ModelState) });
            }
            bool? is2_OK = (bool?)TempData["ForgetPassword2_OK"];
            if (is2_OK != true)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "您没有通过短信验证码的验证" });
            }
            string phoneNum = (string)TempData["ForgotPasswordPhoneNum"];
            var userId = UserService.GetByPhoneNum(phoneNum).Id;
            UserService.UpdatePwd(userId, model.Password);
            return Json(new AjaxResult() { Status = "ok" });
        }
        [HttpGet]
        public ActionResult ForgotPassword4()
        {
            return View();
        } 
        #endregion
    }
}