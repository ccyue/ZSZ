using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZSZ.AdminWeb.Controllers
{
    public class AdminUserController : Controller
    {
        //
        // GET: /AdminUser/
        public ActionResult List()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
	}
}