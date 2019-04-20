using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using WebApplication1.Models;

namespace WebApplication1.Classes
{
    public class connection
    {
        public static DB11V2Entities1 db = null;
        public static DB11V2Entities1 getDB()
        {
            if(db == null)
            {
                DB11V2Entities1 db = new DB11V2Entities1();
            }
            return db;
        }
    }

    public class BatchAction
    {
        public static bool Create(BatchViewModels model)
        {
            Batch modelA = new Batch();
            modelA.Session = model.BatchName;
            DB11V2Entities1 db = new DB11V2Entities1();
            if(!db.Batches.Any(b=>b.Session.Equals(modelA.Session)))
            {
                db.Batches.Add(modelA);
                db.SaveChanges();

                Batch bt = db.Batches.Where(b => b.Session.Equals(modelA.Session)).FirstOrDefault();

                for (int i = 0; i < 8; i++)
                {
                    Semester sm = new Semester();
                    sm.Name = (i + 1).ToString();
                    sm.BatchID = bt.ID;

                    db.Semesters.Add(sm);
                    db.SaveChanges();
                }
                return true;
            }
            return false;
        }
    }

    public class EmployeeAction
    {
        public static bool Create(EmployeeViewModels employee)
        {
            Employee e = new Employee();
            Person p = new Person();
            DB11V2Entities1 db = new DB11V2Entities1();
            p.Name = employee.Name;
            p.FatherName = employee.FatherName;
            p.CNIC = employee.CNIC;
            p.Contact = employee.Contact;
            p.Address = employee.Address;
            e.Salary = employee.Salary;
            e.Designation = employee.Designation;
            if(!db.People.Any(b=>b.CNIC.Equals(p.CNIC)))
            {
                db.Employees.Add(e);
                db.People.Add(p);
                db.SaveChanges();
                return true;
            }
            return false;
            }
        }

    public class StudentAction
    {
        //public static bool Create (StudentViewModels student)
        //{

        //}
    }
    public class CourseAction
    {
        public static bool Create (CoursesViewModels course)
        {
            DB11V2Entities1 db = new DB11V2Entities1();
            Course c = new Course();
            c.Name = course.Name;
            if(!db.Courses.Any(b=>b.Name.Equals(c.Name)))
            {
                db.Courses.Add(c);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}