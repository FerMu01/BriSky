using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BriSky.Startup))]
namespace BriSky
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
