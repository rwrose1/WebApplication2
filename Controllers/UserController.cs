using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NazdaqSearch.Logic.UserLogic;
using System.Web.Mvc;
using NazdaqSearch.Models;
using NazdaqSearch.Logic.SQLLogic;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authenticator(userModel item)
        {

            userModel AuthenticatedUser = SQLHelper.ValidateUser(item);

            if (AuthenticatedUser != null)
            {
                InformationMethods.createUserData(item);
                return View("../Home/Index");
            }

            return RedirectToAction("loginError");
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUserAction(userModel item)
        {
            Console.WriteLine("\n\nUSER = " + item.UserID + " Password = " + item.Password + " \n\n");


            //if (SQLHelper.AddUser(item)) return RedirectToAction("createError");

            SQLHelper.AddUser(item);

            return View("../User/Login");
        }

        public ActionResult createError()
        {
            return View();
        }

        public ActionResult loginError()
        {
            return View();
        }

    }
}