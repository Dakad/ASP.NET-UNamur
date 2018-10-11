using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UNamur.Startup))]
namespace UNamur
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
