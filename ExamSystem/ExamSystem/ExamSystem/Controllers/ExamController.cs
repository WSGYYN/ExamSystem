using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamSystem.Models;

namespace ExamSystem.Controllers
{
    public class ExamController : Controller
    {
        //在线考试功能
        //考试列表界面
        // GET: Exam

        //模型类公有
        private ExamDBEntities db = new ExamDBEntities();
        //试卷列表，考生选择试卷开始答题
        public ActionResult StartExamIndex()
        {
            var paperIndex = db.Paper.ToList();
            return View(paperIndex);
        }

        //答题界面
        public ActionResult ExamSystemList(int ? pid)
        {
            return View();
        }
        //提交考题数据
        [HttpPost]
        public ActionResult ExamSystemList(Answer answer)
        {
            return View();
        }


        //处理在线考试的数据
        //当点击开始考试的时候，添加考试记录
        public ActionResult BeginAnswer(int ? pid,int ? aid)
        {
            //获取考生信息
            var stu = Session["Student"] as Student;
            //添加一条记录
            var st = new Answer
            {
                StuID = stu.StuID,
                AnswerTime = DateTime.Now,
                AnswerState = 0,
                PaperID = Convert.ToInt32(pid),
                TeacherID = 1,
                AnswerScore = 0
            };
            //查询考生的答题状态：答题中，已答完。
            //答题中的考生点击后继续答题，会保留上次离开前的答题记录（实现：按时提交）
            var res = db.Answer.Where(t => t.StuID == stu.StuID && t.PaperID == pid).FirstOrDefault();
            //如果没有答题记录，则添加一条答题记录
            if(res ==null && aid == null)
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Entry(st).State = EntityState.Added;
                var top = db.Topic.Where(t => t.PaperID == pid).ToList();
                foreach (var item in top)
                {
                    db.Entry(new Detail
                    {
                        AnswerID = st.AnswerID,
                        TopicID = item.TopicID
                    }).State = EntityState.Added;
                }
                db.SaveChanges();
            }
            //如果有答题记录，则跳转到答题界面继续答题
            else
            {
                st.AnswerID = res.AnswerID;
                if (res.AnswerState != 0)
                {
                    return RedirectToAction("MyAnswerDetail", new { AnswersID = st.AnswerID });
                }
            }
            //跳转到答题视图
            return RedirectToAction("ExamSystemList",new { AnswerID = st.AnswerID });
        }
        //我的考试记录
        public ActionResult MyAnswer()
        {
            var student = Session["Student"] as Student;
            var answerList = db.Answer.Where(t => t.StuID == student.StuID).Include("Student").Include("Paper").ToList();
            return View(answerList);
        }
        //我的答题详情
        public ActionResult MyAnswerDetail()
        {
            return View();
        }
        //教师阅卷列表
        public ActionResult TeAnswer()
        {
			var da = db.Answer.Include("Student").Include("Paper").ToList();
			return View(da);
        }
        //教师阅卷模块
        public ActionResult TeAnswerDetail()
        {
            return View();
        }
        //阅卷功能，计算得分
        public ActionResult Verify(int ? id)
        {
            var da = db.Detail.Where(t => t.AnswerID == id).ToList();
            int sum = 0;
            foreach (var t in da)
            {
                if (t.DetailAnswer == t.Topic.TopicAnswer)
                {
                    sum += t.Topic.TopicScore;
                }
            }
            var dd = db.Answer.Find(id);
            dd.AnswerState = 2;
            dd.AnswerScore = sum;
            dd.BatchTime=DateTime.Now;
            db.Entry(dd).State=EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("TeAnswer");
        }
	}
}