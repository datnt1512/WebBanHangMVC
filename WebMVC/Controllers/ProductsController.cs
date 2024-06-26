using System.Linq;
using System.Web.Mvc;
using WebMVC.Data;

namespace WebMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ElectronicStoreContext db = new ElectronicStoreContext();

        [Route("danh-sach-loai-san-pham/{id}")]
        public ActionResult Category(int id)
        {
            var products = db.Products.Where(n=>n.ProductCategoryId == id).OrderByDescending(n=>n.CreatedDate);         
            return View(products.ToList());
          
        }

        [Route("chi-tiet/{id}/{alias}")]
        public ActionResult Detail(string alias, int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(product);
        }
    }
}