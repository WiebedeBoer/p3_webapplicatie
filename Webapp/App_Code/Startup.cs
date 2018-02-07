using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Webapp.Startup))]
namespace Webapp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
