using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        //[CustomAuthentication]
        //[CustomAuthorize(Roles = "người dùng")]
        static WebsiteCinemaEntities db = new WebsiteCinemaEntities();
        public ActionResult Index()
        {
            var currentDate = DateTime.Now;
            ViewData["dsp"] = db.Phims.ToList(); ;
            ViewData["dsbv"] = db.BaiViets.ToList(); ;
            return View();
        }
      
    }
}