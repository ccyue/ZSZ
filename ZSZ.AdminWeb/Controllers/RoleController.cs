﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.CommonMVC;
using ZSZ.DTO;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class RoleController : Controller
    {
        public IRoleService RoleService { get; set; }
        public IPermissionService PermissionService { get; set; }
        //
        // GET: /Role/
        public ActionResult List()
        {
            var roles = RoleService.GetAll();
            return View(roles);
        }
        [HttpGet]
        public ActionResult Add()
        {
            PermissionDTO[] perms = PermissionService.GetAll();
            ViewBag.Permissions = perms;
            return View();
        }
        [HttpPost]
        public ActionResult Add(RoleAddDTO role)
        {
            if(!ModelState.IsValid)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = MVCHelper.GetValidMsg(ModelState) });
            }
            long roleId = RoleService.Add(role.Name);
            PermissionService.UpdatePermForRole(roleId, role.PermissionIds);            
            return Json(new AjaxResult() { Status = "ok" });
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            RoleDisplayDTO model = new RoleDisplayDTO();
            model.Role = RoleService.GetById(id);
            model.RolePermissionIds = PermissionService.GetByRoleId(id).Select(p => p.Id).ToArray();
            model.AllPermission = PermissionService.GetAll();
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(RoleEditDTO model)
        {
            RoleService.Update(model.Role.Id, model.Role.Name);
            PermissionService.UpdatePermForRole(model.Role.Id, model.PermissionIds);
            return Json(new AjaxResult() { Status = "ok" });
        }
        [HttpPost]
        public ActionResult Delete(long id)
        {
            RoleService.Delete(id);
            return Json(new AjaxResult() { Status = "ok" });
        }
        [HttpPost]
        public ActionResult BatchDelete(long[] roleIds)
        {
            foreach(var id in roleIds)
            {
                RoleService.Delete(id);
            }           
            return Json(new AjaxResult() { Status = "ok" });
        }
    }
}