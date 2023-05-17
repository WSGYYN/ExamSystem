using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamSystem.Models;
using Microsoft.Ajax.Utilities;

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
		public ActionResult StuLogin(string StuLoginName, string StuLoginPwd)
		{

			using (ExamDBEntities db = new ExamDBEntities())
			{
				var a = db.Student.Where(t => t.StuLoginName == StuLoginName).Where(t => t.StuLoginPwd == StuLoginPwd).FirstOrDefault();
				if (a != null)
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
		[HttpPost]
		public ActionResult TeacherLogin(string TeacherLoginName, string TeacherLoginPwd)
		{
			using (ExamDBEntities db = new ExamDBEntities())
			{
				var a = db.Teacher.Where(t => t.TeacherLoginName == TeacherLoginName).Where(t => t.TeacherLoginPwd == TeacherLoginPwd).FirstOrDefault();
				if (a != null)
				{
					Session["Teacher"] = a;
					return RedirectToAction("Index", "Home");
				}
				return Content("<script>alert('账号或密码错误，请重新输入！');history.go(-1);</script>");
			}
		}
		//学生注册
		public ActionResult StuRegister()
		{
			return View();
		}
		[HttpPost]
		public ActionResult StuRegister(Student student)
		{
			using (ExamDBEntities db = new ExamDBEntities())
			{
				db.Student.Add(student);
				db.SaveChanges();
				if (db.Student.Where(t => t.StuLoginName == student.StuLoginName).Count() > 0)
				{
					return Content("<script>alert('注册成功，请返回登录页面登录');location.replace('/Login/StuLogin')</script>");
				}
				else
				{
					return Content("<script>alert('注册失败，请检查网络问题或联系管理员');history.go(-1);</script>");
				}
			}
		}
		//展示个人信息
		public ActionResult Realme(int? id)
		{
			using (ExamDBEntities db = new ExamDBEntities())
			{
				var liststu = db.Student.Find(id);
				return View(liststu);
			}
		}
		//修改个人信息界面
		public ActionResult EditReadme(int? id)
		{
			using (ExamDBEntities db = new ExamDBEntities())
			{
				var liststu = db.Student.Find(id);
				return View(liststu);
			}
		}
		//提交修改
		[HttpPost]
		public ActionResult EditReadme(Student student)
		{
			using (ExamDBEntities db = new ExamDBEntities())
			{
				//模型验证通过，保存
				if (ModelState.IsValid)
				{
					db.Entry(student).State = EntityState.Modified;
					db.SaveChanges();
				}
				//模型验证不通过，找错误
				if (!ModelState.IsValid)
				{
					foreach (var key in ModelState.Keys)
					{
						var errors = ModelState[key].Errors;
						foreach (var error in errors)
						{
							var errorMessage = error.ErrorMessage;
						}
					}
				}
			}
			//修改完成后，回到我的信息页面，传递一个参数
			return RedirectToAction("Realme", new { id = student.StuID });
		}








	}
}