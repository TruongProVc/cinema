using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Areas.PrivateSite.Controllers
{
    [CustomAuthentication]
    [CustomAuthorize(Roles = "quản trị, quản lý")]
    public class CountryController : Controller
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
                List<QuocGia> countries = db.QuocGias.Where(p => string.IsNullOrEmpty(keyword) || p.tenQuocGia.ToLower().Contains(keyword.ToLower())).ToList();
              
                var totalPage = countries.Count;
                var numberPage = Math.Ceiling((float)totalPage / size);

                var start = (pageIndex - 1) * size;
                countries = countries.Skip(start).Take(size).ToList();

                return Json(new { status = true, Data = countries, CurrentPage = pageIndex, TotalItem = totalPage, NumberPage = numberPage, PageSize = size, message = "Đang load" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = false, CurrentPage = pageIndex, TotalItem = 0, NumberPage = 0, PageSize = size, message = "Tải dữ liệu lên thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddCountry(QuocGia country)
        {
            if (country.tenQuocGia != null && country.maISO != null)
            {
                if (checkEdit)
                {
                    QuocGia c = db.QuocGias.Find(country.sttQuocGia);
                    c.tenQuocGia = country.tenQuocGia;
                    c.maISO = country.maISO;
                    checkEdit = false;
                    db.SaveChanges();
                    return Json(new { status = true, message = "Đã sửa thành công" });
                }
                else
                {
                    db.QuocGias.Add(country);
                    db.SaveChanges();
                    return Json(new { status = true, message = "Đã thêm thành công quốc gia" });
                }
            }
            return Json(new { status = false, message = "Vui lòng nhập đầy đủ thông tin" }); ;
        }
        [HttpGet]
        public JsonResult Edit(int id)
        {
            QuocGia country= db.QuocGias.Find(id);
            checkEdit = true;
            return Json(new { data = country }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            QuocGia country = db.QuocGias.FirstOrDefault(m => m.sttQuocGia == id);
            string name = country.tenQuocGia;
            if (country != null)
            {
                db.QuocGias.Remove(country);
                db.SaveChanges();
            }
            return Json(new { status = true, message = "Đã xóa thành công thể loại: " + name });
        }
    }
}