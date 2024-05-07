using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class NewDetailController : Controller
    {
        static WebsiteCinemaEntities db = new WebsiteCinemaEntities();
        // GET: NewDetail
        public ActionResult Index(string id)
        {
            var objnew = db.BaiViets.Where(n => n.idBaiViet.Equals(id)).FirstOrDefault();

            return View(objnew);
        }
        public ActionResult AllNew()
        {

            List<BaiViet> bv = db.BaiViets.ToList();
            ViewData["dsbv"] = bv;
            return View();
        }
    }
}