using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel;
namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        static WebsiteCinemaEntities db = new WebsiteCinemaEntities();
        private static string PasswdEmail = ConfigurationManager.AppSettings["PasswordEmail"];
        private static string Username = ConfigurationManager.AppSettings["Email"];
        [HttpGet]
        public ActionResult Login()
        {
            if(Session["InformationAccount"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }
        /// <summary>
        /// Hàm này dùng để đăng nhập tài khoản
        /// </summary>
        /// <param name="email">Nhận tên tài khoản từ người dùng khi nhập vào</param>
        /// <param name="password">Mật khẩu của tài khoản</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            ViewBag.displayError = "";
            string passhash = HashPassword.SHA512HashPass(password);
            TaiKhoan accountLogin = db.TaiKhoans.Where(m => m.email.Equals(email.ToLower()) && m.matKhau.Equals(passhash)).FirstOrDefault();
            if (accountLogin != null)
            {
                Session["InformationAccount"] = accountLogin;
                DiaChiIPDangNhap ipAddress = new DiaChiIPDangNhap()
                {
                    idTaiKhoan = accountLogin.idTaiKhoan,
                    ip = Request.UserHostAddress,
                    trinhDuyet = Request.UserAgent,
                    thietBi = GetDevice(Request.UserAgent),
                    thoiGian = DateTime.Now
                };
                db.DiaChiIPDangNhaps.Add(ipAddress);
                db.SaveChanges();

                if (accountLogin.maNhom == 4)
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Index", "Dashboard", new { area = "PrivateSite" });
            }
            ViewBag.displayError = "Thông tin không chính xác";
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// Hàm này dùng để đăng ký tài khoản
        /// </summary>
        /// <param name="acc">Nhận các thông tin sau khi người dùng nhập vào form đăng ký tài khoản</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterVM acc)
        {
            string AC = ""; // AC là một chuỗi mã kích hoạt tài khoản
            int numberRandom = 0; // Số ngẫu nhiên dùng để ghép lại thành 6 chữ số trong chuỗi mã kích hoạt tài khoản
            bool checkPhoneAndEmail = (db.TaiKhoans.Where(m=>m.email.Equals(acc.email.ToLower()) || m.soDienThoai.Equals(acc.numberPhone)).Count()>0); //Kiểm tra số điện thoại và email này đã được đăng ký trong hệ thống chưa
            if (ModelState.IsValid && !checkPhoneAndEmail )
            {
                Random random = new Random();
                TaiKhoan registerAcc = new TaiKhoan()
                {
                    idTaiKhoan = random.Next(20000).ToString(),
                    email = acc.email,
                    soDienThoai = acc.numberPhone,
                    ho = acc.lastName,
                    ten = acc.firstName,
                    sttTrangThai = 2,
                    matKhau = HashPassword.SHA512HashPass(acc.passWord),
                    maNhom = 4,
                };
                db.TaiKhoans.Add(registerAcc);
                Session["InformationAccount"] = registerAcc; // Lưu trữ phiên sau khi đăng ký

                //random mã kích hoạt tài khoản từ số 0-10 và lặp lại cộng dồn các dữ số random 6 vòng lặp để được 6 con số trong mã kích hoạt
                MaKichHoatTaiKhoan activateCode = new MaKichHoatTaiKhoan();
                activateCode.idTaiKhoan = registerAcc.idTaiKhoan;
                for(int i = 0; i < 6; i++)
                {
                    numberRandom = random.Next(10);
                    AC += numberRandom;
                }
                activateCode.maKichHoat = AC;
                activateCode.thoiGianTaoMa = DateTime.Now;
                activateCode.trangThaiMa = true;
                activateCode.ghiChu = "Mã kích hoạt đăng ký tài khoản";
                db.MaKichHoatTaiKhoans.Add(activateCode);

                bool check = await Sendmail("TitansCinema", "Mã kích hoạt tài khoản", activateCode.maKichHoat, registerAcc.email);
                if (check == true)
                {
                    db.SaveChanges();
                    return RedirectToAction("ActivateAccount");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session["InformationAccount"] = null;
            return RedirectToAction("Homepage","Home");
        }
        [HttpGet]
        public ActionResult ActivateAccount()
        {
            return View();
        }
        /// <summary>
        /// Hàm này dùng để kích hoạt tài khoản
        /// </summary>
        /// <param name="code">mã kích hoạt tài khoản</param>
        /// <param name="IDAccount">id tài khoản cần kích hoạt</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActivateAccount(string code, string IDAccount)
        {
            MaKichHoatTaiKhoan activateCode = db.MaKichHoatTaiKhoans.Where(m=>m.idTaiKhoan.Equals(IDAccount) || m.maKichHoat.Equals(code)).FirstOrDefault();
            if (activateCode!=null)
            {
                if (activateCode.trangThaiMa == true)
                {
                    TaiKhoan acc = db.TaiKhoans.Find(IDAccount);
                    acc.sttTrangThai = 1;
                    activateCode.trangThaiMa = false;
                    db.SaveChanges();
                    //return veef trang bán hàng
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.displayError = "Mã kích hoạt đã hết hạn sử dụng. Mỗi mã chỉ có thời hạn trong 10 phút!";
                    return View();
                }
            }
            ViewBag.displayError = "Mã kích hoạt không đúng vui lòng kiểm tra lại";
            return View();

        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        /// <summary>
        /// Hàm này dùng để lấy lại mật khẩu
        /// </summary>
        /// <param name="email">Email hàm cần gửi mã xác nhận lấy lại tài khoản</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            return View();
        }
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DenielAccess()
        {
            return View();
        }
        /// <summary>
        /// Hàm này dùng để gửi mail
        /// </summary>
        /// <param name="name">tên người gửi</param>
        /// <param name="subject">tiêu đề của mail</param>
        /// <param name="content">nội dung mail</param>
        /// <param name="tomail">địa chỉ mail người nhận</param>
        /// <returns></returns>
        private static async Task<bool> Sendmail(string name, string subject, string content, string tomail)
        {
            bool check = false;
            try
            {
                MailMessage mailMessage = new MailMessage();
                var smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(){UserName = Username,Password = PasswdEmail};

                MailAddress mailAddress = new MailAddress(Username, name);
                mailMessage.From = mailAddress;
                mailMessage.To.Add(tomail);
                mailMessage.Subject = subject;
                mailMessage.Body = content;
                mailMessage.IsBodyHtml = true;
                smtp.Send(mailMessage);
                check = true;
            }
            catch (Exception)
            {
                check = false;
            }
            return check;
        }
        /// <summary>
        /// Hàm này dùng để xác định chi tiết thiết bị khi đăng nhập vào website
        /// </summary>
        /// <param name="userAgent">xác định tên thiết bị và trình duyệt của người dùng khi truy cập</param>
        /// <returns></returns>
        private string GetDevice(string userAgent)
        {
            string result = "Không xác định";
            string[] devices = new string[] { "iPhone", "iPad", "Android", "Windows Phone", "Mac", "Windows", "Linux" };
            for (var i = 0; i < devices.Length; i++)
            {
                if (userAgent.Contains(devices[i]))
                {
                    result = devices[i];
                }
            }
            return result;
        }

       

    }
}