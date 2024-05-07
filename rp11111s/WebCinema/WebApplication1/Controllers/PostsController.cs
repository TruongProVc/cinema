using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class PostsController : Controller
    {
        static WebsiteCinemaEntities db = new WebsiteCinemaEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Detail(string id)
        {
            return View();
        }
    }
}