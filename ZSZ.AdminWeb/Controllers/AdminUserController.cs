using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.App_Start;
using ZSZ.AdminWeb.Models;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class AdminUserController : Controller
    {
        public IAdminUserService AdminUserService { get; set; }
        public IRoleService RoleService { get; set; }
        public ICityService CityService { get; set; }

        [CheckPermission("AdminUser.List")]

        public ActionResult List()
        {
            var admins = AdminUserService.GetAll();
            return View(admins);
        }

        [CheckPermission("AdminUser.Delete")]
        [HttpPost]
        public ActionResult Delete(long id)
        {
            AdminUserService.Delete(id);
            return Json(new AjaxResult() { Status = "ok" });
        }
        [CheckPermission("AdminUser.Add")]
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Roles = RoleService.GetAll();
            var cities = CityService.GetAll().ToList();
            cities.Insert(0, new DTO.CityDTO { Id = 0, Name = "总部" });
            ViewBag.Cities = cities.Select(p => new SelectListItem()
            {
                Text = p.Name,
                Value = p.Id.ToString(),
                Selected = p.Id==0
            });
            return View(new AdminUserAddModel());
        }
        [CheckPermission("AdminUser.Add")]
        [HttpPost]
        public ActionResult Add(AdminUserAddModel model)
        {
            if(!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = CommonMVC.MVCHelper.GetValidMsg(ModelState) });
            }
            var user = AdminUserService.GetbyPhoneNum(model.PhoneNum);
            if (user != null)
            {
                return Json(new AjaxResult() { Status = "exist", ErrorMsg = "这个手机号已经存在!" });
            }
            model.CityId = model.CityId == 0 ? null : model.CityId;
            long newId = AdminUserService.Add(model.Name, model.PhoneNum, model.Password, model.Email, model.CityId);
            if (model.RoleIds!=null && model.RoleIds.Count() > 0)
            {
                RoleService.GrantRoleToAdmin(newId, model.RoleIds);
            }
            return Json(new AjaxResult() { Status = "ok" });
        }
        [CheckPermission("AdminUser.Edit")]
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var user = AdminUserService.GetById(id);
            if (user == null)
            {
                return View("Error", (object)"id指定的操作员不存在");
            }
            AdminUserEditModel model = new AdminUserEditModel()
            {
                Id = user.Id,
                CityId = user.CityId,
                Email = user.Email,
                Name = user.Name,
                PhoneNum = user.PhoneNum
            };
            //role
            var roles = RoleService.GetByAdminUser(user.Id);
            model.RoleIds = roles != null ? roles.Select(p => p.Id).ToArray() : new long[] { };
            ViewBag.Roles = RoleService.GetAll();
            //city
            var cities = CityService.GetAll().ToList();
            cities.Insert(0, new DTO.CityDTO { Id = 0, Name = "总部" });
            ViewBag.Cities = cities.Select(p => new SelectListItem()
            {
                Text = p.Name,
                Value = p.Id.ToString(),
                Selected = user.CityId == p.Id
            });
            return View(model);
        }
        [CheckPermission("AdminUser.Edit")]
        [HttpPost]
        public ActionResult Edit(AdminUserEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = CommonMVC.MVCHelper.GetValidMsg(ModelState) });
            }
            var user = AdminUserService.GetById(model.Id);
            if (user == null)
            {
                return Json(new AjaxResult() { Status = "error", ErrorMsg = "用户不存在!" });
            }
            model.CityId = model.CityId == 0 ? null : model.CityId;
            AdminUserService.Update(model.Id, model.Name, model.PhoneNum, model.Password, model.Email, model.CityId);
            RoleService.GrantRoleToAdmin(model.Id, model.RoleIds);
            return Json(new AjaxResult() { Status = "ok" });
        }
        [HttpPost]
        public ActionResult CheckPhoneNum(string phoneNum)
        {
            var user = AdminUserService.GetbyPhoneNum(phoneNum);
            if(user!=null)
            {
                return Json(new AjaxResult() { Status = "exist", ErrorMsg = "这个手机号已经存在!" });
            }
            else
            {
                return Json(new AjaxResult() { Status = "ok" });
            }
        }
    }
}