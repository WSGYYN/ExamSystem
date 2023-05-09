using ExamSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamSystem.Controllers
{
	//试卷管理
	public class PaperController : Controller
	{
		private ExamDBEntities db = new ExamDBEntities();
		// GET: Paper
		//展示所有试卷
		public ActionResult PaperIndex()
		{
			var paperList = db.Paper.ToList();
			return View(paperList);
		}
		public ActionResult EditPaper(int? id)
		{
			var EditPaper=db.Paper.Find(id);
			return View(EditPaper);
		}
		//修改试卷
		[HttpPost]
		public ActionResult EditPaper(Paper paper)
		{
			//模型验证通过，保存
			if (ModelState.IsValid)
			{
				db.Entry(paper).State = EntityState.Modified;
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
			return RedirectToAction("PaperIndex");
		}
		//添加考卷的视图
		public ActionResult AddPaper()
		{
			return View();
		}
		//添加试卷
		[HttpPost]
		public ActionResult AddPaper(Paper paper)
		{
			db.Paper.Add(paper);
			db.SaveChanges();
			if (db.Paper.Where(t => t.PaperName == paper.PaperName).Count() > 0)
			{
				return Content("ture");
			}
			else
			{
				return Content("false");
			}
			
		}
		//删除试卷
		public ActionResult DeletePaper(int ? id) 
		{

			//当这张试卷已经进行过考试则不能删除
			//已有考生作答记录则不能删除
			var IsExam = db.Answer.Where(t => t.PaperID == id).Count();
			var IsTopic = db.Topic.Where(t => t.TopicID == id).Count();
			if (IsExam > 0 && IsTopic >0)
			{
				return Content("<script>alert('此试卷已经进行过考试，如要删除请清空当前试卷的考试记录！');history.go(-1);</script>");
			}
			else
			{
				var paper=db.Paper.Find(id);
				if (paper != null)
				{
					db.Paper.Remove(paper);
					db.SaveChanges();

				}
				return RedirectToAction("PaperIndex");
			}
		}
	}
}