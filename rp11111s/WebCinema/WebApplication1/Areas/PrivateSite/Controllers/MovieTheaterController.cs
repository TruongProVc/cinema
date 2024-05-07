using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Areas.PrivateSite.Controllers
{
    public class MovieTheaterController : Controller
    {
        static WebsiteCinemaEntities db = new WebsiteCinemaEntities();
        static bool checkEdit = false;
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult LoadData(string keyword, int? page, int? pageSize)
        {
            var size = pageSize ?? 2;
            var pageIndex = page ?? 1;
            try
            {
                var movieTheater= db.RapPhims.Where(p => string.IsNullOrEmpty(keyword) || p.soPhongChieu.ToLower().Contains(keyword.ToLower())).Select(d=>new 
                { 
                    d.sttRapPhim,d.soLuongGhe,d.soPhongChieu,d.trangThai
                }).ToList();

                var totalPage = movieTheater.Count;
                var numberPage = Math.Ceiling((float)totalPage / size);

                var start = (pageIndex - 1) * size;
                movieTheater = movieTheater.Skip(start).Take(size).ToList();

                return Json(new { status = true, Data = movieTheater, CurrentPage = pageIndex, TotalItem = totalPage, NumberPage = numberPage, PageSize = size, message = "Đang load" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = false, CurrentPage = pageIndex, TotalItem = 0, NumberPage = 0, PageSize = size, message = "Tải dữ liệu thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Hàm này dùng để thêm hoặc cập nhật 1 rạp phim
        /// </summary>
        /// <param name="id">id của rạp</param>
        /// <param name="name">Tên hoặc số phòng của rạp phim</param>
        /// <param name="numberOfSeat">Số lượng ghế</param>
        /// <param name="__RequestVerificationToken">Kiểm tra token của phiên coi có khớp không. Tránh được các cuộc tấn công giả mạo phiên</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Insert(RapPhim mt,string __RequestVerificationToken)
        {
            if (checkEdit && mt.sttRapPhim >0)
            {
                RapPhim movieTheater = db.RapPhims.Find(mt.sttRapPhim);
                movieTheater.soLuongGhe = mt.soLuongGhe;
                movieTheater.soPhongChieu = mt.soPhongChieu;
                db.SaveChanges();
                checkEdit = false;
                return Json(new { status = true, message = "Cập nhật phòng thành công" });
            }
            else
            {
                RapPhim movieTheater = new RapPhim()
                {
                    soPhongChieu = mt.soPhongChieu,
                    soLuongGhe = mt.soLuongGhe,
                    trangThai = true
                };
                db.RapPhims.Add(movieTheater);
                db.SaveChanges();
                return Json(new { status = true, message = "Thêm phòng thành công" });
            }
        }

        [HttpPost]
        public JsonResult Edit(int id)
        {
            RapPhim movieTheater = db.RapPhims.Find(id);
            checkEdit = true;
            return Json(new { status = true, data = movieTheater });
        }

        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            RapPhim movieTheater = db.RapPhims.Find(id);
            if(movieTheater != null)
            {
                if (movieTheater.trangThai == true) movieTheater.trangThai = false;
                else movieTheater.trangThai = true;
                db.SaveChanges();
                return Json(new { status = true, message = "Thay đổi trang thái thành công" });
            }
            return Json(new { status = false, message = "Thay đổi trang thái thất bại" });
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            RapPhim movieTheater = db.RapPhims.Find(id);
            if (movieTheater != null)
            {
                List<LichChieu> showTime = db.LichChieux.Where(d=>d.sttRap == id).ToList();
                foreach(var item in showTime)
                {
                    if(item.sttRap == id)
                        db.LichChieux.Remove(item);
                }
                db.RapPhims.Remove(movieTheater);
                db.SaveChanges();
                return Json(new { status = true, message = "Đã xóa thành công " });
            }
            return Json(new { status = false, message = "Đã xoá thất bại" });
        }

    }
}