using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class MainController : Controller
    {
        public ICityService cityService { get; set; }
        public ActionResult Index()
        {
            if (Session["ss"] != null)
            {
                return Content((string)Session["ss"]);
            }
            Session["ss"] = "test session";
            cityService.Add("北京");
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
	}
}