using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Areas.PrivateSite.Controllers
{
    public class ShowTimeController : Controller
    {
        static WebsiteCinemaEntities db = new WebsiteCinemaEntities();
        static bool checkEdit = false;
        public ActionResult Index()
        {
            UpdateSite();
            return View();
        }
        [HttpGet]
        public JsonResult LoadData(string keyword, int? page, int? pageSize)
        {
            var size = pageSize ?? 2;
            var pageIndex = page ?? 1;
            try
            {
                var now = DateTime.Now;
                var showTime = db.LichChieux.Where(c=>String.IsNullOrEmpty(keyword) || c.Phim.tenPhim.Contains(keyword)).Select(m=>new {
                    m.idLichChieu,
                    m.trangThai,
                    ten = m.Phim.tenPhim,
                    m.giaVe,
                    sp = m.RapPhim.soPhongChieu,
                    m.thoiGianChieu,
                }).OrderBy(d => (d.thoiGianChieu)).ToList();

                var totalPage = showTime.Count;
                var numberPage = Math.Ceiling((float)totalPage / size);
                var start = (pageIndex - 1) * size;
                showTime = showTime.Skip(start).Take(size).ToList();

                return Json(new { status = true, Data = showTime, CurrentPage = pageIndex, TotalItem = totalPage, NumberPage = numberPage, PageSize = size, message = "Đang load" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = false, CurrentPage = pageIndex, TotalItem = 0, NumberPage = 0, PageSize = size, message = "Tải dữ liệu thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Insert(LichChieu st)
        {
            bool check = (st.idPhim != null && st.sttRap > 0);
            if (check)
            {
                if (checkEdit)
                {
                    LichChieu showTime = db.LichChieux.FirstOrDefault(d=>d.idLichChieu.Equals(st.idLichChieu));
                    checkEdit = false;
                    if(showTime != null)
                    {
                        showTime.thoiGianChieu = st.thoiGianChieu;
                        showTime.idLichChieu = st.idLichChieu;
                        showTime.sttRap = st.sttRap;
                        showTime.giaVe = st.giaVe;
                        db.SaveChanges();
                        return Json(new { status = true, message = "Đã sửa thành công"}); ;
                    }
                    return Json(new { status = false, message = "Đã sửa thất bại" }); ;
                }
                else
                {
                    LichChieu showTime = new LichChieu()
                    {
                        idLichChieu = Common.CreateID(),
                        giaVe = st.giaVe,
                        idPhim = st.idPhim,
                        sttRap = st.sttRap,
                        thoiGianChieu = st.thoiGianChieu,
                        trangThai = true
                    };
                    db.LichChieux.Add(showTime);
                    db.SaveChanges();
                    UpdateSite();
                    return Json(new { status = true, message = "Đã thêm thành công" });
                }
            }
            UpdateSite();
            return Json(new { status = false, message = "Đã thêm thất bại" });
        }
        [HttpGet]
        public JsonResult Edit(string id)
        {
            var showTime = db.LichChieux.Select(m=>new 
            {
                m.idLichChieu,m.idPhim,m.sttRap,m.giaVe,m.thoiGianChieu
            
            }).FirstOrDefault(d=>d.idLichChieu.Equals(id));
            checkEdit = true;
            return Json(new { status = true, data = showTime },JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ChangeStatus(string id)
        {
            LichChieu showTime = db.LichChieux.Find(id);
            if(showTime != null)
            {
                if (showTime.trangThai == true) showTime.trangThai = false;
                else showTime.trangThai = true;
                db.SaveChanges();
                return Json(new { status = true, message = "Đã thay đổi trạng thái thành công" });
            }
            return Json(new { status = false, message = "Đã thay đổi trạng thái thất bại "});
        }
        [HttpPost]
        public JsonResult Delete(string id)
        {
            LichChieu showTime = db.LichChieux.Find(id);
            if (showTime != null)
            {
                db.LichChieux.Remove(showTime);
                db.SaveChanges();
                return Json(new { status = true, message = "Đã xóa thành công" });
            }
            return Json(new { status = false, message = "Đã xóa thất bại" });
        }
        private void UpdateSite()
        {
            ViewData["MovieList"] = db.Phims.Where(d=>d.trangThai == true).ToList();
            ViewData["MoieTheaterList"] = db.RapPhims.ToList();
        }
  

    }
}