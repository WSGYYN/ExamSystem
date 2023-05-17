using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamSystem.Controllers
{
    public class HomeController : Controller
    {
        //主页面
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        
        //退出登录
        public ActionResult Layout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }


	}
}
