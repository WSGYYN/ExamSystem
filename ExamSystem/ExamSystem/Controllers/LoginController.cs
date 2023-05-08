using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamSystem.Models;

namespace ExamSystem.Controllers
{
    
    //学生和教师登录
    public class LoginController : Controller
    {
        //学生登录
        // GET: Login
        public ActionResult StuLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult StuLogin(string StuLoginName,string StuLoginPwd)
        {

            using (ExamDBEntities db=new ExamDBEntities())
            {
                var a = db.Student.Where(t => t.StuLoginName == StuLoginName).Where(t => t.StuLoginPwd == StuLoginPwd).FirstOrDefault();
                if (a!=null)
                {
                    Session["Student"] = a;
                    return RedirectToAction("Index", "Home");
                }
                return Content("<script>alert('账号或密码错误，请重新输入！');history.go(-1);</script>");
            }
        }
        //教师登录
        public ActionResult TeacherLogin()
        {
            return View();
        }
    }
}