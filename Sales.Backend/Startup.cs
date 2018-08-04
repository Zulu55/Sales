using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sales.Backend.Startup))]
namespace Sales.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
