using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class MovieDetailController : Controller
    {
        // GET: MovieDetail
        private WebsiteCinemaEntities db = new WebsiteCinemaEntities();
        public ActionResult Index(string id)
        {
            if (id != null)
            {
                //var phim = db.Phims.ToList();
                //var binhLuan = db.BinhLuans.ToList();
                //ViewBag.BinhLuan = binhLuan;
                var objmoviedetail = db.Phims.Where(n => n.idPhim.Equals(id)).FirstOrDefault();
                UpdateSite();
                return View(objmoviedetail);
            }
            return RedirectToAction("Index", "Home");
        }
        // POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthentication]
        public ActionResult Create(string noiDung1, string idPhim)
        {
            var objmoviedetail = db.Phims.Where(n => n.idPhim.Equals(idPhim)).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (Session["InformationAccount"] != null)
                {
                    BinhLuan comment = new BinhLuan()
                    {
                        idTaiKhoan = (Session["InformationAccount"] as TaiKhoan).idTaiKhoan,
                        idPhim = idPhim,
                        noiDung = noiDung1,
                        ngayDang = DateTime.Now,
                    };
                    db.BinhLuans.Add(comment);
                    db.SaveChanges();
                    UpdateSite();
                    return View("Index", objmoviedetail);
                }
            }
            UpdateSite();
            return View("Index", objmoviedetail);
        }
        private void UpdateSite()
        {
            List<BinhLuan> bl = db.BinhLuans.ToList();
            ViewData["dsbl"] = bl;

        }
    }
}