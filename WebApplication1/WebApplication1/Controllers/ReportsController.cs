using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ReportsController : Controller
    {
        public List<ReportViewModels> getReportList()
        {
            int numberOfReports = 11;
            ReportViewModels[] ReportsList = new ReportViewModels[numberOfReports];

            ReportsList[0] = new ReportViewModels(1, "Batch wise student report");
            ReportsList[1] = new ReportViewModels(2, "Course wise student report");
            ReportsList[2] = new ReportViewModels(3, "Batch wise attendance report");
            ReportsList[3] = new ReportViewModels(4, "Semester wise attendance report");
            ReportsList[4] = new ReportViewModels(5, "Course wise employee report");
            ReportsList[5] = new ReportViewModels(6, "Semester wise employee report");
            ReportsList[6] = new ReportViewModels(7, "Credit hr wise employee report");
            ReportsList[7] = new ReportViewModels(8, "Credit hr wise courses report");
            ReportsList[8] = new ReportViewModels(9, "Course wise result report");
            ReportsList[9] = new ReportViewModels(10, "Batch wise result report");
            ReportsList[10] = new ReportViewModels(11, "People report");

            List<ReportViewModels> resultList = new List<ReportViewModels>();
            for (int i = 0; i < numberOfReports; i++)
            {
                resultList.Add(ReportsList[i]);
            }

            return resultList;
        }
        public ActionResult Index()
        {
            List<ReportViewModels> result = getReportList();

            return View(result);
        }

        //public ActionResult Create()
        //{

        //}
    }
}