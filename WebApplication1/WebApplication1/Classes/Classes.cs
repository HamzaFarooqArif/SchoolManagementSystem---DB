using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Classes
{
    public class connection
    {
        public static DB11V2Entities db = null;
        public static DB11V2Entities getDB()
        {
            if(db == null)
            {
                DB11V2Entities db = new DB11V2Entities();
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
            DB11V2Entities db = new DB11V2Entities();
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
}