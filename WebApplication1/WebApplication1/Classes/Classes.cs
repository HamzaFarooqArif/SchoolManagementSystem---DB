using System;
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
        public String ConnectionString = @"Data Source=(local);Initial Catalog=DB11V2;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
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
                    SemesterAction.Delete(sm.Name, sm.BatchID);
                }
                db.Batches.Remove(bt);
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
        public static bool Delete(string name, int batchID)
        {
            DB11V2Entities db = new DB11V2Entities();
            Semester sm = db.Semesters.Where(s=>s.Name.Equals(name) && s.BatchID == batchID).FirstOrDefault();
            if(sm != null)
            {
                db.Semesters.Remove(sm);
                db.SaveChanges();
                return true;
            }
            return false;
        }
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
    }
}