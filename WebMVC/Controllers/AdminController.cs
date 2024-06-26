using PagedList;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebMVC.Data;
using WebMVC.Extensions;
using WebMVC.Models;
using WebMVC.ViewModels;

namespace WebMVC.Controllers
{
    [Authorize(Roles = "Admin")] // Người dùng có quyền admin mới có thể vào trang quản trị
    public class AdminController : Controller
    {
        private ElectronicStoreContext db = new ElectronicStoreContext();

        #region Loại sản phẩm
        public ActionResult Category(int? page)
        {
            // Số lượng loại sản phẩm trên mỗi trang
            int pageSize = 10;
            // Số trang hiện tại (mặc định là 1 nếu không có giá trị)
            int pageNumber = (page ?? 1);

            // Lấy danh sách loại sản phẩm phân trang
            var productCategories = db.ProductCategories.OrderByDescending(pc => pc.CreatedDate)
                                                        .ToPagedList(pageNumber, pageSize);

            return View(productCategories);
        }

        // View thêm mới loại sản phẩm
        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        // xử lý lưu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(ProductCategory model)
        {

            try
            {
                if (!ModelState.IsValid) return View(model);
                model.CreatedDate = DateTime.Now;
                model.UpdatedDate = DateTime.Now;
                db.ProductCategories.Add(model);
                db.SaveChanges();
                TempData["success"] = "Thêm mới thành công.";
                return RedirectToAction(nameof(Category));
            }
            catch (Exception)
            {
                TempData["error"] = "Thêm không thành công.";
                return View();
            }

        }

        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Category));
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return RedirectToAction(nameof(Category));
            }
            return View(productCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(ProductCategory model)
        {

            try
            {
                if (!ModelState.IsValid) return View(model);
                ProductCategory productCategory = db.ProductCategories.Find(model.ProductCategoryId);
                if (productCategory == null)
                {
                    TempData["error"] = "Cập nhật không thành công.";
                    return View(model);

                }
                productCategory.UpdatedDate = DateTime.Now;
                productCategory.CategoryName = model.CategoryName;
                db.Entry(productCategory).State = EntityState.Modified;
                db.SaveChanges();
                TempData["success"] = "Cập nhật thành công.";
                return RedirectToAction(nameof(Category));


            }
            catch (Exception)
            {
                TempData["error"] = "Cập nhật không thành công.";
                return View(model);
            }

        }

        [HttpPost]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                ProductCategory productCategory = await db.ProductCategories.FindAsync(id);

                if (productCategory == null)
                {
                    TempData["error"] = "Xóa không thành công.";
                    return RedirectToAction(nameof(Category));
                }

                db.ProductCategories.Remove(productCategory);
                await db.SaveChangesAsync();
                TempData["success"] = "Xóa thành công.";
                return RedirectToAction(nameof(Category));
            }
            catch (Exception)
            {
                TempData["error"] = "Xóa không thành công.";
                return RedirectToAction(nameof(Category));
            }

        }
        #endregion

        #region Sản phẩm
        public ActionResult Product(int? page)
        {
            // Số lượng sản phẩm trên mỗi trang
            int pageSize = 10;
            // Số trang hiện tại (mặc định là 1 nếu không có giá trị)
            int pageNumber = (page ?? 1);

            // Lấy danh sách sản phẩm phân trang
            var products = db.Products.OrderByDescending(pc => pc.CreatedDate)
                                                        .ToPagedList(pageNumber, pageSize);

            return View(products);
        }

        // View thêm mới sản phẩm
        [HttpGet]
        public ActionResult CreateProduct()
        {
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "CategoryName");
            return View();
        }

        // xử lý lưu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(CreateUpdateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "CategoryName");
                return View(model);
            }

            try
            {
                if(model.PostedFile != null && model.PostedFile.ContentLength > 0)
                {
                    // Kiểm tra kích thước file không vượt quá 5MB
                    if (model.PostedFile.ContentLength > (5 * 1024 * 1024))
                    {
                        TempData["error"] = "Hình ảnh không vượt quá 5MB.";
                        ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "CategoryName");
                        return View(model);
                    }

                    // Lấy tên file từ thuộc tính FileName của đối tượng PostedFile
                    var fileName = Path.GetFileName(model.PostedFile.FileName);

                    // Loại bỏ phần mở rộng của tên file
                    fileName = Path.GetFileNameWithoutExtension(fileName);

                    // Lấy phần mở rộng của file
                    string extension = Path.GetExtension(model.PostedFile.FileName);

                    // Tạo tên file mới bằng cách kết hợp tên file gốc, ngày giờ hiện tại và phần mở rộng
                    fileName = fileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;

                    // Tạo đường dẫn đầy đủ đến thư mục lưu file bằng cách kết hợp đường dẫn thư mục và tên file
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);

                    // Kiểm tra xem thư mục lưu file có tồn tại hay không
                    if (!Directory.Exists(Server.MapPath("~/Images/")))
                    {
                        // Nếu thư mục không tồn tại thì tạo mới
                        Directory.CreateDirectory(Server.MapPath("~/Images/"));
                    }

                    // Lưu file vào đường dẫn đã chỉ định
                    model.PostedFile.SaveAs(path);

                    // Gán tên file đã lưu vào thuộc tính SrcImage của model
                    model.SrcImage = fileName;
                }
                else
                {
                    model.SrcImage = string.Empty;
                }


                var product = new Product
                {
                    SrcImage = model.SrcImage,
                    ProductName = model.ProductName,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Alias = StringExtensions.ToFriendlyUrl(model.ProductName),
                    Price = model.Price,
                    ProductCategoryId = model.ProductCategoryId,
                    Description = model.Description

                };
         
                db.Products.Add(product);
                db.SaveChanges();
                TempData["success"] = "Thêm mới thành công.";
                return RedirectToAction(nameof(Product));
            }
            catch (Exception ex)
            {
                TempData["error"] = "Thêm không thành công.";
                ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "CategoryName");
                return View();
            }

        }

        // View cập nhật sản phẩm
        [HttpGet]
        public ActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Product));
            }

            Product product = db.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Product));
            }

            var viewModel = new CreateUpdateProductViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                SrcImage = product.SrcImage,
                Description = product.Description,
                ProductCategoryId = product.ProductCategoryId,

            };
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "CategoryName", product.ProductCategoryId);
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(CreateUpdateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "CategoryName", model.ProductCategoryId);
                return View(model);
            }

            try
            {
                if (model.PostedFile != null && model.PostedFile.ContentLength > 0)
                {
                    // Kiểm tra kích thước file không vượt quá 5MB
                    if (model.PostedFile.ContentLength > (5 * 1024 * 1024))
                    {
                        TempData["error"] = "Hình ảnh không vượt quá 5MB.";
                        ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "CategoryName");
                        return View(model);
                    }

                    // Lấy tên file từ thuộc tính FileName của đối tượng PostedFile
                    var fileName = Path.GetFileName(model.PostedFile.FileName);

                    // Loại bỏ phần mở rộng của tên file
                    fileName = Path.GetFileNameWithoutExtension(fileName);

                    // Lấy phần mở rộng của file
                    string extension = Path.GetExtension(model.PostedFile.FileName);

                    // Tạo tên file mới bằng cách kết hợp tên file gốc, ngày giờ hiện tại và phần mở rộng
                    fileName = fileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;

                    // Tạo đường dẫn đầy đủ đến thư mục lưu file bằng cách kết hợp đường dẫn thư mục và tên file
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);

                    // Kiểm tra xem thư mục lưu file có tồn tại hay không
                    if (!Directory.Exists(Server.MapPath("~/Images/")))
                    {
                        // Nếu thư mục không tồn tại thì tạo mới
                        Directory.CreateDirectory(Server.MapPath("~/Images/"));
                    }

                    // Lưu file vào đường dẫn đã chỉ định
                    model.PostedFile.SaveAs(path);

                    // Gán tên file đã lưu vào thuộc tính SrcImage của model
                    model.SrcImage = fileName;
                }
               

                Product product = db.Products.Find(model.ProductId);

                if (!string.IsNullOrEmpty(model.SrcImage))
                {
                    product.SrcImage = model.SrcImage;
                }
                product.UpdatedDate = DateTime.Now;
                product.ProductName = model.ProductName;
                product.Price = model.Price;
                product.Description = model.Description;
                product.ProductCategoryId = model.ProductCategoryId;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                TempData["success"] = "Cập nhật thành công.";
                return RedirectToAction(nameof(Product));
            }
            catch (Exception ex)
            {
                TempData["error"] = "Thêm không thành công.";
                ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "CategoryName", model.ProductCategoryId);
                return View();
            }

        }

        [HttpPost]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                Product product = await db.Products.FindAsync(id);

                if (product == null)
                {
                    TempData["error"] = "Xóa không thành công.";
                    return RedirectToAction(nameof(Product));
                }

                db.Products.Remove(product);
                await db.SaveChangesAsync();
                TempData["success"] = "Xóa thành công.";
                return RedirectToAction(nameof(Product));
            }
            catch (Exception)
            {
                TempData["error"] = "Xóa không thành công.";
                return RedirectToAction(nameof(Product));
            }

        }
        #endregion

        #region Người dùng
        public ActionResult User(int? page)
        {
            // Số lượng sản phẩm trên mỗi trang
            int pageSize = 10;
            // Số trang hiện tại (mặc định là 1 nếu không có giá trị)
            int pageNumber = (page ?? 1);

            // Lấy danh sách người dùng phân trang
            var data = db.Users.OrderByDescending(pc => pc.CreatedDate)
                                                        .ToPagedList(pageNumber, pageSize);

            return View(data);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}