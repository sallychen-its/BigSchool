using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BTLab5.Startup))]
namespace BTLab5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
