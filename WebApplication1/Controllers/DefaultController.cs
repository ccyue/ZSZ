using ITestService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class DefaultController : Controller
    {
        public IUserService UserService { get; set; }

        // GET: Default
        public ActionResult Index()
        {
            bool b = UserService.CheckUserExist("yue");
            return Content(b.ToString());
        }
        [HttpGet]
        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Test(string name)
        {
            Dog dog = new Dog() { Name = name, Age = 1, Birthday = DateTime.Now.AddYears(-1) };
            return Json(dog);
        }

        public ActionResult Pager1()
        {
            return View();
        }

        public ActionResult jstemp()
        {
            return View();
        }
    }
}