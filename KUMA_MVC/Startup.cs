using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KUMA_MVC.Startup))]
namespace KUMA_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
