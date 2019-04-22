using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Classes;

namespace WebApplication1.Controllers
{
    public class SemesterController : Controller
    {
        public JsonResult LoadBatches()
        {
            DB11V2Entities db = new DB11V2Entities();
            //IEnumerable<GetProducts_Result> res = db.GetProducts();
            List<Batch> res = db.Batches.ToList();
            return Json(res.Select(x => new
            {
                ID = x.ID,
                Name = x.Session
            }));
        }

        public JsonResult LoadSemester(int item)
        {
            DB11V2Entities db = new DB11V2Entities();
            //IEnumerable<GetProducts_Result> res = db.GetProducts();
            List<Semester> res = db.Semesters.Where(s=>s.BatchID == item).ToList();
            return Json(res.Select(x => new
            {
                ID = x.ID,
                Name = x.Name
            }));
        }

        public JsonResult LoadSemesterCourses(int semesterID)
        {
            //IEnumerable<GetProducts_Result> res = db.GetProducts();
            List<Course> courseList = new List<Course>();
            SqlDataReader reader;
            string cmd = "SELECT * FROM Course WHERE Course.ID IN (SELECT CourseID FROM CourseSemester_MTM WHERE SemesterID = "+semesterID+")";
            reader = dbConnection.getInstance().getData(cmd);
            while(reader.Read())
            {
                Course c = new Course();
                c.ID = reader.GetInt32(0);
                c.Name = reader.GetString(1);
                courseList.Add(c);
            }
            return Json(courseList.Select(x => new
            {
                ID = x.ID,
                Name = x.Name
            }));
        }
        // GET: Semester
        public ActionResult Index()
        {
            return View();
        }

        // GET: Semester/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Semester/Create
        public ActionResult Create()
        {
            DB11V2Entities db = new DB11V2Entities();

            return View(db.Courses.ToList());
        }

        // POST: Semester/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Semester/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Semester/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Semester/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Semester/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
