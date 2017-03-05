using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(piquedo.Startup))]
namespace piquedo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
