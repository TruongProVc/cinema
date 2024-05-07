using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.ViewModel;
using WebApplication1.Models;
namespace WebApplication1.Areas.PrivateSite.Controllers
{
    public class MovieController : Controller
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
                var movie = db.Phims.Where(p => string.IsNullOrEmpty(keyword) || p.tenPhim.ToLower().Contains(keyword.ToLower())).Select(d=>new 
                {
                    d.idPhim,
                    d.tenPhim,
                    d.thoiGian,
                    d.ngayChieu,
                    d.thoiLuong,
                    d.trangThai
                }).ToList();

                var totalPage = movie.Count;
                var numberPage = Math.Ceiling((float)totalPage / size);

                var start = (pageIndex - 1) * size;
                movie = movie.Skip(start).Take(size).ToList();

                return Json(new { status = true, Data = movie, CurrentPage = pageIndex, TotalItem = totalPage, NumberPage = numberPage, PageSize = size, message = "Đang load" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = false, CurrentPage = pageIndex, TotalItem = 0, NumberPage = 0, PageSize = size, message = "Tải dữ liệu thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult Insert()
        {
            CountryList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Insert(MovieVM mVM)
        {
            if (ModelState.IsValid)
            {
                if (checkEdit )
                {
                    return Json(new { status = true, message = "Sửa thành công" });
                }
                else
                {
                    Random random = new Random();
                    string id = "";
                    int id1 = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        id1 = random.Next(9);
                        if (i == 1 && id1 == 0)
                            continue;
                        else
                            id += id1;
                    }

                    Phim movie = new Phim()
                    {
                        idPhim = id,
                        tenPhim = mVM.name,
                        trailer = mVM.trailer,
                        quocGia = mVM.idCountry,
                        thoiGian = DateTime.Now,
                        ngayChieu = mVM.showDate,
                        gioiThieu = mVM.introduce,
                        thoiLuong = mVM.time,
                        namSanXuat = mVM.yearManufacture,
                        congTySanXuat = mVM.company,
                        trangThai = true
                    };
                    SaveImage(mVM.imageMovie, movie);

                    db.Phims.Add(movie);
                    db.SaveChanges();
                    return Json(new { status = true, message = "Thêm thành công" });
                }
            }
            return Json(new { status = false, message="Thêm thất bại"}) ;

        }
        [HttpPost]
        public JsonResult Delete(string id)
        {
            Phim movie = db.Phims.Find(id);
            if (movie != null)
            {
                db.Phims.Remove(movie);
                db.SaveChanges();
                return Json(new{status = true,message="Xóa thành công"});
            }
            return Json(new { status = false, message = "Xóa thất bại" });
        }
        [HttpPost]
        public JsonResult Edit(string id)
        {
            var movie = db.Phims.Find(id);
            checkEdit = true;
            return Json(new { status = true, data = movie, url = Url.Action("Insert","Movie")});
        }
        [HttpPost]
        public JsonResult ChangeStatus(string id)
        {
            var movie = db.Phims.Find(id);
            if (movie != null)
            {
                if (movie.trangThai == true) movie.trangThai = false;
                else movie.trangThai = true;
                return Json(new { status = true, message = "Thay đổi trang thái thành công" });
            }
            return Json(new { status = false, message = "Thay đổi trang thái thất bại" });
        }
        private void CountryList()
        {
            ViewData["CountryList"] = db.QuocGias.ToList();
        }
        //[HttpGet]
        //public JsonResult CountryList()
        //{
        //    List<QuocGia> countryList = db.QuocGias.ToList();
        //    return Json(new { status = true,data = countryList},JsonRequestBehavior.AllowGet);
        //}
        private void SaveImage(HttpPostedFileBase image, Phim movie)
        {
            if (image != null && image.ContentLength > 0)
            {
                string virtualPath = "/Asset/image/movie/";
                string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string physicalPath = Server.MapPath("~" + virtualPath);

                byte[] avatarImage = new byte[image.ContentLength];
                image.InputStream.Read(avatarImage, 0, image.ContentLength);

                image.SaveAs(physicalPath + fileName);
                movie.hinhDaiDien = virtualPath + fileName;
            }
            else
            {
                movie.hinhDaiDien = "";
            }
        }

    }
}