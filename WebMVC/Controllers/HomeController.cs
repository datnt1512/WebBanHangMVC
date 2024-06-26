using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMVC.Data;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class HomeController : Controller
    {
        private ElectronicStoreContext db = new ElectronicStoreContext();
        public ActionResult Index()
        {
            var data = db.Products.AsNoTracking().OrderByDescending(n => n.CreatedDate).Take(12).ToList();
            return View(data);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Lấy loại sản phẩm hiển thị bên trái trang chủ
        /// </summary>
        /// <returns></returns>
        public ActionResult _GetCategories()
        {
            var data = db.ProductCategories.AsNoTracking().OrderByDescending(n=>n.CreatedDate).ToList();

            return PartialView("_GetCategories", data ?? new List<ProductCategory>());
        }

        /// <summary>
        /// Lấy sản phẩm hiển thị bên phải phía trên trang chủ
        /// </summary>
        /// <returns></returns>
        public ActionResult _GetNewProduct()
        {
            var data = db.Products.AsNoTracking().OrderByDescending(n => n.CreatedDate).Take(2).ToList();

            return PartialView("_GetNewProduct", data ?? new List<Product>());
        }
    }
}