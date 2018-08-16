using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Core.Web.Startup))]
namespace Core.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
