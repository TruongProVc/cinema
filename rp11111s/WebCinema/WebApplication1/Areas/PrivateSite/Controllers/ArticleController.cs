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
    [CustomAuthentication]
    [CustomAuthorize(Roles = "quản trị, quản lý, đăng bài")]
    public class ArticleController : Controller
    {
        static WebsiteCinemaEntities db = new WebsiteCinemaEntities();
        public ActionResult ArticleList()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Insert(ArticleVM aVM)
        {
            TaiKhoan account = Session["InformationAccount"] as TaiKhoan;
            if (ModelState.IsValid)
            {
                Random random = new Random();
                string id = "";
                int  id1 = 0;
                for (int i = 0; i < 10; i++)
                {
                    id1 = random.Next(9);
                    if (i == 1 && id1 == 0)
                    {
                        continue;
                    }
                    else
                    {
                        id += id1;
                    }
                }
                BaiViet article = new BaiViet()
                {
                    idBaiViet = id,
                    tenBaiViet = aVM.articleName,
                    noiDungTomTat = aVM.summaryContent,
                    noiDung = aVM.content,
                    luotXem = 0,
                    ngayDang = DateTime.Now,
                    trangThai = true,
                    idTaiKhoanDang = "2375427638"
                };
                SaveImage(aVM.avatarArticle, article);

                db.BaiViets.Add(article);
                db.SaveChanges();

                return Json(new { status = true, message = "Đã thêm bài viết thành công" });
            }
            return Json(new { status = false, message = "Vui lòng điền đầy đủ các thông tin" });

        }
        [HttpGet]
        public JsonResult LoadData(string keyword, int? page, int? pageSize)
        {
            var size = pageSize ?? 2;
            var pageIndex = page ?? 1;
            try
            {
                var articles = db.BaiViets.Where(a => string.IsNullOrEmpty(keyword) || a.tenBaiViet.ToLower().Contains(keyword.ToLower())).Select(m => new {
                    m.idBaiViet,
                    m.tenBaiViet,
                    m.luotXem,
                    m.ngayDang,
                    taiKhoanDang = (db.TaiKhoans.Where(k=>k.idTaiKhoan.Equals(m.idTaiKhoanDang)).Select(k=>k.email)),
                    m.noiDung,
                    m.noiDungTomTat,
                    m.trangThai
                }).ToList();


                var totalPage = articles.Count;
                var numberPage = Math.Ceiling((float)totalPage / size);

                var start = (pageIndex - 1) * size;
                articles = articles.Skip(start).Take(size).ToList();

                return Json(new { status = true, Data = articles, CurrentPage = pageIndex, TotalItem = totalPage, NumberPage = numberPage, PageSize = size, message = "Đang load" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
            }
            return Json(new { status = true, CurrentPage = pageIndex, TotalItem = 0, NumberPage = 0, PageSize = size, message = "Đang load" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Edit(string id)
        {
            return Json(true);
        }
        [HttpPost]
        public JsonResult Delete(string id)
        {
            BaiViet article = db.BaiViets.Find(id);
            if(article != null)
            {
                db.BaiViets.Remove(article);
                db.SaveChanges();
                return Json(new { status = true, message = "Bạn đã xóa thành công"});
            }
            return Json(new { status = false, message = "Xóa thất bại" });
        }
        [HttpPost]
        public JsonResult ActiveArticle(string id)
        {
            BaiViet article = db.BaiViets.Where(m=>m.idBaiViet.Equals(id)).FirstOrDefault();
            if (article != null)
            {
                if (article.trangThai == true) 
                    article.trangThai = false;
                else 
                    article.trangThai = true;
                db.SaveChanges();
                return Json(new { status = true, message = "Đã thay đổi trạng thái bài viết thành công " });
            }
            return Json(new { status = false, message = "Đã thay đổi trạng thái bài viết thất bại " });
        }
        private void SaveImage(HttpPostedFileBase image, BaiViet article)
        {
            if (image != null && image.ContentLength > 0)
            {
                string virtualPath = "/Asset/image/article/";
                string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string physicalPath = Server.MapPath("~" + virtualPath);

                byte[] avatarImage = new byte[image.ContentLength];
                image.InputStream.Read(avatarImage, 0, image.ContentLength);

                image.SaveAs(physicalPath + fileName);
                article.hinhDaiDien = virtualPath + fileName;
            }
            else
            {
                article.hinhDaiDien = "";
            }
        }

    }
}