using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentWebsite.Startup))]
namespace StudentWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
