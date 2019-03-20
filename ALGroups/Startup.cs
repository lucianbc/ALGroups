using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ALGroups.Startup))]
namespace ALGroups
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
