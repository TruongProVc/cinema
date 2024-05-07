using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel;
namespace WebApplication1.Areas.PrivateSite.Controllers
{
    [CustomAuthentication]
    [CustomAuthorize(Roles = "quản trị, quản lý")]
    public class ActorAndDirectorController : Controller
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
                var actorAndDirectors = db.DienVienDaoDiens.Where(a => string.IsNullOrEmpty(keyword) || a.tenDienVienDaoDien.Contains(keyword)).Select(m => new {
                    m.sttDienVienDaoDien,
                    m.tenDienVienDaoDien,
                    m.ngaySinh,
                    m.gioiTinh,
                    m.hinhDaiDien,
                    m.trangThai
                }).ToList();
                var totalPage = actorAndDirectors.Count;
                var numberPage = Math.Ceiling((float)totalPage / size);

                var start = (pageIndex - 1) * size;
                actorAndDirectors = actorAndDirectors.Skip(start).Take(size).ToList();

                return Json(new { status = true, Data = actorAndDirectors, CurrentPage = pageIndex, TotalItem = totalPage, NumberPage = numberPage, PageSize = size, message = "Đang load" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = false, CurrentPage = pageIndex, TotalItem = 0, NumberPage = 0, PageSize = size, message = "Tải dữ liệu lên thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Thêm, cập nhật một diễn viên hoặc đạo diễn
        /// </summary>
        /// <param name="adVM">Diễn viên đạo diễn view model</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Insert(ActorAndDirectorVM adVM, int id)
        {
            if (ModelState.IsValid)
            {
                if (!checkEdit && id < 1)
                {
                    DienVienDaoDien actorAndDirector = new DienVienDaoDien()
                    {
                        tenDienVienDaoDien = adVM.name,
                        gioiTinh = adVM.gender,
                        ngaySinh = adVM.dateOfBirth,
                        trangThai = true
                    };
                    SaveImage(adVM.imageAvatar, actorAndDirector);
                    db.DienVienDaoDiens.Add(actorAndDirector);
                    db.SaveChanges();
                    return Json(new { status = true, message = "Đã thêm thành công" });
                }
                else if(checkEdit && id > 0)
                {
                    DienVienDaoDien actorAndDirector = db.DienVienDaoDiens.Find(id);
                    actorAndDirector.gioiTinh = adVM.gender;
                    actorAndDirector.tenDienVienDaoDien = adVM.name;
                    actorAndDirector.ngaySinh = adVM.dateOfBirth;
                    if(adVM.imageAvatar != null) SaveImage(adVM.imageAvatar, actorAndDirector);
                    db.SaveChanges();
                    checkEdit = false;
                    return Json(new { status = true, message = "Đã sửa thành công" });
                }
            }
            checkEdit = false;
            return Json(new { status = false, message = (checkEdit ? "Thêm" : "Sửa") +" thất bại" }) ;
        }
        [HttpGet]
        public JsonResult Edit(int id)
        {
            DienVienDaoDien AD = db.DienVienDaoDiens.Find(id);
            if(AD != null)
            {
                checkEdit = true;
                return Json(new { status = true, data = AD }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false}, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Active(int id)
        {
            DienVienDaoDien AD = db.DienVienDaoDiens.Where(m => m.sttDienVienDaoDien == id).FirstOrDefault();
            if (AD != null)
            {
                if (AD.trangThai == true)
                    AD.trangThai = false;
                else
                    AD.trangThai = true;
                db.SaveChanges();
                return Json(new { status = true, message = "Đã thay đổi trạng thái thành công " });
            }
            return Json(new { status = false, message = "Đã thay đổi trạng thái thất bại " });

        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            DienVienDaoDien AD = db.DienVienDaoDiens.Where(m => m.sttDienVienDaoDien == id).FirstOrDefault();
            if (AD != null)
            {
                db.DienVienDaoDiens.Remove(AD);
                db.SaveChanges();
                return Json(new { status = true, message = "Đã xóa thành công" });
            }
            return Json(new { status = false, message = "Xóa thất bại" });
        }
        private void SaveImage(HttpPostedFileBase image, DienVienDaoDien actorAndDirector)
        {
            if (image != null && image.ContentLength > 0)
            {
                string virtualPath = "/Asset/image/article/";
                string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string physicalPath = Server.MapPath("~" + virtualPath);

                byte[] avatarImage = new byte[image.ContentLength];
                image.InputStream.Read(avatarImage, 0, image.ContentLength);

                image.SaveAs(physicalPath + fileName);
                actorAndDirector.hinhDaiDien = virtualPath + fileName;
            }
            else
            {
                actorAndDirector.hinhDaiDien = "";
            }
        }
    }
}