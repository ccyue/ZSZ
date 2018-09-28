using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.App_Start;
using ZSZ.CommonMVC;
using ZSZ.DTO;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class HouseAppointmentController : Controller
    {
        public IHouseAppointmentService HouseAppointmentService { get; set; }
        public IAdminUserService AdminUserService { get; set; }
        [CheckPermission("HouseAppointment.List")]
        public ActionResult List(int pageIndex = 1)
        {
            string status = "未处理";
            long? userId = AdminHelper.GetLoginUserId(HttpContext);
            if(userId==null)
            {
                return RedirectToAction("Login", "AdminUser");
            }
            long? cityId = AdminUserService.GetById(userId.Value).CityId;
            HouseAppointmentDTO[] dtos =  HouseAppointmentService.GetPageData(cityId.Value, status, 10,(pageIndex-1)*10,null);
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalCount = HouseAppointmentService.GetTotalCount(cityId.Value, status,null);
            return View(dtos);
        }
        [CheckPermission("HouseAppointment.List")]
        public ActionResult MyList(int pageIndex = 1)
        {
            long? userId = AdminHelper.GetLoginUserId(HttpContext);
            if (userId == null)
            {
                return RedirectToAction("Login", "AdminUser");
            }
            long? cityId = AdminUserService.GetById(userId.Value).CityId;
            HouseAppointmentDTO[] dtos = HouseAppointmentService.GetPageData(cityId.Value, string.Empty, 10, (pageIndex - 1) * 10,userId.Value);
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalCount = HouseAppointmentService.GetTotalCount(cityId.Value, string.Empty,userId.Value);
            return View(dtos);
        }

        [CheckPermission("HouseAppointment.Follow")]
        public ActionResult Follow(long appointmentId)
        {
            long? userId = AdminHelper.GetLoginUserId(HttpContext);
            bool res = HouseAppointmentService.Follow(userId.Value, appointmentId);
            return Json(new AjaxResult() { Status = res ? "ok" : "fail" });
        }
	}
}