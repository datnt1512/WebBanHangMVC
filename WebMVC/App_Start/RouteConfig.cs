using System.Web.Mvc;
using System.Web.Routing;

namespace WebMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes(); 


           routes.MapRoute(
           name: "ProductDetail",
           url: "chi-tiet/{id}/{alias}",
           defaults: new { controller = "Product", action = "Detail", alias = UrlParameter.Optional });

            routes.MapRoute(
           name: "ProductCategory",
           url: "danh-sach-loai-san-pham/{id}",
           defaults: new { controller = "Product", action = "Category", alias = UrlParameter.Optional });


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
