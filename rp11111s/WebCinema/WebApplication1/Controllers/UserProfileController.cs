using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class UserProfileController : Controller
    {
        static WebsiteCinemaEntities db = new WebsiteCinemaEntities();
        // GET: UserProfile
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LoginHistory()
        {
            return View();
        }
        [HttpGet]
        public ActionResult TicketHistory()
        {
            return View();
        }
        [HttpGet]
        public ActionResult TicketHistoryDetail()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}