using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RCTPL_WebProjects.Startup))]
namespace RCTPL_WebProjects
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
