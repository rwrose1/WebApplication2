using NazdaqSearch.Logic.UserLogic;
using System.Collections.Generic;
using NazdaqSearch.Models;
using System.Web.Mvc;
using NazdaqSearch.Logic.SQLLogic;
using System;
using System.Diagnostics;
using NazdaqSearch.Logic.CSVConversions;

namespace NazdaqSearch.Controllers
{

    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {

            return View();

        }

        public ViewResult Contact() 
        {

            return View();

        }

        [HttpPost]
        public ActionResult SymbolDisplay(SymbolInputModel model) {

            List<NDData> display = null;

            String dateString = model.DateInput.ToString("MM/dd/yyyy");

            Debug.WriteLine("\n\n\n" + dateString + "\n\n\n");

            if ((model.SymbolInput == null || model.SymbolInput == "") && !dateString.Equals("01/01/0001"))
            {
                display = SQLHelper.GetAllWithDate(dateString, InformationMethods.retrieveUserData());
            }
            else if ((model.SymbolInput != null || model.SymbolInput != "") && dateString.Equals("01/01/0001"))
            {
                display = SQLHelper.GetAllWithSymbol(model.SymbolInput, InformationMethods.retrieveUserData());
            } else
            {
                display = SQLHelper.CompareData(SQLHelper.GetAllWithSymbol(model.SymbolInput, InformationMethods.retrieveUserData()), dateString);
            }

            if (display != null)
            {
                NazdaqCSV.dataToCSV(display);
                NazdaqCSV.convertCSVtoPDF(display);
            }

            return View(display);

        }

        public FileResult CsvDownload ()
        {

            var path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/data.csv");

            return File(path, "text/csv", "data.csv");
        }

        public FileResult PdfDownload()
        {

            var path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/datapdf.pdf");

            return File(path, "text/pdf", "datapdf.pdf");
        }

        public ActionResult ErrorPage()
        {


            return View();
        }

    }

}