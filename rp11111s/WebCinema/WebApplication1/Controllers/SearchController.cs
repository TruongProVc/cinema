using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class SearchController : Controller
    {
        static WebsiteCinemaEntities db = new WebsiteCinemaEntities();
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Autocomplete()
        {
            // Khởi tạo kết nối tới cơ sở dữ liệu

            // Lấy danh sách phim từ cơ sở dữ liệu
            var phims = db.Phims.ToList();

            // Chuyển danh sách phim thành chuỗi JSON
            string jsonPhims = JsonConvert.SerializeObject(phims, Formatting.Indented, new JsonSerializerSettings
            {
                // Loại bỏ các vòng lặp tham chiếu
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            // Trả về chuỗi JSON dưới dạng JsonResult
            return Json(jsonPhims, JsonRequestBehavior.AllowGet);

        }

    }
}