using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AllLifePricingWeb.Startup))]
namespace AllLifePricingWeb
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
