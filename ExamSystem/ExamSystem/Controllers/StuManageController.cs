using ExamSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamSystem.Controllers
{
    public class StuManageController : Controller
    {

        //将模型公开
        private ExamDBEntities db = new ExamDBEntities();

        //教师功能模块，学生管理
        // GET: StuManage
        //展示所有学生
        public ActionResult StuTable()
        {
            var stuList = db.Student.ToList();
            return View(stuList);
        }

        //删除指定的学生
        public ActionResult StuDelete(int? id)
        {
            //当一个学生进行过考试，提醒无法删除，保持数据完整性
            var IsExam = db.Answer.Where(t => t.PaperID == id).Count();
            if (IsExam > 0)
            {
				return Content("<script>alert('此考生已经进行过考试，如要删除：请清空当前考生的考试记录！');history.go(-1);</script>");
            }
            else
            {
                var student = db.Student.Find(id);
                if (student != null)
                {
                    db.Student.Remove(student);
                    db.SaveChanges();
                    
                }
                return RedirectToAction("StuTable");
            }
            
        }
    }
}