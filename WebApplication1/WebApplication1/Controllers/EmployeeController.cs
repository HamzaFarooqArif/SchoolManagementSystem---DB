using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Classes;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            DB11V2Entities db = new DB11V2Entities();

            List<Employee> employeeList = db.Employees.ToList();
            List<PersonEmployeeViewModels> personEmployeeList = new List<PersonEmployeeViewModels>();

            foreach (Employee e in employeeList)
            {
                PersonEmployeeViewModels pe = new PersonEmployeeViewModels();
                pe.ID = e.PersonID;
                pe.Designation = e.Designation;
                pe.Salary = e.Salary;

                Person pr = db.People.Where(p => p.ID == e.PersonID).FirstOrDefault();
                pe.Name = pr.Name;
                pe.FatherName = pr.FatherName;
                pe.CNIC = pr.CNIC;
                pe.Address = pr.Address;
                pe.Contact = pr.Contact;

                personEmployeeList.Add(pe);
            }
            return View(personEmployeeList);
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(PersonEmployeeViewModels collection)
        {
            if (!ModelState.IsValid) return View();
            int result = EmployeeAction.Create(collection);
            if (result == 1)
            {
                return RedirectToAction("Index");
            }
            else if(result == -1)
            {
                ViewBag.color = "red";
                ViewBag.message = "Employee Already Exists";
                return View();
            }
            else
            {
                ViewBag.color = "red";
                ViewBag.message = "Student Already Exists";
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            DB11V2Entities db = new DB11V2Entities();
            PersonEmployeeViewModels pe = new PersonEmployeeViewModels();

            Person pr = db.People.Where(p => p.ID == id).FirstOrDefault();
            pe.Name = pr.Name;
            pe.FatherName = pr.FatherName;
            pe.CNIC = pr.CNIC;
            pe.Address = pr.Address;
            pe.Contact = pr.Contact;

            Employee em = db.Employees.Where(e=>e.PersonID == id).FirstOrDefault();
            pe.Designation = em.Designation;
            pe.Salary = em.Salary;

            return View(pe);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PersonEmployeeViewModels collection)
        {
            bool result = EmployeeAction.Edit(id, collection);
            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.color = "red";
                ViewBag.message = "Person CNIC Already Exists";
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Delete/5
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
