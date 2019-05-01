using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1;
using WebApplication1.Models;

namespace WebApplication1.Classes
{

    public class StudentAction
    {
        public static bool Create(StudentViewModels std)
        {
            DB11V2Entities1 db = new DB11V2Entities1();
            Person p = new Person();
            Student c = new Student();
            //c.BatchID = db.Batches.Where(b => b.ID == BatchID).FirstOrDefault().ID;
            //c.SemesterID= db.Semesters.Where(b => b.BatchID == c.BatchID).FirstOrDefault().ID;
            // c.BatchID = BatchID;
            // c.SemesterID = SemesterID;
            c.BatchID = std.Batch;
            c.SemesterID = std.Semester;
            p.Name = std.Name;
            p.FatherName = std.FatherName;
            p.CNIC = std.CNIC;
            p.Contact = std.Contact;
            p.Address = std.Address;
            c.RegNo = std.RegNo;
            c.Fee = std.Fee;
            if (!db.People.Any(b => b.CNIC.Equals(p.CNIC)) && !db.Students.Any(b => b.RegNo.Equals(c.RegNo) && b.BatchID == c.BatchID))
            {
                db.Students.Add(c);
                db.People.Add(p);
                db.SaveChanges();
                return true;
            }
            return false;


        }

        public static bool Edit(int id, StudentViewModels student)
        {
            DB11V2Entities1 db = new DB11V2Entities1();
            db.People.Find(id).Name = student.Name;
            db.People.Find(id).CNIC = student.CNIC;
            db.People.Find(id).Address = student.Address;
            db.People.Find(id).Contact = student.Contact;
            db.Students.Find(id).RegNo = student.RegNo;
            db.Students.Find(id).Fee = student.Fee;
            db.Students.Find(id).BatchID = student.Batch;
            db.Students.Find(id).SemesterID = student.Semester;
            if (!db.People.Any(b => b.CNIC == student.CNIC) && !db.Students.Any(b => b.RegNo == student.RegNo))
            {
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }


    public class ResultAction
    {
        public static bool Create(ResultViewModels result)
        {
            DB11V2Entities1 db = new DB11V2Entities1();
            Result r = new Result();
            r.StudentID = result.StudentID;
            r.CourseSemesterID = db.CourseSemester_MTM.Where(b => b.CourseID == result.courseID && b.SemesterID == result.semesterID).FirstOrDefault().ID;
            r.ObtainedMarks = result.ObtainedMarks;
            r.TotalMarks = result.TotalMarks;
            if (!db.Results.Any(b => b.StudentID == r.StudentID && b.CourseSemesterID == r.CourseSemesterID))
            {
                db.Results.Add(r);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        public static bool Edit(int id, ResultViewModels r)
        {
            DB11V2Entities1 db = new DB11V2Entities1();
            int c = db.Results.Where(b => b.ID == id).SingleOrDefault().CourseSemesterID;
            int s = db.Results.Where(b => b.ID == id).SingleOrDefault().StudentID;
            db.Results.Find(id).CourseSemesterID = c;
            db.Results.Find(id).StudentID = s;
            db.Results.Find(id).TotalMarks = r.TotalMarks;
            db.Results.Find(id).ObtainedMarks = r.ObtainedMarks;
            int t = Convert.ToInt32(r.TotalMarks);
            int o = Convert.ToInt32(r.ObtainedMarks);
            if (t >= o)
            {
                db.SaveChanges();
                return true;
            }
            return false;

        }


        public static bool Delete(int id, ResultViewModels result)
        {
            try
            {
                DB11V2Entities1 db = new DB11V2Entities1();
                foreach (Result r in db.Results)
                {
                    if (r.ID == id)
                    {
                        db.Results.Remove(r);
                    }
                }
                db.SaveChanges();
                return true;
                
            }
           
            catch(Exception e)
            {
                throw (e);
            }
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
                if (!db.People.Any(b => b.CNIC.Equals(p.CNIC)))
                {
                    db.Employees.Add(e);
                    db.People.Add(p);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }



        public class BatchAction
        {
            public static bool Create(BatchViewModels model)
            {
                Batch modelA = new Batch();
                modelA.Session = model.BatchName;
                DB11V2Entities1 db = new DB11V2Entities1();
                if (!db.Batches.Any(b => b.Session.Equals(modelA.Session)))
                {
                    db.Batches.Add(modelA);
                    db.SaveChanges();

                    Batch bt = db.Batches.Where(b => b.Session.Equals(modelA.Session)).FirstOrDefault();

                    for (int i = 0; i < 8; i++)
                    {
                        SemesterAction.Create((i + 1).ToString(), bt.ID);
                    }
                    return true;
                }
                return false;
            }
            public static bool Edit(int id, BatchViewModels model)
            {
                DB11V2Entities1 db = new DB11V2Entities1();
                if (db.Batches.Any(b => b.Session.Equals(model.BatchName)))
                {
                    return false;
                }
                db.Batches.Where(b => b.ID == id).FirstOrDefault().Session = model.BatchName;
                db.SaveChanges();
                return true;
            }
            //public static bool Delete(int id)
            //{
            //    DB11V2Entities1 db = new DB11V2Entities1();

            //    Batch bt = db.Batches.Where(b => b.ID == id).FirstOrDefault();
            //    if(bt != null)
            //    {
            //        List<Semester> semesterList = db.Semesters.Where(s => s.BatchID == bt.ID).ToList();
            //        foreach (Semester sm in semesterList)
            //        {
            //            SemesterAction.Delete(sm.Name, sm.BatchID);
            //        }
            //        db.Batches.Remove(bt);
            //        db.SaveChanges();
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
        }

        public class SemesterAction
        {
            public static bool Create(string name, int batchID)
            {
                DB11V2Entities1 db = new DB11V2Entities1();
                Semester sm = new Semester();
                sm.Name = name;
                sm.BatchID = batchID;
                if (db.Semesters.Any(s => s.Name.Equals(name) && s.BatchID == batchID))
                {
                    return false;
                }
                db.Semesters.Add(sm);
                db.SaveChanges();
                return true;
            }
            public static bool Delete(string name, int batchID)
            {
                DB11V2Entities1 db = new DB11V2Entities1();
                Semester sm = db.Semesters.Where(s => s.Name.Equals(name) && s.BatchID == batchID).FirstOrDefault();
                if (sm != null)
                {
                    db.Semesters.Remove(sm);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

    }
    public class CourseAction
    {
        public static bool Create(CourseViewModels model)
        {
            Course modelA = new Course();
            modelA.Name = model.CourseName;
            DB11V2Entities1 db = new DB11V2Entities1();
            if (!db.Courses.Any(c => c.Name.Equals(modelA.Name)))
            {
                db.Courses.Add(modelA);
                db.SaveChanges();

                return true;
            }
            return false;
        }
        public static bool Edit(int id, CourseViewModels model)
        {
            // DB11V2Entities1 
            DB11V2Entities1 db = new DB11V2Entities1();
            if (db.Courses.Any(b => b.Name.Equals(model.CourseName)))
            {
                return false;
            }
            db.Courses.Where(c => c.ID == id).FirstOrDefault().Name = model.CourseName;
            db.SaveChanges();
            return true;
        }
    }

