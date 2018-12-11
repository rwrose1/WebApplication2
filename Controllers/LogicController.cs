using NazdaqSearch.Logic.HtmlParser;
using System.Collections.Generic;
using NazdaqSearch.Models;
using System.Web.Mvc;
using NazdaqSearch.Logic.SQLLogic;
using NazdaqSearch.Logic.UserLogic;

namespace WebApplication1.Controllers
{
    public class LogicController : Controller
    {

        public ActionResult Refresh()
        {

            List<NDData> toInsert = new List<NDData>();

            try
            {

                toInsert = HtmlParsing.getData();

            } catch
            {
                return RedirectToAction("ErrorPage", "Home");
            }
            foreach(NDData item in toInsert)
            {
                SQLHelper.Insert(item, InformationMethods.retrieveUserData());
            }

            return RedirectToAction("Index", "Home");
        }

    }
}