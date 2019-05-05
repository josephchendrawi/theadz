using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Adz.User.CMS.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace Adz.User.CMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
