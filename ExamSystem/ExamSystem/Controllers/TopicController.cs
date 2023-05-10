using ExamSystem.Models;
using System;
using System.Collections.Generic;
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
            var topic = db.Topic.ToList();
            return View();
        }
        
        //功能：删除指定的题目
        public ActionResult TopicDelete(int ? id) 
        {
            return View();
        }
        //功能：编辑/修改所选的题目
        public ActionResult TopicList(int? id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult TopicList(Topic topic) 
        {
            return View();  
        }
    }
}