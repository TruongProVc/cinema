using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Areas.PrivateSite.Controllers
{
    [CustomAuthentication]
    [CustomAuthorize(Roles = "quản trị")]
    public class AccountManagementController : Controller
    {
        static WebsiteCinemaEntities db = new WebsiteCinemaEntities();
        [HttpGet]
        public ActionResult ListOfAccount()
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
                var accounts = db.TaiKhoans.Where(ac => string.IsNullOrEmpty(keyword) || ac.email.ToLower().Contains(keyword.ToLower())).Select(m => new {
                    m.idTaiKhoan,
                    m.ho,
                    m.ten,
                    m.email,
                    m.sttTrangThai,
                    m.maNhom
                }).ToList();

                var totalPage = accounts.Count;
                var numberPage = Math.Ceiling((float)totalPage / size);

                var start = (pageIndex - 1) * size;
                accounts = accounts.Skip(start).Take(size).ToList();

                return Json(new { status = true, Data = accounts, CurrentPage = pageIndex, TotalItem = totalPage, NumberPage = numberPage, PageSize = size, message = "Đang load" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = false, CurrentPage = pageIndex, TotalItem = 0, NumberPage = 0, PageSize = size, message = "Tải dữ liệu thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Delete(string idAccount)
        {
            TaiKhoan account = db.TaiKhoans.Find(idAccount);
            string userName = account.email;
            if (idAccount != null && account != null)
            {
                List<MaKichHoatTaiKhoan> activateCodeList = db.MaKichHoatTaiKhoans.Where(m => m.idTaiKhoan.Equals(idAccount)).ToList();
                foreach (var activateCode in activateCodeList)
                {
                    db.MaKichHoatTaiKhoans.Remove(activateCode);
                }
                db.TaiKhoans.Remove(account);
                db.SaveChanges();
            }
            return Json(new {status = true, message="Đã xóa thành công tài khoản: " + userName});
        }
        [HttpPost]
        public JsonResult AccountStatus(string idAccount, int newStatus)
        {
            TaiKhoan account = db.TaiKhoans.Find(idAccount);
            string userName = account.email;
            if (account != null)
            {
                account.sttTrangThai = newStatus;
                db.SaveChanges();
                return Json(new { status = true, message = "Bạn đã thay đổi trạng thái tài khoản " + userName + " thành công" });
            }
            return Json(new { status = false, message = "Thay đổi thất bại" });
        }
        [HttpGet]
        public ActionResult DetailView(string id)
        {
            return View();
        }
        [HttpGet]
        public JsonResult Detail(string id)
        {
            TaiKhoan account = db.TaiKhoans.FirstOrDefault(d => d.idTaiKhoan.Equals(id));
            if (account != null)
            {
                return Json(new { status = true, url = Url.Action("DetailView", "AccountManagement", new { id = id }) }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false,message="Tài khoản không tồn tại" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Detail()
        {
            return Json(new { status = true });
        }
       
    }
}