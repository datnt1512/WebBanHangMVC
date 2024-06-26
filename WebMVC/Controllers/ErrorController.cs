using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMVC.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404; // Đặt mã trạng thái 404
            return View("~/Views/Shared/Error404.cshtml");
        }
    }
}