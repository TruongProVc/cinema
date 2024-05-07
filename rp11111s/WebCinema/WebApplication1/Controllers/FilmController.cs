using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class FilmController : Controller
    {
        // GET: Film
        static WebsiteCinemaEntities db = new WebsiteCinemaEntities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PhimDangChieu()
        {
            var currentDate = DateTime.Now;
            List<Phim> phim = db.Phims.Where(n => !string.IsNullOrEmpty(n.idPhim) && n.ngayChieu < currentDate).ToList();
            ViewData["dsp"] = phim;
            return View();

        }
        public ActionResult PhimSapChieu()
        {
            var currentDate = DateTime.Now;
            List<Phim> phim = db.Phims.Where(n => !string.IsNullOrEmpty(n.idPhim) && n.ngayChieu > currentDate).ToList();
            ViewData["dsp"] = phim;
            return View();

        }
    }
}