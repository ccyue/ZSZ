using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.CommonMVC;
using ZSZ.DTO;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class PermissionController : Controller
    {
        public IPermissionService PermissionService { get; set; }
        public ActionResult List()
        {
            var perms = PermissionService.GetAll();
            return View(perms);
        }
        [HttpPost]
        public ActionResult Delete(long id)
        {
            PermissionService.Deleted(id);
            return Json(new AjaxResult { Status = "ok" });
        }
        [HttpGet]
        public ActionResult Add()
        {           
            return View(new PermissionDTO());
        }
        [HttpPost]
        public ActionResult Add(string name,string description)
        {
            PermissionService.Add(name,description);
            return Json(new AjaxResult { Status = "ok" });
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var perm = PermissionService.GetById(id);
            return View(perm);
        }
        [HttpPost]
        public ActionResult Edit(PermissionDTO model)
        {
            PermissionService.UpdatePermission(model.Id, model.Name, model.Description);
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}