using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(commerce_bot_mvc.Startup))]
namespace commerce_bot_mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
