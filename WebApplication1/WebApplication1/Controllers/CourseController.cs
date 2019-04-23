using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            DB11V2Entities1 db = new DB11V2Entities1();
            List<Course> result = db.Courses.ToList();
            return View(result);
        }

        // GET: Course/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        public ActionResult Create(CourseViewModels collection)
        {
            bool result = CourseAction.Create(collection);
            if (result)
            {
                ViewBag.color = "green";
                ViewBag.message = "Batch Added Successfully";
            }
            else
            {
                ViewBag.color = "red";
                ViewBag.message = "Batch Already Exists";
            }
            return View();
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Course/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CourseViewModels collection)
        {
            bool result = CourseAction.Edit(id, collection);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.color = "red";
                ViewBag.message = "Batch Already Exists";
                return View();
            }
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Course/Delete/5
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
