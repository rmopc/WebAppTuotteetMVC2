using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppTuotteetMVC.Models;

namespace WebAppTuotteetMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";
            }
            else ViewBag.LoggedStatus = Session["UserName"].ToString();
            ViewBag.LoginError = 0;
            return View();

        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult About()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";                
            }
            else ViewBag.LoggedStatus = Session["UserName"].ToString();
            return View();
        }

        public ActionResult Contact()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "Logged out";
            }
            else ViewBag.LoggedStatus = Session["UserName"].ToString();
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(Logins LoginModel)
        {
            TilausDBEntities db = new TilausDBEntities();           
            var LoggedUser = db.Logins.SingleOrDefault(x => x.UserName == LoginModel.UserName && x.PassWord == LoginModel.PassWord);
            if (LoggedUser != null)
            {
                ViewBag.LoginMessage = "Successfull login";
                ViewBag.LoggedStatus = "In";
                ViewBag.Loginerror = 0;
                Session["UserName"] = LoggedUser.UserName;
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                ViewBag.LoginMessage = "Login unsuccessfull";
                ViewBag.LoggedStatus = "Logged out";
                ViewBag.Loginerror = 1;
                LoginModel.LoginErrorMessage = "Unknown user or password.";
                return View("Index", LoginModel);
            }
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Logged out";
            return RedirectToAction("Index", "Home");
        }
    }
}