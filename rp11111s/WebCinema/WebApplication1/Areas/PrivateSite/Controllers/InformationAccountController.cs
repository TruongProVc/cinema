using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Areas.PrivateSite.Controllers
{
    [CustomAuthentication]
    [CustomAuthorize(Roles = "quản trị, quản lý, nhân viên")]

    public class InformationAccountController : Controller
    {
        static WebsiteCinemaEntities db = new WebsiteCinemaEntities();
        public ActionResult Index()
        {
            return View();
        }
        private void UpdateData()
        {
            TaiKhoan account = Session["InformationAccount"] as TaiKhoan;
            ViewData["IPAddressList"] = db.DiaChiIPDangNhaps.Where(d => d.idTaiKhoan.Equals(account.idTaiKhoan)).ToList();
        }
        [HttpGet]
        public JsonResult LoadData(int? page, int? pageSize)
        {
            TaiKhoan account = Session["InformationAccount"] as TaiKhoan;
            try
            {
                //10 lần lích sử đăng nhập gần nhất
                var IPAddressList = db.DiaChiIPDangNhaps.Where(d => d.idTaiKhoan.Equals(account.idTaiKhoan)).Select(d =>
                  new
                  {
                      d.ip,
                      d.sttDiaChi,
                      d.thietBi,
                      d.trinhDuyet,
                      d.thoiGian
                  }).OrderByDescending(d => d.sttDiaChi).Take(10).ToList();

                return Json(new { status = true, Data = IPAddressList, message = "Đang load" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { status = false, message = "Tải dữ liệu lên thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(string firstName,string lastName , string mobile,string __RequestVerificationToken)
        {
            string id = (Session["InformationAccount"] as TaiKhoan).idTaiKhoan;
            TaiKhoan account = db.TaiKhoans.Find(id);
            if (account != null && IsValid( mobile,  lastName,  firstName) == true) 
            {
                account.ho = lastName;
                account.ten = firstName;
                account.soDienThoai = mobile;
                Session["InformationAccount"] = account;
                db.SaveChanges();
                return Json(new { status = true, message = "Thay đổi thông tin thành công" });
            }
            return Json(new { status = false, message = "Thay đổi thông tin thất bại" });
        }
        private bool IsValid(string mobile,string lastName, string firstName)
        {
            if (!Regex.IsMatch(mobile, @"^\d+$") || (mobile.Length < 9 || mobile.Length > 12) || lastName.Length < 3 || firstName.Length < 3)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Hàm này dùng để đổi mật khẩu tài khoản trong trang quản trị
        /// </summary>
        /// <param name="passwordCurrent">Mật khẩu hiện tại</param>
        /// <param name="newPassword">Mật khẩu mới</param>
        /// <param name="__RequestVerificationToken">Kiểm tra token của phiên coi có khớp không. Tránh được các cuộc tấn công giả mạo phiên</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangePassword(string passwordCurrent, string newPassword,string __RequestVerificationToken)
        {
            string id = (Session["InformationAccount"] as TaiKhoan).idTaiKhoan;
            TaiKhoan account = db.TaiKhoans.Find(id);
            string currentPWHash = HashPassword.SHA512HashPass(passwordCurrent.Trim());
            if (account != null && currentPWHash.Equals(account.matKhau))
            {
                account.matKhau = HashPassword.SHA512HashPass(newPassword);
                db.SaveChanges();
                return Json(new {status = true, message="Đổi mật khẩu thành công" });
            }
            return Json(new { status = false, message = "Mật khẩu không chính xác" });
        }
    }
}

