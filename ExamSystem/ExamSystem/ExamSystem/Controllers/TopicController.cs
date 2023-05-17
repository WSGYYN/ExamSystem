using ExamSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamSystem.Controllers
{
    public class TopicController : Controller
    {
        //题目管理
        // GET: Topic
        private ExamDBEntities db=new ExamDBEntities();

        //查看指定试卷的所有题目
        public ActionResult TopicIndex(int ? id)
        {
            var topicList = db.Topic.Where(t => t.PaperID == id).ToList();
            var paperList = db.Paper.Find(id);
            ViewBag.pid = paperList.PaperID;
            return View(topicList);
        }
        
        //功能：删除指定的题目
        public ActionResult TopicDelete(int ? id,int ? pid) 
        {
            //当这张试卷已经进行过考试则不能删除
            //已有考生作答记录则不能删除
            var IsExam = db.Detail.Where(t => t.TopicID == id).Count();
            if (IsExam > 0 )
            {
                return Content("<script>alert('该考题已经在某次考试中被使用，如要删除请清空当前考题的考试记录！');history.go(-1);</script>");
            }
            else
            {
                var topic = db.Topic.Find(id);
                if (topic != null)
                {
                    db.Topic.Remove(topic);
                    db.SaveChanges();

                }
                return RedirectToAction("TopicIndex", new {id=pid});
            }
        }
        //功能：添加考题
        public ActionResult TopicAdd(int ? pid)
        {
            var paperName = db.Paper.Find(pid);
            ViewBag.paperName = paperName.PaperName;
            ViewBag.pid = paperName.PaperID;
            return View();
        }
        [HttpPost]
        public ActionResult TopicAdd(Topic topic)
        {
            db.Topic.Add(topic);
            db.SaveChanges();
            if (db.Topic.Where(t => t.TopicExplain == topic.TopicExplain).Count() > 0)
            {
                return Content("false");
            }
            else
            {
                return Content("true");
            }
        }
        //功能：编辑/修改所选的题目
        public ActionResult TopicEdit(int ? id,int ? pid)
        {
            var topicList = db.Topic.Find(id);
            ViewBag.TopicID = topicList.TopicID;
            ViewBag.TopicExplain = topicList.TopicExplain;
            ViewBag.TopicScore = topicList.TopicScore;
            ViewBag.TopicType = topicList.TopicType;
            ViewBag.TopicA = topicList.TopicA;
            ViewBag.TopicB = topicList.TopicB;
            ViewBag.TopicC = topicList.TopicC;
            ViewBag.TopicD = topicList.TopicD;
            ViewBag.TopicSort = topicList.TopicSort;
            ViewBag.TopicAnswer = topicList.TopicAnswer;
            ViewBag.PaperID = topicList.PaperID;
            var paperList = db.Paper.Find(pid);
            ViewBag.paperName = paperList.PaperName;
            return View();
        }
        [HttpPost]
        public ActionResult TopicEdit(Topic topic) 
        {
            //模型验证通过，保存
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                return Content("true");
            }
            return Content("false");
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
    }
}