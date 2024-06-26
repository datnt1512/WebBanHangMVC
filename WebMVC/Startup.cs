using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebMVC.Startup))]
namespace WebMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}