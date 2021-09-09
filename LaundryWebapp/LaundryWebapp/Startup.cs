using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LaundryWebapp.Startup))]
namespace LaundryWebapp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
