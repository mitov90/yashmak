using Microsoft.Owin;

using Yashmak.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace Yashmak.Web
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}