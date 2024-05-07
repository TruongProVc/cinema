using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Areas.PrivateSite.Controllers
{
    public class DashboardController : Controller
    {
        // GET: PrivateSite/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}