using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(idoc.Startup))]
namespace idoc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
