using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebMVC.Data;
using WebMVC.Enums;
using WebMVC.Helpers;
using WebMVC.Models;
using WebMVC.ViewModels;

namespace ElectronicStore.Controllers
{
    public class AccountController : Controller
    {
        private ElectronicStoreContext db = new ElectronicStoreContext();

        // GET: Account/Login
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Account/Login
        // Sử dụng các attribute để chỉ định phương thức này không yêu cầu xác thực và sử dụng ValidateAntiForgeryToken
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Login(AccountLoginViewModel model, string returnUrl)
        {
            // Kiểm tra ModelState nếu không hợp lệ thì hiển thị form đăng nhập lại với model và lỗi
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Tìm người dùng trong cơ sở dữ liệu dựa trên tên đăng nhập
            User user = await db.Users.FirstOrDefaultAsync(u => u.Username == model.UserName);

            // Nếu không tìm thấy người dùng, hiển thị thông báo lỗi và hiển thị lại form đăng nhập
            if (user == null)
            {
                TempData["error"] = "Sai tên đăng nhập hoặc mật khẩu.";
                return View(model);
            }

            // Nếu tìm thấy người dùng và mật khẩu hợp lệ, tiến hành đăng nhập
            if (PasswordHelper.VerifyPassword(model.Password, user.Password))
            {
                // Xác định vai trò của người dùng
                string roleName = user.RoleType.ToString();

                // Tạo danh tính (identity) cho người dùng
                var identity = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, user.FullName),
                        new Claim(ClaimTypes.GivenName, user.Username),
                        new Claim(ClaimTypes.Role, roleName),
                        new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user)),
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                    }, "CookieAuth"); // Tên của schema authentication

                // Lấy context của OWIN và sử dụng AuthenticationManager để đăng nhập
                var owinContext = Request.GetOwinContext();
                var authManager = owinContext.Authentication;
                authManager.SignIn(identity);

                // Đăng nhập thành công, chuyển hướng đến trang chủ
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Nếu mật khẩu không đúng, hiển thị thông báo lỗi và hiển thị lại form đăng nhập
                TempData["error"] = "Đăng nhập không thành công.";
                return View(model);
            }
        }


        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem username đã tồn tại chưa
                var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Username == user.UserName);
                if (existingUser != null)
                {
                    TempData["error"] = "Tên người dùng đã được sử dụng.";
                    return View(user);
                }

                // Kiểm tra xem email đã tồn tại chưa
                existingUser = await db.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    TempData["error"] = "Email đã được sử dụng.";
                    return View(user);
                }

                // Nếu chưa tồn tại thì thêm người dùng mới
                var userModel = new User
                {
                    Password = PasswordHelper.HashPassword(user.Password),
                    FullName = user.FullName,
                    RoleType = RoleType.User,
                    Username = user.UserName,
                    Email = user.Email,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                db.Users.Add(userModel);
                await db.SaveChangesAsync();
                TempData["success"] = "Đăng ký tài khoản thành công.";
                return View();
            }

            // Nếu ModelState không hợp lệ thì hiển thị lại form đăng ký với các lỗi
            return View(user);
        }


        // GET: Account/Logout
        public ActionResult Logout()
        {
            var owinContext = Request.GetOwinContext();
            var authManager = owinContext.Authentication;
            authManager.SignOut("CookieAuth");
            return RedirectToAction("Index", "Home");
        }
    }
}
