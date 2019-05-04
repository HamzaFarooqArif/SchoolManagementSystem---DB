using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TimetableController : Controller
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

        public JsonResult LoadTimetables(int isDatesheet, int batchId)
        {
            DB11V2Entities db = new DB11V2Entities();
            //IEnumerable<GetProducts_Result> res = db.GetProducts();
            List<Semester> semesterList = db.Semesters.Where(temp => temp.BatchID == batchId).ToList();
            List<Timetable> res = new List<Timetable>();
            bool ind = false;
            if (isDatesheet == 0) ind = false;
            if (isDatesheet == 1) ind = true;

            foreach(Semester sm in semesterList)
            {
                if(db.Timetables.Any(temp=>temp.SemesterID == sm.ID && temp.IsDatesheet == ind))
                {
                    res.Add(db.Timetables.Where(temp => temp.SemesterID == sm.ID && temp.IsDatesheet == ind).FirstOrDefault());
                }
            }
            return Json(res.Select(x => new
            {
                ID = x.ID,
                Name = db.Semesters.Where(temp => temp.ID == x.SemesterID).FirstOrDefault().Name
        }));
        }
        public JsonResult LoadSemester(int item)
        {
            DB11V2Entities db = new DB11V2Entities();
            //IEnumerable<GetProducts_Result> res = db.GetProducts();
            List<Semester> res = db.Semesters.Where(s => s.BatchID == item).ToList();
            return Json(res.Select(x => new
            {
                ID = x.ID,
                Name = x.Name
            }));
        }
        public ActionResult PreviewIndex(int id)
        {
            return View();
        }
        // GET: Timetable
        public ActionResult Index()
        {
            return View();
        }

        // GET: Timetable/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Timetable/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Timetable/Create
        [HttpPost]
        public ActionResult Create(TimetableViewModels collection)
        {
            try
            {
                if (TimetableAction.Create(collection))
                {
                    ViewBag.color = "green";
                    ViewBag.message = "Added Successfully";
                    return View();
                }
                else
                {
                    ViewBag.color = "red";
                    ViewBag.message = "Already Exists";
                    return View();
                }
            }
            catch
            {
                ViewBag.color = "red";
                ViewBag.message = "Exception Catched";
                return View();
            }
        }

        // GET: Timetable/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Timetable/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, TimetableViewModels collection)
        {
            try
            {
                if (TimetableAction.Edit(id, collection))
                {
                    ViewBag.color = "green";
                    ViewBag.message = "Edited Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.color = "red";
                    ViewBag.message = "Already Exists";
                    return View();
                }
            }
            catch
            {
                ViewBag.color = "red";
                ViewBag.message = "Exception Catched";
                return View();
            }
        }

        // GET: Timetable/Delete/5
        public ActionResult Delete(int id)
        {
            DB11V2Entities db = new DB11V2Entities();

            int semesterid = db.Timetables.Where(temp=>temp.ID == id).FirstOrDefault().SemesterID;
            int batchid = db.Semesters.Where(temp => temp.ID == semesterid).FirstOrDefault().BatchID;

            TimetableViewModels ttvm = new TimetableViewModels();
            ttvm.ID = id;
            ttvm.Semester = db.Semesters.Where(temp => temp.ID == semesterid).FirstOrDefault().Name;
            ttvm.Batch = db.Batches.Where(temp => temp.ID == batchid).FirstOrDefault().Session;
            ttvm.isDatesheet = db.Timetables.Where(temp => temp.ID == id).FirstOrDefault().IsDatesheet.ToString();

            return View(ttvm);
        }

        // POST: Timetable/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            DB11V2Entities db = new DB11V2Entities();

            int semesterid = db.Timetables.Where(temp => temp.ID == id).FirstOrDefault().SemesterID;
            int batchid = db.Semesters.Where(temp => temp.ID == semesterid).FirstOrDefault().BatchID;

            TimetableViewModels ttvm = new TimetableViewModels();
            ttvm.ID = id;
            ttvm.Semester = db.Semesters.Where(temp => temp.ID == semesterid).FirstOrDefault().Name;
            ttvm.Batch = db.Batches.Where(temp => temp.ID == batchid).FirstOrDefault().Session;
            ttvm.isDatesheet = db.Timetables.Where(temp => temp.ID == id).FirstOrDefault().IsDatesheet.ToString();

            try
            {
                TimetableAction.Delete(id);
                ViewBag.color = "green";
                ViewBag.message = "Deleted Successfully";
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.color = "red";
                ViewBag.message = "Exception Catched";
                return View(ttvm);
            }
        }
    }
}
