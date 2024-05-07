using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Areas.PrivateSite.Controllers
{
    public class CategoryMovieController : Controller
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
                List<TheLoai> categories = db.TheLoais.Where(p => string.IsNullOrEmpty(keyword) || p.tenTheLoai.ToLower().Contains(keyword.ToLower())).ToList();

                var totalPage = categories.Count;
                var numberPage = Math.Ceiling((float)totalPage / size);

                var start = (pageIndex - 1) * size;
                categories = categories.Skip(start).Take(size).ToList();
                    
                return Json(new { status = true, Data = categories, CurrentPage = pageIndex, TotalItem = totalPage, NumberPage = numberPage, PageSize = size, message = "Đang load" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = false, CurrentPage = pageIndex, TotalItem = 0, NumberPage = 0, PageSize = size, message = "Tải dữ liệu thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Add(TheLoai category, string __RequestVerificationToken)
        {
            if (category.tenTheLoai != null )
            {
                if (checkEdit)
                {
                    TheLoai ct = db.TheLoais.Find(category.sttTheLoai);
                    ct.tenTheLoai = category.tenTheLoai;
                    ct.ghiChu = category.ghiChu;
                    checkEdit = false;
                    db.SaveChanges();
                    return Json(new { status = true, message="Đã sửa thành công" });
                }
                else
                {
                    db.TheLoais.Add(category);
                    db.SaveChanges();
                    return Json(new { status = true, message="Đã thêm thành công thể loại" });
                }
            }
            return Json(new { status = false, message = "Vui lòng nhập đầy đủ thông tin"}); ;
        }
        [HttpGet]
        public JsonResult Edit(int id)
        {   
            TheLoai category = db.TheLoais.Find(id);
            checkEdit = true;
            return Json(new {data = category},JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(int idCategory)
        {
            TheLoai category = db.TheLoais.FirstOrDefault(m => m.sttTheLoai == idCategory);
            string name = category.tenTheLoai;
            if (ModelState.IsValid && category!=null)
            {
                db.TheLoais.Remove(category);
                db.SaveChanges();
            }
            return Json(new {status=true, message="Đã xóa thành công thể loại: " + name}); 
        }
    }
}