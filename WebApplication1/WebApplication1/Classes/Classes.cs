﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Classes
{
    public class dbConnection
    {
        private static dbConnection instance;
        public String ConnectionString = @"Data Source=AYESHA\MSSQLSERVER2;Initial Catalog=DB11V2;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
        private SqlConnection connection;

        /// <summary>
        /// Allows to make only one object of class.
        /// </summary>
        /// <returns> Instance of class. </returns>
        public static dbConnection getInstance()
        {
            if (instance == null)
                instance = new dbConnection();
            return instance;
        }

        private dbConnection()
        {

        }

        /// <summary>
        /// Opens SQL Connection
        /// </summary>
        /// <returns> SQL connection object </returns>
        public SqlConnection getConnection()
        {
            connection = new SqlConnection(ConnectionString);
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            return connection;
        }

        /// <summary>
        /// Reads the data from SQL database.
        /// </summary>
        /// <param name="commnadText"> String in string.Format style. It is the string that goes in query. </param>
        /// <returns> SQL Reader Object </returns>
        public SqlDataReader getData(String commnadText)
        {
            connection = getConnection();
            SqlCommand cmd = new SqlCommand(commnadText, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        public int getScalerData(String commnadText)
        {
            connection = getConnection();
            SqlCommand cmd = new SqlCommand(commnadText, connection);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            return i;
            //return reader;
        }

        /// <summary>
        /// Executes the commands in query and calculates number of rows effected.
        /// </summary>
        /// <param name="commnadText"> String in string.Format style. It is the string that goes in query. </param>
        /// <returns> number of affected rows. </returns>
        public int exectuteQuery(String commnadText)
        {
            connection = getConnection();
            SqlCommand cmd = new SqlCommand(commnadText, connection);
            int rows = cmd.ExecuteNonQuery();
            return rows;
        }

        /// <summary>
        /// Close the connection.
        /// </summary>
        public void closeConnection()
        {
            if (connection != null)
            {
                connection.Close();
            }

        }
    }
    public class BatchAction
    {
        public static bool Create(BatchViewModels model)
        {
            Batch modelA = new Batch();
            modelA.Session = model.BatchName;
            DB11V2Entities db = new DB11V2Entities();
            if(!db.Batches.Any(b=>b.Session.Equals(modelA.Session)))
            {
                db.Batches.Add(modelA);
                db.SaveChanges();

                Batch bt = db.Batches.Where(b => b.Session.Equals(modelA.Session)).FirstOrDefault();

                for (int i = 0; i < 8; i++)
                {
                    SemesterAction.Create((i+1).ToString(), bt.ID);
                }
                return true;
            }
            return false;
        }
        public static bool Edit(int id, BatchViewModels model)
        {
            DB11V2Entities db = new DB11V2Entities();
            if(db.Batches.Any(b=>b.Session.Equals(model.BatchName)))
            {
                return false;
            }
            db.Batches.Where(b => b.ID == id).FirstOrDefault().Session = model.BatchName;
            db.SaveChanges();
            return true;
        }
        public static bool Delete(int id)
        {
            DB11V2Entities db = new DB11V2Entities();

            Batch bt = db.Batches.Where(b => b.ID == id).FirstOrDefault();
            if(bt != null)
            {
                List<Semester> semesterList = db.Semesters.Where(s => s.BatchID == bt.ID).ToList();
                foreach (Semester sm in semesterList)
                {
                    db.Entry(sm).State = System.Data.Entity.EntityState.Deleted;
                }
                db.Entry(bt).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class SemesterAction
    {
        public static bool Create(string name, int batchID)
        {
            DB11V2Entities db = new DB11V2Entities();
            Semester sm = new Semester();
            sm.Name = name;
            sm.BatchID = batchID;
            if(db.Semesters.Any(s=>s.Name.Equals(name) && s.BatchID == batchID))
            {
                return false;
            }
            db.Semesters.Add(sm);
            db.SaveChanges();
            return true;
        }
        //public static bool Delete(string name, int batchID)
        //{
        //    DB11V2Entities db = new DB11V2Entities();
        //    Semester sm = db.Semesters.Where(s=>s.Name.Equals(name) && s.BatchID == batchID).FirstOrDefault();
        //    if(sm != null)
        //    {
        //        db.Entry(sm).State = System.Data.Entity.EntityState.Deleted;
        //        db.SaveChanges();
        //        return true;
        //    }
        //    return false;
        //}
    }
    public class CourseAction
    {
        public static bool Create(CourseViewModels model)
        {
            Course modelA = new Course();
            modelA.Name = model.CourseName;
            DB11V2Entities db = new DB11V2Entities();
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
            DB11V2Entities db = new DB11V2Entities();
            if (db.Courses.Any(b => b.Name.Equals(model.CourseName)))
            {
                return false;
            }
            db.Courses.Where(c => c.ID == id).FirstOrDefault().Name = model.CourseName;
            db.SaveChanges();
            return true;
        }
        public static bool Delete(int id)
        {
            DB11V2Entities db = new DB11V2Entities();

            Course cr = db.Courses.Where(c => c.ID == id).FirstOrDefault();
            if (cr != null)
            {
                db.Entry(cr).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public class EmployeeAction
    {
        public static int Create(PersonEmployeeViewModels model)
        {
            DB11V2Entities db = new DB11V2Entities();

            Person existingPerson = db.People.Where(p => p.CNIC == model.CNIC).FirstOrDefault();
            if (existingPerson != null)
            {
                if (db.Employees.Any(e => e.PersonID == existingPerson.ID)) return -1;
                if (db.Students.Any(s => s.PersonID == existingPerson.ID)) return -2;
            }

            Person person = new Person();
            person.Name = model.Name;
            person.FatherName = model.FatherName;
            person.CNIC = model.CNIC;
            person.Address = model.Address;
            person.Contact = model.Contact;
            db.People.Add(person);
            db.SaveChanges();

            Employee employee = new Employee();
            employee.PersonID = db.People.Where(p => p.CNIC == model.CNIC).FirstOrDefault().ID;
            employee.Designation = model.Designation;
            employee.Salary = model.Salary;
            db.Employees.Add(employee);
            db.SaveChanges();

            return 1;
        }
        public static bool Edit(int id, PersonEmployeeViewModels model)
        {
            DB11V2Entities db = new DB11V2Entities();
            if (db.People.Any(p => p.CNIC.Equals(model.CNIC) && p.ID != id))
            {
                return false;
            }

            db.People.Where(p => p.ID == id).FirstOrDefault().Name = model.Name;
            db.People.Where(p => p.ID == id).FirstOrDefault().FatherName = model.FatherName;
            db.People.Where(p => p.ID == id).FirstOrDefault().CNIC = model.CNIC;
            db.People.Where(p => p.ID == id).FirstOrDefault().Address = model.Address;
            db.People.Where(p => p.ID == id).FirstOrDefault().Contact = model.Contact;

            db.Employees.Where(e => e.PersonID == id).FirstOrDefault().Designation = model.Designation;
            db.Employees.Where(e => e.PersonID == id).FirstOrDefault().Salary = model.Salary;

            db.SaveChanges();
            return true;
        }
        public static bool Delete(int id)
        {
            DB11V2Entities db = new DB11V2Entities();

            Employee em = db.Employees.Where(e => e.PersonID == id).FirstOrDefault();
            db.Entry(em).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            //db.Course
            //db.CourseSemester_MTM.Remove(cs);
            //db.Entry(cs).State = System.Data.Entity.EntityState.Deleted;
            //db.SaveChanges();
            return true;
        }
    }
    public class CourseSemesterAction
    {
        public static bool Create(int sid, int cid)
        {
            DB11V2Entities db = new DB11V2Entities();

            CourseSemester_MTM cs = new CourseSemester_MTM();
            cs.SemesterID = sid;
            cs.CourseID = cid;

            if (db.CourseSemester_MTM.Any(c => c.CourseID == cid && c.SemesterID == sid)) return false;

            db.CourseSemester_MTM.Add(cs);
            db.SaveChanges();

            return true;
        }

        public static bool Edit(int sid, int cid, Course collection)
        {
            DB11V2Entities db = new DB11V2Entities();

            if (db.CourseSemester_MTM.Any(cs => cs.SemesterID == sid && cs.CourseID == collection.ID)) return false;

            CourseSemester_MTM csm = new CourseSemester_MTM();
            csm.SemesterID = sid;
            csm.CourseID = collection.ID;

            db.CourseSemester_MTM.Where(cs => cs.SemesterID == sid && cs.CourseID == cid).FirstOrDefault().CourseID = collection.ID;
            db.SaveChanges();

            return true;
        }

        public static bool Delete(int sid, int cid)
        {
            DB11V2Entities db = new DB11V2Entities();

            CourseSemester_MTM cs = new CourseSemester_MTM();
            cs.ID = db.CourseSemester_MTM.Where(csme => csme.SemesterID == sid && csme.CourseID == cid).FirstOrDefault().ID;
            cs.SemesterID = sid;
            cs.CourseID = cid;

            //db.Course
            //db.CourseSemester_MTM.Remove(cs);
            db.Entry(cs).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return true;
        }
    }
    public class StudentAction
    {
        public static bool Create(StudentViewModels std)
        {
            DB11V2Entities db = new DB11V2Entities();
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
            DB11V2Entities db = new DB11V2Entities();
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
            DB11V2Entities db = new DB11V2Entities();
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
            DB11V2Entities db = new DB11V2Entities();
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
                DB11V2Entities db = new DB11V2Entities();
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

            catch (Exception e)
            {
                throw (e);
            }
        }
    }
    public class EmployeeCourseSemesterAction
    {
        public static bool Create(int id, EmployeeCourseSemesterViewModels model)
        {
            DB11V2Entities db = new DB11V2Entities();

            int courseid = Int32.Parse(model.Course);
            int semesterid = Int32.Parse(model.Semester);
            int csid = db.CourseSemester_MTM.Where(temp => temp.SemesterID == semesterid && temp.CourseID == courseid).FirstOrDefault().ID;

            if (db.EmployeeCourseSemester_MTM.Any(temp => temp.EmployeeID == id && temp.CourseSemesterID == csid)) return false;

            EmployeeCourseSemester_MTM ecs = new EmployeeCourseSemester_MTM();
            ecs.EmployeeID = id;
            ecs.CourseSemesterID = csid;

            db.EmployeeCourseSemester_MTM.Add(ecs);
            db.SaveChanges();

            return true;
        }
        public static bool Edit(int id, EmployeeCourseSemesterViewModels model)
        {
            DB11V2Entities db = new DB11V2Entities();

            
            int courseid = Int32.Parse(model.Course);
            int semesterid = Int32.Parse(model.Semester);
            int csid = db.CourseSemester_MTM.Where(temp => temp.SemesterID==semesterid && temp.CourseID == courseid).FirstOrDefault().ID;
            int employeeid = db.EmployeeCourseSemester_MTM.Where(temp => temp.ID == id).FirstOrDefault().EmployeeID;

            bool result = db.EmployeeCourseSemester_MTM.Any(temp => temp.EmployeeID == employeeid && temp.CourseSemesterID == csid);
            if (result) return false;

            db.EmployeeCourseSemester_MTM.Where(temp=>temp.ID == id).FirstOrDefault().CourseSemesterID = csid;
            db.SaveChanges();

            return true;
        }
        public static bool Delete(int id)
        {
            DB11V2Entities db = new DB11V2Entities();
            EmployeeCourseSemester_MTM obj = db.EmployeeCourseSemester_MTM.Where(temp => temp.ID == id).FirstOrDefault();
            db.EmployeeCourseSemester_MTM.Remove(obj);
            db.SaveChanges();
            return true;
        }
    }
    public class TimetableAction
    {
        public static bool Create(TimetableViewModels model)
        {
            DB11V2Entities db = new DB11V2Entities();

            int batchID = Int32.Parse(model.Batch);
            int semesterID = db.Semesters.Where(temp => temp.Name.Equals(model.Semester) && temp.BatchID == batchID).FirstOrDefault().ID;
            int tmpIsDatesheet = Int32.Parse(model.isDatesheet);
            bool isDatesheet = false;
            if (tmpIsDatesheet == 0) isDatesheet = false;
            if (tmpIsDatesheet == 1) isDatesheet = true;

            if (db.Timetables.Any(temp => temp.SemesterID == semesterID && temp.IsDateSheet == isDatesheet)) return false;

            Timetable tt = new Timetable();
            tt.SemesterID = semesterID;
            tt.IsDateSheet = isDatesheet;

            db.Timetables.Add(tt);
            db.SaveChanges();

            return true;
        }

        public static bool Edit(int id, TimetableViewModels model)
        {
            DB11V2Entities db = new DB11V2Entities();

            int batchID = Int32.Parse(model.Batch);
            int semesterID = db.Semesters.Where(temp => temp.Name.Equals(model.Semester) && temp.BatchID == batchID).FirstOrDefault().ID;
            int tmpIsDatesheet = Int32.Parse(model.isDatesheet);
            bool isDatesheet = false;
            if (tmpIsDatesheet == 0) isDatesheet = false;
            if (tmpIsDatesheet == 1) isDatesheet = true;

            if (db.Timetables.Any(temp => temp.SemesterID == semesterID && temp.IsDateSheet == isDatesheet)) return false;

            db.Timetables.Where(temp => temp.ID == id).FirstOrDefault().SemesterID = semesterID;
            db.Timetables.Where(temp => temp.ID == id).FirstOrDefault().IsDateSheet = isDatesheet;
            db.SaveChanges();

            return true;
        }
        public static bool Delete(int id)
        {
            DB11V2Entities db = new DB11V2Entities();
            Timetable obj = db.Timetables.Where(temp => temp.ID == id).FirstOrDefault();
            db.Timetables.Remove(obj);
            db.SaveChanges();
            return true;
        }

        public static int TimeslotCreate(TimeslotViewModels model)
        {
            DB11V2Entities db = new DB11V2Entities();

            if (!db.Timetables.Any(temp => temp.ID == model.TimetableID)) return -1;

            List<TimetableTimeslot_MTM> timetableTimeslotList = db.TimetableTimeslot_MTM.Where(temp => temp.TimetableID == model.TimetableID).ToList();
            List<Timeslot> timeslotList = new List<Timeslot>();
            foreach(TimetableTimeslot_MTM tttsmtm in timetableTimeslotList)
            {
                timeslotList.Add(db.Timeslots.Where(temp=>temp.ID == tttsmtm.TimeslotID).FirstOrDefault());
            }
            
            CourseSemester_MTM cs = db.CourseSemester_MTM.Where(temp => temp.CourseID == model.CourseID && temp.SemesterID == model.SemesterID).FirstOrDefault();
            EmployeeCourseSemester_MTM ecs = db.EmployeeCourseSemester_MTM.Where(temp => temp.CourseSemesterID == cs.ID && temp.EmployeeID == model.EmployeeID).FirstOrDefault();
            Timeslot ts = new Timeslot();
            ts.EmployeeCourseSemesterID = ecs.ID;
            ts.WorkingDaysID = model.WorkingDayID;

            if (db.Timetables.Where(temp => temp.ID == model.TimetableID).FirstOrDefault().IsDateSheet == true)
            {
                ts.IsExam = true;
            }
            else ts.IsExam = false;

            //-------------------------------------------------------------------
            if (model.StartEndTime == 1)
            {
                ts.StartTime = new DateTime(2000, 1, 1, 8, 0, 0);
                ts.EndTime = new DateTime(2000, 1, 1, 9, 0, 0);
            }
            if (model.StartEndTime == 2)
            {
                ts.StartTime = new DateTime(2000, 1, 1, 9, 0, 0);
                ts.EndTime = new DateTime(2000, 1, 1, 10, 0, 0);
            }
            if (model.StartEndTime == 3)
            {
                ts.StartTime = new DateTime(2000, 1, 1, 10, 0, 0);
                ts.EndTime = new DateTime(2000, 1, 1, 11, 0, 0);
            }
            if (model.StartEndTime == 4)
            {
                ts.StartTime = new DateTime(2000, 1, 1, 11, 0, 0);
                ts.EndTime = new DateTime(2000, 1, 1, 12, 0, 0);
            }
            if (model.StartEndTime == 5)
            {
                ts.StartTime = new DateTime(2000, 1, 1, 1, 0, 0);
                ts.EndTime = new DateTime(2000, 1, 1, 2, 0, 0);
            }
            if (model.StartEndTime == 6)
            {
                ts.StartTime = new DateTime(2000, 1, 1, 2, 0, 0);
                ts.EndTime = new DateTime(2000, 1, 1, 3, 0, 0);
            }
            if (model.StartEndTime == 7)
            {
                ts.StartTime = new DateTime(2000, 1, 1, 3, 0, 0);
                ts.EndTime = new DateTime(2000, 1, 1, 4, 0, 0);
            }
            //-------------------------------------------------------------------
            foreach (Timeslot tst in timeslotList)
            {
                if (tst.EmployeeCourseSemesterID == ts.EmployeeCourseSemesterID && tst.StartTime == ts.StartTime && tst.EndTime == ts.EndTime && tst.WorkingDaysID == ts.WorkingDaysID && tst.IsExam == ts.IsExam) return -2;
            }
            db.Timeslots.Add(ts);
            db.SaveChanges();

            int tsid = db.Timeslots.Where(temp => temp.EmployeeCourseSemesterID == ts.EmployeeCourseSemesterID && temp.StartTime == ts.StartTime && temp.EndTime == ts.EndTime && temp.WorkingDaysID == ts.WorkingDaysID && temp.IsExam == ts.IsExam).FirstOrDefault().ID;
            TimetableTimeslot_MTM ttts = new TimetableTimeslot_MTM();
            ttts.TimeslotID = tsid;
            ttts.TimetableID = model.TimetableID;
            db.TimetableTimeslot_MTM.Add(ttts);
            db.SaveChanges();

            return 1;
        }
        public static int TimeslotEdit(int timeslotID, TimeslotViewModels model)
        {
            DB11V2Entities db = new DB11V2Entities();

            if (!db.Timetables.Any(temp => temp.ID == model.TimetableID)) return -1;

            List<TimetableTimeslot_MTM> timetableTimeslotList = db.TimetableTimeslot_MTM.Where(temp => temp.TimetableID == model.TimetableID).ToList();
            List<Timeslot> timeslotList = new List<Timeslot>();
            foreach (TimetableTimeslot_MTM tttsmtm in timetableTimeslotList)
            {
                timeslotList.Add(db.Timeslots.Where(temp => temp.ID == tttsmtm.TimeslotID).FirstOrDefault());
            }

            CourseSemester_MTM cs = db.CourseSemester_MTM.Where(temp => temp.CourseID == model.CourseID && temp.SemesterID == model.SemesterID).FirstOrDefault();
            EmployeeCourseSemester_MTM ecs = db.EmployeeCourseSemester_MTM.Where(temp => temp.CourseSemesterID == cs.ID && temp.EmployeeID == model.EmployeeID).FirstOrDefault();
            Timeslot ts = new Timeslot();
            ts.EmployeeCourseSemesterID = ecs.ID;
            ts.WorkingDaysID = model.WorkingDayID;

            if (db.Timetables.Where(temp => temp.ID == model.TimetableID).FirstOrDefault().IsDateSheet == true)
            {
                ts.IsExam = true;
            }
            else ts.IsExam = false;

            //-------------------------------------------------------------------
            if (model.StartEndTime == 1)
            {
                ts.StartTime = new DateTime(2000, 1, 1, 8, 0, 0);
                ts.EndTime = new DateTime(2000, 1, 1, 9, 0, 0);
            }
            if (model.StartEndTime == 2)
            {
                ts.StartTime = new DateTime(2000, 1, 1, 9, 0, 0);
                ts.EndTime = new DateTime(2000, 1, 1, 10, 0, 0);
            }
            if (model.StartEndTime == 3)
            {
                ts.StartTime = new DateTime(2000, 1, 1, 10, 0, 0);
                ts.EndTime = new DateTime(2000, 1, 1, 11, 0, 0);
            }
            if (model.StartEndTime == 4)
            {
                ts.StartTime = new DateTime(2000, 1, 1, 11, 0, 0);
                ts.EndTime = new DateTime(2000, 1, 1, 12, 0, 0);
            }
            if (model.StartEndTime == 5)
            {
                ts.StartTime = new DateTime(2000, 1, 1, 1, 0, 0);
                ts.EndTime = new DateTime(2000, 1, 1, 2, 0, 0);
            }
            if (model.StartEndTime == 6)
            {
                ts.StartTime = new DateTime(2000, 1, 1, 2, 0, 0);
                ts.EndTime = new DateTime(2000, 1, 1, 3, 0, 0);
            }
            if (model.StartEndTime == 7)
            {
                ts.StartTime = new DateTime(2000, 1, 1, 3, 0, 0);
                ts.EndTime = new DateTime(2000, 1, 1, 4, 0, 0);
            }
            //-------------------------------------------------------------------
            //foreach (Timeslot tst in timeslotList)
            //{
            //    if (tst.EmployeeCourseSemesterID == ts.EmployeeCourseSemesterID && tst.StartTime == ts.StartTime && tst.EndTime == ts.EndTime && tst.WorkingDaysID == ts.WorkingDaysID && tst.IsExam == ts.IsExam) return -2;
            //}
            db.Timeslots.Where(temp => temp.ID == timeslotID).FirstOrDefault().EmployeeCourseSemesterID = ts.EmployeeCourseSemesterID;
            db.SaveChanges();

            //int tsid = db.Timeslots.Where(temp => temp.EmployeeCourseSemesterID == ts.EmployeeCourseSemesterID && temp.StartTime == ts.StartTime && temp.EndTime == ts.EndTime && temp.WorkingDaysID == ts.WorkingDaysID && temp.IsExam == ts.IsExam).FirstOrDefault().ID;
            //TimetableTimeslot_MTM ttts = new TimetableTimeslot_MTM();
            //ttts.TimeslotID = tsid;
            //ttts.TimetableID = model.TimetableID;
            //db.TimetableTimeslot_MTM.Add(ttts);
            //db.SaveChanges();

            return 1;
        }
        public static bool TimeslotDelete(int timeslotID, TimeslotViewModels model)
        {
            DB11V2Entities db = new DB11V2Entities();
            Timeslot obj = db.Timeslots.Where(temp => temp.ID == timeslotID).FirstOrDefault();
            db.Timeslots.Remove(obj);
            db.SaveChanges();
            return true;
        }
    }
}