using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BatchController : Controller
    {
        // GET: Batch
        public ActionResult Index()
        {
            return View();
        }

        // GET: Batch/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Batch/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Batch/Create
        [HttpPost]
        public ActionResult Create(BatchViewModels collection)
        {
            bool result = BatchAction.Create(collection);
            if(result)
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

        // GET: Batch/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Batch/Edit/5
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

        // GET: Batch/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Batch/Delete/5
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
