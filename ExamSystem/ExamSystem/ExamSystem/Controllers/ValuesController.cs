using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using ExamSystem.Models;
using Newtonsoft.Json.Linq;

namespace ExamSystem.Controllers
{
    public class ValuesController : ApiController
    {
        private ExamDBEntities db = new ExamDBEntities();
        [Route("Teachers/GetAll")]
        [HttpGet]
        [HttpPost]
        public IHttpActionResult TeachersGetAll()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Ok(db.Teacher.ToList());
        }
        [Route("Topics/Get")]
        [HttpGet]
        [HttpPost]
        public IHttpActionResult TopicsGet(int id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Ok(db.Topic.Find(id));
        }
        [Route("Papers/GetAll")]
        [HttpGet]
        [HttpPost]
        public IHttpActionResult PapersGet()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Ok(db.Paper.ToList());
        }
        /// <summary>
        /// 审卷列表
        /// </summary>
        /// <returns></returns>
        [Route("Answers/GetAll")]
        [HttpPost]
        public IHttpActionResult AnswersGet()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var da = db.Answer.Include("Student").Include("Paper").ToList();
            return Ok(da);
        }
        [Route("AnswersApi/GetInfo")]
        [HttpPost]
        [HttpGet]
        public IHttpActionResult AnswersApiGet(int AnswerID)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var da = db.Answer.Include("Student").Include("Paper").Where(a => a.AnswerID == AnswerID).ToList();
            return Ok(da);
        }
        /// <summary>
        /// 答题详情
        /// </summary>
        /// <returns></returns>
        [Route("DetailApi/GetAll")]
        [HttpPost]
        [HttpGet]
        public IHttpActionResult DetailGet(int AnswerID)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var da = db.Detail.Include("Answer").Include("Topic").Where(a => a.AnswerID == AnswerID).ToList();
            return Ok(da);
        }
        /// <summary>
        /// 检查用户名是否重复
        /// </summary>
        /// <param name="AnswerID"></param>
        /// <returns></returns>
        [Route("Student/GetName")]
        [HttpGet]
        public IHttpActionResult StudentName(string StuLoginName)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var da = db.Student.Where(s => s.StuLoginName == StuLoginName).FirstOrDefault();
            var a = new { valid = true };

            if (da != null)
            {
                a = new { valid = false };
            }
            return Ok(a);
        }
        /// <summary>
        /// 查询用户登陆名是否存在
        /// </summary>
        /// <param name="obj">前端ajix传来的参数 </param>
        /// <returns></returns>
        [Route("UserName/GetName")]
        [HttpPost]
        public object StudentNamePost([FromBody] JObject obj)
        {
            db.Configuration.LazyLoadingEnabled = false;
            string sa = obj.Value<string>("StuLoginName");
            string sd = obj.Value<string>("TeacherLoginName");
            if (sa != null)
            {
                var da = db.Student.Where(s => s.StuLoginName == sa).FirstOrDefault();
                var a = new { valid = true };
                if (da != null)
                {
                    a = new { valid = false };
                }
                return a;
            }
            else
            {
                var da = db.Teacher.Where(s => s.TeacherLoginName == sd).FirstOrDefault();
                var a = new { valid = true };
                if (da != null)
                {
                    a = new { valid = false };
                }
                return a;
            }
        }
        /// <summary>
        /// 学生考试提交试卷
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [Route("Paper/GetSubmit")]
        [HttpPost]
        public IHttpActionResult PaperSubmit([FromBody] object obj)
        {
            var list = obj as List<Detail>;
            return RedirectToRoute("index", null);
        }

        /// <summary>
        /// 获取在线考试列表
        /// </summary>
        /// <returns></returns>
        [Route("Paper/GetAll")]
        [HttpGet]
        [HttpPost]
        public IHttpActionResult PaperList()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var da = db.Paper.ToList();

            return Ok(da);
        }

        /// <summary>
        /// 提交试卷
        /// </summary>
        /// <param name="Detail"></param>
        /// <returns></returns>
        [Route("Details/PostAll")]
        [HttpGet]
        [HttpPost]
        public bool AnswerDetail(JObject Detail)
        {
            using (ExamDBEntities db = new ExamDBEntities())
            {
                JavaScriptSerializer s = new JavaScriptSerializer();

                //拿到集合
                var d = Detail.GetValue("Detail");
                //得到考试记录ID
                int id = d.First().Value<JToken>("Answer").Value<int>("AnswerID");
                int type = Detail.GetValue("type").Value<int>();
                List<Detail> list = db.Detail.Where(a => a.AnswerID == id).ToList();

                if (list.Count == d.Count())
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].DetailAnswer = d[i].Value<string>("DetailAnswer");
                        list[i].Answer.AnswerState = type;
                    }
                db.SaveChanges();
                return true;
            }
        }

    }
}
